using Assets._Game._Scripts._6_Entities._Units._Sellers;

namespace Assets._Game._Scripts._6_Entities._Store._Slots
{
    public class SellerSlot : BaseSlot {
        public Seller Seller {
            get => Unit as Seller;
            set => Unit = value;
        }
    }
}
