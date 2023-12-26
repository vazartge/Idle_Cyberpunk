using System.Collections.Generic;
using System.Linq;
using Assets._Game._Scripts._0.Data._DataForLevelsUpgrade;
using Assets._Game._Scripts._2_Game;
using Assets._Game._Scripts._3_UI._HUD._Windows;
using Assets._Game._Scripts._3_UI._UIUnits._Base;
using Assets._Game._Scripts._3_UI._UpgradeButton;
using Assets._Game._Scripts._4_Services;
using Assets._Game._Scripts._6_Entities._Store._Products;
using TMPro;
using UnityEngine;

namespace Assets._Game._Scripts._5_Managers {
    public class UIMode : MonoBehaviour {
        [SerializeField] private TMP_Text _moneyText;
        public TMP_InputField inputField; // Ссылка на InputField  !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!! УДАЛИТЬ УДАЛИТЬ УДАЛИТЬ УДАЛИТЬ УДАЛИТЬ УДАЛИТЬ 
        public GameObject AvailabilityIndicatorForUpgradeWindows; // Ссылка на индикатор доступности для кнопки Окна прокачки
        public GameObject AvailabilityIndicatorForNextLevelButton;
        public EconomyAndUpgradeService EconomyAndUpgrade {
            get => _economyAndUpgrade;
            set => _economyAndUpgrade = value;
        }

        //[SerializeField]public UIHUDCanvas _hudCanvas;

        // private DataMode_ _dataMode;

        //  private GameMode _gameMode;

        private EconomyAndUpgradeService _economyAndUpgrade;


        [SerializeField] private Transform upgradeButtonsContainer; // Родительский элемент для кнопок
        public GameObject UpgradeWindowGO;
        public GameObject NextLevelWindowGO;
        public GameObject PurchaseWindowsGO;
        public GameObject UIWindowSettingsToggleViewGO;
        public GameObject OpenShopPurchaseButton;
        public GameObject GameOverWindowGO;
        private UIWindowUpgradeView _upgradeWindowView;
        private UIWindowNextLevelView _nextLevelWinodwView;
        private UIWindowPurchaseView _uiWindowPurchaseView;
        private UIWindowSettingsToggleView _uiWindowSettingsToggleView;
        public GameObject OpenNextLevelWindowButton;
        public GameObject UpgradeButtonPrefab;
        private UpgradeButton[] _upgradeButtons;
        private bool _isInitUpdateButtons;
        public Dictionary<ProductStoreType, string> ProductTypeAndNameMap;

        private UnitViewModel _currentUnitViewModel;
        [SerializeField] private GameObject _closeWindowsButton;
        private IUiUnitView _currentUiUnitView;
        public GameMode GameMode {
            get => Game.Instance.GameMode;
            set => Game.Instance.GameMode = value;

        }
        public DataMode_ DataMode {
            get => Game.Instance.DataMode;
            set => Game.Instance.DataMode = value;

        }
        // Метод, который можно вызвать, например, при потере фокуса поля ввода
        public void OnInputFieldChanged() {
            // Проверяем, можно ли преобразовать текст в long
            if (long.TryParse(inputField.text, out long result)) {
                Debug.Log("Преобразованное значение: " + result);
                _economyAndUpgrade.AddMoney(result);
            } else {
                Debug.Log("Введено некорректное значение. Поле будет обнулено.");
            }

            // Обнуляем поле ввода в любом случае
            inputField.text = "";
        }

        private void Awake()
        {
            Game.Instance.UIMode = this;
        }

        private void Start() {

            AvailabilityIndicatorForNextLevelButton.SetActive(false);
            AvailabilityIndicatorForUpgradeWindows.SetActive(false);
            _upgradeWindowView = UpgradeWindowGO.GetComponent<UIWindowUpgradeView>();
            _nextLevelWinodwView = NextLevelWindowGO.GetComponent<UIWindowNextLevelView>();
            _nextLevelWinodwView.Construct(this);
            _uiWindowPurchaseView = PurchaseWindowsGO.GetComponent<UIWindowPurchaseView>();
            _uiWindowSettingsToggleView = UIWindowSettingsToggleViewGO.GetComponent<UIWindowSettingsToggleView>();

            // Game.Instance.RegisterUIMode(this);
            Invoke("Construct",2f);
        }



