using System;
using Assets._Game._Scripts._0.Data;
using Assets._Game._Scripts._6_Entities._Store;
using Assets._Game._Scripts._6_Entities._Units._Desktop;
using Assets._Game._Scripts._6_Entities._Units._PrebuilderDesktop;
using UnityEngine;

namespace Assets._Game._Scripts._5_Managers {
    public class EconomyService {
        public long Money => Store.Stats.Money;
        public int Level => Store.Stats.Level;
        public Store Store;
        private GameMode _gameMode;
        private MechanicalEyeUpgradeSO _mechanicalEyeUpgradeSo;



        public EconomyService(GameMode gameMode, Store store) {
            _gameMode = gameMode;
            Store = store;
            _mechanicalEyeUpgradeSo = _gameMode.DataMode.MechanicalEyeUpgradeSo;
        }
        public bool TryBuyPrebuilder(PrebuilderDesktop prebuilderDesktop) {
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
            int level =desktop.Level;
            ProductType productType = desktop.ProductType;
            int cost = _gameMode.DataMode.MechanicalEyeUpgradeSo.Upgrades[level].Cost;
            if ( cost <= Store.Stats.Money)
            {
                Store.Stats.RemoveMoney(cost);
                return true;
            }
            else
            {
                return false;
            }
        }
        public void SellProductByStore(DesktopUnit desktop) {
            int level = desktop.Level;
            ProductType productType = desktop.ProductType;
            int income = _gameMode.DataMode.MechanicalEyeUpgradeSo.Upgrades[level - 1].IncomeMoney;
            
            Store.Stats.AddMoney(income);
        }

        public long SetCostBuyProductAndLevel(int level, ProductType productType)
        {
            long cost;
           
            switch (productType)
            {
                case ProductType.MechanicalEyeProduct:
                   // productType = ProductType.MechanicalEyeProduct;
                    cost = _mechanicalEyeUpgradeSo.Upgrades[level-1].Cost;
                    break;
                case ProductType.RoboticArmProduct:
                    //productType = ProductType.RoboticArmProduct;
                    // доделать остальные
                    cost = -1;
                    break;
                case ProductType.IronHeartProduct:
                    //productType = ProductType.IronHeartProduct;
                    // доделать остальные
                    cost = -1;
                    break;
                case ProductType.NeurochipProduct:
                    // productType = ProductType.NeurochipProduct;
                    // доделать остальные
                    cost = -1;
                    break;
                default:
                    Debug.LogWarning(" Нет такого типа продукта");
                    cost = -1;
                    break;
            }

            return cost;
        }


     
    }
}
