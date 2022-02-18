using TestProject.Common;
using TestProject.Interact;
using UnityEngine;

namespace TestProject.Characters.Enemies
{
	public abstract class Enemy : MonoBehaviour
	{
		[SerializeField] protected ColliderTrigger2D Collider;
		[SerializeField] protected DamageInfo Damage;
		
		public abstract void Attack(Player player);
	}
}