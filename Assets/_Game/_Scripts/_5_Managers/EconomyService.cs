using System;
using Assets._Game._Scripts._0.Data;
using Assets._Game._Scripts._6_Entities._Store;
using Assets._Game._Scripts._6_Entities._Units._Desktop;
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
            _mechanicalEyeUpgradeSo = _gameMode._mechanicalEyeUpgradeSo;
        }

        public void BuyDesktop(DesktopUnit desktop)
        {

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
