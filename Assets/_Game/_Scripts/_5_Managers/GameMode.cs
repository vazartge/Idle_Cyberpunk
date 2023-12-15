using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Assets._Game._Scripts._2_Game;
using Assets._Game._Scripts._4_Services;
using Assets._Game._Scripts._6_Entities._Store;
using Assets._Game._Scripts._6_Entities._Store._Products;
using Assets._Game._Scripts._6_Entities._Store._Slots;
using Assets._Game._Scripts._6_Entities._Units;
using Assets._Game._Scripts._6_Entities._Units._Customers;
using Assets._Game._Scripts._6_Entities._Units._Desktop;
using Assets._Game._Scripts._6_Entities._Units._PrebuilderDesktop;
using Assets._Game._Scripts._6_Entities._Units._Sellers;
using Unity.VisualScripting;
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
        public int GameLevel => Store.Stats.LevelGame;

        private bool _isInitialized;
        private int _idOrder;

        [Header("Refererences")]
        public Store Store;
        public long Coins => Store.Stats.Coins;
       
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
        private bool _isFirstDesktopCreate;

        [Header("PrebuildersDesktop")]
        [SerializeField] private GameObject[] _prebuildersGO;

        public bool IsOpenedAllPrebuilders => _prebuildersGO.Length <= _counterForUpgradelEvelOpenedPrebuilder;
        public int _counterForUpgradelEvelOpenedPrebuilder = 0;
        private int _countOpenedPrebuilder = 0;

        [Header("Desktops")]
        public Transform ParentForDesktops;


        [Header("Customers")]
        //[SerializeField] private GameObject _buttonAddCustomer;
        private int _countCustomerForID=0;
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
        private int _countSellerForID=0;
        // Текущее количество активных продавцов на сцене
        private int CurrentActiveSellers { get; set; }
        // Количество продавцов, которое надо создать при старте в пул
        private int CountSellersForPool { get; } = 1;
        public List<Seller> Sellers { get; set; }

        public int MaxNuberCustomers
        {
            get => _maxNuberCustomers;
            set => _maxNuberCustomers = value;
        }

        public int MaxNuberSellers
        {
            get => _maxNuberSellers;
            set => _maxNuberSellers = value;
        }

        [Header("StartPoints")]
        public Transform SellerStartTransform;
        public Transform CustomerStartTransform;
        public Transform CustomerEndTransform;
        public GameObject CustomerPrefab;
        public GameObject ButtonForNextLevel;

        [Header("Service")]
        public EconomyAndUpgradeService EconomyAndUpgrade;

        #endregion

        #region Initialization

        public void Construct(DataMode_ dataMode, UIMode uiMode, StoreStats storeStats) {
            _dataMode = dataMode;
            Debug.Log($"_dataMode != null {_dataMode!=null}");
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
            Store.Construct(storeStats);
            ActiveCustomers = new List<Customer>();
            Sellers = new List<Seller>();
            _customersPool = new Queue<Customer>();

          //  StartCoroutine(UpdateCustomersOnScene());
            EconomyAndUpgrade = new EconomyAndUpgradeService(this, Store);
            _inputControlService = new InputControlService(this);
            UiMode.Construct(_dataMode, this);

        }
        public void InitializeComponents() {
            OnChangedStatsOrMoney?.Invoke();
            StartCoroutine(InitializeUnitsPrebuldersOnScene(_countOpenedPrebuilder));
           
            AddSellerMainMethod();
           
            _isInitialized = true;
            Debug.Log("_gameMode Start");
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


        private void Update() {
            if (!_isInitialized) return;
            if (Game.Instance.IsPaused) return;
            CanTransToNextLevel();
            //_inputControlService.UpdateInputControl();

        }

        private void CanTransToNextLevel()
        {
            if (CanUpgradeLevel())
            {
                ShowButtonNextLevel();
            }
        }

        #endregion

        #region Factory

        // private void CreateCustomers() {
        //     for (int i = 0; i < CountCustomersForPool; i++) {
        //         InstantiateNewCustomer();
        //     }
        // }
        public void AddSellerMainMethod() {
            if (RequiredNumberSellersOnScene >= MaxNuberSellers) return;
            RequiredNumberSellersOnScene++;
            InstantiateNewSeller();
        }
        // Метод для создания нового покупателя
        private Customer InstantiateNewCustomer() {

            Customer customer = Instantiate(CustomerPrefab, CustomerStartTransform.position, Quaternion.identity).GetComponent<Customer>();
            customer.gameObject.transform.parent = Store.CustomersParentTransform;
            
            customer.ID = "CustomerID:" + _countCustomerForID;
            customer.Construct(this, Store, CustomerStartTransform, CustomerEndTransform
                , CharacterType.Seller, _countCustomerForID);
            _customersPool.Enqueue(customer); // Добавляем в пул
            customer.gameObject.SetActive(false); // Скрываем покупателя
            ActiveCustomers.Add(customer); // Добавляем в список для учета
            _countCustomerForID++;

            return customer;
        }

        private Seller InstantiateNewSeller() {
            Seller seller = Instantiate(SellerPrefab, SellerStartTransform.position, Quaternion.identity).GetComponent<Seller>();
            seller.gameObject.transform.parent = Store.SellersParentTransform;
            
            seller.ID = "SellerID:" + _countSellerForID;
            seller.Construct(this, Store, CharacterType.Seller, _countSellerForID);
            Sellers.Add(seller); // Добавляем в список для учета

            _countSellerForID++;

            return seller;
        }

        // Метод для получения текущего количества заказов по типам продуктов у активных покупателей
        private Dictionary<ProductType, int> GetCurrentOrdersCountByType() {
            // Возвращаем словарь с количеством текущих заказов по каждому типу продукта
            return ActiveCustomers
                .SelectMany(c => c?.Orders ?? Enumerable.Empty<Order>()) // Проверяем на null и используем пустую последовательность, если Orders == null
                .GroupBy(o => o.ProductType)
                .ToDictionary(g => g.Key, g => g.Count());
        }


        // Публичный метод для получения покупателя из пула
        public Customer GetCustomer() {
            if (ActiveCustomers == null || _customersPool.Count == 0)
            {
                InstantiateNewCustomer();
            }
            Customer customer = _customersPool.Dequeue();

            customer.gameObject.SetActive(true);
            var freeSlot = Store.GetFreeCustomerSlot();

            // Находим максимальное количество товара на уровень для одного покупателя
            int maxProductsPerCustomer = GetMaxProductsPerCustomer(GameLevel);

            // Находим максимально возможное количество товара одного типа исходя из количества продавцов
            int maxOrderCount = Sellers.Count * 3;
            Debug.Log($" Продавцов ===== {Sellers.Count}");

            // Получаем типы продуктов, для которых есть открытые столы
            var availableProductTypesWithDesks = Store.GetAvailableProductTypesWithDesks();

            // Получаем текущее количество заказов по типам продуктов у активных покупателей
            var currentOrdersCount = GetCurrentOrdersCountByType();

            // Фильтруем доступные типы продуктов на основе текущего количества заказов у покупателей
            var eligibleProductTypes = availableProductTypesWithDesks
                .Where(type => !currentOrdersCount.ContainsKey(type) || currentOrdersCount[type] < maxOrderCount)
                .ToList();
            // Добавляем логирование
            Debug.Log("Доступные для выбора типы продуктов и их текущее количество заказов:");
            foreach (var type in eligibleProductTypes) {
                int orderCount = currentOrdersCount.ContainsKey(type) ? currentOrdersCount[type] : 0;
                Debug.Log($"Тип продукта: {type}, Текущее количество заказов: {orderCount}");
            }

            ProductType? selectedType;

            if (eligibleProductTypes.Count > 0)
            {
                // Пытаемся выбрать случайный тип продукта из подходящих
                selectedType = _productRandomizerService.GetRandomProductType(eligibleProductTypes);
                Debug.Log($"Итоговый заказ: Заказ для покупателя {customer.ID} успешно создан: Тип продукта - {selectedType.Value}, Количество - {maxProductsPerCustomer}");

            } else {
                selectedType = Store.GetAlternativeProductType();
                Debug.Log($"Альтернативный заказ: Заказ для покупателя {customer.ID} успешно создан: Тип продукта - {selectedType.Value}, Количество - {maxProductsPerCustomer}");

            }

            // Создаем заказы
            var orders = CreateOrders(customer, selectedType.Value, maxProductsPerCustomer);
            customer.SetupCustomer(freeSlot, orders);

            // Увеличиваем количество активных покупателей
            CurrentActiveCustomers++;

            return customer;
        }

        public void DebugOrderCountsAndThreshold() {
            var orderCounts = Store.GetOrderCountsByProductType();
            int sellerThreshold = Sellers.Count * 3;

            Debug.Log($"Количество продавцов: {Sellers.Count}, Порог заказов на тип продукта: {sellerThreshold}");

            foreach (var productType in Enum.GetValues(typeof(ProductType))) {
                int currentOrderCount = orderCounts.ContainsKey((ProductType)productType) ? orderCounts[(ProductType)productType] : 0;
                Debug.Log($"Тип продукта: {productType}, Количество заказов: {currentOrderCount}, Порог: {sellerThreshold}");
            }
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
                return 1;
            }
        }

        private List<Order> CreateOrders(Customer customer, ProductType typeProduct, int count) {
            List<Order> orders = new List<Order>();
            for (int i = 0; i < count; i++) {
                _idOrder++;
                orders.Add(new Order(customer, typeProduct, _idOrder));
            }
            // Дебаг-лог для вывода информации о заказах
            return orders;
        }
        // Дополнительный метод для логирования информации о заказах
        #endregion

        #region CustomersRegistration

        public void CustomerLeftTradeSlot() {
        
         //   StartCoroutine(UpdateCustomersOnScene());
        }
        public void CustomerLeftScene(Customer customer) {
            ReturnCustomerToPool(customer);
            GetCustomer();
            // StartCoroutine(UpdateCustomersOnScene());
        }

        // private IEnumerator UpdateCustomersOnScene() {
        //     //Debug.Log($"CurrentActiveCustomers = {CurrentActiveCustomers}");
        //     //if (RequiredNumberCustomersOnScene == 0) yield break;
        //     //  if (!Store.HasFreeCustomerSlot()/*CurrentActiveCustomers >= _maxNuberCustomers*/) yield break;
        //     while (Store.GetNumberOfOccupiedCustomerSlots() < RequiredNumberCustomersOnScene &&  Store.HasFreeCustomerSlot()) {
        //         if (_customersPool.Count == 0) {
        //             // Создаем нового покупателя, если в пуле нет доступных
        //             InstantiateNewCustomer();
        //         }
        //
        //         // Получаем покупателя из пула и активируем его
        //         GetCustomer();
        //
        //         // Задержка перед появлением следующего покупателя
        //         yield return new WaitForSeconds(_timeRateGetCustomers);
        //     }
        // }

        // Публичный метод для возвращения покупателя в пул
        public void ReturnCustomerToPool(Customer customer) {
            customer.gameObject.SetActive(false); // Деактивируем покупателя
            _customersPool.Enqueue(customer); // Возвращаем в пул
            CurrentActiveCustomers--; // Уменьшаем число активных покупателей
            // Установка начального положения и ориентации покупателя
            customer.transform.position = CustomerStartTransform.position;
            customer.transform.rotation = CustomerStartTransform.rotation;


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
            var newDesktopObj = Instantiate(_dataMode.GetPrefabForDesktop()
                , prebuilderDesktop.gameObject.transform.position,
                Quaternion.identity);
            newDesktopObj.transform.parent = Store.DesktopsParentTransform;
            var newDesktop = newDesktopObj.GetComponentInChildren<DesktopUnit>();
            newDesktop.ContainerForRotate.transform.rotation =  Quaternion.Euler(0f, 0f, prebuilderDesktop.RotationAngleZ);
            newDesktop.ProductType = prebuilderDesktop.ProductType;
            newDesktop.ConstructMain(this);
            SetupNewDesktopAndSlot(newDesktop, newDesktopObj);
            // prebuilderDesktop.ViewModel.HideWindow();

            // _buttonAddCustomer.gameObject.SetActive(true);
            prebuilderDesktop.gameObject.SetActive(false);
            _counterForUpgradelEvelOpenedPrebuilder++;
            if (ActiveCustomers == null)
            {
                AddCustomer();
            }

            return true;
        }

        private void SetupNewDesktopAndSlot(DesktopUnit newDesktop, GameObject newDesktopObj) {
            var newDesktopSlot = newDesktopObj.GetComponentInChildren<DesktopSlot>();
            newDesktopSlot.ProductType = newDesktop.ProductType;
            Store.AddDesktop(newDesktop);
            Store.DesktopSlots.Add(newDesktopSlot);
            newDesktopObj.SetActive(true);

            if (!_isFirstDesktopCreate) {
                _isFirstDesktopCreate = true;
                Invoke("AddNewCustomerMainMethod", 0.5f);
                Invoke("AddNewCustomerMainMethod", 1f);
            }
        }

        #endregion

        #region Desktops  

        public void NeedOpenAdditionalDesktop(DesktopUnit desktopMain) {
            CreateAdditionalDesktop(desktopMain);
        }

        private void CreateAdditionalDesktop(DesktopUnit desktopMain) {
            var newDesktopGO = Instantiate(_dataMode.GetPrefabForDesktop(), desktopMain.AdditionalDesktopPointTransform.position
            , Quaternion.identity); // Создаем новый объект стола
            newDesktopGO.transform.parent = Store.DesktopsParentTransform;
            var newDesktop = newDesktopGO.GetComponent<DesktopUnit>();
            newDesktop.ContainerForRotate.transform.rotation = desktopMain.ContainerForRotate.transform.rotation;
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

     
        public void AddNewCustomerMainMethod() {
            if (RequiredNumberCustomersOnScene >=MaxNuberCustomers) return;
            RequiredNumberCustomersOnScene++;
            GetCustomer();
            //  StartCoroutine(UpdateCustomersOnScene());
        }
        #endregion

        #region Destroy
        private void OnDestroy() {
            OnChangedStatsOrMoney = null;
            // OnChangedLevelPlayer = null;
        }

        #endregion

        public void AddMoney() /// Удалить!!!!!!!!!!!!!!!!!!!!!!!!!
        {
            EconomyAndUpgrade.AddMoney(100000);
        }


        public void AddSeller() {
            AddSellerMainMethod();
        }

        public void AddCustomer() {
            AddNewCustomerMainMethod();
        }
        
        private void ShowButtonNextLevel() {
            ButtonForNextLevel.SetActive(true);
        }
        public bool CanUpgradeLevel()
        {
            return Store.AreAllDesktopsUpgradedForLevel();
        }
        public void OnNextLevelButton() {
            int nextLevel = Store.Stats.LevelGame + 1;
            long cost = 0;

            switch (nextLevel) {
                case 2:

                    cost = 2000;

                    break;
                case 3:
                    cost = 200000;
                    break;
                case 4:
                    cost = 3000000;
                    break;
                default:
                    // Если уровень не определен, возвращаем false.
                    break;
            }
            if(Coins<cost) return;
            EconomyAndUpgrade.RemoveMoney(cost);
            Store.Stats.LevelGame++;
            Game.Instance.NextLevelStart();
        }

        public bool HasDesktops() {
            return Store.HasActiveDesktops();
        }
    }
}