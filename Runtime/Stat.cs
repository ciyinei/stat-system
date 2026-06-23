using System;
using System.Collections.Generic;
using UnityEngine;

namespace StatSystem
{
    [Serializable]
    public class Stat
    {
        [SerializeField] private float baseValue;
        [HideInInspector][SerializeField] private List<StatModifier> modifiers = new();

        private float currentValue;

        // Marks cached value as stale; recalculated lazily on next access
        private bool isDirty = true;

        /// <summary> Triggered when the final calculated value changes. </summary>
        public event Action<float, float> OnValueChanged;

        /// <summary> The current value after all modifiers are applied. </summary>
        public float Value
        {
            get
            {
                if (isDirty) currentValue = CalculateValue();
                return currentValue;
            }
        }

        public Stat(float baseValue)
        {
            this.baseValue = baseValue;
            modifiers = new List<StatModifier>();
            isDirty = true;
        }

        /// <summary> Adds a modifier and updates the value if necessary. </summary>
        public void AddModifier(StatModifier modifier)
        {
            float oldValue = Value;
            modifiers.Add(modifier);
            isDirty = true;

            float newValue = Value;
            if (!Mathf.Approximately(oldValue, newValue))
                OnValueChanged?.Invoke(oldValue, newValue);
        }

        /// <summary> Removes a modifier if it exists. </summary>
        public void RemoveModifier(StatModifier modifier)
        {
            float oldValue = Value;
            if (modifiers.Remove(modifier))
            {
                isDirty = true;
                float newValue = Value;
                if (!Mathf.Approximately(oldValue, newValue))
                    OnValueChanged?.Invoke(oldValue, newValue);
            }
        }

        /// <summary> Removes all modifiers that originated from the given source. </summary>
        public void RemoveModifiers(object source)
        {
            float oldValue = Value;
            int removedCount = modifiers.RemoveAll(m => m.Source == source);

            if (removedCount > 0)
            {
                isDirty = true;
                float newValue = Value;
                if (!Mathf.Approximately(oldValue, newValue))
                    OnValueChanged?.Invoke(oldValue, newValue);
            }
        }

        /// <summary>
        /// Applies modifiers in a fixed order:
        /// Flat → PercentAdd → PercentMult → AbsoluteAdditive
        /// </summary>
        private float CalculateValue()
        {
            float finalValue = baseValue;
            float sumPercentAdd = 0;
            float totalPercentMult = 1;
            float sumAbsoluteAdditive = 0;

            for (int i = 0; i < modifiers.Count; i++)
            {
                StatModifier m = modifiers[i];
                switch (m.Type)
                {
                    case StatModType.Flat: finalValue += m.Value; break;
                    case StatModType.PercentAdd: sumPercentAdd += m.Value; break;
                    case StatModType.PercentMult: totalPercentMult *= m.Value; break;
                    case StatModType.AbsoluteAdditive: sumAbsoluteAdditive += m.Value; break;
                }
            }

            finalValue *= (1 + sumPercentAdd);
            finalValue *= totalPercentMult;
            finalValue += sumAbsoluteAdditive;
            isDirty = false;

            return (float)Math.Round(finalValue, 2);
        }

        /// <summary> Creates a deep copy of the Stat and its modifiers. </summary>
        public Stat Clone()
        {
            Stat clonedStat = new Stat(baseValue);
            foreach (StatModifier modifier in modifiers)
                clonedStat.modifiers.Add(modifier.Clone());
            clonedStat.isDirty = true;
            return clonedStat;
        }
    }
}