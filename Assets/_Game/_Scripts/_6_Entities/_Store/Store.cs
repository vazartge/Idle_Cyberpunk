using System.Collections.Generic;
using System.Linq;
using Assets._Game._Scripts._6_Entities._Store._Slots;
using Assets._Game._Scripts._6_Entities._Units._Customers;
using UnityEngine;

namespace Assets._Game._Scripts._6_Entities._Store {
    public class Store : MonoBehaviour {
        public int CountOrders;
        public List<SellerSlot> SellerSlots { get; set; }
        public List<CustomerSlot> CustomerSlots { get; set; }
        public List<DesktopSlot> DesktopSlots { get; set; }
        public List<Order> Orders { get; set; }
        private Queue<CustomerSlot> _waitingOrederCustomerSlots;
        public bool IsCustomerAvailable { get; set; }


        private void Awake() {
            GetAllStoreSlots();
        }

        private void GetAllStoreSlots() {
            SellerSlots = GetComponentsInChildren<SellerSlot>().ToList();
            CustomerSlots = GetComponentsInChildren<CustomerSlot>().ToList();
            DesktopSlots = GetComponentsInChildren<DesktopSlot>().ToList();

        }

        public CustomerSlot GetFreeCustomerSlot() {
            foreach (var slot in CustomerSlots) {
                if (!slot.IsOccupied) {
                    
                    return slot;
                }
            }
            Debug.Log("Все слоты заняты");
            return null; // Все слоты заняты
        }

        public void CustomerIsReachedStore(Customer customer, CustomerSlot slot) {

            slot.Customer = customer;
            // Добавить в очередь свободных слотов 
            AddToWaitingCustomersSlotsQueue(slot);


        }
        private void AddToWaitingCustomersSlotsQueue(CustomerSlot slot) {
            _waitingOrederCustomerSlots.Enqueue(slot);
            IsCustomerAvailable = true;
        }

        public CustomerSlot GetWaitingCustomerSlot() {
            if (_waitingOrederCustomerSlots.Count <= 0) {
                IsCustomerAvailable = false;
                return null;
            }
            return  _waitingOrederCustomerSlots.Dequeue();
        }

        public SellerSlot GetSellerSlotByCustomerSlot(CustomerSlot customerSlot) {
            
            return SellerSlots.FirstOrDefault(slot => slot.ID == customerSlot.ID);
        }

        public void AddNewOrder(Order order)
        {
            Orders.Add(order);
            CountOrders++;
        }

        public Order GetNewJobForSeller()
        {
            return null;
        }
    }
}