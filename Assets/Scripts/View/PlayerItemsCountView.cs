namespace TestProject.View
{
	public sealed class PlayerItemsCountView : PlayerInventoryCounters
	{
		protected override void OnInitialized()
		{
			Text.text = 0.ToString();
			Player.Inventory.OnItemsCountChanged += OnPlayerInventoryItemCountChangedHandler;
		}

		protected override void OnObjectDestroy()
		{
			Player.Inventory.OnItemsCountChanged -= OnPlayerInventoryItemCountChangedHandler;
		}

		private void OnPlayerInventoryItemCountChangedHandler(int before, int after) => Text.text = after.ToString();
	}
}