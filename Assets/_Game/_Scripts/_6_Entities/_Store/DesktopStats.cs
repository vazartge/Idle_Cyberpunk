using Assets._Game._Scripts._6_Entities._Store._Products;
using System;
using UnityEngine;

namespace Assets._Game._Scripts._6_Entities._Store {
    [Serializable]
    public class DesktopStats {
        public Vector3 Position;
        public ProductType ProductType;
        public int UpgradeLevel;
        public bool IsMainDesktop;
        public bool IsAdditionalDesktop;
        public bool IsUpgradedForLevel;

        public DesktopStats(){}
        public DesktopStats(Vector3 position, ProductType productType, int upgradeLevel, bool isMainDesktop, bool isAdditionalDesktop, bool isUpgradedForLevel) {
            Position = position;
            ProductType = productType;
            UpgradeLevel = upgradeLevel;
            IsMainDesktop = isMainDesktop;
            IsAdditionalDesktop = isAdditionalDesktop;
            IsUpgradedForLevel = isUpgradedForLevel;
        }
    }

}
