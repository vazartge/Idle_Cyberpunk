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

        #region FIELDS

        public GameObject UpgradeWindowGO;
        public GameObject NextLevelWindowGO;
        public GameObject PurchaseWindowsGO;
        public GameObject UIWindowSettingsToggleViewGO;
        public GameObject OpenShopPurchaseButton;
        public GameObject GameOverWindowGO;
        public TMP_InputField inputField; // ������ �� InputField  !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!! ������� ������� ������� ������� ������� ������� 
        public GameObject AvailabilityIndicatorForUpgradeWindows; // ������ �� ��������� ����������� ��� ������ ���� ��������
        public GameObject AvailabilityIndicatorForNextLevelButton;
        public GameObject OpenNextLevelWindowButton;
        public GameObject UpgradeButtonPrefab;
        public Dictionary<ProductStoreType, string> ProductTypeAndNameMap;

        [SerializeField] private TMP_Text _moneyText;
        [SerializeField] private Transform upgradeButtonsContainer; // ������������ ������� ��� ������
        [SerializeField] private GameObject _closeWindowsButton;

        private EconomyAndUpgradeService _economyAndUpgrade;
        private UIWindowUpgradeView _upgradeWindowView;
        private UIWindowNextLevelView _nextLevelWinodwView;
        private UIWindowPurchaseView _uiWindowPurchaseView;
        private UIWindowSettingsToggleView _uiWindowSettingsToggleView;
        private UpgradeButton[] _upgradeButtons;
        private bool _isInitUpdateButtons;
        private UnitViewModel _currentUnitViewModel;
        private IUiUnitView _currentUiUnitView;


        #endregion

        #region PROPERTIES

        public EconomyAndUpgradeService EconomyAndUpgrade {
            get => _economyAndUpgrade;
            set => _economyAndUpgrade = value;
        }
        public GameMode GameMode {
            get => Game.Instance.GameMode;
            set => Game.Instance.GameMode = value;

        }
        public DataMode_ DataMode {
            get => Game.Instance.DataMode;
            set => Game.Instance.DataMode = value;

        }
        #endregion

        #region INITIALIZATIONS

        private void Awake() {
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
            Invoke("Construct", 2f);
        }


        public void Construct(/*DataMode_ dataMode, GameMode gameMode*/) {

            ProductTypeAndNameMap = new Dictionary<ProductStoreType, string>
            {
                {ProductStoreType.MechanicalEyeProduct, "������������ ����"},
                {ProductStoreType.RoboticArmProduct, "���������������� ����"},
                {ProductStoreType.IronHeartProduct, "�������� ������"},
                {ProductStoreType.NeurochipProduct, "��������"},
                // ��������� ���������
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
            // ������������� ������� ������
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
        #endregion

        #region OPEN CLOSE WINDOWS


        public void OpenNewView(IUiUnitView view) {
            // ���� ������� ���� ������� � ��������� � ���������� view, ��������� ���
            if (_currentUiUnitView != null && _currentUiUnitView == view) {
                _currentUiUnitView.HideWindow();
                _currentUiUnitView = null;
                _closeWindowsButton.SetActive(false); // ������������ ������ �������� ����
                return;
            }

            // ��������� ������� ���� UnitViewModel, ���� ��� �������
            if (_currentUnitViewModel != null) {
                _currentUnitViewModel.HideWindow();
                _currentUnitViewModel = null;
            }

            // ��������� ������� ���� IUiUnitView, ���� ��� �������
            if (_currentUiUnitView != null) {
                _currentUiUnitView.HideWindow();
            }

            // ������������� ����� ������� ����
            _currentUiUnitView = view;

            // ���������� ����� ����, ���� view �� null
            if (view != null) {
                view.ShowWindow();
                _closeWindowsButton.SetActive(true); // ���������� ������ �������� ����
            }
        }



        public void OpenNewViewModel(UnitViewModel viewModel) {
            // ���������, ���������� �� ����� viewModel �� ��������
            if (_currentUnitViewModel != viewModel) {
                if (_currentUnitViewModel != null) {
                    _currentUnitViewModel.HideWindow(); // ��������� ������� ViewModel
                }

                _currentUnitViewModel = viewModel; // ��������� ������� ViewModel

                if (viewModel != null) {
                    viewModel.ShowWindow(); // ���������� ����� ViewModel
                }
            }

            _closeWindowsButton.SetActive(viewModel != null); // ���������� ��� ������������ ������ �������� ����
        }
        public void ShowButtonForNextLevel() {
            OpenNextLevelWindowButton.SetActive(true);
            CheckAvailabilityIndicatorForNextLevelButton();
        }
        public void OnClosedAllWindowsButton() {
            if (_currentUnitViewModel != null) {
                _currentUnitViewModel.HideWindow();
                _currentUnitViewModel = null; // �������� ������� ViewModel
            }

            if (_currentUiUnitView != null) {
                _currentUiUnitView.HideWindow();
                _currentUiUnitView = null; // �������� ������� UiUnitView
            }

            _closeWindowsButton.SetActive(false);
        }

        #endregion

        #region UPDATES CHECKERS


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


        public void UpdateAllUpgradeButtons() {
            long currentCoins = GameMode.Coins;
            // ���������, ��� ������ ������ ���������������
            if (_upgradeButtons != null) {
                foreach (var button in _upgradeButtons) {
                    // ���������, ��� ������ ������� � ��� � ������ ���� ������������������ ������� _upgradeItem
                    if (button.gameObject.activeSelf && button.HasInitializedItem()) {
                        button.UpdateButtonState(currentCoins);
                    }
                }
            }
        }

        private void CheckUpgradesAvailability() {
            if (!_isInitUpdateButtons) return;
            bool hasAvailableUpgrades = false;

            foreach (var button in _upgradeButtons) {
                // ��������, ��� � ������ ���� ������������������ ������� _upgradeItem
                if (!button.HasInitializedItem()) {
                    continue;
                }

                var upgradeItem = button._upgradeItem;
                bool hasEnoughCoins = upgradeItem.Price <= _economyAndUpgrade.Coins;
                bool isPurchased = upgradeItem.IsPurchased;

                // �������� �� UpgradeCustomer � ������� ������� ������
                bool isUpgradeCustomer = upgradeItem is UpgradeCustomer;
                bool hasDesktops = GameMode.HasDesktops();

                if (!isPurchased && hasEnoughCoins && (!isUpgradeCustomer || (isUpgradeCustomer && hasDesktops))) {
                    hasAvailableUpgrades = true;
                    break;
                }
            }

            // ��������� ���������� �����������
            AvailabilityIndicatorForUpgradeWindows.SetActive(hasAvailableUpgrades);
        }
        public void GenerateButtons(StoreStats storeStats) {
            var upgrades = storeStats.LevelUpgrade.GetAllUnpurchasedUpgrades();
            var sortedUpgrades = upgrades.OrderBy(u => u.Price).ToList();

            for (int i = 0; i < _upgradeButtons.Length; i++) {
                if (i < sortedUpgrades.Count) {
                    _upgradeButtons[i].Initialize(sortedUpgrades[i], this, GameMode.HasDesktops());
                    _upgradeButtons[i].gameObject.SetActive(true);
                } else {
                    _upgradeButtons[i].gameObject.SetActive(false); // ���� ��������� ������ - ��� ������
                    // ������������������ ��� ��� ���������� ������ � IsPurchased=true � UI
                    // if (_upgradeButtons[i].HasInitializedItem() && _upgradeButtons[i]._upgradeItem.IsPurchased) {
                    //     _upgradeButtons[i].gameObject.SetActive(true);
                    // } else {
                    //     _upgradeButtons[i].gameObject.SetActive(false);
                    // }

                }
            }


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
                    // ���� ������� �� ���������, ���������� false.
                    costNextLevel = 100000000;
                    break;
            }

            return costNextLevel;
        }
        public bool GetEnoughMoney() {
            return EconomyAndUpgrade.Coins >= GetCostBuyNextLevel();
        }

        #endregion

        #region BUTTONS CALLS


        public void OpenUpgradeWindow() {
            Debug.Log("Open Upgrade wWindow");
            OpenNewView(_upgradeWindowView);
            GenerateButtons(Game.Instance.StoreStats);
            UpdateAllUpgradeButtons();


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
            //����� ������� ������������ ��������������� ������ - ���� ����
            foreach (var button in _upgradeButtons) {
                if (button.HasInitializedItem() && button._upgradeItem == upgradeItem) {
                    button.DeactivateButton();
                    break;
                }
            }
        }


        public void OnButtonResetSaves() {
            PlayerPrefs.DeleteAll();
            PlayerPrefs.Save(); // �� ������ ��������� ���������
        }



        public void OnOpenWindowForNextLevelButton() {
            Debug.Log("Open Window For Next Level");
            OpenNewView(_nextLevelWinodwView);
        }
        public void OnNextLevelButton() {
            GameMode.OnNextLevelButton();
        }


        public void OnOpenShopPurchaseButton() {
            Debug.Log("Open Shop");
            OpenNewView(_uiWindowPurchaseView);
        }
        public void OnOpenSettingsButton() {
            Debug.Log("Open Settings Window");
            OpenNewView(_uiWindowSettingsToggleView);
        }
        // �����, ������� ��������� ����� ��� ����� ����� input field
        public void OnInputFieldChanged() {
            // ���������, ����� �� ������������� ����� � long
            if (long.TryParse(inputField.text, out long result)) {
                Debug.Log("��������������� ��������: " + result);
                _economyAndUpgrade.AddMoney(result);
            } else {
                Debug.Log("������� ������������ ��������. ���� ����� ��������.");
            }

            // �������� ���� ����� � ����� ������
            inputField.text = "";
        }
        #endregion
    }
}