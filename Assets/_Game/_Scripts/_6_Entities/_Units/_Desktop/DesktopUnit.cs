using Assets._Game._Scripts._0.Data;
using Assets._Game._Scripts._5_Managers;
using Assets._Game._Scripts._6_Entities._Store;
using Assets._Game._Scripts._6_Entities._Store._Products;
using Assets._Game._Scripts._6_Entities._Units._Desktop._Base;
using UnityEngine;

namespace Assets._Game._Scripts._6_Entities._Units._Desktop {
    public class DesktopUnit : DesktopUnitBase
    {
        
        [SerializeField] private GameMode _gameMode;
        //[SerializeField] private UIMode _uiMode;
        [SerializeField] private ProductType _productType;
        public long Cost { get; set; }
        public int Level { get; set; } = 1;
        public long Money => _gameMode.Economy.Money;
        [SerializeField] private Order _order;
        [SerializeField] private UIDesktopViewModel _viewModel;
        [SerializeField] private UIDesktopView _view;
        private EconomyService _economy;

        public GameMode GameMode
        {
            get => _gameMode;
            set => _gameMode = value;
        }

        public ProductType ProductType
        {
            get => _productType;
            set => _productType = value;
        }

        void Awake()
        {

            GameMode = FindObjectOfType<GameMode>();
            _gameMode.OnChangedMoney += UpdateOnChangeMoney;
            _view = GetComponentInChildren<UIDesktopView>();
            _viewModel = new UIDesktopViewModel(this, _view);
            ViewModel = _viewModel;
            _economy = _gameMode.Economy;
            

            switch (ProductType)
            {
                case ProductType.MechanicalEyeProduct:
                    _order = new Order(null, new MechanicalEyeProduct(), 0);
                    _order.ProductType = ProductType.MechanicalEyeProduct;
                    break;
                case ProductType.RoboticArmProduct:
                    _order = new Order(null, new RoboticArmProduct(), 0);
                    _order.ProductType = ProductType.RoboticArmProduct;
                    break;
                case ProductType.IronHeartProduct:
                    _order = new Order(null, new IronHeartProduct(), 0);
                    _order.ProductType = ProductType.IronHeartProduct;
                    break;
                case ProductType.NeurochipProduct:
                    _order = new Order(null, new NeurochipProduct(), 0);
                    _order.ProductType = ProductType.MechanicalEyeProduct;
                    break;
                default:
                    Debug.Log("Нет такого продукта!");
                    break;
            }

            //UpdateOnChangeMoney();
        }

        private void SetCost()
        {
            Cost = _economy.SetCostBuyProductAndLevel(Level+1, _order.ProductType);

        }

        public void UpdateOnChangeMoney()
        {
            //if(_viewModel.IsOpenedWindow) return;
            SetCost();
            _viewModel.UpdateOnChangeMoney();
        }

        protected override void OnTouchAction()
        {
            

            // _viewModel.ShowWindow();
            // UpdateOnChangeMoney();
            

        }

        public void OnButtonUpgradeDesktop()
        {

            Level += GameMode.OnButtonUpgradeDesktop(this) ? 1 : 0;
            UpdateOnChangeMoney();
        }
    }
}
