using Assets._Game._Scripts._0.Data._Base;
using Assets._Game._Scripts._6_Entities._Store;
using Assets._Game._Scripts._6_Entities._Units._Desktop;
using Assets._Game._Scripts._6_Entities._Units._PrebuilderDesktop;
using System;
using Assets._Game._Scripts._0.Data._DataForLevelsUpgrade;
using Assets._Game._Scripts._6_Entities._Store._Products;
using UnityEngine;
using Assets._Game._Scripts._2_Game;

namespace Assets._Game._Scripts._5_Managers
{
    public class EconomyAndUpgradeService
    {
        public long Coins
        {
            get => Game.Instance.StoreStats.GameStats.Coins;
            set => Game.Instance.StoreStats.GameStats.Coins = value;
        }

        public int LevelGame
        {
            get => Game.Instance.StoreStats.GameStats.LevelGame;
            set => Game.Instance.StoreStats.GameStats.LevelGame = value;
        }

        public GameMode GameMode
        {
            get => _gameMode;
            set => _gameMode = value;
        }

        public Store Store;
        private GameMode _gameMode;
        private BaseDataForUpgrade _baseUpgradeSo;
        public bool IsBoostedFromRewarded;


        public EconomyAndUpgradeService(GameMode gameMode, Store store)
        {
            GameMode = gameMode;
            Store = store;
            //_gameMode.UpdateOnChangedStatsOrMoney();
            //_gameMode.InitializeComponents();
        }

        public bool TryBuyPrebuilder(PrebuilderDesktop prebuilderDesktop)
        {
            ProductStoreType productStoreType = prebuilderDesktop.ProductStoreType;
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
            ProductStoreType productStoreType = desktop.ProductStoreType;
            int cost = GameMode.DataMode.GetProductUpgradeSO(productStoreType).Upgrades[level].Cost;
            if (cost <= Coins)
            {
                desktop.UpgradeLevelUp(1);
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
            ProductStoreType productStoreType = desktop.ProductStoreType;
            int income = GameMode.DataMode.GetProductUpgradeSO(productStoreType).Upgrades[level-1].IncomeMoney;
            if (Game.Instance.StoreStats.GameStats.PurchasedIncreaseProfit)
            {
                income *= 2;
            }

            if (IsBoostedFromRewarded)
            {
                income *= 2;
            }
            AddMoney(income);
        }

        public long SetCostBuyProductAndLevel(int level, ProductStoreType productStoreType)
        {
            int cost = GameMode.DataMode.GetProductUpgradeSO(productStoreType).Upgrades[level].Cost;


            return cost;
        }

        public void CheckDesktopAfterUpgrade(DesktopUnit desktop)
        {
            var eventData = GameMode.DataMode.GetProductUpgradeSO(desktop.ProductStoreType).Upgrades[desktop.Level-1]
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
            _gameMode.UpdateOnChangedStatsOrMoney();
            return true;
        }

        public bool RemoveMoney(long amount) {
            if (Coins >= 0) {
                Coins -= amount;
                _gameMode.UpdateOnChangedStatsOrMoney();
                return true;
            }
            return false;
        }

        public bool AddLevelGame() {
            // _gameMode.ChangeLevel();

            _gameMode.UpdateOnChangedStatsOrMoney();
            return true;
        }

        public void OnBuyUpgradeSeller(UpgradeSeller upgradeSeller)
        {
            if (upgradeSeller.Price <= Coins && upgradeSeller.IsPurchased != true)
            {
                RemoveMoney(upgradeSeller.Price);
                upgradeSeller.IsPurchased = true;
                GameMode.AddSeller();
                _gameMode.UpdateOnChangedStatsOrMoney();
                Debug.Log("покупка продавца в сервисе");
            }
        }
        public void OnBuyUpgradeCustomer(UpgradeCustomer upgradeCustomer)
        {
            if (upgradeCustomer.Price <= Coins && upgradeCustomer.IsPurchased != true) {
                RemoveMoney(upgradeCustomer.Price);
                upgradeCustomer.IsPurchased = true;
                GameMode.AddCustomer();
                _gameMode.UpdateOnChangedStatsOrMoney();
            }
        }
        public void OnBuyUpgradeProductionBoost(ProductBoost productBoost)
        {
            if (productBoost.Price <= Coins && productBoost.IsPurchased != true) {
                RemoveMoney(productBoost.Price);
                productBoost.IsPurchased = true;
                Game.Instance.StoreStats.GameStats.ProductionSpeed *= productBoost.ProductMultiplier;
                _gameMode.UpdateOnChangedStatsOrMoney();
            }
        }

        public void OnBuyUpgradeSpeedBoost(SpeedBoost speedBoost) {
            if (speedBoost.Price <= Coins && speedBoost.IsPurchased != true) {
                RemoveMoney(speedBoost.Price);
                speedBoost.IsPurchased = true;
                Game.Instance.StoreStats.GameStats.SpeedMoveSeller *= speedBoost.SpeedMultiplier;
                _gameMode.UpdateOnChangedStatsOrMoney();
            }
        }


        
    }
}
