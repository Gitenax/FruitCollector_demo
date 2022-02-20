using System;
using System.Collections.Generic;
using System.Linq;
using TestProject.Characters;
using TestProject.Characters.Enemies;
using TestProject.Resources;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace TestProject.Save
{
    public sealed class MapSerializer : MonoBehaviour
    {
        private const string MapAssetSavePathMask = "Assets/Resources/Levels/{0}.asset";
        private const string MapAssetLoadPathMask = "Levels/{0}";
        private const string MapAssetTypeFilter = "t:SerializedLevelData";

        [SerializeField] private int _currentLevelIndex;

        [Header("Player")]
        [SerializeField] private Player _playerPrefab;
        [SerializeField] private Transform _playerRoot;

        [Header("Enemies and Items")]
        [SerializeField] private Enemy _enemyPrefab;
        [SerializeField] private Transform _enemiesRoot;
        [SerializeField] private Transform _itemsRoot;

        [Header("Environment")] 
        [SerializeField] private Tilemap _ground;
        [SerializeField] private Tilemap _walls;

        public event Action<LevelData> OnLevelLoaded;
        public event Action OnNoMoreMaps;

        private void Awake()
        {
            LoadMap();
        }

        [ContextMenu("Save map")]
        public void SaveMap()
        {
            SerializedLevelData mapData = ScriptableObject.CreateInstance<SerializedLevelData>();
            mapData.LevelIndex = _currentLevelIndex;
            mapData.name = _currentLevelIndex.ToString();

            mapData.Player = new PlayerInfo(FindObjectOfType<Player>().transform.position);
            mapData.Enemies = new List<EnemyInfo>();
            mapData.Items = new List<ItemInfo>();
            mapData.GroundTiles = GetTiles(_ground).ToList();
            mapData.WallTiles = GetTiles(_walls).ToList();

            foreach (Enemy enemy in FindObjectsOfType<Enemy>())
                mapData.Enemies.Add(new EnemyInfo(enemy.transform.position));

            foreach (Item item in FindObjectsOfType<Item>())
            {
                var prefabPath = PrefabUtility.GetPrefabAssetPathOfNearestInstanceRoot(item);
                mapData.Items.Add(new ItemInfo(prefabPath, item.transform.position));
            }

            AssetDatabase.CreateAsset(mapData, string.Format(MapAssetSavePathMask, mapData.name));
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        public void LoadNextMap()
        {
            _currentLevelIndex++;
            LoadMap();
        }

        public void LoadMap(int levelIndex)
        {
            _currentLevelIndex = levelIndex;
            LoadMap();
        }

        public bool CheckNextLevelExits()
        {
            IEnumerable<string> result =
                AssetDatabase.FindAssets(MapAssetTypeFilter)
                    .Select(AssetDatabase.GUIDToAssetPath);

            return result.Any(predicate: x => x.EndsWith($"{_currentLevelIndex + 1}.asset"));
        }

        [ContextMenu("Load map")]
        public void LoadMap()
        {
            SerializedLevelData map = UnityEngine.Resources.Load<SerializedLevelData>(string.Format(MapAssetLoadPathMask, _currentLevelIndex));
            if (map == null)
            {
                OnNoMoreMaps?.Invoke();
                return;
            }

            ClearMap();
            
            var levelData = new LevelData
            {
                Enemies = new List<Enemy>(),
                Items = new List<Item>(),
                Player = Instantiate(_playerPrefab, map.Player.Position, Quaternion.identity, _playerRoot)
            };

            SetTiles(_ground, map.GroundTiles);
            SetTiles(_walls, map.WallTiles);
            
            InstantiateEnemies(map, levelData);
            InstantiateItems(map, levelData);

            OnLevelLoaded?.Invoke(levelData);
        }

        [ContextMenu("Clear map")]
        public void ClearMap()
        {
            Player plyr = FindObjectOfType<Player>();
            if (plyr != null)
                DestroyImmediate(plyr.gameObject);

            foreach (Tilemap map in FindObjectsOfType<Tilemap>())
                map.ClearAllTiles();

            foreach (Enemy enemy in FindObjectsOfType<Enemy>(true))
                DestroyImmediate(enemy.gameObject);

            foreach (Item item in FindObjectsOfType<Item>(true))
                DestroyImmediate(item.gameObject);
        }

        private IEnumerable<TileInfo> GetTiles(Tilemap map)
        {
            foreach (Vector3Int position in map.cellBounds.allPositionsWithin)
            {
                if (!map.HasTile(position))
                    continue;

                yield return new TileInfo
                {
                    Position = position,
                    Tile = map.GetTile<Tile>(position)
                };
            }
        }

        private void SetTiles(Tilemap map, IEnumerable<TileInfo> tiles)
        {
            foreach (TileInfo tile in tiles)
                map.SetTile(tile.Position, tile.Tile);
        }

        private void InstantiateEnemies(SerializedLevelData map, LevelData levelData)
        {
            foreach (EnemyInfo enemyInfo in map.Enemies)
                levelData.Enemies.Add(Instantiate(_enemyPrefab, enemyInfo.Position, Quaternion.identity, _enemiesRoot));
        }

        private void InstantiateItems(SerializedLevelData map, LevelData levelData)
        {
            foreach (ItemInfo itemInfo in map.Items)
            {
                Item prefab = AssetDatabase.LoadAssetAtPath<Item>(itemInfo.PrefabPath);
                levelData.Items.Add(Instantiate(prefab, itemInfo.Position, Quaternion.identity, _itemsRoot));
            }
        }
    }
}