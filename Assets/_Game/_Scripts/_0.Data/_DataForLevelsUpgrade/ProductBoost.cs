using System;

namespace Assets._Game._Scripts._0.Data._DataForLevelsUpgrade {

    // Структура используется для определения Апгрейда магазина, также в сохранениях StoreStats для сохранения купленных улучшений для текущего уровня
    [Serializable]
    public class ProductBoost: IUpgradeItem {
        public string Name { get; set; }

        public float ProductMultiplier { get; set; }
        public int Price { get; set; }
        public bool IsPurchased { get; set; }


        public ProductBoost()
        {
        }

        public ProductBoost(string name,float productMultiplier, int price, bool isPurchased) {
            Name = name;
            ProductMultiplier = productMultiplier;
            Price = price;
            IsPurchased = isPurchased;
        }

        // Конструктор копирования
        public ProductBoost(ProductBoost other) {
            Name = other.Name;
            ProductMultiplier = other.ProductMultiplier;
            Price = other.Price;
            IsPurchased = other.IsPurchased;
        }
    }
}