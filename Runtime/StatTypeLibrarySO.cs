using System.Collections.Generic;
using UnityEngine;

namespace StatSystem
{
    [CreateAssetMenu(fileName = "StatTypeLibrary", menuName = "Stat System/Stat Type Library")]
    public class StatTypeLibrarySO : ScriptableObject
    {
        [SerializeField] private List<StatType> statTypes;

        private Dictionary<string, StatType> lookup;

        public StatType Get(string statName)
        {
            if (lookup == null)
                BuildLookup();

            return lookup.GetValueOrDefault(statName);
        }

        private void BuildLookup()
        {
            lookup = new Dictionary<string, StatType>();
            foreach (var statType in statTypes)
            {
                if (statType == null) continue;
                if (!lookup.TryAdd(statType.name, statType))
                    Debug.LogWarning($"Duplicate StatType '{statType.name}' in library, skipping.");
            }
        }
    }
}
