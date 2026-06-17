using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace StatSystem
{
    public static class StatTypeRegistry
    {
        private static readonly Dictionary<int, StatType> _allTypes = new();
        private static readonly Dictionary<int, string> _names = new();

        public static StatType Register(int id, string name)
        {
            if (_allTypes.TryGetValue(id, out var existing))
            {
                Debug.LogError($"StatType ID {id} already used by '{_names[id]}'. Cannot register '{name}'.");
                return existing;
            }

            var type = new StatType(id);
            _allTypes[id] = type;
            _names[id] = name;
            return type;
        }

        public static string GetName(int id)
        {
            return _names.TryGetValue(id, out var name) ? name : "Unknown";
        }

        public static StatType GetById(int id)
        {
            return _allTypes.TryGetValue(id, out var type) ? type : null;
        }

        public static StatType[] GetAll() => _allTypes.Values.ToArray();
    }
}
