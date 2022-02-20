using TestProject.Common;
using TestProject.Resources;
using UnityEngine;
using UnityEngine.AI;

namespace TestProject.Characters
{
    [RequireComponent(typeof(NavMeshAgent))]
    public sealed class Player : MonoBehaviour
    {
        [SerializeField] private HealthInfo _health;
        private NavMeshAgent _navMeshAgent;
        private Vector2 _destination;
        private bool _isMoveing;

        public HealthInfo Health => _health;
        
        public Inventory Inventory { get; } = new Inventory();

        private static Camera Camera
            => ProjectContext.Instance.GameplayCamera;
        
        private static bool IsLeftMouseClicked 
            => ProjectContext.Instance.Input.LeftMouseClick;

        private void Awake()
        {
            _destination = transform.position;
            _navMeshAgent = GetComponent<NavMeshAgent>();
            _navMeshAgent.updateRotation = false;
            _navMeshAgent.updateUpAxis = false;
        }

        private void Update()
        {
            if (IsLeftMouseClicked)
            {
                _destination = Camera.ScreenToWorldPoint(Input.mousePosition);
                _isMoveing = true;
            }

            if (_isMoveing == false)
                return;

            if (transform.position.Equals(_destination))
                _isMoveing = false;

            _navMeshAgent.SetDestination(_destination);
        }

        public void SetDamage(DamageInfo damage)
        {
            _health.Decrease(damage.Amount);
        }

        public void PickUpItem(Item item)
        {
            Inventory.AddItem(item);
            Health.Increase(item.ItemData.HealthPoints);
        }
    }
}