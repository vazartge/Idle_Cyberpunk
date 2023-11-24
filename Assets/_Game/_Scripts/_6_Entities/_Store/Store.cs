using System;
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
        private List<CustomerSlot> _waitingOrderCustomerSlots;
        public bool IsCustomerAvailable { get; set; }
        public bool IsDesktopAvailable { get; set; }


        private void Awake() {
            GetAllStoreSlots();
        }

        private void GetAllStoreSlots() {
            SellerSlots = GetComponentsInChildren<SellerSlot>().ToList();
            CustomerSlots = GetComponentsInChildren<CustomerSlot>().ToList();
            DesktopSlots = GetComponentsInChildren<DesktopSlot>().ToList();
            _waitingOrderCustomerSlots = new List<CustomerSlot>();
            Orders = new List<Order>();

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
            _waitingOrderCustomerSlots.Add(slot);
            IsCustomerAvailable = true;
        }

        public CustomerSlot GetWaitingCustomerSlot() {
            if (_waitingOrderCustomerSlots.Count == 0) {
                IsCustomerAvailable = false;
                return null;
            }
            var firstCustomer = _waitingOrderCustomerSlots.First();
            _waitingOrderCustomerSlots.RemoveAt(0);
            if (_waitingOrderCustomerSlots.Count == 0)
            {
                IsCustomerAvailable = false;
            }

            return firstCustomer;
        }

        public SellerSlot GetSellerSlotByCustomerSlot(CustomerSlot customerSlot) {
            
            return SellerSlots.FirstOrDefault(slot => slot.ID == customerSlot.ID);
        }
        

        public void AddNewOrder(Order order)
        {
            Orders.Add(order);
            if (Orders.Count > 0)
            {
                AvailableDesktopSlots();
            }
            CountOrders++;
        }

        public void AvailableDesktopSlots()
        {
            if (DesktopSlots.FirstOrDefault(slot => !slot.IsOccupied))
            {
               IsDesktopAvailable = true;
            }
            else
            {
                IsDesktopAvailable = false;
            }

        }

        public (Order, DesktopSlot) TryGetNewJobForSeller() {
            foreach (var order in Orders) {
                // Найти свободный DesktopSlot, который может обработать продукт этого заказа
                var suitableSlot = DesktopSlots.FirstOrDefault(slot => !slot.IsOccupied && slot.AllowedProductType == order.Product.GetType());

                if (suitableSlot != null) {
                    // Если подходящий слот найден, удаляем заказ из списка и возвращаем его
                    Orders.Remove(order);
                    CountOrders--;
                    return (order, suitableSlot);
                }
            }

            // Если подходящий заказ не найден, возвращаем null
            return (null, null);
        }
    }
}