        public void Construct(/*DataMode_ dataMode, GameMode gameMode*/) {

            ProductTypeAndNameMap = new Dictionary<ProductStoreType, string>
            {
                {ProductStoreType.MechanicalEyeProduct, "Механический глаз"},
                {ProductStoreType.RoboticArmProduct, "Роботизированная рука"},
                {ProductStoreType.IronHeartProduct, "Железное сердце"},
                {ProductStoreType.NeurochipProduct, "Нейрочип"},
                // остальные заполнить
            };
            /*DataMode = dataMode;
            GameMode = gameMode;*/

            BeginPlay();


        }

        private void BeginPlay() {
            _economyAndUpgrade = GameMode.EconomyAndUpgrade;
            //_hudCanvas.Construct(this);
            GameMode.OnChangedStatsOrMoney += UpdateOnChangedStatsOrMoney;
            // _gameMode.OnChangedLevelPlayer += UpdateOnChangedLevelPlayer;
            UpdateOnChangedStatsOrMoney();
            CheckAvailabilityIndicatorForNextLevelButton();
            GameMode.UpdateOnChangedStatsOrMoney();
            InitializeUpgradeButtons();

        }
        private void InitializeUpgradeButtons() {
            // Инициализация массива кнопок
            _upgradeButtons = new UpgradeButton[20];
            for (int i = 0; i < _upgradeButtons.Length; i++) {
                GameObject buttonObj = Instantiate(UpgradeButtonPrefab, upgradeButtonsContainer);
                _upgradeButtons[i] = buttonObj.GetComponent<UpgradeButton>();
                buttonObj.SetActive(false);
            }

            _isInitUpdateButtons = true;
            GenerateButtons(Game.Instance.StoreStats);
            UpdateAllUpgradeButtons();
            CheckUpgradesAvailability();
            UpdateOnChangedStatsOrMoney();
        }
        public void OpenNewView(IUiUnitView view) {
            // Если текущее окно открыто и совпадает с переданным view, закрываем его
            if (_currentUiUnitView != null && _currentUiUnitView == view) {
                _currentUiUnitView.HideWindow();
                _currentUiUnitView = null;
                _closeWindowsButton.SetActive(false); // Деактивируем кнопку закрытия окон
                return;
            }

            // Закрываем текущее окно UnitViewModel, если оно открыто
            if (_currentUnitViewModel != null) {
                _currentUnitViewModel.HideWindow();
                _currentUnitViewModel = null;
            }

            // Закрываем текущее окно IUiUnitView, если оно открыто
            if (_currentUiUnitView != null) {
                _currentUiUnitView.HideWindow();
            }

            // Устанавливаем новое текущее окно
            _currentUiUnitView = view;

            // Показываем новое окно, если view не null
            if (view != null) {
                view.ShowWindow();
                _closeWindowsButton.SetActive(true); // Активируем кнопку закрытия окон
            }
        }



        public void OpenNewViewModel(UnitViewModel viewModel) {
            // Проверяем, отличается ли новый viewModel от текущего
            if (_currentUnitViewModel != viewModel) {
                if (_currentUnitViewModel != null) {
                    _currentUnitViewModel.HideWindow(); // Закрываем текущий ViewModel
                }

                _currentUnitViewModel = viewModel; // Обновляем текущий ViewModel

                if (viewModel != null) {
                    viewModel.ShowWindow(); // Показываем новый ViewModel
                }
            }

            _closeWindowsButton.SetActive(viewModel != null); // Активируем или деактивируем кнопку закрытия окон
        }

