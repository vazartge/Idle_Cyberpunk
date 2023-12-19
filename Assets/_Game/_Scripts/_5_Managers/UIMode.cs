using System.Collections.Generic;
using System.Linq;
using Assets._Game._Scripts._0.Data._DataForLevelsUpgrade;
using Assets._Game._Scripts._3_UI._HUD;
using Assets._Game._Scripts._3_UI._HUD._Windows;
using Assets._Game._Scripts._3_UI._UIUnits._Base;
using Assets._Game._Scripts._3_UI._UpgradeButton;
using Assets._Game._Scripts._6_Entities._Store;
using Assets._Game._Scripts._6_Entities._Store._Products;
using UnityEngine;
using UnityEngine.UI;

namespace Assets._Game._Scripts._5_Managers {
    public class UIMode : MonoBehaviour {
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
        public GameObject OpenNextLevelWinodwButton;
        public GameObject UpgradeButtonPrefab;
        private UpgradeButton[] _upgradeButtons;
        public Dictionary<ProductType, string> ProductTypeAndNameMap;

        private UnitViewModel _currentUnitViewModel;
        [SerializeField] private GameObject _closeWindowsButton;
        private IUiUnitView _currentUiUnitView;
        public GameMode GameMode {
            get => _gameMode;
            set => _gameMode = value;

        }

        private void Awake() {
           


        }

        private void Start()
        {
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
        }


        public void Construct(DataMode_ dataMode, GameMode gameMode) {

            ProductTypeAndNameMap = new Dictionary<ProductType, string>
            {
                {ProductType.MechanicalEyeProduct, "������������ ����"},
                {ProductType.RoboticArmProduct, "���������������� ����"},
                {ProductType.IronHeartProduct, "�������� ������"},
                {ProductType.NeurochipProduct, "��������"},
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
            UpdateAllUpgradeButtons();
        }


        public string GetStringNameByProductType(ProductType productType) {
            return ProductTypeAndNameMap.GetValueOrDefault(productType);
        }

        public void OpenUpgradeWindow() {
            Debug.Log("Open Upgrade wWindow");
            OpenNewView(_upgradeWindowView);
            GenerateButtons(_gameMode.Store.Stats);
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


        public int GetCostBuyNextLevel() {
            int costNextLevel;
            switch (GameMode.GameLevel + 1) {
                case 2:

                    costNextLevel = 2000;

                    break;
                case 3:
                    costNextLevel = 200000;
                    break;
                case 4:
                    costNextLevel = 3000000;
                    break;
                default:
                    // ���� ������� �� ���������, ���������� false.
                    costNextLevel = 100000000;
                    break;
            }

            return costNextLevel;
        }
        public void ShowButtonForNextLevel() {
            OpenNextLevelWinodwButton.SetActive(true);
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