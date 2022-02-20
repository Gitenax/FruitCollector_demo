using UnityEngine;
using UnityEngine.EventSystems;

namespace TestProject
{
    [RequireComponent(typeof(EventSystem))]
    public sealed class UserInput : MonoBehaviour
    {
        private EventSystem _eventSystem;

        public bool LeftMouseClick { get; private set; }

        private void Awake()
        {
            _eventSystem = GetComponent<EventSystem>();
        }

        private void Update()
        {
            LeftMouseClick = Input.GetMouseButtonDown(0);
        }
    }
}