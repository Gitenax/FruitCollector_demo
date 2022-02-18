using TestProject.Characters;
using UnityEngine;
using UnityEngine.UI;

namespace TestProject.View
{
	public sealed class PlayerHealthView : MonoBehaviour
	{
		private readonly Color LowHealthColor = Color.red;
		private readonly Color FullHealthColor = Color.green;

		[Header("Slider")]
		[SerializeField] private Slider _slider;
		[SerializeField] private Image _sliderFillImage;

		private Player _player;

		private void Awake()
		{
			if (_slider == null)
				Debug.LogWarning($"[{nameof(PlayerHealthView)}] {nameof(Awake)}, slider fields are not filled correctly!");
		}
		
		private void OnDisable() => _player.Health.OnValueChanged -= OnPlayerHealthValueChangedHandler;

		private void OnDestroy() => _player.Health.OnValueChanged -= OnPlayerHealthValueChangedHandler;

		public void Initialize(Player player)
		{
			_player = player;
			_slider.maxValue = _player.Health.MaxValue;
			_slider.value = _player.Health.Value;
			_sliderFillImage.color = Color.Lerp(LowHealthColor, FullHealthColor, _slider.normalizedValue);
			_player.Health.OnValueChanged += OnPlayerHealthValueChangedHandler;
		}
		
		private void OnPlayerHealthValueChangedHandler(int before, int after)
		{
			_slider.value = after;
			_sliderFillImage.color = Color.Lerp(LowHealthColor, FullHealthColor, _slider.normalizedValue);
		}
	}
}