        public void OnClosedAllWindowsButton() {
            if (_currentUnitViewModel != null) {
                _currentUnitViewModel.HideWindow();
                _currentUnitViewModel = null; // Обнуляем текущий ViewModel
            }

            if (_currentUiUnitView != null) {
                _currentUiUnitView.HideWindow();
                _currentUiUnitView = null; // Обнуляем текущий UiUnitView
            }

            _closeWindowsButton.SetActive(false);
        }


        public void UpdateOnChangedStatsOrMoney() {
            if (GameMode == null) return;
            //_hudCanvas.UpdateUIHUD(GameMode.EconomyAndUpgrade.Coins);
            CheckUpgradesAvailability();
            UpdateAllUpgradeButtons();
            CheckAvailabilityIndicatorForNextLevelButton();
            CheckShopPurchaseButton();
            CheckShopWindowButtons();
            _moneyText.text = NumberFormatterService.FormatNumber(GameMode.EconomyAndUpgrade.Coins);
        }

        private void CheckShopWindowButtons() {
            if (!PurchaseWindowsGO.activeSelf) return;
            _uiWindowPurchaseView.UpdateState();
        }

        private void CheckShopPurchaseButton() {
            if (Game.Instance.StoreStats.GameStats.PurchasedIncreaseProfit && Game.Instance.StoreStats.GameStats.PurchasedDisabledAds) {
                if (!OpenShopPurchaseButton.activeSelf) return;
                OpenShopPurchaseButton.SetActive(false);
            }
        }


        public string GetStringNameByProductType(ProductStoreType productStoreType) {
            return ProductTypeAndNameMap.GetValueOrDefault(productStoreType);
        }

        public void OpenUpgradeWindow() {
            Debug.Log("Open Upgrade wWindow");
            OpenNewView(_upgradeWindowView);
            GenerateButtons(Game.Instance.StoreStats);
            UpdateAllUpgradeButtons();


        }
        public void UpdateAllUpgradeButtons() {
            long currentCoins = GameMode.Coins;
            // Проверяем, что массив кнопок инициализирован
            if (_upgradeButtons != null) {
                foreach (var button in _upgradeButtons) {
                    // Проверяем, что кнопка активна и что у кнопки есть инициализированный элемент _upgradeItem
                    if (button.gameObject.activeSelf && button.HasInitializedItem()) {
                        button.UpdateButtonState(currentCoins);
                    }
                }
            }
        }

        // private void CheckUpgradesAvailability() {
        //     if(!_isInitUpdateButtons) return;
        //     // Проверка наличия доступных улучшений
        //     bool hasAvailableUpgrades = _upgradeButtons.Any(button => button.gameObject.activeSelf && !button._upgradeItem.IsPurchased && button._upgradeItem.Price <= EconomyAndUpgrade.Coins);
        //
        //     // Активация индикатора доступности, если есть доступные улучшения
        //     AvailabilityIndicatorForUpgradeWindows.SetActive(hasAvailableUpgrades);
        // }
        private void CheckUpgradesAvailability() {
            if (!_isInitUpdateButtons) return;
            bool hasAvailableUpgrades = false;

            foreach (var button in _upgradeButtons) {
                // Проверка, что у кнопки есть инициализированный элемент _upgradeItem
                if (!button.HasInitializedItem()) {
                    continue;
                }

                var upgradeItem = button._upgradeItem;
                bool hasEnoughCoins = upgradeItem.Price <= _economyAndUpgrade.Coins;
                bool isPurchased = upgradeItem.IsPurchased;

                // Проверка на UpgradeCustomer и наличие рабочих столов
                bool isUpgradeCustomer = upgradeItem is UpgradeCustomer;
                bool hasDesktops = GameMode.HasDesktops();

                if (!isPurchased && hasEnoughCoins && (!isUpgradeCustomer || (isUpgradeCustomer && hasDesktops))) {
                    hasAvailableUpgrades = true;
                    break;
                }
            }

            // Активация индикатора доступности
            AvailabilityIndicatorForUpgradeWindows.SetActive(hasAvailableUpgrades);
        }


