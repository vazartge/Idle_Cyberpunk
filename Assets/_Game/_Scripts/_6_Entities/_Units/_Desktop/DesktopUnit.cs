using Assets._Game._Scripts._5_Managers;
using Assets._Game._Scripts._6_Entities._Store;
using Assets._Game._Scripts._6_Entities._Units._Desktop._Base;
using UnityEngine;

namespace Assets._Game._Scripts._6_Entities._Units._Desktop {
    public class DesktopUnit : DesktopBaseUnitBase {
        public enum DesktopType {
            main, additional
        }
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

        public long Money => GameMode.EconomyAndUpgrade.Money;

        public bool IsAdditionalDesktop {
            get => _isAdditionalDesktop;
            set => _isAdditionalDesktop = value;
        }

        public DesktopUnit AdditionalDesktop
        {
            get => _additionalDesktop;
            set => _additionalDesktop = value;
        }

        public GameObject ContainerForRotate;
        [SerializeField] private DesktopType _desktopType;
        [SerializeField] public Transform AdditionalDesktopPointTransform;
        [SerializeField] private bool _isAdditionalDesktop;
        [SerializeField] private int _level = 1;
        [SerializeField] private ProductType _productType;
        [SerializeField] private SpriteRenderer _spriteIconProductType;
        private GameMode _gameMode;
        //[SerializeField] private UIMode _uiMode;
        private Order _order;
        private UIDesktopViewModel _viewModel;
        private UIDesktopView _view;
        // [SerializeField] private GameObject _additionalDesktopGO;

        private EconomyAndUpgradeService _economyAndUpgrade;
        private DesktopUnit _additionalDesktop;
        private DesktopUnit _mainDesktop;
        private long _cost;


        public void ConstructMain(GameMode gameMode) {
            _mainDesktop = this;
            _desktopType = DesktopType.main;
            
            GameMode = gameMode;
            
            _view = GetComponentInChildren<UIDesktopView>();
            _viewModel = new UIDesktopViewModel(this, _view);
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
            _viewModel = new UIDesktopViewModel(_mainDesktop, _view); 
            ViewModel = _viewModel;
           // _gameMode.OnChangedStatsOrMoney += UpdateOnChangeStatsOrMoney;
            _spriteIconProductType.sprite = _mainDesktop._spriteIconProductType.sprite;
            _mainDesktop._gameMode.OnChangedStatsOrMoney += UpdateOnChangeStatsOrMoney;



        }
      
        public void UpdateOnChangeStatsOrMoney() {
            //if(_viewModel.IsOpenedWindow) return;
            SetCost();
            _viewModel.UpdateOnChangeMoney();
        }


        public void OnButtonUpgradeDesktop() {

            var isSuccess = _mainDesktop.GameMode.OnButtonUpgradeDesktop(_mainDesktop);
            //UpdateOnChangeStatsOrMoney();
        }

        public void UpgradeLevelUp() {
            _mainDesktop.Level++;
            //UpdateOnChangeStatsOrMoney();
        }

       
        private void SetCost() {
            Cost = _mainDesktop._economyAndUpgrade.SetCostBuyProductAndLevel(_mainDesktop.Level+1, ProductType);

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
