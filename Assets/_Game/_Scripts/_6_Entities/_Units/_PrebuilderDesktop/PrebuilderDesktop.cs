using Assets._Game._Scripts._0.Data;
using Assets._Game._Scripts._2_Game;
using Assets._Game._Scripts._5_Managers;
using Assets._Game._Scripts._6_Entities._Store;
using Assets._Game._Scripts._6_Entities._Store._Products;
using Assets._Game._Scripts._6_Entities._Units._Base;
using UnityEngine;

namespace Assets._Game._Scripts._6_Entities._Units._PrebuilderDesktop {
    public class PrebuilderDesktop : BaseUnitGame {
        [SerializeField] private GameMode _gameMode;
        [SerializeField] private UIMode _uiMode;
        [SerializeField] private ProductType _productType;
        [SerializeField] private int _price;
        [SerializeField] private int _cost;

        [SerializeField] private Order _order;
        [SerializeField] private UIPrebuilderViewModel _viewModel;
        [SerializeField] private UIPrebuilderView _view;
        private DataMode_ _dataMode;
        public bool IsActive { get; set;}

        public int Cost
        {
            get => _cost;
            set => _cost = value;
        }

        public ProductType ProductType
        {
            get => _productType;
            set => _productType = value;
        }

        private void Awake()
        {
            _view = GetComponentInChildren<UIPrebuilderView>();
        }
        // Use this for initialization
        public override void Construct(GameMode gameMode, DataMode_ dataMode)
        {

            _gameMode =gameMode;
            _dataMode = dataMode;
            _view = GetComponentInChildren<UIPrebuilderView>();
            _viewModel = new UIPrebuilderViewModel(this, _view);
            _view.Construct(_viewModel);
            
            Cost = _gameMode.DataMode.GetProductUpgradeSO(ProductType).Upgrades[0].Cost; 
           

        }
        protected override void OnTouchAction()
        {
           
            _viewModel.ShowWindow();
        }

        public void OnButtonBuyDesktop()
        {
            _gameMode.OnButtonBuyDesktop(this);
        }

       
    }
}