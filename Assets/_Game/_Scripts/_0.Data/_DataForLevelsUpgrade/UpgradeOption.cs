namespace Assets._Game._Scripts._0.Data._DataForLevelsUpgrade
{
    [System.Serializable]
    public class UpgradeOption
    {
        public int Amount { get; set; }
        public int Price { get; set; }

        public UpgradeOption(int amount, int price)
        {
            Amount = amount;
            Price = price;
        }

    }
}

