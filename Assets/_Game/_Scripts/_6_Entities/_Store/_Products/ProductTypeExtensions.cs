using System.Collections.Generic;

namespace Assets._Game._Scripts._6_Entities._Store._Products
{
    public enum ProductStoreType {
        MechanicalEyeProduct,
        RoboticArmProduct,
        IronHeartProduct,
        NeurochipProduct
    }

    public static class ProductTypeExtensions {
        private static readonly Dictionary<ProductStoreType, int> Priorities = new Dictionary<ProductStoreType, int> {
            { ProductStoreType.MechanicalEyeProduct, 1 },
            { ProductStoreType.RoboticArmProduct, 2 },
            { ProductStoreType.IronHeartProduct, 3 },
            { ProductStoreType.NeurochipProduct, 4 }
        };

        public static int GetPriority(this ProductStoreType productStoreType) {
            return Priorities[productStoreType];
        }
    }
}