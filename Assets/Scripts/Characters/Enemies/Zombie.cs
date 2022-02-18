using TestProject.Interact;
using UnityEngine;
using UnityEngine.AI;

namespace TestProject.Characters.Enemies
{
	public sealed class Zombie : Enemy
	{
		[SerializeField] private ColliderTrigger2D _detectionArea;
		[SerializeField] private ColliderTrigger2D _attackArea;
		private Player _target;
		private NavMeshAgent _navMeshAgent;
		private bool _isChasePlayer;

		private void Awake()
		{
			_navMeshAgent = GetComponentInChildren<NavMeshAgent>();
			_navMeshAgent.updateRotation = false;
			_navMeshAgent.updateUpAxis = false;

			if (_attackArea == null)
			{
				Debug.LogWarning($"[{nameof(Zombie)}] {nameof(Awake)}, field {nameof(Collider)} is null!");
				return;
			}

			_attackArea.OnTriggerEnter += OnCollisionWithPlayer;

			if (_detectionArea == null)
			{
				Debug.LogWarning($"[{nameof(Zombie)}] {nameof(Awake)}, field {nameof(_detectionArea)} is null!");
				return;
			}

			_detectionArea.OnTriggerStay += OnDetectionAreaStayHandler;
			_detectionArea.OnTriggerExit += OnDetectionAreaExitHandler;
		}

		private void Update()
		{
			if (_isChasePlayer && _target != null)
				_navMeshAgent.SetDestination(_target.transform.position);
		}

		private void OnDestroy()
		{
			if (_attackArea != null)
				_attackArea.OnTriggerEnter -= OnCollisionWithPlayer;
			
			if(_detectionArea != null)
			{
				_detectionArea.OnTriggerStay -= OnDetectionAreaStayHandler;
				_detectionArea.OnTriggerExit -= OnDetectionAreaExitHandler;
			}
		}

		public override void Attack(Player player)
		{
			player.SetDamage(Damage);
		}

		public void Stop()
		{
			_isChasePlayer = false;
			_target = null;
		}

		private void OnCollisionWithPlayer(Collider2D obj)
		{
			if (TryGetPlayerComponent(obj, out Player playerComponent))
				Attack(playerComponent);
		}

		private void OnDetectionAreaStayHandler(Collider2D obj)
		{
			if (!TryGetPlayerComponent(obj, out Player playerComponent))
				return;
			
			_isChasePlayer = true;
			_target = playerComponent;
		}

		private void OnDetectionAreaExitHandler(Collider2D obj)
		{
			if (!TryGetPlayerComponent(obj, out _))
				return;
			
			_isChasePlayer = false;
			_target = null;
		}
		
		private static bool TryGetPlayerComponent(Collider2D obj, out Player playerComponent)
		{
			playerComponent = obj.GetComponentInParent<Player>() ?? obj.GetComponentInChildren<Player>();
			return playerComponent != null;
		}
	}
}