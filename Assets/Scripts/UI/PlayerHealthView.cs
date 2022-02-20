using TestProject.Characters;
using UnityEngine;
using UnityEngine.UI;

namespace TestProject.UI
{
    public sealed class PlayerHealthView : MonoBehaviour
    {
        private readonly Color FullHealthColor = Color.green;
        private readonly Color LowHealthColor = Color.red;

        [Header("Slider")] 
        [SerializeField] private Slider _slider;
        [SerializeField] private Image _sliderFillImage;

        private Player Player => ProjectContext.Instance.Player;
        private int PlayerHealth => Player.Health.Value;
        private int PlayerMaxHealth => Player.Health.MaxValue;

        private void Awake()
        {
            if (_slider == null)
                Debug.LogWarning($"[{nameof(PlayerHealthView)}] {nameof(Awake)}, slider fields are not filled correctly!");
        }

        private void OnDisable()
        {
            Player.Health.OnValueChanged -= OnPlayerHealthValueChangedHandler;
        }

        private void OnDestroy()
        {
            Player.Health.OnValueChanged -= OnPlayerHealthValueChangedHandler;
        }

        public void Initialize()
        {
            _slider.maxValue = PlayerMaxHealth;
            _slider.value = PlayerHealth;
            _sliderFillImage.color = Color.Lerp(LowHealthColor, FullHealthColor, _slider.normalizedValue);
            Player.Health.OnValueChanged += OnPlayerHealthValueChangedHandler;
        }

        private void OnPlayerHealthValueChangedHandler(int before, int after)
        {
            _slider.value = after;
            _sliderFillImage.color = Color.Lerp(LowHealthColor, FullHealthColor, _slider.normalizedValue);
        }
    }
}