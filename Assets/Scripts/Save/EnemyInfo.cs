using System;
using UnityEngine;

namespace TestProject.Save
{
    [Serializable]
    public sealed class EnemyInfo
    {
        public Vector3 Position;

        public EnemyInfo(Vector3 position) => Position = position;
    }
}