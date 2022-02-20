using System;
using UnityEngine;

namespace TestProject.Interact
{
    [RequireComponent(typeof(Collider2D))]
    public sealed class ColliderTrigger2D : MonoBehaviour
    {
        public event Action<Collider2D> OnTriggerEnter;
        public event Action<Collider2D> OnTriggerStay;
        public event Action<Collider2D> OnTriggerExit;
        
        public Collider2D Collider { get; private set; }

        private void Awake()
        {
            Collider = GetComponent<Collider2D>();
            Collider.isTrigger = true;
        }

        private void OnTriggerEnter2D(Collider2D other) => OnTriggerEnter?.Invoke(other);

        private void OnTriggerStay2D(Collider2D other) => OnTriggerStay?.Invoke(other);
        
        private void OnTriggerExit2D(Collider2D other) => OnTriggerExit?.Invoke(other);
    }
}