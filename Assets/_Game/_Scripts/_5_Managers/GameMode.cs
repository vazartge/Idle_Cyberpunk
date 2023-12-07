using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Assets._Game._Scripts._2_Game;
using Assets._Game._Scripts._4_Services;
using Assets._Game._Scripts._6_Entities._Store;
using Assets._Game._Scripts._6_Entities._Store._Products;
using Assets._Game._Scripts._6_Entities._Store._Slots;
using Assets._Game._Scripts._6_Entities._Units._Base;
using Assets._Game._Scripts._6_Entities._Units._Customers;
using Assets._Game._Scripts._6_Entities._Units._Desktop;
using Assets._Game._Scripts._6_Entities._Units._PrebuilderDesktop;
using Assets._Game._Scripts._6_Entities._Units._Sellers;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;


namespace Assets._Game._Scripts._5_Managers {
    // public enum SideOfScreenEnum { left, right }

    public class GameMode : MonoBehaviour {

        private int _countOpenedPrebuilder = 0;
        public event Action OnChangedMoney;
        public event Action OnChangedLevelPlayer;
        public DataMode_ _dataMode;
        private UIMode _uiMode;
        public Store Store;
        public EconomyService Economy;
        public Camera UiCamera;
        private ProductRandomizerService _productRandomizerService;
        private InputControlService _inputControlService;
        public List<Customer> ActiveCustomers { get; set; }
        public List<Seller> Sellers { get; set; }
        public Transform SellerStartTransform;
        public Transform CustomerStartTransform;
        public Transform CustomerEndTransform;
        public GameObject CustomerPrefab;
        public GameObject SellerPrefab;
        private Queue<Customer> _customersPool;
        private Dictionary<int, int> maxProductsPerCustomer;
        [SerializeField] private GameObject[] _prebuildersGO;

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

        public UIMode UiMode {
            get => _uiMode;
            set => _uiMode = value;
        }

        public DataMode_ DataMode {
            get => _dataMode;
            set => _dataMode = value;
        }
        // Требуемое количество покупателей на сцене

        private int _countSellerForID;
        [SerializeField] private int _requiredNumberCustomersOnScene;
        [SerializeField] private int _requiredNumberSellersOnScene;
        [SerializeField] private int numberOrders = 1;
        private float _timeRateGetCustomers = 1f;
        private bool _isInitialized;
        private int _idOrder;
        private int GameLevel=1;


        public void Construct(DataMode_ dataMode, UIMode uiMode) {
            DataMode = dataMode;
            UiMode = uiMode;
            maxProductsPerCustomer = new Dictionary<int, int>
            {
                { 1, 2 },
                { 2, 2 },
                { 3, 3 },
                { 4, 3 }
            };

            Store = FindObjectOfType<Store>();
            _productRandomizerService = new ProductRandomizerService();

            ActiveCustomers = new List<Customer>();
            Sellers = new List<Seller>();
            _customersPool = new Queue<Customer>();

            StartCoroutine(UpdateCustomersOnScene());
            Economy = new EconomyService(this, Store);
            _inputControlService = new InputControlService(this);
            UiMode.Construct(DataMode, this);


        }
        public void InitializedStoreStats() {
            InitializeUI();
            Debug.Log("GameMode Start");
        }
        private void InitializeUI() {




            //CreateCustomers();
            // CreateSellers();
            //InstantiateNewSeller();

            OnChangedMoney?.Invoke();
            StartCoroutine(InitializeUnitsPrebuldersOnScene(_countOpenedPrebuilder));

            _isInitialized = true;

        }

        public IEnumerator InitializeUnitsPrebuldersOnScene(int countOpenedPrebuilder) {
            yield return new WaitForSeconds(0.1f);


            Debug.Log(gameObject.name);
            foreach (var prebuilderGO in _prebuildersGO) {
                var prebuilderDesktop = prebuilderGO.GetComponent<PrebuilderDesktop>();
                prebuilderDesktop.Construct(this, DataMode);
                prebuilderDesktop.IsActive = true;
                prebuilderGO.SetActive(true);

            }



        }

        public void ChangedMoney() {
            OnChangedMoney?.Invoke();
            CheckPrebuilders();
            UiMode?.UpdateOnChangedMoney();
        }
        public void ChangeLevel() {
            OnChangedLevelPlayer?.Invoke();
            CheckPrebuilders();
            UiMode?.UpdateOnChangedLevelPlayer();
        }

        public void OnAnyInputControllerEvent() {
            UiMode?.OnAnyInputControllerEvent();
        }

