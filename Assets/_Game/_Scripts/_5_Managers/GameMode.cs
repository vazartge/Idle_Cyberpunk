using System;
using System.Collections;
using System.Collections.Generic;
using Assets._Game._Scripts._0.Data;
using Assets._Game._Scripts._2_Game;
using Assets._Game._Scripts._4_Services;
using Assets._Game._Scripts._6_Entities._Store;
using Assets._Game._Scripts._6_Entities._Store._Products;
using Assets._Game._Scripts._6_Entities._Store._Slots;
using Assets._Game._Scripts._6_Entities._Units._Customers;
using Assets._Game._Scripts._6_Entities._Units._Desktop;
using Assets._Game._Scripts._6_Entities._Units._PrebuilderDesktop;
using Assets._Game._Scripts._6_Entities._Units._Sellers;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Assets._Game._Scripts._5_Managers {
   // public enum SideOfScreenEnum { left, right }

    public class GameMode : MonoBehaviour
    {
        public event Action OnChangedMoney;
        public event Action OnChangedLevelPlayer;
        private DataMode _dataMode;
        private UIMode _uiMode;
        public Store Store;
        public EconomyService Economy;
        public MechanicalEyeUpgradeSO _mechanicalEyeUpgradeSo;
        private InputControlService _inputControlService;
        public List<Customer> ActiveCustomers { get; set; }
        public List<Seller> Sellers { get; set; }
        public Transform SellerStartTransform;
        public Transform CustomerStartTransform;
        public Transform CustomerEndTransform;
        public GameObject CustomerPrefab;
        public GameObject SellerPrefab;
        private Queue<Customer> _customersPool;

        [SerializeField] private GameObject _firstPrebuilderDesktopMechanicalEye;
        [SerializeField] private GameObject _firstDesktopMechanicalEye;

        [SerializeField] private GameObject _buttonAddCustomer;
       // private Game _game;


        // Текущее количество активных покупателей на сцене
        private int CurrentActiveCustomers { get; set; }
        // Количество покупателей для пула покупателей
        private int CountCustomersForPool { get; } = 6;

        // Требуемое количество покупателей на сцене
        public int RequiredNumberCustomersOnScene {
            get => _requiredNumberCustomersOnScene;
            set => _requiredNumberCustomersOnScene = value;
        }

        public int RequiredNumberSellersOnScene {
            get => _requiredNumberSellersOnScene;
            set => _requiredNumberSellersOnScene = value;
        }

        [SerializeField] private int _countCustomerForID;

        private int _maxNuberCustomers = 4;
        private int _maxNuberSellers = 2;
        // Текущее количество активных продавцов на сцене
        private int CurrentActiveSellers { get; set; }
        // Количество продавцов, которое надо создать при старте в пул
        private int CountSellersForPool { get; } = 1;

        public UIMode UiMode
        {
            get => _uiMode;
            set => _uiMode = value;
        }
        // Требуемое количество покупателей на сцене

        private int _countSellerForID;
        [SerializeField] private int _requiredNumberCustomersOnScene;
        [SerializeField] private int _requiredNumberSellersOnScene;
        [SerializeField] private int numberOrders = 1;
        private float _timeRateGetCustomers = 1f;
        private bool _isConstructed;
       

        public void Construct(DataMode dataMode, UIMode uiMode) {
            _dataMode = dataMode;
            UiMode = uiMode;
            BeginPlay();
            Debug.Log("GameMode Start");
            _isConstructed = true;
        }
        public void ChangedMoney() {
            OnChangedMoney?.Invoke();
        }
        public void ChangeLevel() {
            OnChangedLevelPlayer?.Invoke();
        }
        private void BeginPlay() {
            _inputControlService = new InputControlService(this);
           
            Store = FindObjectOfType<Store>();
            Economy = new EconomyService(this, Store);
            ActiveCustomers = new List<Customer>();
            Sellers = new List<Seller>();
            _customersPool = new Queue<Customer>();

            //CreateCustomers();
            // CreateSellers();
            //InstantiateNewSeller();
            StartCoroutine(UpdateCustomersOnScene());

        }

        private void Update()
        {
            if (!_isConstructed) return;
            if (Game.Instance.IsPaused) return;
            _inputControlService.UpdateInputControl();
        }

        // private void CreateCustomers() {
        //     for (int i = 0; i < CountCustomersForPool; i++) {
        //         InstantiateNewCustomer();
        //     }
        // }
        // Метод для создания нового покупателя
        private Customer InstantiateNewCustomer() {

            Customer obj = Instantiate(CustomerPrefab, CustomerStartTransform.position, Quaternion.identity).GetComponent<Customer>();
            obj.Construct(this, Store, CustomerStartTransform, CustomerEndTransform);
            _customersPool.Enqueue(obj); // Добавляем в пул
            obj.gameObject.SetActive(false); // Скрываем покупателя
            ActiveCustomers.Add(obj); // Добавляем в список для учета
            _countCustomerForID++;
            obj.ID = "CustomerID:" + _countCustomerForID;
            return obj;
        }
        private void CreateSellers() {
            for (int i = 0; i < CountSellersForPool; i++) {
                InstantiateNewSeller();

            }
        }
        private Seller InstantiateNewSeller() {
            Seller obj = Instantiate(SellerPrefab, SellerStartTransform.position, Quaternion.identity).GetComponent<Seller>();
            obj.Construct(this, Store);

            Sellers.Add(obj); // Добавляем в список для учета
            _countSellerForID++;
            obj.ID = "SellerID:" + _countSellerForID;
            return obj;
        }

        public void NewSellerButton()
        {
            if(RequiredNumberSellersOnScene >= _maxNuberSellers) return;
            RequiredNumberSellersOnScene++;
            InstantiateNewSeller();
            
        }
        
        // Публичный метод для получения покупателя из пула
        public Customer GetCustomer() {

            Customer customer = _customersPool.Dequeue(); // Получаем покупателя из пула
            customer.gameObject.SetActive(true); // Активируем покупателя
            var freeSlot = Store.GetFreeCustomerSlot();

            var orders = CreateOrders(customer, new MechanicalEyeProduct(), Random.Range(1, 4));
            customer.SetupCustomer(freeSlot, orders);
            CurrentActiveCustomers++;
            return customer;

        }

        private List<Order> CreateOrders(Customer customer, IProduct product, int number) {
            var orders = new List<Order>();
            for (int i = 0; i < number; i++) {
                orders.Add(new Order(customer, product, Store.GetOrderId()));
                Debug.Log($"Order  {i} ");

            }
            return orders;
        }
        public void CustomerLeftTradeSlot() {

            StartCoroutine(UpdateCustomersOnScene());
        }
        public void CustomerLeftScene(Customer customer) {
            ReturnCustomerToPool(customer);
           // StartCoroutine(UpdateCustomersOnScene());
        }
        public void NewCustomerButton() {
            if(RequiredNumberCustomersOnScene >=_maxNuberCustomers) return;
            RequiredNumberCustomersOnScene++;
            StartCoroutine(UpdateCustomersOnScene());
        }
        private IEnumerator UpdateCustomersOnScene() {
            Debug.Log($"CurrentActiveCustomers = {CurrentActiveCustomers}");
            //if (RequiredNumberCustomersOnScene == 0) yield break;
          //  if (!Store.HasFreeCustomerSlot()/*CurrentActiveCustomers >= _maxNuberCustomers*/) yield break;
            while (Store.GetNumberOfOccupiedCustomerSlots() < RequiredNumberCustomersOnScene &&  Store.HasFreeCustomerSlot()) {
                if (_customersPool.Count == 0) {
                    // Создаем нового покупателя, если в пуле нет доступных
                    InstantiateNewCustomer();
                }

                // Получаем покупателя из пула и активируем его
                GetCustomer();

                // Задержка перед появлением следующего покупателя
                yield return new WaitForSeconds(_timeRateGetCustomers);
            }
        }

        // Публичный метод для возвращения покупателя в пул
        public void ReturnCustomerToPool(Customer customer) {
            customer.gameObject.SetActive(false); // Деактивируем покупателя
            _customersPool.Enqueue(customer); // Возвращаем в пул
            CurrentActiveCustomers--; // Уменьшаем число активных покупателей
            // Установка начального положения и ориентации покупателя
            customer.transform.position = CustomerStartTransform.position;
            customer.transform.rotation = CustomerStartTransform.rotation;
            Debug.Log($"CurrentActiveCustomers = {CurrentActiveCustomers}");

            ActiveCustomers.Remove(customer);

        }


        public void OnButtonBuyDesktop(PrebuilderDesktop prebuilderDesktop)
        {
            var newDesktop = Instantiate(_firstDesktopMechanicalEye, prebuilderDesktop.gameObject.transform.position,
                Quaternion.identity);
            var newDesktopSlot = newDesktop.GetComponentInChildren<DesktopSlot>();
            Destroy(prebuilderDesktop.gameObject);
            Store.DesktopSlots.Add(newDesktopSlot);
            _buttonAddCustomer.gameObject.SetActive(true);
        }


        public void OnButtonUpgradeDesktop(DesktopUnit desktopUnit)
        {
            Debug.Log("Try Upgrade desktop");
        }


       
    }
}