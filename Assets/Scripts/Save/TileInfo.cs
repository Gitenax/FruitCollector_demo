using System;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace TestProject.Save
{
    [Serializable]
    public sealed class TileInfo
    {
        public Vector3Int Position;
        [HideInInspector] public TileBase Tile;
    }
}