        private void Update() {
            if (!_isInitialized) return;
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

        public void NewSellerButton() {
            if (RequiredNumberSellersOnScene >= _maxNuberSellers) return;
            RequiredNumberSellersOnScene++;
            InstantiateNewSeller();

        }

        // Публичный метод для получения покупателя из пула
        public Customer GetCustomer() {
            Customer customer = _customersPool.Dequeue();
            customer.gameObject.SetActive(true);
            var freeSlot = Store.GetFreeCustomerSlot();

            int maxProductsPerCustomer = GetMaxProductsPerCustomer(GameLevel);
            int maxOrderCount = Sellers.Count * 3;

            // Получаем типы продуктов, для которых есть свободные столы
            var availableProductTypesWithDesks = Store.GetAvailableProductTypesWithDesks();

            // Фильтруем доступные типы продуктов на основе текущего количества заказов
            var orderCounts = Store.GetOrderCountsByProductType();
            var eligibleProductTypes = availableProductTypesWithDesks
                .Where(type => !orderCounts.ContainsKey(type) || orderCounts[type] < maxOrderCount)
                .ToList();

            // Выбираем случайный тип продукта из подходящих
            ProductType selectedType;
            if (eligibleProductTypes.Any()) {
                selectedType = _productRandomizerService.GetRandomProductType(eligibleProductTypes);
            } else {
                // Все типы продуктов либо достигли максимума, либо для них нет свободных столов
                //selectedType = Store.GetAlternativeProductType(new List<ProductType>());
                selectedType = _productRandomizerService.GetRandomProductType(availableProductTypesWithDesks);
            }

            // Создаем заказы
            var orders = CreateOrders(customer, selectedType, maxProductsPerCustomer);
            customer.SetupCustomer(freeSlot, orders);
            CurrentActiveCustomers++;
            return customer;
        }


        private int GetMaxProductsPerCustomer(int gameLevel) {
            // Здесь должна быть логика, определяющая максимальное количество продуктов в заказе на основе уровня игры
            // Например, можно использовать простой switch или словарь, если логика более сложная
            // Получаем максимальное количество товаров для уровня
            if (maxProductsPerCustomer.TryGetValue(gameLevel, out int maxProducts)) {
                // Возвращаем случайное число от 1 до максимального количества товаров включительно
                return Random.Range(1, maxProducts + 1);
            } else {
                // Если уровень не найден, возвращаем значение по умолчанию, например, случайное от 1 до 2
                Debug.LogError("Нет такого значения в словаре");
                return 1;
            }
        }

        private List<Order> CreateOrders(Customer customer, ProductType typeProduct, int count) {
            List<Order> orders = new List<Order>();
            for (int i = 0; i < count; i++) {
                _idOrder++;
                orders.Add(new Order(customer, typeProduct, _idOrder));
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
            if (RequiredNumberCustomersOnScene >=_maxNuberCustomers) return;
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


        public bool OnButtonBuyDesktop(PrebuilderDesktop prebuilderDesktop) {
            if (Economy.TryBuyPrebuilder(prebuilderDesktop)) {
                return CreateDesktopFromPrebuilder(prebuilderDesktop);
            }


            return false;
        }

        private void CheckPrebuilders() {
            if (_prebuildersGO[_countOpenedPrebuilder] == null && _countOpenedPrebuilder<=_prebuildersGO.Length) {
                InitializeUnitsPrebuldersOnScene(_countOpenedPrebuilder);
            }
        }

        private bool CreateDesktopFromPrebuilder(PrebuilderDesktop prebuilderDesktop) {
            var newDesktop = Instantiate(DataMode.GetPrefabForDesktop()
                , prebuilderDesktop.gameObject.transform.position,
                Quaternion.identity);
            newDesktop.GetComponent<DesktopUnit>().Construct(this, prebuilderDesktop.ProductType);
            var newDesktopSlot = newDesktop.GetComponentInChildren<DesktopSlot>();
            newDesktopSlot.ProductType = prebuilderDesktop.ProductType;

            prebuilderDesktop.gameObject.SetActive(false);
            Store.AddDesktop(newDesktop);
            Store.DesktopSlots.Add(newDesktopSlot);
            _buttonAddCustomer.gameObject.SetActive(true);
            return true;
        }


        public bool OnButtonUpgradeDesktop(DesktopUnit desktopUnit) {
            return Economy.TryUpgradeDesktop(desktopUnit);

        }

        private void OnDestroy() {
            OnChangedMoney = null;
            OnChangedLevelPlayer = null;
        }


    }
}