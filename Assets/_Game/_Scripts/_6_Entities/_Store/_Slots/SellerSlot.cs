using Assets._Game._Scripts._6_Entities._Units._Sellers;
using UnityEngine;

public class SellerSlot : BaseSlot {
    public Seller Seller {
        get => Unit as Seller;
        set => Unit = value;
    }
}
