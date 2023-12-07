using Assets._Game._Scripts._0.Data._Base;
using Assets._Game._Scripts._6_Entities._Store;
using Assets._Game._Scripts._6_Entities._Units._Desktop;
using Assets._Game._Scripts._6_Entities._Units._PrebuilderDesktop;
using System;
using UnityEngine;

namespace Assets._Game._Scripts._5_Managers
{
    public class EconomyAndUpgradeService
    {
        public long Money => Store.Stats.Money;
        public int Level => Store.Stats.LevelStore;

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

        }

        public bool TryBuyPrebuilder(PrebuilderDesktop prebuilderDesktop)
        {
            ProductType productType = prebuilderDesktop.ProductType;
            int cost = prebuilderDesktop.Cost;
            if (cost <= Store.Stats.Money)
            {
                Store.Stats.RemoveMoney(cost);
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
            if (cost <= Store.Stats.Money)
            {
                desktop.UpgradeLevelUp();
                CheckDesktopAfterUpgrade(desktop);
                Store.Stats.RemoveMoney(cost);
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
            int income = GameMode.DataMode.GetProductUpgradeSO(productType).Upgrades[level].IncomeMoney;

            Store.Stats.AddMoney(income);
        }

        public long SetCostBuyProductAndLevel(int level, ProductType productType)
        {
            int cost = GameMode.DataMode.GetProductUpgradeSO(productType).Upgrades[level].Cost;


            return cost;
        }

        private void CheckDesktopAfterUpgrade(DesktopUnit desktop)
        {
            var eventData = GameMode.DataMode.GetProductUpgradeSO(desktop.ProductType).Upgrades[desktop.Level - 1]
                .Events;
            if (!string.IsNullOrWhiteSpace(eventData))
            {
                if (eventData == "Additional table opens") {
                    Debug.Log("Additional table opens");
                    GameMode.NeedOpenAdditionalDesktop(desktop);
                }
                
            }
          


        }
    }
}
