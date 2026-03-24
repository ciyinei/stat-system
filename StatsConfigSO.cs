using System.Collections.Generic;
using AYellowpaper.SerializedCollections;
using UnityEngine;

namespace StatSystem
{
    [CreateAssetMenu(fileName = "Stats Config", menuName = "Scriptable Objects/Stats Config")]
    public class StatsConfigSO : ScriptableObject
    {
        /// <summary>
        /// The internal dictionary holding the stat definitions. 
        /// Uses SerializedDictionary to allow editing in the Unity Inspector.
        /// </summary>
        [SerializeField] [SerializedDictionary("Type", "Stat")]
        private SerializedDictionary<StatType, Stat> stats;

        /// <summary>
        /// Provides read-only access to the stat data. 
        /// Used by StatsModel to initialize its internal state.
        /// </summary>
        public IReadOnlyDictionary<StatType, Stat> Data => stats;
    }
}
