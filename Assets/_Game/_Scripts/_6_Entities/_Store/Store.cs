using System;
using System.Collections.Generic;
using System.Linq;
using Assets._Game._Scripts._5_Managers;
using Assets._Game._Scripts._6_Entities._Store._Slots;
using Assets._Game._Scripts._6_Entities._Units._Customers;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Assets._Game._Scripts._6_Entities._Store {
    public class Store : MonoBehaviour
    {

        public GameMode GameMode;
        public int CountOrders;
        public List<SellerSlot> SellerSlots { get; set; }
        public List<CustomerSlot> CustomerSlots { get; set; }
        public List<DesktopSlot> DesktopSlots { get; set; }
        public  List<Order> Orders { get; set; }
        private Queue<Customer> _waitingOrderCustomer;
        public bool IsCustomerAvailable => _waitingOrderCustomer != null && _waitingOrderCustomer.Count > 0;
        public bool IsDesktopAvailable => DesktopSlots.FirstOrDefault(slot => !slot.IsOccupied);
        [SerializeField] private int CountOrderList;

        
        private void Awake() {
            GetAllStoreSlots();
        }

        private void Start()
        {
            GameMode = FindObjectOfType<GameMode>();
        }

        private void Update()
        {
            CountOrderList = Orders.Count;
        }
        public bool HasFreeCustomerSlot() {
            return CustomerSlots.Any(slot => !slot.IsOccupied);
        }
        public int GetNumberOfFreeCustomerSlots() {
            return CustomerSlots.Count(slot => !slot.IsOccupied);
        }
        public int GetNumberOfOccupiedCustomerSlots() {
            return CustomerSlots.Count(slot => slot.IsOccupied);
        }

        private void GetAllStoreSlots() {
            SellerSlots = GetComponentsInChildren<SellerSlot>().ToList();
            CustomerSlots = GetComponentsInChildren<CustomerSlot>().ToList();
            DesktopSlots = GetComponentsInChildren<DesktopSlot>().ToList();
            _waitingOrderCustomer = new Queue<Customer>();
            Orders = new List<Order>();

        }

        public CustomerSlot GetFreeCustomerSlot() {
            // Создание списка свободных слотов
            List<CustomerSlot> freeSlots = CustomerSlots.Where(slot => !slot.IsOccupied).ToList();

            if (freeSlots.Count == 0) {
                Debug.Log("Все слоты заняты");
                return null; // Все слоты заняты
            }

            // Выбор случайного свободного слота
            int randomIndex = Random.Range(0, freeSlots.Count);
            return freeSlots[randomIndex];
        }

        public int GetOrderId()
        {
            return ++CountOrders;
        }

        private void AddToWaitingCustomersQueue(Customer customer) {
            _waitingOrderCustomer.Enqueue(customer);
            
            
        }

        public void AddNewOrders(List<Order> orders)
        {

            Orders.AddRange(orders);

        }


        public void CustomerIsReachedStore(Customer customer, CustomerSlot slot) {

          //  slot.Customer = customer;
            // Добавить в очередь свободных слотов 
            AddToWaitingCustomersQueue(customer);


        }

        public void CustomerTransferedOrder(Customer customer) {
            // Для статистики и управления в дальнейшем
            AddNewOrders(customer.Orders);
        }

        public void CustomerTakeProduct(Customer customer) {
            // Для статистики и управления в дальнейшем
           
        }

        public void CustomerLeftSlot(Customer customer) {
            // Для статистики и управления в дальнейшем
            GameMode.CustomerLeftTradeSlot();

        }

        public Customer GetWaitingCustomer() {
            if (!IsCustomerAvailable) {
                Debug.Log("!IsCustomerAvailable");
                return null;
            }
            var firstCustomer = _waitingOrderCustomer.Dequeue();
            

            return firstCustomer;
        }

        public SellerSlot GetSellerSlotByCustomerSlot(CustomerSlot customerSlot) {
            
            return SellerSlots.FirstOrDefault(slot => slot.ID == customerSlot.ID);
        }


        public (Order, DesktopSlot) SellerTryGetNewJob() {
            Order foundOrder = null;
            DesktopSlot suitableSlot = null;

            foreach (var order in Orders) {
                // Найти свободный DesktopSlot, который может обработать продукт этого заказа
                suitableSlot = DesktopSlots.FirstOrDefault(slot => !slot.IsOccupied && slot.AllowedProductType == order.ProductOrder.GetType());

                if (suitableSlot != null) {
                    // Если подходящий слот найден, сохраняем ссылку на заказ
                    foundOrder = order;
                    break; // Выходим из цикла, так как подходящий заказ найден
                }
            }

            if (foundOrder != null) {
                // Удаляем найденный заказ из списка
                Orders.Remove(foundOrder);
                return (foundOrder, suitableSlot);
            }

            // Если подходящий заказ не найден, возвращаем null
            return (null, null);
        }




    }
}