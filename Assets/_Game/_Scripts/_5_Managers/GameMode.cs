using System.Collections.Generic;
using Assets._Game._Scripts._2_Game;
using Assets._Game._Scripts._6_Entities._Customers;
using Assets._Game._Scripts._6_Entities._Sellers;
using Assets._Game._Scripts._6_Entities._Store;
using UnityEngine;

namespace Assets._Game._Scripts._5_Managers
{
    public enum SideOfScreenEnum { left, right }

    public class GameMode : MonoBehaviour {
       
        [SerializeField]
        private float spawnHeight = 0.9f; // От 0 до 1, где 0.5 это середина экрана по высоте.

        public int MaxActiveCustomers { get; } = 1;
        public int CurrentActiveCustomers { get; set; } = 1;
        public int CountCustomers { get; } = 1;
        public int CountSellers { get; }= 1;
        public Store Store;
        public List<Customer> Customers { get; set; }
        public List<Seller> Sellers { get; set; }
        public Vector3 SellerStartPosition;
        public Vector3 CustomerStartPosition;
        public Vector3 CustomerEndPosition;
        public GameObject CustomerPrefab;
        public GameObject SellerPrefab;
        private Queue<Customer> _customerPool;
        private Game _game;

        private void Start()
        {
            CustomerStartPosition = GetSpawnPointOffScreen(SideOfScreenEnum.left);
            // Теперь можно использовать spawnPoint для создания покупателей.
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
        private Vector3 GetSpawnPointOffScreen(SideOfScreenEnum side) {
            
            // Получаем позицию слева от видимой области камеры
            Vector2 viewPortPoint = new Vector2(0f, spawnHeight); // X установлен в 0, что соответствует левой границе экрана
            Vector2 worldPoint = Camera.main.ViewportToWorldPoint(viewPortPoint);
            switch (side) {
                case SideOfScreenEnum.left:
                    // Добавляем небольшой отступ, чтобы покупатель появлялся за пределами видимой области
                    worldPoint.x -= 2f; // Подгоните значение отступа в зависимости от размеров спрайта покупателя
                    break;
                case SideOfScreenEnum.right:
                    // Добавляем небольшой отступ, чтобы покупатель появлялся за пределами видимой области
                    worldPoint.x += 2f; // Подгоните значение отступа в зависимости от размеров спрайта покупателя
                    break;
                default:
                    // Добавляем небольшой отступ, чтобы покупатель появлялся за пределами видимой области
                    worldPoint.x -= 2f; // Подгоните значение отступа в зависимости от размеров спрайта покупателя
                    break;
                    
            }
            
            return worldPoint;
        }
        private void CreateCustomers() {
            for (int i = 0; i < CountCustomers; i++) {
                InstantiateNewCustomer();
            }
        }
        // Метод для создания нового покупателя
        private Customer InstantiateNewCustomer() {
            Customer obj = Instantiate(CustomerPrefab, CustomerStartPosition, Quaternion.identity).GetComponent<Customer>();
            obj.Construct(this, Store);
            _customerPool.Enqueue(obj); // Добавляем в пул
            //obj.gameObject.SetActive(false); // Скрываем покупателя
            Customers.Add(obj); // Добавляем в список для учета
            var freeSlot = Store.GetFreeCustomerSlot();
            var order = new Order(new Product(ProductType.MechanicalEye, 1), 1);
            obj.SetupCustomer(freeSlot, order);
            return obj;
        }
        private void CreateSellers() {
            for (int i = 0; i < CountSellers; i++) {
                InstantiateNewSeller();

            }
        }
        private Seller InstantiateNewSeller() {
            Seller obj = Instantiate(SellerPrefab, SellerStartPosition, Quaternion.identity).GetComponent<Seller>();
            obj.Construct(this, Store);
            //obj.gameObject.SetActive(false); // Скрываем покупателя
            Sellers.Add(obj); // Добавляем в список для учета
            return obj;
        }
        // Публичный метод для получения покупателя из пула
        public Customer GetCustomer() {
            if (CurrentActiveCustomers < MaxActiveCustomers && _customerPool.Count > 0)
            {
                Customer customer = _customerPool.Dequeue(); // Получаем покупателя из пула
                customer.gameObject.SetActive(true); // Активируем покупателя
                var freeSlot = Store.GetFreeCustomerSlot();
                var order = new Order(new Product(ProductType.MechanicalEye, 1), 1);
                customer.SetupCustomer(freeSlot, order);
                CurrentActiveCustomers++;
                return customer;
            } else {
                // Опционально: можно создать нового покупателя, если пул пуст
                return InstantiateNewCustomer();
            }
        }

        // Публичный метод для возвращения покупателя в пул
        public void ReturnCustomerToPool(Customer customer) {
            customer.gameObject.SetActive(false); // Деактивируем покупателя
            _customerPool.Enqueue(customer); // Возвращаем в пул
            CurrentActiveCustomers = Mathf.Max(0, CurrentActiveCustomers - 1); // Уменьшаем число активных покупателей
        }


    }
}