using System;
using UnityEngine;

namespace TestProject.Save
{
    [Serializable]
    public sealed class PlayerInfo
    {
        public Vector3 Position;

        public PlayerInfo(Vector3 position) => Position = position;
    }
}