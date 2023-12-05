using System.Collections.Generic;

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