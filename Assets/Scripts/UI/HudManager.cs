using TestProject.Characters;
using TMPro;
using UnityEngine;

namespace TestProject.UI
{
    public sealed class HudManager : MonoBehaviour
    {
        private const string HighScoreMask = "РЕКОРД: {0}"; 
        
        [SerializeField] private PlayerHealthView _playerHealthView;
        [SerializeField] private PlayerItemsCountView _itemsCountView;
        [SerializeField] private ScoreCounterView _counterView;
        [SerializeField] private WinScreen _winScreen;
        [SerializeField] private LoseScreen _loseScreen;
        [SerializeField] private LastMapScreen _lastMapScreen;
        [SerializeField] private TMP_Text _highScoreText;
        
        private ProjectContext ProjectContext => ProjectContext.Instance;

        private void OnDisable()
        {
            ProjectContext.OnLevelWin -= OnLevelWin;
            ProjectContext.OnLevelFailed -= OnLevelFailed;
            ProjectContext.OnLevelLoaded -= OnLevelLoaded;
            ProjectContext.OnGameEnd -= OnGameEnd;
        }

        public void Initialize(Player player)
        {
            _playerHealthView.Initialize();
            _itemsCountView.Initialize();
            _counterView.Initialize();

            ProjectContext.OnLevelWin += OnLevelWin;
            ProjectContext.OnLevelFailed += OnLevelFailed;
            ProjectContext.OnLevelLoaded += OnLevelLoaded;
            ProjectContext.OnGameEnd += OnGameEnd;
        }

        private void OnLevelWin(int score)
        {
            _winScreen.gameObject.SetActive(true);
            ShowHighScore(score);
        }

        private void OnLevelFailed(int score)
        {
            _loseScreen.gameObject.SetActive(true);
            ShowHighScore(score);
        }

        private void OnGameEnd(int score)
        {
            _lastMapScreen.gameObject.SetActive(true);
            ShowHighScore(score);
        }

        private void ShowHighScore(int score)
        {
            _highScoreText.gameObject.SetActive(true);
            _highScoreText.text = string.Format(HighScoreMask, score);
        }

        private void OnLevelLoaded(int score)
        {
            _winScreen.gameObject.SetActive(false);
            _loseScreen.gameObject.SetActive(false);
            _lastMapScreen.gameObject.SetActive(false);
            _highScoreText.gameObject.SetActive(false);
        }
    }
}