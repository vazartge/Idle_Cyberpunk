using System;
using System.Collections.Generic;
using System.Linq;

namespace Assets._Game._Scripts._0.Data._DataForLevelsUpgrade
{
    // Структура используется для определения Апгрейда магазина, также в сохранениях StoreStats для сохранения купленных улучшений для текущего уровня
    [Serializable]
    public class LevelUpgrade {
        public List<UpgradeCustomer> Customers { get; set; }
        public List<UpgradeSeller> Sellers { get; set; }
        public ProductBoost ProductionBoost { get; set; }
        public SpeedBoost SellersSpeedIncrease { get; set; }


        public LevelUpgrade()
        {
        }

        // Существующий конструктор
        public LevelUpgrade(List<UpgradeCustomer> customers, List<UpgradeSeller> sellers, ProductBoost productionBoost,
            SpeedBoost sellersSpeedIncrease) {
            Customers = customers;
            Sellers = sellers;
            ProductionBoost = productionBoost;
            SellersSpeedIncrease = sellersSpeedIncrease;
        }

        // Конструктор копирования
        public LevelUpgrade(LevelUpgrade other) {
            Customers = new List<UpgradeCustomer>(other.Customers); // Создание копии списка
            Sellers = new List<UpgradeSeller>(other.Sellers); // Создание копии списка
            ProductionBoost = new ProductBoost(other.ProductionBoost); // Предполагается, что у ProductBoost есть конструктор копирования
            SellersSpeedIncrease = new SpeedBoost(other.SellersSpeedIncrease); // Предполагается, что у SpeedBoost есть конструктор копирования
        }
        public List<IUpgradeItem> GetAllUnpurchasedUpgrades() {
            var upgrades = new List<IUpgradeItem>();

            upgrades.AddRange(Customers.Where(b => !b.IsPurchased));
            upgrades.AddRange(Sellers.Where(s => !s.IsPurchased));
            if (!ProductionBoost.IsPurchased) upgrades.Add(ProductionBoost);
            if (!SellersSpeedIncrease.IsPurchased) upgrades.Add(SellersSpeedIncrease);

            return upgrades;
        }
    }
}