        public void OnBuyUpgradeItem(IUpgradeItem upgradeItem) {
            if (upgradeItem is UpgradeSeller upgradeSeller) {
                _economyAndUpgrade.OnBuyUpgradeSeller(upgradeSeller);
            } else if (upgradeItem is UpgradeCustomer upgradeCustomer) {
                _economyAndUpgrade.OnBuyUpgradeCustomer(upgradeCustomer);
            } else if (upgradeItem is ProductBoost productBoost) {
                _economyAndUpgrade.OnBuyUpgradeProductionBoost(productBoost);
            } else if (upgradeItem is SpeedBoost speedBoost) {
                _economyAndUpgrade.OnBuyUpgradeSpeedBoost(speedBoost);
            }
            //После покупки деактивируем соответствующую кнопку - если надо
            foreach (var button in _upgradeButtons) {
                if (button.HasInitializedItem() && button._upgradeItem == upgradeItem) {
                    button.DeactivateButton();
                    break;
                }
            }
        }



        // public void CloseUpgradeWindow() {
        //     _upgradeWindowView.SetActive(false);
        // }
        public void GenerateButtons(StoreStats storeStats) {
            var upgrades = storeStats.LevelUpgrade.GetAllUnpurchasedUpgrades();
            var sortedUpgrades = upgrades.OrderBy(u => u.Price).ToList();

            for (int i = 0; i < _upgradeButtons.Length; i++) {
                if (i < sortedUpgrades.Count) {
                    _upgradeButtons[i].Initialize(sortedUpgrades[i], this, GameMode.HasDesktops());
                    _upgradeButtons[i].gameObject.SetActive(true);
                } else {
                    _upgradeButtons[i].gameObject.SetActive(false); // Если оставлять кнопки - это убрать
                    // Закомментированный код для оставления кнопок с IsPurchased=true в UI
                    // if (_upgradeButtons[i].HasInitializedItem() && _upgradeButtons[i]._upgradeItem.IsPurchased) {
                    //     _upgradeButtons[i].gameObject.SetActive(true);
                    // } else {
                    //     _upgradeButtons[i].gameObject.SetActive(false);
                    // }

                }
            }


        }

        public void OnButtonResetSaves() {
            PlayerPrefs.DeleteAll();
            PlayerPrefs.Save(); // Не забудь сохранить изменения
        }

        public void CheckAvailabilityIndicatorForNextLevelButton() {
            if (OpenNextLevelWindowButton == null) return;
            if (!OpenNextLevelWindowButton.activeSelf) return;
            AvailabilityIndicatorForNextLevelButton.SetActive(Game.Instance.StoreStats.GameStats.Coins >= GetCostBuyNextLevel());
        }
        public int GetCostBuyNextLevel() {
            int costNextLevel;
            switch (GameMode.GameLevel + 1) {
                case 2:

                    costNextLevel = 700;

                    break;
                case 3:
                    costNextLevel = 9000;
                    break;
                case 4:
                    costNextLevel = 300000;
                    break;
                default:
                    // Если уровень не определен, возвращаем false.
                    costNextLevel = 100000000;
                    break;
            }

            return costNextLevel;
        }
        public void ShowButtonForNextLevel() {
            OpenNextLevelWindowButton.SetActive(true);
            CheckAvailabilityIndicatorForNextLevelButton();
        }
        public void OnOpenWindowForNextLevelButton() {
            Debug.Log("Open Window For Next Level");
            OpenNewView(_nextLevelWinodwView);
        }
        public void OnNextLevelButton() {
            GameMode.OnNextLevelButton();
        }


        public bool GetEnoughMoney() {
            return EconomyAndUpgrade.Coins >= GetCostBuyNextLevel();
        }

        public void OnOpenShopPurchaseButton() {
            Debug.Log("Open Shop");
            OpenNewView(_uiWindowPurchaseView);
        }
        public void OnOpenSettingsButton() {
            Debug.Log("Open Settings Window");
            OpenNewView(_uiWindowSettingsToggleView);
        }
    }
}