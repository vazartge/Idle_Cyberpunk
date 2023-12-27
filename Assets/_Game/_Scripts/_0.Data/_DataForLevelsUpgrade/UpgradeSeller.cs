using System;

namespace Assets._Game._Scripts._0.Data._DataForLevelsUpgrade
{
    // Структура используется для определения Апгрейда магазина, также в сохранениях StoreStats для сохранения купленных улучшений для текущего уровня
    [Serializable]
    public class UpgradeSeller: IUpgradeItem {
        public string Name { get; set; }
        public int Amount { get; set; }
        public int Price { get; set; }
        public bool IsPurchased { get; set; }

        public UpgradeSeller()
        {

        }
        public UpgradeSeller(string name, int amount, int price, bool isPurchased)
        {
            Name = name;
            Amount = amount;
            Price = price;
            IsPurchased = isPurchased;

        }

        public UpgradeSeller(UpgradeSeller other)
        {
            Name = other.Name;
            Amount = other.Amount;
            Price = other.Price;
            IsPurchased = other.IsPurchased;
        }
    }
}

