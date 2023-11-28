using System.Collections.Generic;
using UnityEngine;

namespace Assets._Game._Scripts._0.Data
{
    public enum ProductType {
        MechanicalEyeProduct,
        RoboticArmProduct,
        IronHeartProduct,
        NeurochipProduct
    }

    [CreateAssetMenu(fileName = "ResourceData", menuName = "Data/ResourceData", order = 1)]
    public class ResourceData : ScriptableObject
    {
       
        public List<ProductInfo> ProductsInfo;
        public Sprite BaseIcon;
    }
}