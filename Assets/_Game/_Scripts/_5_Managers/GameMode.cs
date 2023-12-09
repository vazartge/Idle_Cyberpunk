using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Assets._Game._Scripts._2_Game;
using Assets._Game._Scripts._4_Services;
using Assets._Game._Scripts._6_Entities._Store;
using Assets._Game._Scripts._6_Entities._Store._Slots;
using Assets._Game._Scripts._6_Entities._Units._Customers;
using Assets._Game._Scripts._6_Entities._Units._Desktop;
using Assets._Game._Scripts._6_Entities._Units._PrebuilderDesktop;
using Assets._Game._Scripts._6_Entities._Units._Sellers;
using UnityEngine;
using Random = UnityEngine.Random;


namespace Assets._Game._Scripts._5_Managers {
    // public enum SideOfScreenEnum { left, right }

    public class GameMode : MonoBehaviour {
        #region Fields

        [Header("Changed Stats")]
        [SerializeField] private int numberOrders = 1;
        // Changed Stats
        public event Action OnChangedStatsOrMoney;
        //  public event Action OnChangedLevelPlayer;
        public int GameLevel {
            get => Game.Instance.GameLevel;
            set => Game.Instance.GameLevel = value;
        }
        private bool _isInitialized;
        private int _idOrder;

        [Header("Refererences")]
        public Store Store;
        public EconomyAndUpgradeService EconomyAndUpgrade;
        public Camera UiCamera;
        public UIMode UiMode {
            get => _uiMode;
            set => _uiMode = value;
        }
        public DataMode_ DataMode {
            get => _dataMode;
            set => _dataMode = value;
        }
        private ProductRandomizerService _productRandomizerService;
        private InputControlService _inputControlService;
        private DataMode_ _dataMode;
        private UIMode _uiMode;

        [Header("PrebuildersDesktop")]
        [SerializeField] private GameObject[] _prebuildersGO;

        private int _countOpenedPrebuilder = 0;

        [Header("Desktops")]



        [Header("Customers")]
        [SerializeField] private GameObject _buttonAddCustomer;
        [SerializeField] private int _countCustomerForID;
        // Требуемое количество покупателей на сцене
        [SerializeField] private int _requiredNumberCustomersOnScene;
        // Требуемое количество покупателей на сцене
        public int RequiredNumberCustomersOnScene {
            get => _requiredNumberCustomersOnScene;
            set => _requiredNumberCustomersOnScene = value;
        }
        public List<Customer> ActiveCustomers { get; set; }
        private Queue<Customer> _customersPool;
        private Dictionary<int, int> maxProductsPerCustomer;
        private int _maxNuberCustomers = 10;
        // Текущее количество активных покупателей на сцене
        private int CurrentActiveCustomers { get; set; }
        // Количество покупателей для пула покупателей
        private int CountCustomersForPool { get; } = 16;
        private float _timeRateGetCustomers = 1f;

        [Header("Sellers")]
        public GameObject SellerPrefab;
        [SerializeField] private int _requiredNumberSellersOnScene;
        public int RequiredNumberSellersOnScene {
            get => _requiredNumberSellersOnScene;
            set => _requiredNumberSellersOnScene = value;
        }
        private int _maxNuberSellers = 8;
        private int _countSellerForID;
        // Текущее количество активных продавцов на сцене
        private int CurrentActiveSellers { get; set; }
        // Количество продавцов, которое надо создать при старте в пул
        private int CountSellersForPool { get; } = 1;

        [Header("StartPoints")]
        public Transform SellerStartTransform;
        public Transform CustomerStartTransform;
        public Transform CustomerEndTransform;
        public GameObject CustomerPrefab;
        public List<Seller> Sellers { get; set; }


        #endregion

        #region Initialization

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
            EconomyAndUpgrade = new EconomyAndUpgradeService(this, Store);
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

            OnChangedStatsOrMoney?.Invoke();
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
        #endregion

        #region EventsUpdate

        public void ChangedStatsOrMoney() {
            OnChangedStatsOrMoney?.Invoke();
            CheckPrebuilders();
            UiMode?.UpdateOnChangedStatsOrMoney();
        }
        // public void ChangeLevel() {
        //    // OnChangedLevelPlayer?.Invoke();
        //     OnChangedStatsOrMoney?.Invoke();
        //     CheckPrebuilders();
        //     UiMode?.UpdateOnChangedLevelPlayer();
        // }

        public void OnAnyInputControllerEvent() {
            UiMode?.OnAnyInputControllerEvent();
        }

        private void Update() {
            if (!_isInitialized) return;
            if (Game.Instance.IsPaused) return;
            _inputControlService.UpdateInputControl();
        }
        #endregion

        #region Factory

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

        #endregion

        #region CustomersRegistration

        public void CustomerLeftTradeSlot() {

            StartCoroutine(UpdateCustomersOnScene());
        }
        public void CustomerLeftScene(Customer customer) {
            ReturnCustomerToPool(customer);
            // StartCoroutine(UpdateCustomersOnScene());
        }

