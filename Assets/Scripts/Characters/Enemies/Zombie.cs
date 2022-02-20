using System.Collections;
using TestProject.Interact;
using UnityEngine;
using UnityEngine.AI;

namespace TestProject.Characters.Enemies
{
    [RequireComponent(typeof(NavMeshAgent))]
    public sealed class Zombie : Enemy
    {
        [SerializeField] private ColliderTrigger2D _detectionArea;
        [SerializeField] private ColliderTrigger2D _attackArea;
        private NavMeshAgent _navMeshAgent;
        private Player _target;
        private bool _isChasePlayer;
        private bool _isAttackPlayer;

        private void Awake()
        {
            _navMeshAgent = GetComponent<NavMeshAgent>();
            _navMeshAgent.updateRotation = false;
            _navMeshAgent.updateUpAxis = false;

            if (_attackArea == null)
            {
                Debug.LogWarning($"[{nameof(Zombie)}] {nameof(Awake)}, field {nameof(Collider)} is null!");
                return;
            }
            _attackArea.OnTriggerEnter += OnCollisionWithPlayer;
            _attackArea.OnTriggerExit += OnCollisionWithPlayerEnd;

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

            if (_detectionArea != null)
            {
                _detectionArea.OnTriggerStay -= OnDetectionAreaStayHandler;
                _detectionArea.OnTriggerExit -= OnDetectionAreaExitHandler;
            }
        }

        public override void Attack(Player player)
        {
            player.SetDamage(Damage);
        }

        public override void Stop()
        {
            _isChasePlayer = false;
            _isAttackPlayer = false;
            _target = default;
        }

        private static bool TryGetPlayerComponent(Collider2D obj, out Player playerComponent)
        {
            playerComponent = obj.GetComponentInParent<Player>() ?? obj.GetComponentInChildren<Player>();
            return playerComponent != null;
        }

        private void OnCollisionWithPlayer(Collider2D obj)
        {
            if (!TryGetPlayerComponent(obj, out Player playerComponent))
                return;
            
            Attack(playerComponent);
            _isAttackPlayer = true;
            StartCoroutine(nameof(TickDamage));
        }

        private void OnCollisionWithPlayerEnd(Collider2D obj)
        {
            _isAttackPlayer = false;
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

            Stop();
        }

        private IEnumerator TickDamage()
        {
            const float cooldown = 1f;

            while (_isAttackPlayer)
            {
                yield return new WaitForSeconds(cooldown);
                Attack(_target);
            }
        }
    }
}