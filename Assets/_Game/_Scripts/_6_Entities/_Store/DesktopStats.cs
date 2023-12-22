using Assets._Game._Scripts._6_Entities._Store._Products;
using System;
using Assets._Game._Scripts._6_Entities._Units._Desktop;
using UnityEngine;

namespace Assets._Game._Scripts._6_Entities._Store {
    [Serializable]
    public class DesktopStats {
        //public Vector3 Position;
        public ProductStoreType ProductStoreType;
        public int UpgradeLevel;
        public DesktopType DesktopType;
        public bool IsAdditionalDesktop;
        public bool IsUpgradedForLevel;

        public DesktopStats(){}
        public DesktopStats(/*Vector3 position,*/ ProductStoreType productStoreType, int upgradeLevel, DesktopType desktopType, bool isAdditionalDesktop, bool isUpgradedForLevel) {
           // Position = position;
            ProductStoreType = productStoreType;
            UpgradeLevel = upgradeLevel;
            DesktopType = desktopType;
            IsAdditionalDesktop = isAdditionalDesktop;
            IsUpgradedForLevel = isUpgradedForLevel;
        }
    }

}
