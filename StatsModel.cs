using System;
using System.Collections.Generic;
using UnityEngine;

namespace StatSystem
{
    public enum StatType
    {
        MaxHealth,
        Damage,
        Speed
    }
    
    public class StatsModel
    {
        /// <summary> Triggers when any stat in this model changes. (Type, OldValue, NewValue) </summary>
        public event Action<StatType, float, float> OnStatChanged;
    
        /// <summary> Triggers when a specific stat type is first registered. </summary>
        public event Action<StatType> OnStatInitialized;

        private readonly Dictionary<StatType, Stat> _stats = new();
    
        // Stores delegate references per stat type so they can be properly removed later
        private readonly Dictionary<StatType, Action<float, float>> _statHandlers = new();
    
        /// <summary> Public access to the stat collection (Read-Only). </summary>
        public IReadOnlyDictionary<StatType, Stat> StatsData => _stats;
        
        public StatsModel() { }

        public StatsModel(StatsConfigSO statsConfig)
        {
            foreach (var kvp in statsConfig.Data)
            {
                InitializeStat(kvp.Key, kvp.Value.Clone());
            }
        }
        
        /// <summary> 
        /// Safely retrieves a Stat object. 
        /// If it doesn't exist, it initializes a default one to prevent NullReferenceExceptions.
        /// </summary>
        public Stat GetStat(StatType statType)
        {
            if (!_stats.TryGetValue(statType, out Stat stat))
            {
                Debug.LogWarning($"Stat {statType} not found. Initializing with default 0.");
                stat = new Stat(0f);
                InitializeStat(statType, stat);
            }
            return stat;
        }
        
        /// <summary> 
        /// Safely gets the current float value of a stat. 
        /// Returns 0 if the stat does not exist. 
        /// </summary>
        public float GetStatValue(StatType statType)
        {
            return _stats.TryGetValue(statType, out Stat stat) ? stat.Value : 0f;
        }
        
        /// <summary>
        /// Checks if a stat exists and returns its value. 
        /// Returns false if the stat is missing.
        /// </summary>
        public bool TryGetStatValue(StatType statType, out float value)
        {
            if (_stats.TryGetValue(statType, out Stat stat))
            {
                value = stat.Value;
                return true;
            }
            value = 0f;
            return false;
        }
        
        /// <summary>
        /// Registers or overwrites a stat.
        /// Properly unsubscribes from old stats to prevent memory leaks and event ghosts.
        /// </summary>
        private void InitializeStat(StatType type, Stat stat)
        {
            if (_stats.TryGetValue(type, out Stat oldStat) && _statHandlers.TryGetValue(type, out var oldHandler))
            {
                oldStat.OnValueChanged -= oldHandler;
            }

            _stats[type] = stat;

            Action<float,float> handler = (oldVal, newVal) => RelayEvent(type, oldVal, newVal);
            _statHandlers[type] = handler;
            stat.OnValueChanged += handler;
        
            OnStatInitialized?.Invoke(type);
        }
        
        private void RelayEvent(StatType type, float oldVal, float newVal)
        {
            OnStatChanged?.Invoke(type, oldVal, newVal);
        }
        
        /// <summary>
        /// Unsubscribes from all internal stat events to allow for proper garbage collection.
        /// </summary>
        public void Dispose()
        {
            foreach (var kvp in _stats)
            {
                if (_statHandlers.TryGetValue(kvp.Key, out var handler))
                    kvp.Value.OnValueChanged -= handler;
            }
            _stats.Clear();
            _statHandlers.Clear();
        }
    }
}
