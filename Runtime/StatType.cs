using System;
using UnityEngine;

namespace StatSystem
{
    [Serializable]
    public class StatType : IEquatable<StatType>
    {
        [SerializeField] private int _value;
        [SerializeField] private string _name;

        public int Value => _value;
        public string Name => StatTypeRegistry.GetName(_value);

        internal StatType(int value)
        {
            _value = value;
        }

        public static readonly StatType MaxHealth = StatTypeRegistry.Register(0, nameof(MaxHealth));
        public static readonly StatType Damage = StatTypeRegistry.Register(1, nameof(Damage));

        public bool Equals(StatType other) => other != null && _value == other._value;
        public override bool Equals(object obj) => obj is StatType other && Equals(other);
        public override int GetHashCode() => _value;
        public override string ToString() => Name;

        public static bool operator ==(StatType a, StatType b)
        {
            if (a is null && b is null) return true;
            if (a is null || b is null) return false;
            return a._value == b._value;
        }

        public static bool operator !=(StatType a, StatType b) => !(a == b);
    }
}
