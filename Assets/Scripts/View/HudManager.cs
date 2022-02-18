using TestProject.Characters;
using TMPro;
using UnityEngine;

namespace TestProject.View
{
	public sealed class HudManager : MonoBehaviour
	{
		[SerializeField] private PlayerHealthView _playerHealthView;
		[SerializeField] private PlayerItemsCountView _itemsCountView;
		[SerializeField] private ScoreCounterView _counterView;
		[SerializeField] private WinScreen _winScreen;
		[SerializeField] private LoseScreen _loseScreen;
		[SerializeField] private LastMapScreen _lastMapScreen;
		[SerializeField] private TMP_Text _highScoreText;
		private GameController _gameController;

		private void OnDisable()
		{
			_gameController.OnLevelWin -= OnLevelWin;
			_gameController.OnLevelFailed -= OnLevelFailed;
			_gameController.OnLevelLoaded -= OnLevelLoaded;
			_gameController.OnGameEnd -= OnGameEnd;
		}

		public void Initialize(GameController controller, Player player)
		{
			_gameController = controller;
			_playerHealthView.Initialize(player);
			_itemsCountView.Initialize(player);
			_counterView.Initialize(player);
			
			_gameController.OnLevelWin += OnLevelWin;
			_gameController.OnLevelFailed += OnLevelFailed;
			_gameController.OnLevelLoaded += OnLevelLoaded;
			_gameController.OnGameEnd += OnGameEnd;
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
			_highScoreText.text = $"РЕКОРД: {score}";
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