﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Assets._Game._Scripts._2_Game;
using Assets._Game._Scripts._5_Managers;
using Assets._Game._Scripts._6_Entities._Store._Products;
using Assets._Game._Scripts._6_Entities._Store._Slots;
using Assets._Game._Scripts._6_Entities._Units._Customers;
using Assets._Game._Scripts._6_Entities._Units._Desktop;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Assets._Game._Scripts._6_Entities._Store {
    // Магазин
    public class Store : MonoBehaviour {
        public Transform DesktopsParentTransform;
        public Transform CustomersParentTransform;
        public Transform SellersParentTransform;
        public Transform PrebuilderParentTransform;
        public GameMode GameMode;
        // public StoreStats StoreStats;

        [SerializeField] private List<DesktopUnit> _desktopsList;

        private Queue<Customer> _waitingOrderCustomer;
        private int remainingTime;
        public List<Order> Orders { get; set; }

        [SerializeField] public List<DesktopSlot> DesktopSlots { get; set; }
        public bool IsCustomerAvailable => _waitingOrderCustomer != null && _waitingOrderCustomer.Count > 0;
        public bool IsDesktopAvailable => DesktopSlots.FirstOrDefault(slot => !slot.IsOccupied);
        //[SerializeField] private int CountOrderList;
        public List<SellerSlot> SellerSlots { get; set; }
        public List<CustomerSlot> CustomerSlots { get; set; }



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
            //StoreStats = storeStats;
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
        public int GetOrderCountByProductType(ProductStoreType productStoreType) {
            return Orders.Count(order => order.ProductStoreType == productStoreType);
        }
        public Dictionary<ProductStoreType, int> GetOrderCountsByProductType() {
            return Orders
                .GroupBy(order => order.ProductStoreType) // Группировка по типу продукта
                .ToDictionary(group => group.Key, group => group.Count()); // Преобразование в словарь
        }


        public List<ProductStoreType> GetAvailableProductTypesWithDesks() {
            return DesktopSlots
                .Select(slot => slot.ProductStoreType) // Выбираем типы продуктов
                .Distinct() // Убираем дубликаты
                .ToList();
        }


        public ProductStoreType GetAlternativeProductType() {
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
            throw new InvalidOperationException("No available productStore types found.");
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

        public void AddNewOrders(List<Order> orders) {

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

                suitableSlot = DesktopSlots.FirstOrDefault(slot => !slot.IsOccupied && slot.ProductStoreType == order.ProductStoreType);

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
        public void DeliveredForSellProductSuccess(Order order) {
            ProductStoreType storeTypeProductStore = order.ProductStoreType;
            var desktop = _desktopsList.FirstOrDefault(desktops => desktops.ProductStoreType == storeTypeProductStore);
            // взять  тип продукта и выбрать нужный уровень стола в массиве - сдклать метод
            GameMode.EconomyAndUpgrade.SellProductByStore(desktop);

        }

        public void AddDesktop(DesktopUnit newDesktop) {
            _desktopsList.Add(newDesktop.GetComponentInChildren<DesktopUnit>());
        }

        public List<DesktopUnit> GetDesktopUnitsList() {
            return _desktopsList;
        }
        public bool HasActiveDesktops() {
            return _desktopsList != null && _desktopsList.Count > 0;
        }

        public bool AreAllDesktopsUpgradedForLevel() {
            return _desktopsList.Count > 0 && _desktopsList.All(desktop => desktop.IsUpgradedForLevel);
           

        }

        public void AddDesktopSlots(DesktopSlot slot) {
            DesktopSlots.Add(slot);
        }

        public void StartBoostProductionCoroutine() {
            StartCoroutine(BoostProductionCoroutine());

        }
        private IEnumerator BoostProductionCoroutine()
        {
            remainingTime = 60;
            BoostProduction(); // Ускоряем производство
            while (remainingTime > 0)
            {
                
                yield return new WaitForSeconds(1f); // Ждем одну минуту (1 секунду)
                remainingTime--;
            } 
            // timerText.text = "00:00";
            remainingTime = 60;
            NormalProduction(); // Возвращаем производство к нормальному режиму
            
        }
     

        private void BoostProduction() {
            // Логика для ускорения производства
            Debug.Log("Производство ускорено!");
            GameMode.EconomyAndUpgrade.IsBoostedFromRewarded = true;
            GameMode.UiMode.StartTimerBoostButton(remainingTime);
            //    GameMode.UiMode.StartRewardedIcreaseIncome();
        }

        private void NormalProduction() {
            // Логика для возвращения к нормальному производству
            Debug.Log("Производство возвращено к нормальному режиму.");
            GameMode.EconomyAndUpgrade.IsBoostedFromRewarded = false;
            
        }
    }
}