namespace Assets._Game._Scripts._0.Data._DataForLevelsUpgrade {
    [System.Serializable]
    public class UpgradeCustomer : IUpgradeItem {
        public string Name { get; set; }
        public int Amount { get; set; }
        public int Price { get; set; }
        public bool IsPurchased { get; set; }
        public UpgradeCustomer(string name, int amount, int price, bool isPurchased) {
            Name = name;
            Amount = amount;
            Price = price;
            IsPurchased = isPurchased;

        }

        public UpgradeCustomer(UpgradeCustomer other) {
            Name = other.Name;
            Amount = other.Amount;
            Price = other.Price;
            IsPurchased = other.IsPurchased;
        }
    }
}