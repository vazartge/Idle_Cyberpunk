using System;
using Assets._Game._Scripts._6_Entities._Store._Products;
using UnityEngine;

namespace Assets._Game._Scripts._6_Entities._Store
{
    [Serializable]
    public class PrebuilderStats {
        public Vector3 Position;
        public ProductStoreType ProductStoreType;
        public float RotationAngleZ;
        /*public bool IsActive;
        public bool IsDesktopPurchased;*/
        

        // Пустой приватный конструктор для Newtonsoft.Json
        private PrebuilderStats() { }

        public PrebuilderStats(Vector3 position, 
            ProductStoreType productStoreType, 
            float rotationAngleZ
            /*bool isActive,
            bool isDesktopPurchased,*/
            ) {
            ProductStoreType = productStoreType;
            Position = position;
            RotationAngleZ = rotationAngleZ;
            /*IsActive = isActive;
            IsDesktopPurchased = isDesktopPurchased;*/
            
        }
    }
}