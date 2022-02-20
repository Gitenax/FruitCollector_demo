namespace TestProject.UI
{
    public sealed class PlayerItemsCountView : PlayerInventoryCounters
    {
        protected override void OnInitialized()
        {
            Text.text = "0";
            ScoreCounter.OnPickedItemsChanged += OnPickedItemsChangedHandler;
        }

        protected override void OnObjectDestroy()
        {
            ScoreCounter.OnPickedItemsChanged -= OnPickedItemsChangedHandler;
        }

        private void OnPickedItemsChangedHandler(int value)
        {
            Text.text = value.ToString();
        }
    }
}