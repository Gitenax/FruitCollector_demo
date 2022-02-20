namespace TestProject.Common
{
    [System.Serializable]
    public sealed class DamageInfo
    {
        [UnityEngine.SerializeField] private int _amount;

        public int Amount => _amount;
    }
}