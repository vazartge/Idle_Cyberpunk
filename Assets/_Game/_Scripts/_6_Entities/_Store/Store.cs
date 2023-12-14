using System;
using System.Collections.Generic;
using System.Linq;
using Assets._Game._Scripts._5_Managers;
using Assets._Game._Scripts._6_Entities._Store._Products;
using Assets._Game._Scripts._6_Entities._Store._Slots;
using Assets._Game._Scripts._6_Entities._Units._Customers;
using Assets._Game._Scripts._6_Entities._Units._Desktop;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Assets._Game._Scripts._6_Entities._Store {
    public class Store : MonoBehaviour
    {
        public Transform DesktopsParentTransform;
        public Transform CustomersParentTransform;
        public Transform SellersParentTransform;

        public GameMode GameMode;
        public StoreStats Stats;
        
        public List<SellerSlot> SellerSlots { get; set; }
        public List<CustomerSlot> CustomerSlots { get; set; }

        [SerializeField] public List<DesktopSlot> DesktopSlots { get; set; }
        public  List<Order> Orders { get; set; }
        private Queue<Customer> _waitingOrderCustomer;
        private List< DesktopUnit> _desktopsList;
        public bool IsCustomerAvailable => _waitingOrderCustomer != null && _waitingOrderCustomer.Count > 0;
        public bool IsDesktopAvailable => DesktopSlots.FirstOrDefault(slot => !slot.IsOccupied);
        //[SerializeField] private int CountOrderList;
       
        
        private void Awake() {
           
            _desktopsList = new List<DesktopUnit>();
            GetAllStoreSlots();
            _waitingOrderCustomer = new Queue<Customer>();
            Orders = new List<Order>();
            GameMode = FindObjectOfType<GameMode>();
            DesktopsParentTransform = GetComponentInChildren<DesktopParentTransform>().gameObject.transform;
            CustomersParentTransform = GetComponentInChildren<CustomersParentTransform>().gameObject.transform;
            SellersParentTransform = GetComponentInChildren<SellersParentTransform>().gameObject.transform;
        }

        public void Construct(StoreStats storeStats) {
            Stats = storeStats;
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
        public int GetOrderCountByProductType(ProductType productType) {
            return Orders.Count(order => order.ProductType == productType);
        }
        public Dictionary<ProductType, int> GetOrderCountsByProductType() {
            return Orders
                .GroupBy(order => order.ProductType) // Группировка по типу продукта
                .ToDictionary(group => group.Key, group => group.Count()); // Преобразование в словарь
        }

      
        public List<ProductType> GetAvailableProductTypesWithDesks() {
            return DesktopSlots
                .Select(slot => slot.ProductType) // Выбираем типы продуктов
                .Distinct() // Убираем дубликаты
                .ToList();
        }


        public ProductType GetAlternativeProductType() {
            var orderCounts = GetOrderCountsByProductType();

            // Если заказов нет, возвращаем случайный доступный тип продукта
            if (orderCounts.Count == 0) {
                var availableProductTypes = GetAvailableProductTypesWithDesks();
                if (availableProductTypes.Any()) {
                    return availableProductTypes[Random.Range(0, availableProductTypes.Count)];
                }
            }

            // Если заказы есть, выбираем тип продукта с наименьшим количеством заказов
            else {
                var minOrderType = orderCounts.Aggregate((l, r) => l.Value < r.Value ? l : r).Key;
                return minOrderType;
            }

            // Если нет доступных типов продуктов, то можно возвратить значение по умолчанию или выбросить исключение
            throw new InvalidOperationException("No available product types found.");
        }


        private void GetAllStoreSlots() {
            SellerSlots = GetComponentsInChildren<SellerSlot>().ToList();
            CustomerSlots = GetComponentsInChildren<CustomerSlot>().ToList();
            DesktopSlots = GetComponentsInChildren<DesktopSlot>().ToList();
           

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
            Debug.Log($"Free Customer Slots = {freeSlots.Count}");
            return freeSlots[randomIndex];
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
               
                suitableSlot = DesktopSlots.FirstOrDefault(slot => !slot.IsOccupied && slot.ProductType == order.ProductType);

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

        // Доделать для разных продуктов
        public void DeliveredForSellProductSuccess(Order order)
        {
            ProductType typeProduct = order.ProductType;
            var desktop = _desktopsList.FirstOrDefault(desktops => desktops.ProductType == typeProduct);
            // взять  тип продукта и выбрать нужный уровень стола в массиве - сдклать метод
            GameMode.EconomyAndUpgrade.SellProductByStore(desktop);

        }

        public void AddDesktop(DesktopUnit newDesktop)
        {
            _desktopsList.Add(newDesktop.GetComponentInChildren<DesktopUnit>());
        }


        public bool HasActiveDesktops()
        {
            return _desktopsList != null && _desktopsList.Count > 0;
        }

        public bool AreAllDesktopsUpgradedForLevel()
        {
            if(!GameMode.IsOpenedAllPrebuilders) return false;
            if (_desktopsList.Count == 0) return false;
                // Проверяем, удовлетворяет ли каждый стол в списке _desktopsList условию IsUpgradedForLevel == true
                int count = 0;
            foreach (var desktop in _desktopsList)
            {
                if (desktop._mainDesktop._desktopType == DesktopUnit.DesktopType.main &&
                    desktop._mainDesktop.IsUpgradedForLevel)
                {
                    count++;
                }
            }

            if (count == _desktopsList.Count)
            {
                return true;
            }
            else
            {
                return false;
            }
               
           
            //return _desktopsList.All(desktop => desktop._mainDesktop._desktopType == DesktopUnit.DesktopType.main && desktop._mainDesktop.IsUpgradedForLevel) && _desktopsList.Count>0;

        }
    }
}