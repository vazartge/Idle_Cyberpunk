using Assets._Game._Scripts._6_Entities._Customers;
using UnityEngine;

namespace Assets._Game._Scripts._6_Entities._Store {
    public class CustomerSlot : MonoBehaviour {
        public int ID { get; }
        public bool IsOccupied { get; set; }
        public Customer Customer { get; set; }
    }
}