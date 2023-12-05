using Assets._Game._Scripts._0.Data._Base;
using Assets._Game._Scripts._6_Entities._Store;
using Assets._Game._Scripts._6_Entities._Units._Desktop;
using Assets._Game._Scripts._6_Entities._Units._PrebuilderDesktop;
using UnityEditor;
using UnityEngine;

namespace Assets._Game._Scripts._5_Managers {
    public class EconomyService {
        public long Money => Store.Stats.Money;
        public int Level => Store.Stats.Level;
        public Store Store;
        private GameMode _gameMode;
        private BaseDataForUpgrade _baseUpgradeSo;



        public EconomyService(GameMode gameMode, Store store) {
            _gameMode = gameMode;
            Store = store;
           
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
            int cost = _gameMode.DataMode.GetProductUpgradeSO(productType).Upgrades[level].Cost;
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
            int income = _gameMode.DataMode.GetProductUpgradeSO(productType).Upgrades[level].IncomeMoney;
            // int income; 
            // switch (productType) {
            //     case ProductType.MechanicalEyeProduct:
            //         // productType = ProductType.MechanicalEyeProduct;
            //         income = _gameMode.DataMode._mechanicalEyeUpgradeSo.Upgrades[level-1].Cost;
            //         break;
            //     case ProductType.RoboticArmProduct:
            //         //productType = ProductType.RoboticArmProduct;
            //         // доделать остальные
            //         income = _gameMode.DataMode._roboticArmUpgradeSO.Upgrades[level-1].Cost;
            //         break;
            //     case ProductType.IronHeartProduct:
            //         //productType = ProductType.IronHeartProduct;
            //         // доделать остальные
            //         income = _gameMode.DataMode._ironHeartUpgradeSO.Upgrades[level-1].Cost;
            //         break;
            //     case ProductType.NeurochipProduct:
            //         // productType = ProductType.NeurochipProduct;
            //         // доделать остальные
            //         income = _gameMode.DataMode._neurochipUpgradeSo.Upgrades[level-1].Cost;
            //         break;
            //     default:
            //         Debug.Log(" Нет такого типа продукта");
            //         income = -1;
            //         break;
            // }
            Store.Stats.AddMoney(income);
        }

        public long SetCostBuyProductAndLevel(int level, ProductType productType)
        {
            int cost = _gameMode.DataMode.GetProductUpgradeSO(productType).Upgrades[level].Cost;
            // long cost; 
            //
            // switch (productType)
            // {
            //     case ProductType.MechanicalEyeProduct:
            //        // productType = ProductType.MechanicalEyeProduct;
            //         cost = _gameMode.DataMode._mechanicalEyeUpgradeSo.Upgrades[level-1].Cost;
            //         break;
            //     case ProductType.RoboticArmProduct:
            //         //productType = ProductType.RoboticArmProduct;
            //         // доделать остальные
            //         cost = _gameMode.DataMode._roboticArmUpgradeSO.Upgrades[level-1].Cost;
            //         break;
            //     case ProductType.IronHeartProduct:
            //         //productType = ProductType.IronHeartProduct;
            //         // доделать остальные
            //         cost = _gameMode.DataMode._ironHeartUpgradeSO.Upgrades[level-1].Cost;
            //         break;
            //     case ProductType.NeurochipProduct:
            //         // productType = ProductType.NeurochipProduct;
            //         // доделать остальные
            //         cost = _gameMode.DataMode._neurochipUpgradeSo.Upgrades[level-1].Cost;
            //         break;
            //     default:
            //         Debug.Log(" Нет такого типа продукта");
            //         cost = -1;
            //         break;
            // }

            return cost;
        }


     
    }
}
