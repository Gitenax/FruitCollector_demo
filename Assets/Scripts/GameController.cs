using System;
using System.Collections.Generic;
using TestProject.Characters;
using TestProject.Interact;
using TestProject.Resources;
using TestProject.Save;
using TestProject.View;
using UnityEngine;
using UnityEngine.AI;

namespace TestProject
{
	public sealed class GameController : MonoBehaviour
	{
		private const int FirstLevelIndex = 0;
		
		[SerializeField] private MapSerializer _mapSerializer;
		[SerializeField] private HudManager _hudManager;
		[SerializeField] private CameraFollow _camera;
		[SerializeField] private UserInput _input;
		[SerializeField] private NavMeshSurface2d _navMesh;

		private Player _player;
		private List<Item> _itemsOnLevel;
		private int _levelGoal;
		private int _pickedItems;
		private int _currentLevelScore;
		private int _totalScore;

		public event Action<int> OnLevelWin;
		public event Action<int> OnLevelFailed;
		public event Action<int> OnLevelLoaded;
		public event Action<int> OnGameEnd;

		private void Awake()
		{
			_mapSerializer.OnLevelLoaded += OnLevelLoadedHandler;
			_mapSerializer.OnNoMoreMaps += OnNoMoreMapsHandler;
		}

		private void OnDestroy()
		{
			foreach (Item item in _itemsOnLevel)
				item.OnItemPicked -= OnItemPickedHandler;
		}

		public void NextLevel() => _mapSerializer.LoadNextMap();

		public void RestartGame()
		{
			_totalScore = 0;
			_mapSerializer.LoadMap(FirstLevelIndex);
		}
		
		private void OnLevelLoadedHandler(LevelData data)
		{
			_navMesh.UpdateNavMesh(_navMesh.navMeshData).completed += operation =>
			{
				_currentLevelScore = 0;
				_pickedItems = 0;
				_itemsOnLevel = data.Items;
				_levelGoal = _itemsOnLevel.Count;
				_player = data.Player;
				_hudManager.Initialize(this, _player);
				_camera.Initialize(_player);
				_player.Health.OnValueDroppedToZero += OnPlayerHealthDroppedToZeroHandler;
				_input.enabled = true;

				foreach (Item item in _itemsOnLevel)
					item.OnItemPicked += OnItemPickedHandler;
				
				OnLevelLoaded?.Invoke(_totalScore);
			};
		}

		private void OnNoMoreMapsHandler()
		{
			_totalScore += _currentLevelScore;
			int highscore = ConfigureHighScore();
			OnGameEnd?.Invoke(highscore);
		}

		private void OnItemPickedHandler(Item item)
		{
			_currentLevelScore += item.ItemData.Score;
			_player.PickUpItem(item);
			item.gameObject.SetActive(false);
			_pickedItems++;

			if(_pickedItems == _levelGoal)
			{
				_input.enabled = false;
				_totalScore += _currentLevelScore;
				int highscore = ConfigureHighScore();
				
				if(_mapSerializer.CheckNextLevelExits())
					OnLevelWin?.Invoke(highscore);
				else
					OnGameEnd?.Invoke(highscore);
			}
		}
		
		private void OnPlayerHealthDroppedToZeroHandler()
		{
			_totalScore += _currentLevelScore;
			int highscore = ConfigureHighScore();
			
			OnLevelFailed?.Invoke(highscore);
			_input.enabled = false;
		}

		private int ConfigureHighScore()
		{
			int highscore;
			
			if(StatSaver.Load().HighScore < _totalScore)
			{
				StatSaver.Save(new StatInfo() {HighScore = _totalScore});
				highscore = _totalScore;
			}
			else
			{
				highscore = StatSaver.Load().HighScore;
			}

			return highscore;
		}
	}
}