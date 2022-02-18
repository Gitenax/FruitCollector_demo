using System.Collections;
using TestProject.Resources;
using UnityEngine;

namespace TestProject.View
{
	public sealed class ScoreCounterView : PlayerInventoryCounters
	{
		private const float CountDelay = 0.001f;
			
		private int _score;
		
		protected override void OnInitialized()
		{
			_score = 0;
			Text.text = 0.ToString();
			Player.Inventory.OnItemAdded += OnPlayerInventoryItemAddedHandler;
		}

		protected override void OnObjectDestroy()
		{
			Player.Inventory.OnItemAdded -= OnPlayerInventoryItemAddedHandler;
		}
		
		private void OnPlayerInventoryItemAddedHandler(Item item)
		{
			_score += item.ItemData.Score;
			StartCoroutine(nameof(Countup));
		}

		private IEnumerator Countup()
		{
			int scoreFrom = int.Parse(Text.text);
			
			while (scoreFrom != _score)
			{
				scoreFrom++;
				Text.text = scoreFrom.ToString();
				yield return new WaitForSeconds(CountDelay);
			}
		}
	}
}