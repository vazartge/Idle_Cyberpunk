using Assets._Game._Scripts._5_Managers;
using Assets._Game._Scripts._6_Entities._Store;
using Assets._Game._Scripts._6_Entities._Store._Products;
using Assets._Game._Scripts._6_Entities._Units._Base;
using UnityEngine;

namespace Assets._Game._Scripts._6_Entities._Units._PrebuilderDesktop {
    public class PrebuilderDesktop : BaseUnitGame {
        [Header("Тип продукта - ОБЯЗАТЕЛЬНО ЗАПОЛНИТЬ")]
        [SerializeField] private ProductType _productType;
        [Header("Угол поворота для стола против часовой стрелки")]
        public float RotationAngleZ = 0f;
        private GameMode _gameMode;
        private UIMode _uiMode;
        
         private int _price;
        private int _cost;

        private Order _order;
        private PrebuilderViewModel _viewModel;
        private UIPrebuilderView _view;
        
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

        public GameMode GameMode
        {
            get => _gameMode;
            set => _gameMode = value;
        }

        private void Awake()
        {
            _view = GetComponentInChildren<UIPrebuilderView>();
        }
        // Use this for initialization
        public override void Construct(GameMode gameMode, DataMode_ dataMode)
        {

            GameMode =gameMode;
            _dataMode = dataMode;
            _view = GetComponentInChildren<UIPrebuilderView>();
            _viewModel = new PrebuilderViewModel(this, _view, _uiMode);
            ViewModel = _viewModel;
            _view.Construct(_viewModel);
            
            Cost = GameMode.DataMode.GetProductUpgradeSO(ProductType).Upgrades[0].Cost; 
           

        }

        
        public void OnButtonBuyDesktop()
        {
            GameMode.OnButtonBuyDesktop(this);
        }

       
    }
}