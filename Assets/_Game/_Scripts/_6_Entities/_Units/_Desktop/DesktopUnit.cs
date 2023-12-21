using System;
using System.Linq;
using Assets._Game._Scripts._5_Managers;
using Assets._Game._Scripts._6_Entities._Store;
using Assets._Game._Scripts._6_Entities._Store._Products;
using Assets._Game._Scripts._6_Entities._Units._Desktop._Base;
using UnityEngine;


public enum DesktopType {
    main, additional
}
namespace Assets._Game._Scripts._6_Entities._Units._Desktop {
    public class DesktopUnit : DesktopBaseUnitBase {
        [SerializeField]
        public bool IsUpgradedForLevel = false;
        
        public GameMode GameMode {
            get => _gameMode;
            set => _gameMode = value;
        }

        public ProductType ProductType {
            get => _productType;
            set => _productType = value;
        }

        public long Cost {
            get => _cost;
            set => _cost = value;
        }

        public int Level {
            get => _level;
            set => _level = value;
        }

        public long Money => GameMode.EconomyAndUpgrade.Coins;

        public bool IsAdditionalDesktop {
            get => _isAdditionalDesktop;
            set => _isAdditionalDesktop = value;
        }

        public DesktopUnit AdditionalDesktop {
            get => _additionalDesktop;
            set => _additionalDesktop = value;
        }

        public DesktopType CurDesktopType{
            get => _desktopType;
            set => _desktopType = value;
        }

        public GameObject ContainerForRotate;
        public GameObject AvailabilityIndicator;
        [SerializeField] public DesktopType _desktopType;
        [SerializeField] public Transform AdditionalDesktopPointTransform;
        [SerializeField] private bool _isAdditionalDesktop;
        [SerializeField] private int _level = 1;
        [SerializeField] private ProductType _productType;
        [SerializeField] private SpriteRenderer _spriteIconProductType;

        private GameMode _gameMode;
        //[SerializeField] private UIMode UiMode;
        private Order _order;
        private DesktopViewModel _viewModel;
        private UIDesktopView _view;
        // [SerializeField] private GameObject _additionalDesktopGO;

        private EconomyAndUpgradeService _economyAndUpgrade;
        private DesktopUnit _additionalDesktop;
        public DesktopUnit _mainDesktop;
        private long _cost;


        public void ConstructMain(GameMode gameMode) {
            _mainDesktop = this;
            _desktopType = DesktopType.main;

            GameMode = gameMode;

            _view = GetComponentInChildren<UIDesktopView>();
            _viewModel = new DesktopViewModel(this, _view);
            ViewModel = _viewModel;
            _economyAndUpgrade = _gameMode.EconomyAndUpgrade;
            _spriteIconProductType.sprite = GameMode.DataMode.GetIconByProductType(ProductType);
            _gameMode.OnChangedStatsOrMoney += UpdateOnChangeStatsOrMoney;
        }
        public void ConstructAdditional(DesktopUnit desktopMain) {

            _mainDesktop = desktopMain;
            _mainDesktop.AdditionalDesktop = this;
            _desktopType = DesktopType.additional;
            IsAdditionalDesktop = true;
            _mainDesktop.IsAdditionalDesktop = IsAdditionalDesktop;
            _view = GetComponentInChildren<UIDesktopView>();
            _viewModel = new DesktopViewModel(_mainDesktop, _view);
            ViewModel = _viewModel;
            // _gameMode.OnChangedStatsOrMoney += UpdateOnChangeStatsOrMoney;
            _spriteIconProductType.sprite = _mainDesktop._spriteIconProductType.sprite;
            _mainDesktop._gameMode.OnChangedStatsOrMoney += UpdateOnChangeStatsOrMoney;



        }
        private void UpdateViewAvailabilityIndicator() {
            if (this == _mainDesktop) {
                bool res = GameMode.Coins >= GameMode.DataMode
                    .GetProductUpgradeSO(_mainDesktop.ProductType).Upgrades[_mainDesktop.Level].Cost && !_mainDesktop.IsUpgradedForLevel;
                _mainDesktop.AvailabilityIndicator.SetActive(res);
                if (_additionalDesktop != null) {
                    _additionalDesktop.AvailabilityIndicator.SetActive(res);
                }

            }
        }

        public void UpdateOnChangeStatsOrMoney() {
            //if(_viewModel.IsOpenedWindow) return;
            SetCost();
            UpdateViewAvailabilityIndicator();
            _viewModel.UpdateOnChangeMoney();
            _additionalDesktop?.UpdateOnChangeStatsOrMoney();
        }


        public void OnButtonUpgradeDesktop() {

            var isSuccess = _mainDesktop.GameMode.OnButtonUpgradeDesktop(_mainDesktop);
            //UpdateOnChangeStatsOrMoney();
        }

        public void UpgradeLevelUp() {
            _mainDesktop.Level++;
            //Level++;
            // Находим данные об уровне прокачки стола, соответствующего его текущему уровню
            var upgradeData = _gameMode.DataMode.DataForUpgradeDesktopsMap[_mainDesktop.ProductType];

            // Находим данные об уровне прокачки стола, соответствующего его текущему уровню
            var currentUpgradeData = upgradeData.Upgrades[_mainDesktop.Level];

            // Проверяем, существуют ли данные для данного уровня и не превышает ли уровень игры OpeningAtLevel
            if (currentUpgradeData != null && _gameMode.GameLevel < currentUpgradeData.OpeningAtLevel) {
                _mainDesktop.IsUpgradedForLevel = true;
                IsUpgradedForLevel = true;
            }


            Debug.Log($"IsUpgradedForLevel == {IsUpgradedForLevel}");
        }



        private void SetCost() {
            Cost = _mainDesktop._economyAndUpgrade.SetCostBuyProductAndLevel(_mainDesktop.Level, ProductType);

        }
        // protected override void OnTouchAction() {
        //     _viewModel.ShowWindow();
        //     UpdateOnChangeStatsOrMoney();
        //
        // }
        // public override void OnPointerClick(PointerEventData eventData) {
        //     _viewModel.ShowWindow();
        //     UpdateOnChangeStatsOrMoney();
        // }



    }
}
