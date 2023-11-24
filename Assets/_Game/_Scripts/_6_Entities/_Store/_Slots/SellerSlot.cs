using Assets._Game._Scripts._6_Entities._Units._Sellers;
using UnityEngine;

namespace Assets._Game._Scripts._6_Entities._Store._Slots
{
    public class SellerSlot : BaseSlot {

        public Seller Seller {
            get { return Unit as Seller; }
            set { Unit = value; }
        }



    }
}