using System;
using UnityEngine;

namespace TestProject.Common
{
	[Serializable]
	public sealed class HealthInfo
	{
		[SerializeField] private int _value = 100;
		[SerializeField] private int _maxValue = 100;

		public event Action<int, int> OnValueChanged;
		public event Action OnValueDroppedToZero;
		
		public int Value => _value;
		public int MaxValue => _maxValue;

		public void Increase(int value)
		{
			if(_value == _maxValue || value < 0)
				return;

			int before = _value;
			_value = Mathf.Min(_value + value, _maxValue);
			OnValueChanged?.Invoke(before, _value);
		}

		public void Decrease(int value)
		{
			if (_value == 0 && value > 0 || value < 0)
				return;
			
			int before = _value;
			_value = Mathf.Max(0, _value - value);
			OnValueChanged?.Invoke(before, _value);
			
			if(_value == 0)
				OnValueDroppedToZero?.Invoke();
		}
	}
}