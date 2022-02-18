using System;
using TestProject.Characters;
using TMPro;
using UnityEngine;

namespace TestProject.View
{
	public abstract class PlayerInventoryCounters : MonoBehaviour
	{
		[SerializeField] protected TMP_Text Text;
		[SerializeField] protected Player Player;
		
		protected void Awake()
		{
			if (Text == null)
				Debug.LogWarning($"[{nameof(PlayerItemsCountView)}] {nameof(Awake)}, field {nameof(Text)} is null!");
		}

		private void OnDestroy()
		{
			if(Player == null)
				return;

			OnObjectDestroy();
		}
		
		public void Initialize(Player player)
		{
			Player = player;
			OnInitialized();
		}

		public virtual void Reset(){}

		protected virtual void OnInitialized(){}

		protected virtual void OnObjectDestroy(){}
	}
}