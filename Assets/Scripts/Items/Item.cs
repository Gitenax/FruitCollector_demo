using System;
using TestProject.Characters;
using TestProject.Data;
using TestProject.Interact;
using UnityEngine;

namespace TestProject.Resources
{
	public class Item : MonoBehaviour
	{
		[SerializeField] private ItemData _itemData;
		[SerializeField] private ColliderTrigger2D _itemCollider;

		public event Action<Item> OnItemPicked;

		public ItemData ItemData => _itemData;
		
		private void Awake()
		{
			if (_itemCollider != null)
				return;
			
			Debug.LogWarning($"[{nameof(Item)}] {nameof(Awake)}, field {nameof(_itemCollider)} is null!");
		}

		private void OnEnable() => EnableItemInteraction();

		private void OnDisable() => DisableItemInteraction();

		private void OnDestroy() => DisableItemInteraction();

		private void EnableItemInteraction()
		{
			if(_itemCollider != null)
				_itemCollider.OnTriggerEnter += OnItemColliderTriggerEnterHandler;
		}
		
		private void DisableItemInteraction()
		{
			if(_itemCollider != null)
				_itemCollider.OnTriggerEnter -= OnItemColliderTriggerEnterHandler;
		}

		private void OnItemColliderTriggerEnterHandler(Collider2D other)
		{
			Player playerComponents = other.GetComponentInParent<Player>() ?? other.GetComponentInChildren<Player>();
			
			if(playerComponents == null)
				return;
			
			OnItemPicked?.Invoke(this);
		}
	}
}