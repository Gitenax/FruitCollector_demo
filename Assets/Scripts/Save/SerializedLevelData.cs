using System;
using System.Collections.Generic;
using TestProject.Characters;
using TestProject.Characters.Enemies;
using TestProject.Resources;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace TestProject.Save
{
	public sealed class SerializedLevelData : ScriptableObject
	{
		public int LevelIndex;
		public PlayerInfo Player;
		public List<EnemyInfo> Enemies;
		public List<TileInfo> GroundTiles;
		public List<TileInfo> WallTiles;
		public List<ItemInfo> Items;
	}
	
	public sealed class LevelData
	{
		public Player Player;
		public List<Enemy> Enemies;
		public List<Item> Items;
	}

	[Serializable]
	public class TileInfo
	{
		public Vector3Int Position;
		[HideInInspector] public TileBase Tile;
	}

	[Serializable]
	public sealed class PlayerInfo
	{
		public Vector3 Position;
		
		public PlayerInfo(Vector3 position) => Position = position;
	}
	
	[Serializable]
	public sealed class EnemyInfo
	{
		public Vector3 Position;
		
		public EnemyInfo(Vector3 position) => Position = position;
	}

	[Serializable]
	public sealed class ItemInfo
	{
		public string PrefabPath;
		public Vector3 Position;

		public ItemInfo(string prefabPath, Vector3 position)
		{
			PrefabPath = prefabPath;
			Position = position;
		}
	}
}