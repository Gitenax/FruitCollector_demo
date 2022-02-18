using TestProject.Common;
using TestProject.Interact;
using TestProject.Resources;
using UnityEngine;
using UnityEngine.AI;

namespace TestProject.Characters
{
	[RequireComponent(typeof(NavMeshAgent))]
	public sealed class Player : MonoBehaviour
	{
		private readonly Inventory _inventory = new Inventory();
		
		[SerializeField] private HealthInfo _health;
		[SerializeField] private ColliderTrigger2D _collider;
		private NavMeshAgent _navMeshAgent;
		private Camera _camera;
		private Vector2 _destination;
		private bool _isMoveing;
		private UserInput _input;

		public HealthInfo Health => _health;
		public Inventory Inventory => _inventory;

		private void Awake()
		{
			_destination = transform.position;
			_camera = Camera.main;
			_input = FindObjectOfType<UserInput>();
			_navMeshAgent = GetComponent<NavMeshAgent>();
			_navMeshAgent.updateRotation = false;
			_navMeshAgent.updateUpAxis = false;
		}

		private void Update()
		{
			if (_input.LeftMouseClick)
			{
				_destination = _camera.ScreenToWorldPoint(Input.mousePosition);
				_isMoveing = true;
			}
			
			if(_isMoveing == false)
				return;

			if (transform.position.Equals(_destination))
				_isMoveing = false;

			_navMeshAgent.SetDestination(_destination);
		}

		public void SetDamage(DamageInfo damage) => _health.Decrease(damage.Amount);

		public void PickUpItem(Item item)
		{
			_inventory.AddItem(item);
			Health.Increase(item.ItemData.HealthPoints);
		}
	}
}