using System;
using System.Collections.Generic;
using TestProject.Characters;
using TestProject.Characters.Enemies;
using TestProject.Interact;
using TestProject.Resources;
using TestProject.Save;
using TestProject.UI;
using UnityEngine;
using UnityEngine.AI;

namespace TestProject
{
    public sealed class ProjectContext : MonoBehaviour
    {
        private const int FirstLevelIndex = 0;

        [SerializeField] private MapSerializer _mapSerializer;
        [SerializeField] private HudManager _hudManager;
        [SerializeField] private CameraFollow _cameraFollow;
        [SerializeField] private UserInput _input;
        [SerializeField] private NavMeshSurface2d _navMesh;
        private List<Item> _itemsOnLevel;
        private List<Enemy> _enemiesOnLevel;

        public event Action<int> OnLevelWin;
        public event Action<int> OnLevelFailed;
        public event Action<int> OnLevelLoaded;
        public event Action<int> OnGameEnd;
        
        public static ProjectContext Instance { get; private set; }
        public Player Player { get; private set; }
        public UserInput Input => _input;
        public Camera GameplayCamera => Camera.main;
        public ScoreCounter ScoreCounter { get; private set; }
        
        private void Awake()
        {
            Instance = this;
            ScoreCounter = new ScoreCounter(StatSaver.Load().HighScore);
            
            _mapSerializer.OnLevelLoaded += OnLevelLoadedHandler;
            _mapSerializer.OnNoMoreMaps += OnNoMoreMapsHandler;
            ScoreCounter.OnGoalAchieved += OnCurrentLevelGoalAchievedHandler;
        }
        
        private void OnDestroy()
        {
            foreach (Item item in _itemsOnLevel)
                item.OnItemPicked -= OnItemPickedHandler;
        }
        
        public void NextLevel()
        {
            _mapSerializer.LoadNextMap();
        }

        public void RestartGame()
        {
            ScoreCounter.ResetCurrentSession();
            _mapSerializer.LoadMap(FirstLevelIndex);
        }

        private void OnLevelLoadedHandler(LevelData data)
        {
            _navMesh.UpdateNavMesh(_navMesh.navMeshData).completed += operation =>
            {
                _itemsOnLevel = data.Items;
                _enemiesOnLevel = data.Enemies;
                Player = data.Player;
                
                ScoreCounter.ResetCurrentLevel();
                ScoreCounter.SetLevelGoal(_itemsOnLevel.Count);

                _hudManager.Initialize(Player);
                _cameraFollow.Initialize(Player);
                
                Player.Health.OnValueDroppedToZero += OnPlayerHealthDroppedToZeroHandler;
                Input.enabled = true;

                foreach (Item item in _itemsOnLevel)
                    item.OnItemPicked += OnItemPickedHandler;

                OnLevelLoaded?.Invoke(ScoreCounter.TotalScore);
            };
        }

        private void OnNoMoreMapsHandler()
        {
            SaveHighScore();
            _enemiesOnLevel.ForEach(e => e.Stop());
            OnGameEnd?.Invoke(ScoreCounter.TotalScore);
        }
        
        private void OnCurrentLevelGoalAchievedHandler()
        {
            SaveHighScore();
            Input.enabled = false;
            _enemiesOnLevel.ForEach(e => e.Stop());
            
            if (_mapSerializer.CheckNextLevelExits())
                OnLevelWin?.Invoke(ScoreCounter.TotalScore);
            else
                OnGameEnd?.Invoke(ScoreCounter.TotalScore);
        }

        private void OnItemPickedHandler(Item item)
        {
            Player.PickUpItem(item);
            ScoreCounter.Increase(item.ItemData.Score, 1);
            item.gameObject.SetActive(false);
        }

        private void OnPlayerHealthDroppedToZeroHandler()
        {
            OnLevelFailed?.Invoke(ScoreCounter.TotalScore);
            Input.enabled = false;
            _enemiesOnLevel.ForEach(e => e.Stop());
        }

        private void SaveHighScore()
        {
            if (StatSaver.Load().HighScore < ScoreCounter.TotalScore)
                StatSaver.Save(new StatInfo {HighScore = ScoreCounter.TotalScore});
        }
    }
}