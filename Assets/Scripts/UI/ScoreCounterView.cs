using System.Collections;
using UnityEngine;

namespace TestProject.UI
{
    public sealed class ScoreCounterView : PlayerInventoryCounters
    {
        private const float CountDelay = 0.001f;

        private int _score;
        
        protected override void OnInitialized()
        {
            _score = 0;
            Text.text = "0";
            ScoreCounter.OnScoreChanged += OnScoreChangedHandler;
        }

        protected override void OnObjectDestroy()
        {
            ScoreCounter.OnScoreChanged -= OnScoreChangedHandler;
        }

        private void OnScoreChangedHandler(int value)
        {
            _score = value;
            StartCoroutine(nameof(Countup));
        }

        private IEnumerator Countup()
        {
            var scoreFrom = int.Parse(Text.text);

            while (scoreFrom != _score)
            {
                scoreFrom++;
                Text.text = scoreFrom.ToString();
                yield return new WaitForSeconds(CountDelay);
            }
        }
    }
}