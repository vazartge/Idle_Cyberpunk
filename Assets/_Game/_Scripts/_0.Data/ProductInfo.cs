using System;
using Assets._Game._Scripts._6_Entities._Store._Products;
using UnityEngine;

namespace Assets._Game._Scripts._0.Data
{
    // Используется в ResourceDataSO для хранения иконки для каждого типа продукта
    [Serializable]
    public class ProductInfo {
        public ProductStoreType ProductStoreType;
        public Sprite ProductIcon;
    }
}