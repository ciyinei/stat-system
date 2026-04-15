using System.Collections.Generic;
using UnityEngine;

namespace StatSystem
{
    public static class StatTypeRegistry
    {
        private static int _next = 0;
        private static readonly List<StatType> _all = new();

        public static StatType Register(string name)
        {
            var type = new StatType(_next++, name);
            _all.Add(type);
            return type;
        }

        public static StatType[] GetAll() => _all.ToArray();
    }
}
