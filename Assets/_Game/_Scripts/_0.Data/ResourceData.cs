using System.Collections.Generic;
using UnityEngine;
public enum ProductType {
    MechanicalEyeProduct,
    RoboticArmProduct,
    IronHeartProduct,
    NeurochipProduct
}
namespace Assets._Game._Scripts._0.Data
{
    [CreateAssetMenu(fileName = "ResourceData", menuName = "Data/ResourceData", order = 1)]
    public class ResourceData : ScriptableObject
    {
       
        public List<ProductInfo> ProductsInfo;
        public Sprite BaseIcon;
    }
}
