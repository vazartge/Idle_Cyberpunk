using Assets._Game._Scripts._0.Data;
using Assets._Game._Scripts._5_Managers;
using Assets._Game._Scripts._6_Entities._Store;
using Assets._Game._Scripts._6_Entities._Store._Products;
using Assets._Game._Scripts._6_Entities._Units._Base;
using UnityEngine;

namespace Assets._Game._Scripts._6_Entities._Units._PrebuilderDesktop {
    public class PrebuilderDesktop : UnitGame {
        [SerializeField] private GameMode _gameMode;
        [SerializeField] private UIMode _uiMode;
        [SerializeField] private ProductType _productType;
        [SerializeField] private int _price;
        [SerializeField] private Order _order;
        [SerializeField] private UIPrebuilderViewModel _viewModel;
        [SerializeField] private UIPrebuilderView _view;
        // Use this for initialization
        void Start()
        {

            _gameMode = FindObjectOfType<GameMode>();
            _view = GetComponentInChildren<UIPrebuilderView>();
            _viewModel = new UIPrebuilderViewModel(this, _view);
            _view.Construct(_viewModel);

            switch (_productType)
            {
                case ProductType.MechanicalEyeProduct:
                    _order = new Order(null, new MechanicalEyeProduct(), 0);
                    break;
                case ProductType.RoboticArmProduct:
                    _order = new Order(null, new RoboticArmProduct(), 0);
                    break;
                case ProductType.IronHeartProduct:
                    _order = new Order(null, new IronHeartProduct(), 0);
                    break;
                case ProductType.NeurochipProduct:
                    _order = new Order(null, new NeurochipProduct(), 0);
                    break;
                default:
                    Debug.Log("Нет такого продукта!");
                    break;
            }

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