using System.Collections.Generic;
using UnityEngine;

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
}