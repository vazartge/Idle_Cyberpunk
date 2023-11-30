using Assets._Game._Scripts._0.Data;
using Assets._Game._Scripts._5_Managers;
using Assets._Game._Scripts._6_Entities._Store;
using Assets._Game._Scripts._6_Entities._Store._Products;
using Assets._Game._Scripts._6_Entities._Units._Desktop._Base;
using Assets._Game._Scripts._6_Entities._Units._PrebuilderDesktop;
using UnityEngine;
using static UnityEditor.Profiling.HierarchyFrameDataView;

namespace Assets._Game._Scripts._6_Entities._Units._Desktop {
    public class DesktopUnit : DesktopUnitBase
    {
        
        [SerializeField] private GameMode _gameMode;
        //[SerializeField] private UIMode _uiMode;
        [SerializeField] private ProductType _productType;
        public long Cost { get; set; }
        public int Level { get; set; } = 1;
        [SerializeField] private Order _order;
        [SerializeField] private UIDesktopViewModel _viewModel;
        [SerializeField] private UIDesktopView _view;
        private EconomyService _economy;

        public GameMode GameMode
        {
            get => _gameMode;
            set => _gameMode = value;
        }

        void Start()
        {

            GameMode = FindObjectOfType<GameMode>();
            _gameMode.OnChangedMoney += UpdateOnChangeMoney;
            _view = GetComponentInChildren<UIDesktopView>();
            _viewModel = new UIDesktopViewModel(this, _view);
            _economy = _gameMode.Economy;
            _view.Construct(_viewModel);

            switch (_productType)
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

            Cost = _economy.SetCostBuyProductAndLevel(Level, _order.ProductType);
        }

        private void UpdateOnChangeMoney()
        {
            
        }

        protected override void OnTouchAction()
        {

            _viewModel.ShowWindow();
        }

        public void OnButtonUpgradeDesktop()
        {
            GameMode.OnButtonUpgradeDesktop(this);
        }
    }
}
