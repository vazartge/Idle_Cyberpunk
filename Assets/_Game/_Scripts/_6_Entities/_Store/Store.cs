using System.Collections;
using System.Collections.Generic;
using Assets._Game._Scripts._6_Entities._Customers;
using Assets._Game._Scripts._6_Entities._Sellers;
using UnityEngine;

namespace Assets._Game._Scripts._6_Entities._Store {
    public class Store : MonoBehaviour {
        public SellerSlot[] SellerSlots { get; set; }
        public CustomerSlot[] CustomerSlots { get; set; }
        public DesktopSlot[] DesktopSlots { get; set; }
        public bool IsCustomerAvailable { get; set; }
        private Queue<CustomerSlot> _waitingCustomerSlots = new Queue<CustomerSlot>();
        


        private void Awake()
        {
            GetAllStoreSlots();
        }

        private void GetAllStoreSlots()
        {
            SellerSlots = GetComponentsInChildren<SellerSlot>();
            CustomerSlots = GetComponentsInChildren<CustomerSlot>();
            DesktopSlots = GetComponentsInChildren<DesktopSlot>();
            
        }

        public CustomerSlot GetFreeCustomerSlot()
        {
            foreach (var slot in CustomerSlots) {
                if (!slot.IsOccupied) {
                    slot.IsOccupied = true;
                    return slot;
                }
            }
            return null; // Все слоты заняты
        }

        public void CustomerIsReachedStore(Customer customer,CustomerSlot slot)
        {
            
            slot.Customer = customer;
            // Добавить в очередь свободных слотов 
            AddToWaitingCustomersSlotsQueue(slot);
            

        }
        private void AddToWaitingCustomersSlotsQueue(CustomerSlot slot)
        {
            _waitingCustomerSlots.Enqueue(slot);
            IsCustomerAvailable = true;
        }

        public CustomerSlot GetWaitingCustomerSlot()
        {
            var customer = _waitingCustomerSlots.Dequeue();
            if (_waitingCustomerSlots.Count <= 0)
            {
                IsCustomerAvailable = false;
            }

            return customer;
        }

        public SellerSlot GetSellerSlotByCustomerSlotID(CustomerSlot customerSlot)
        {
            var idCustomer = customerSlot.ID;
            foreach (var slot in SellerSlots)
            {
                if (slot.ID == idCustomer)
                {
                    return slot;
                }
            }

            return null;
        }
    }
}