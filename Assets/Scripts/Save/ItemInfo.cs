using System;
using UnityEngine;

namespace TestProject.Save
{
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