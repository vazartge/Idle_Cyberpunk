using System.Collections.Generic;
using Assets._Game._Scripts._5_Managers;

namespace Assets._Game._Scripts._6_Entities._Store._Products
{
    public static class ProductTypeExtensions {
        private static readonly Dictionary<ProductType, int> Priorities = new Dictionary<ProductType, int> {
            { ProductType.MechanicalEyeProduct, 1 },
            { ProductType.RoboticArmProduct, 2 },
            { ProductType.IronHeartProduct, 3 },
            { ProductType.NeurochipProduct, 4 }
        };

        public static int GetPriority(this ProductType productType) {
            return Priorities[productType];
        }
    }
}