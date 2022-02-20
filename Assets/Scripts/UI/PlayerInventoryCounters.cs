using TestProject.Characters;
using TMPro;
using UnityEngine;

namespace TestProject.UI
{
    public abstract class PlayerInventoryCounters : MonoBehaviour
    {
        [SerializeField] protected TMP_Text Text;
        protected ScoreCounter ScoreCounter => ProjectContext.Instance.ScoreCounter;

        protected void Awake()
        {
            if (Text == null)
                Debug.LogWarning($"[{nameof(PlayerItemsCountView)}] {nameof(Awake)}, field {nameof(Text)} is null!");
        }

        private void OnDestroy()
        {
            if (ScoreCounter == null)
                return;

            OnObjectDestroy();
        }

        public virtual void Reset() { }

        public void Initialize()
        {
            OnInitialized();
        }

        protected virtual void OnInitialized() { }

        protected virtual void OnObjectDestroy() { }
    }
}