using Assets._Game._Scripts._0.Data._Base;
using Assets._Game._Scripts._6_Entities._Store;
using Assets._Game._Scripts._6_Entities._Units._Desktop;
using Assets._Game._Scripts._6_Entities._Units._PrebuilderDesktop;
using System;
using Assets._Game._Scripts._0.Data._DataForLevelsUpgrade;
using Assets._Game._Scripts._6_Entities._Store._Products;
using UnityEngine;

namespace Assets._Game._Scripts._5_Managers
{
    public class EconomyAndUpgradeService
    {
        public long Coins
        {
            get => Store.Stats.Coins;
            set => Store.Stats.Coins = value;
        }

        public int LevelGame
        {
            get => Store.Stats.LevelGame;
            set => Store.Stats.LevelGame = value;
        }

        public GameMode GameMode
        {
            get => _gameMode;
            set => _gameMode = value;
        }

        public Store Store;
        private GameMode _gameMode;
        private BaseDataForUpgrade _baseUpgradeSo;



        public EconomyAndUpgradeService(GameMode gameMode, Store store)
        {
            GameMode = gameMode;
            Store = store;
            //_gameMode.ChangedStatsOrMoney();
            _gameMode.InitializeComponents();
        }

        public bool TryBuyPrebuilder(PrebuilderDesktop prebuilderDesktop)
        {
            ProductType productType = prebuilderDesktop.ProductType;
            int cost = prebuilderDesktop.Cost;
            if (cost <= Coins)
            {
                RemoveMoney(cost);
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool TryUpgradeDesktop(DesktopUnit desktop)
        {
            int level = desktop.Level;
            ProductType productType = desktop.ProductType;
            int cost = GameMode.DataMode.GetProductUpgradeSO(productType).Upgrades[level].Cost;
            if (cost <= Coins)
            {
                desktop.UpgradeLevelUp();
                CheckDesktopAfterUpgrade(desktop);
                RemoveMoney(cost);
                return true;
            }
            else
            {
                return false;
            }
        }

        public void SellProductByStore(DesktopUnit desktop)
        {
            int level = desktop.Level;
            ProductType productType = desktop.ProductType;
            int income = GameMode.DataMode.GetProductUpgradeSO(productType).Upgrades[level-1].IncomeMoney;

            AddMoney(income);
        }

        public long SetCostBuyProductAndLevel(int level, ProductType productType)
        {
            int cost = GameMode.DataMode.GetProductUpgradeSO(productType).Upgrades[level].Cost;


            return cost;
        }

        private void CheckDesktopAfterUpgrade(DesktopUnit desktop)
        {
            var eventData = GameMode.DataMode.GetProductUpgradeSO(desktop.ProductType).Upgrades[desktop.Level-1]
                .Events;
            if (!string.IsNullOrWhiteSpace(eventData))
            {
                if (eventData == "Additional table opens") {
                    Debug.Log("Additional table opens");
                    GameMode.NeedOpenAdditionalDesktop(desktop);
                }
                
            }
          


        }
        public bool AddMoney(long amount) {
            Coins += amount;
            _gameMode.ChangedStatsOrMoney();
            return true;
        }

        public bool RemoveMoney(long amount) {
            if (Coins >= 0) {
                Coins -= amount;
                _gameMode.ChangedStatsOrMoney();
                return true;
            }
            return false;
        }

        public bool AddLevelGame() {
            // _gameMode.ChangeLevel();

            _gameMode.ChangedStatsOrMoney();
            return true;
        }

        public void OnBuyUpgradeSeller(UpgradeSeller upgradeSeller)
        {
            if (upgradeSeller.Price <= Coins && upgradeSeller.IsPurchased != true)
            {
                RemoveMoney(upgradeSeller.Price);
                upgradeSeller.IsPurchased = true;
                GameMode.AddSeller();
                _gameMode.ChangedStatsOrMoney();
                Debug.Log("покупка продавца в сервисе");
            }
        }
        public void OnBuyUpgradeCustomer(UpgradeCustomer upgradeCustomer)
        {
            if (upgradeCustomer.Price <= Coins && upgradeCustomer.IsPurchased != true) {
                RemoveMoney(upgradeCustomer.Price);
                upgradeCustomer.IsPurchased = true;
                GameMode.AddCustomer();
                _gameMode.ChangedStatsOrMoney();
            }
        }
        public void OnBuyUpgradeProductionBoost(ProductBoost productBoost)
        {
            if (productBoost.Price <= Coins && productBoost.IsPurchased != true) {
                RemoveMoney(productBoost.Price);
                productBoost.IsPurchased = true;
                Store.Stats.ProductionSpeed *= productBoost.ProductMultiplier;
                _gameMode.ChangedStatsOrMoney();
            }
        }

        public void OnBuyUpgradeSpeedBoost(SpeedBoost speedBoost) {
            if (speedBoost.Price <= Coins && speedBoost.IsPurchased != true) {
                RemoveMoney(speedBoost.Price);
                speedBoost.IsPurchased = true;
                Store.Stats.SpeedMoveSeller *= speedBoost.SpeedMultiplier;
                _gameMode.ChangedStatsOrMoney();
            }
        }


        
    }
}
