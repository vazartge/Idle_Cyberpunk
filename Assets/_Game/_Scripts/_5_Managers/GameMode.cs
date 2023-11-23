using System.Collections.Generic;
using Assets._Game._Scripts._2_Game;
using Assets._Game._Scripts._6_Entities._Customers;
using Assets._Game._Scripts._6_Entities._Sellers;
using Assets._Game._Scripts._6_Entities._Store;
using UnityEngine;

namespace Assets._Game._Scripts._5_Managers {
    public enum SideOfScreenEnum { left, right }

    public class GameMode : MonoBehaviour {

        public int MaxActiveCustomers { get; } = 1;
        public int CurrentActiveCustomers { get; set; } = 1;
        public int CountCustomers { get; } = 2;
        public int CountSellers { get; } = 1;
        public Store Store;
        public List<Customer> Customers { get; set; }
        public List<Seller> Sellers { get; set; }
        public Transform SellerStartTransform;
        public Transform CustomerStartTransform;
        public Transform CustomerEndTransform;
        public GameObject CustomerPrefab;
        public GameObject SellerPrefab;
        private Queue<Customer> _customerPool;
        private Game _game;

        private void Start() {
           
        }

        public void Construct(Game game) {
            _game = game;
            BeginPlay();
            Debug.Log("GameMode Start");
        }

        private void BeginPlay() {
            Store = FindObjectOfType<Store>();
            Customers = new List<Customer>();
            Sellers = new List<Seller>();
            _customerPool = new Queue<Customer>();
            CreateCustomers();
            CreateSellers();
            GetCustomer();
        }
      
        private void CreateCustomers() {
            for (int i = 0; i < CountCustomers; i++) {
                InstantiateNewCustomer();
            }
        }
        // Метод для создания нового покупателя
        private Customer InstantiateNewCustomer() {
           
            Customer obj = Instantiate(CustomerPrefab, CustomerStartTransform.position, Quaternion.identity).GetComponent<Customer>();
            obj.Construct(this, Store);
            _customerPool.Enqueue(obj); // Добавляем в пул
            obj.gameObject.SetActive(false); // Скрываем покупателя
            Customers.Add(obj); // Добавляем в список для учета

            return obj;
        }
        private void CreateSellers() {
            for (int i = 0; i < CountSellers; i++) {
                InstantiateNewSeller();

            }
        }
        private Seller InstantiateNewSeller() {
            Seller obj = Instantiate(SellerPrefab, SellerStartTransform.position, Quaternion.identity).GetComponent<Seller>();
            obj.Construct(this, Store);

            Sellers.Add(obj); // Добавляем в список для учета
            return obj;
        }
        // Публичный метод для получения покупателя из пула
        public Customer GetCustomer() {
           
            Customer customer = _customerPool.Dequeue(); // Получаем покупателя из пула
            customer.gameObject.SetActive(true); // Активируем покупателя
            var freeSlot = Store.GetFreeCustomerSlot();
            var order = new Order(new Product(ProductType.MechanicalEye, 1), 1);
            customer.SetupCustomer(freeSlot, order);
            CurrentActiveCustomers++;
            return customer;

        }

        // Публичный метод для возвращения покупателя в пул
        public void ReturnCustomerToPool(Customer customer) {
            customer.gameObject.SetActive(false); // Деактивируем покупателя
            _customerPool.Enqueue(customer); // Возвращаем в пул
            CurrentActiveCustomers = Mathf.Max(0, CurrentActiveCustomers - 1); // Уменьшаем число активных покупателей
        }
        

    }
}