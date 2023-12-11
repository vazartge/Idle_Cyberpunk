using System.Collections.Generic;
using System.Linq;
using Assets._Game._Scripts._0.Data._DataForLevelsUpgrade;
using Assets._Game._Scripts._3_UI._HUD;
using Assets._Game._Scripts._3_UI._UIUnits._Base;
using Assets._Game._Scripts._3_UI._UpgradeButton;
using Assets._Game._Scripts._6_Entities._Store;
using UnityEngine;

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
        public GameObject UpgradeWindow;
        public GameObject UpgradeButtonPrefab;
        private UpgradeButton[] _upgradeButtons;
        public Dictionary<ProductType, string> ProductTypeAndNameMap;

        private UIUnitViewModel _currentUnitViewModel;
        [SerializeField] private GameObject _closeWindowsButton;

        public GameMode GameMode {
            get => _gameMode;
            set => _gameMode = value;

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

        public void SetCurrentViewModel(UIUnitViewModel viewModel) {
            
            if (_currentUnitViewModel != null ) {
                _currentUnitViewModel.HideWindow();
            }
            _closeWindowsButton.SetActive(true);
            // ������ ����� _currentUnitViewModel
            _currentUnitViewModel = viewModel;

            // // ���� viewModel �� null, ���������� ����
            // if (viewModel != null) {
            //     viewModel.ShowWindow();
            // }
        }

        public void OnClosedAllWindowsButton()
        {
            if (_currentUnitViewModel != null) {
                _currentUnitViewModel.HideWindow();
            }
            _closeWindowsButton.SetActive(false);
        }

        public void OnAnyInputControllerEvent() {

        }

        public void UpdateOnChangedLevelPlayer() {

        }



        public void UpdateOnChangedStatsOrMoney() {
            _hudCanvas.UpdateUIHUD(_gameMode.EconomyAndUpgrade.Coins);
            UpdateAllUpgradeButtons();
        }


        public string GetStringNameByProductType(ProductType productType) {
            return ProductTypeAndNameMap.GetValueOrDefault(productType);
        }

       
        public void UpdateAllUpgradeButtons() {
            if (_upgradeButtons == null)return;
            long currentCoins = GameMode.Coins;
            foreach (var button in _upgradeButtons) {
                button.UpdateButtonState(currentCoins);
            }
        }

        public void OnBuyUpgradeSeller(UpgradeSeller upgradeSeller)
        {
            _economyAndUpgrade.OnBuyUpgradeSeller(upgradeSeller);
        }
        public void OnBuyUpgradeCustomer(UpgradeCustomer upgradeCustomer) {
            _economyAndUpgrade.OnBuyUpgradeCustomer(upgradeCustomer);
        }
        public void OnBuyProductBoost(ProductBoost productBoost)
        {
            _economyAndUpgrade.OnBuyUpgradeProductionBoost(productBoost);
        }

        public void OnBuySpeedBoost(SpeedBoost speedBoost)
        {
            _economyAndUpgrade.OnBuyUpgradeSpeedBoost(speedBoost);
        }

        public void OpenUpgradeWindow()
        {
            GenerateButtons(_gameMode.Store.Stats);
            UpgradeWindow.SetActive(true);

        }

        // public void CloseUpgradeWindow()
        // {
        //     UpgradeWindow.SetActive(false);
        // }
        public void GenerateButtons(StoreStats storeStats) {
            var upgrades = storeStats.LevelUpgrade.GetAllUnpurchasedUpgrades();

            // ���������� �� ���������
            var sortedUpgrades = upgrades.OrderBy(u => u.Price).ToList();

            for (int i = 0; i < _upgradeButtons.Length; i++) {
                if (i < sortedUpgrades.Count) {
                    var upgrade = sortedUpgrades[i];
                    _upgradeButtons[i].Initialized(this); // ������������� ��������� UI
                    if (upgrade is UpgradeSeller upgradeOption) {
                        _upgradeButtons[i].InitializeUpgrade(upgradeOption); // ������������� ������ ��� UpgradeSeller
                    } else if (upgrade is ProductBoost productBoost) {
                        _upgradeButtons[i].InitializeUpgrade(productBoost); // ������������� ������ ��� ProductBoost
                    } else if (upgrade is SpeedBoost speedBoost) {
                        _upgradeButtons[i].InitializeUpgrade(speedBoost); // ������������� ������ ��� SpeedBoost
                    }
                    _upgradeButtons[i].UpdateButtonState(GameMode.Coins); // ���������� ��������� ������
                    _upgradeButtons[i].gameObject.SetActive(true);
                } else {
                    _upgradeButtons[i].gameObject.SetActive(false);
                }
            }
        }


       
    }
}