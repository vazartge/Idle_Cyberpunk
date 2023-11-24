using Assets._Game._Scripts._6_Entities._Units._Customers;
using UnityEngine;

namespace Assets._Game._Scripts._6_Entities._Store._Slots {
    public class CustomerSlot : BaseSlot {
        public Customer Customer {
            get { return Unit as Customer; }
            set { Unit = value; }
        }
    }

}