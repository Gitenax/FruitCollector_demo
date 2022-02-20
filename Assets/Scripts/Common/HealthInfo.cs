using System;
using UnityEngine;

namespace TestProject.Common
{
    [Serializable]
    public sealed class HealthInfo
    {
        private const int DefaultHealthValue = 100;
        private const int MaxHealthValue = DefaultHealthValue;
        
        [SerializeField] private int _value = DefaultHealthValue;
        [SerializeField] private int _maxValue = MaxHealthValue;

        public event Action<int, int> OnValueChanged;
        public event Action OnValueDroppedToZero;
        
        public int Value => _value;
        public int MaxValue => _maxValue;

        public void Increase(int value)
        {
            if (_value == _maxValue || value < 0)
                return;

            var before = _value;
            _value = Mathf.Min(_value + value, _maxValue);
            OnValueChanged?.Invoke(before, _value);
        }

        public void Decrease(int value)
        {
            if (_value == 0 && value > 0 || value < 0)
                return;

            var before = _value;
            _value = Mathf.Max(0, _value - value);
            OnValueChanged?.Invoke(before, _value);

            if (_value == 0)
                OnValueDroppedToZero?.Invoke();
        }
    }
}