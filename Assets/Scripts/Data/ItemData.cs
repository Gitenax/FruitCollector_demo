using UnityEngine;

namespace TestProject.Data
{
    [CreateAssetMenu(fileName = "ItemData", menuName = "Items/Item data", order = 0)]
    public class ItemData : ScriptableObject
    {
        private const int DefaultItemScore = 100;
        private const int DefaultItemHealthPoints = 10;

        [SerializeField] private int _healthPoints = DefaultItemHealthPoints;
        [SerializeField] private int _score = DefaultItemScore;

        public int HealthPoints => _healthPoints;
        public int Score => _score;
    }
}