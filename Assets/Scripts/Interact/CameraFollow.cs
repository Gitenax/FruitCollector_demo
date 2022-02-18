using TestProject.Characters;
using UnityEngine;

namespace TestProject.Interact
{
    public class CameraFollow : MonoBehaviour
    {
        [SerializeField] private Transform _target;
        [SerializeField] private float _lerpSpeed = 1.0f;
        private Vector3 _offset;
        private Vector3 _targetPos;

        private void Update()
        {
            if (_target == null) 
                return;

            _targetPos = _target.position + _offset;
            transform.position = Vector3.Lerp(transform.position, _targetPos, _lerpSpeed * Time.deltaTime);
        }

        public void Initialize(Player target)
        {
            _target = target.transform;
            transform.position = new Vector3(_target.transform.position.x, _target.transform.position.y, transform.position.z);
            _offset = transform.position - _target.position;
        }
    }
}
