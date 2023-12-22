using System.Collections.Generic;
using System.Linq;
using Assets._Game._Scripts._0.Data._DataForLevelsUpgrade;
using Assets._Game._Scripts._2_Game;
using Assets._Game._Scripts._3_UI._HUD;
using Assets._Game._Scripts._3_UI._HUD._Windows;
using Assets._Game._Scripts._3_UI._UIUnits._Base;
using Assets._Game._Scripts._3_UI._UpgradeButton;
using Assets._Game._Scripts._6_Entities._Store;
using Assets._Game._Scripts._6_Entities._Store._Products;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets._Game._Scripts._5_Managers {
    public class UIMode : MonoBehaviour {

        public TMP_InputField inputField; // ������ �� InputField  !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!! ������� ������� ������� ������� ������� ������� 
        public GameObject AvailabilityIndicatorForUpgradeWindows; // ������ �� ��������� ����������� ��� ������ ���� ��������
        public GameObject AvailabilityIndicatorForNextLevelButton;
        public EconomyAndUpgradeService EconomyAndUpgrade {
            get => _economyAndUpgrade;
            set => _economyAndUpgrade = value;
        }

        private UIHUDCanvas _hudCanvas;

        private DataMode_ _dataMode;

        private GameMode _gameMode;

        private EconomyAndUpgradeService _economyAndUpgrade;


        [SerializeField] private Transform upgradeButtonsContainer; // ������������ ������� ��� ������
        public GameObject UpgradeWindowGO;
        public GameObject NextLevelWindowGO;
        private UIWindowUpgradeView _upgradeWindowView;
        private UIWindowNextLevelView _nextLevelWinodwView;
        public GameObject OpenNextLevelWindowButton;
        public GameObject UpgradeButtonPrefab;
        private UpgradeButton[] _upgradeButtons;
        private bool _isInitUpdateButtons;
        public Dictionary<ProductStoreType, string> ProductTypeAndNameMap;

        private UnitViewModel _currentUnitViewModel;
        [SerializeField] private GameObject _closeWindowsButton;
        private IUiUnitView _currentUiUnitView;
        public GameMode GameMode {
            get => _gameMode;
            set => _gameMode = value;

        }
        // �����, ������� ����� �������, ��������, ��� ������ ������ ���� �����
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
     
        private void Start()
        {
            Game.Instance.RegisterUIMode(this);
            AvailabilityIndicatorForNextLevelButton.SetActive(false);
            AvailabilityIndicatorForUpgradeWindows.SetActive(false);
            _upgradeWindowView = UpgradeWindowGO.GetComponent<UIWindowUpgradeView>();
            _nextLevelWinodwView = NextLevelWindowGO.GetComponent<UIWindowNextLevelView>();
            _nextLevelWinodwView.Construct(this);
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


        public void Construct(DataMode_ dataMode, GameMode gameMode) {

            ProductTypeAndNameMap = new Dictionary<ProductStoreType, string>
            {
                {ProductStoreType.MechanicalEyeProduct, "������������ ����"},
                {ProductStoreType.RoboticArmProduct, "���������������� ����"},
                {ProductStoreType.IronHeartProduct, "�������� ������"},
                {ProductStoreType.NeurochipProduct, "��������"},
                // ��������� ���������
            };
            _dataMode = dataMode;
            GameMode = gameMode;


            _hudCanvas = GetComponent<UIHUDCanvas>();

            BeginPlay();
            InitializeUpgradeButtons();

        }

        private void BeginPlay() {
            _economyAndUpgrade = GameMode.EconomyAndUpgrade;
            _hudCanvas.Construct(this);
            GameMode.OnChangedStatsOrMoney += UpdateOnChangedStatsOrMoney;
            // _gameMode.OnChangedLevelPlayer += UpdateOnChangedLevelPlayer;
            UpdateOnChangedStatsOrMoney();
            CheckAvailabilityIndicatorForNextLevelButton();
            _gameMode.ChangedStatsOrMoney();
          

        }

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


        public void UpdateOnChangedStatsOrMoney() {
            _hudCanvas.UpdateUIHUD(_gameMode.EconomyAndUpgrade.Coins);
            CheckUpgradesAvailability();
            UpdateAllUpgradeButtons();
            CheckAvailabilityIndicatorForNextLevelButton();
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

        // private void CheckUpgradesAvailability() {
        //     if(!_isInitUpdateButtons) return;
        //     // �������� ������� ��������� ���������
        //     bool hasAvailableUpgrades = _upgradeButtons.Any(button => button.gameObject.activeSelf && !button._upgradeItem.IsPurchased && button._upgradeItem.Price <= _economyAndUpgrade.Coins);
        //
        //     // ��������� ���������� �����������, ���� ���� ��������� ���������
        //     AvailabilityIndicatorForUpgradeWindows.SetActive(hasAvailableUpgrades);
        // }
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



        // public void CloseUpgradeWindow() {
        //     _upgradeWindowView.SetActive(false);
        // }
        public void GenerateButtons(StoreStats storeStats) {
            var upgrades = storeStats.LevelUpgrade.GetAllUnpurchasedUpgrades();
            var sortedUpgrades = upgrades.OrderBy(u => u.Price).ToList();

            for (int i = 0; i < _upgradeButtons.Length; i++) {
                if (i < sortedUpgrades.Count) {
                    _upgradeButtons[i].Initialize(sortedUpgrades[i], this, _gameMode.HasDesktops());
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

        public void OnButtonResetSaves() {
            PlayerPrefs.DeleteAll();
            PlayerPrefs.Save(); // �� ������ ��������� ���������
        }

        public void CheckAvailabilityIndicatorForNextLevelButton()
        {
            if(!OpenNextLevelWindowButton.activeSelf) return;
            AvailabilityIndicatorForNextLevelButton.SetActive(Game.Instance.StoreStats.Coins >= GetCostBuyNextLevel()); 
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


        public bool GetEnoughMoney()
        {
            return EconomyAndUpgrade.Coins >= GetCostBuyNextLevel();
        }
    }
}