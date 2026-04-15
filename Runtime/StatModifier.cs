using System;
using UnityEngine;
using Object = UnityEngine.Object;

namespace StatSystem
{
    public enum StatModType
    {
        Flat,
        PercentAdd,
        PercentMult,
        AbsoluteAdditive
    }
    
    [Serializable]
    public class StatModifier
    {
        [SerializeField] private float value;
        [SerializeField] private StatModType type;
        [SerializeField] private string sourceName;
        private object source;

        public float Value => value;
        public StatModType Type => type;
        public object Source => source;
        public string SourceName => sourceName;
    
        /// <summary> Defines a modifier with a value, math type, and the object that owns it. </summary>
        public StatModifier(float value, StatModType type, object source)
        {
            this.value = value;
            this.type = type;
            this.source = source;

            // Assign a readable name based on the source type for easier debugging.
            if (source is Object uObj) sourceName = uObj.name;
            else if (source != null) sourceName = source.GetType().Name;
            else sourceName = "Unknown";
        }
    
        public StatModifier Clone() => new StatModifier(value, type, source);
    }
}
