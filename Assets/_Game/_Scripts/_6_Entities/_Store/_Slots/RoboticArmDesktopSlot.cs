using System;
using Assets._Game._Scripts._6_Entities._Store._Products;

namespace Assets._Game._Scripts._6_Entities._Store._Slots
{
    class RoboticArmDesktopSlot : DesktopSlot
    {
        // Тип продукта, который может быть обработан на этом столе
        public override Type AllowedProductType {
            get { return typeof(RoboticArmProduct); }
        }
    }
}