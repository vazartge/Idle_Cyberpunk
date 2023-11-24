using Assets._Game._Scripts._6_Entities._Units._Customers;
using UnityEngine;

public class CustomerSlot : BaseSlot {
    public Customer Customer {
        get => Unit as Customer;
        set => Unit = value;
    }
}