        private IEnumerator UpdateCustomersOnScene() {
            //Debug.Log($"CurrentActiveCustomers = {CurrentActiveCustomers}");
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

        #endregion

        #region PrebuildersDesktop
        private void CheckPrebuilders() {
            if (_prebuildersGO[_countOpenedPrebuilder] == null && _countOpenedPrebuilder<=_prebuildersGO.Length) {
                InitializeUnitsPrebuldersOnScene(_countOpenedPrebuilder);
            }
        }

        private bool CreateDesktopFromPrebuilder(PrebuilderDesktop prebuilderDesktop) {
            var newDesktopObj = Instantiate(DataMode.GetPrefabForDesktop()
                , prebuilderDesktop.gameObject.transform.position,
                Quaternion.identity);
            var newDesktop = newDesktopObj.GetComponentInChildren<DesktopUnit>();
            newDesktop.ProductType = prebuilderDesktop.ProductType;
            newDesktop.ConstructMain(this);
            SetupNewDesktopAndSlot(newDesktop, newDesktopObj);
           // prebuilderDesktop.ViewModel.HideWindow();
            
            _buttonAddCustomer.gameObject.SetActive(true);
            prebuilderDesktop.gameObject.SetActive(false);
            return true;
        }
        
        private void SetupNewDesktopAndSlot(DesktopUnit newDesktop, GameObject newDesktopObj) {
            var newDesktopSlot = newDesktopObj.GetComponentInChildren<DesktopSlot>();
            newDesktopSlot.ProductType = newDesktop.ProductType;
            Store.AddDesktop(newDesktop);
            Store.DesktopSlots.Add(newDesktopSlot);
            newDesktopObj.SetActive(true);
        }

        #endregion

        #region Desktops  

        public void NeedOpenAdditionalDesktop(DesktopUnit desktopMain) {
            CreateAdditionalDesktop(desktopMain);
        }

        private void CreateAdditionalDesktop(DesktopUnit desktopMain) {
            var newDesktopGO = Instantiate(DataMode.GetPrefabForDesktop(), desktopMain.AdditionalDesktopPointTransform.position
            , desktopMain.ContainerForRotate.transform.rotation); // Создаем новый объект стола
            var newDesktop = newDesktopGO.GetComponent<DesktopUnit>();
            newDesktop.IsAdditionalDesktop = true;
            // // Рассчитываем позицию для нового стола
            // var spriteRenderer = desktopMain.GetComponentInChildren<DesktopUnitMainSpriteRenderer>().GetComponent<SpriteRenderer>(); // Получаем компонент SpriteRenderer основного стола
            // if (spriteRenderer != null) {
            //     var spriteWidth = spriteRenderer.bounds.size.x; // Получаем ширину спрайта
            //     var newPosition = desktopMain.transform.position + new Vector3(spriteWidth, 0, 0); // Сдвигаем новый стол на ширину спрайта вправо
            //     newDesktopGO.transform.position = newPosition; // Устанавливаем позицию для нового стола
            // }

            SetupNewDesktopAndSlot(newDesktop, newDesktopGO, desktopMain);

        }

        private void SetupNewDesktopAndSlot(DesktopUnit newDesktop,
             GameObject newDesktopObj, DesktopUnit desktopMain) {

            newDesktop.ConstructAdditional(desktopMain);
            var newDesktopSlot = newDesktopObj.GetComponentInChildren<DesktopSlot>();
            newDesktopSlot.ProductType = desktopMain.ProductType;
            Store.AddDesktop(newDesktop);
            Store.DesktopSlots.Add(newDesktopSlot);
            newDesktopObj.SetActive(true);
        }

        #endregion

        #region Buttons




        public bool OnButtonBuyDesktop(PrebuilderDesktop prebuilderDesktop) {
            if (EconomyAndUpgrade.TryBuyPrebuilder(prebuilderDesktop)) {
                return CreateDesktopFromPrebuilder(prebuilderDesktop);
            }


            return false;
        }
        public bool OnButtonUpgradeDesktop(DesktopUnit desktopUnit) {
            return EconomyAndUpgrade.TryUpgradeDesktop(desktopUnit);

        }

        private void CreateSellers() {
            for (int i = 0; i < CountSellersForPool; i++) {
                InstantiateNewSeller();

            }
        }
        public void NewCustomerButton() {
            if (RequiredNumberCustomersOnScene >=_maxNuberCustomers) return;
            RequiredNumberCustomersOnScene++;
            StartCoroutine(UpdateCustomersOnScene());
        }
        #endregion

        #region Destroy
        private void OnDestroy() {
            OnChangedStatsOrMoney = null;
            // OnChangedLevelPlayer = null;
        }

        #endregion

        public void AddMoney()
        {
            EconomyAndUpgrade.Store.Stats.AddMoney(1000);
        }
    }
}