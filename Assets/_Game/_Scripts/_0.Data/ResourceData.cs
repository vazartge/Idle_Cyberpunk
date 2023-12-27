using System.Collections.Generic;
using UnityEngine;

namespace Assets._Game._Scripts._0.Data
{

    // Используется для хранения иконки для каждого типа продукта
    [CreateAssetMenu(fileName = "ResourceData", menuName = "ResData/ResourceData")]
    public class ResourceData : ScriptableObject
    {
       
        public List<ProductInfo> ProductsInfo;
        public Sprite BaseIcon;
    }
}