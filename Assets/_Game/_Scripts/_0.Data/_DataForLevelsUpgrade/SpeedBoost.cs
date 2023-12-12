namespace Assets._Game._Scripts._0.Data._DataForLevelsUpgrade {
    public class SpeedBoost: IUpgradeItem {

        public string Name { get; set; }
        public float SpeedMultiplier { get; set; }
        public int Price { get; set; }
        public bool IsPurchased { get; set; }

        public SpeedBoost()
        {
        }

        public SpeedBoost(string name ,float speedMultiplier, int price, bool isPurchased) {
            Name = name;
            SpeedMultiplier = speedMultiplier;
            Price = price;
            IsPurchased = isPurchased;
        }

        // Конструктор копирования
        public SpeedBoost(SpeedBoost other) {
            Name = other.Name;
            SpeedMultiplier = other.SpeedMultiplier;
            Price = other.Price;
            IsPurchased = other.IsPurchased;
        }
    }
}