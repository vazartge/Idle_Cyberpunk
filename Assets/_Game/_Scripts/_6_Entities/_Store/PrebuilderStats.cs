using System;
using Assets._Game._Scripts._6_Entities._Store._Products;
using UnityEngine;

namespace Assets._Game._Scripts._6_Entities._Store
{
    [Serializable]
    public class PrebuilderStats {
        public ProductType ProductType;
        public float RotationAngleZ;
        public bool IsActive;
        public bool IsDesktopPurchased;
        //public Vector3 Position;

        // Пустой приватный конструктор для Newtonsoft.Json
        private PrebuilderStats() { }

        public PrebuilderStats(ProductType productType, float rotationAngleZ, bool isActive, bool isDesktopPurchased/*, Vector3 position*/) {
            ProductType = productType;
            RotationAngleZ = rotationAngleZ;
            IsActive = isActive;
            IsDesktopPurchased = isDesktopPurchased;
            //Position = position;
        }
    }
}