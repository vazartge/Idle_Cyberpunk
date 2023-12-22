﻿using Assets._Game._Scripts._5_Managers;
using Assets._Game._Scripts._6_Entities._Store._Products;

namespace Assets._Game._Scripts._4_Services
{
    [System.Serializable]
    public class ProductWeight {
        public ProductStoreType ProductStoreType;
        public float Weight; // Массив весов для каждого уровня
        public ProductWeight(ProductStoreType productStore, float weight) {
            ProductStoreType = productStore;
            Weight = weight;
        }
    }
}