using System;
using Assets._Game._Scripts._6_Entities._Store._Products;
using UnityEngine;

namespace Assets._Game._Scripts._6_Entities._Store
{
    [Serializable]
    public class PrebuilderStats {
        public ProductStoreType ProductStoreType;
        public float RotationAngleZ;
        public bool IsActive;
        public bool IsDesktopPurchased;
        //public Vector3 Position;

        // Пустой приватный конструктор для Newtonsoft.Json
        private PrebuilderStats() { }

        public PrebuilderStats(ProductStoreType productStoreType, float rotationAngleZ, bool isActive, bool isDesktopPurchased/*, Vector3 position*/) {
            ProductStoreType = productStoreType;
            RotationAngleZ = rotationAngleZ;
            IsActive = isActive;
            IsDesktopPurchased = isDesktopPurchased;
            //Position = position;
        }
    }
}