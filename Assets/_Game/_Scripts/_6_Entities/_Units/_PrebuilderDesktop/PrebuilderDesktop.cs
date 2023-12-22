using Assets._Game._Scripts._5_Managers;
using Assets._Game._Scripts._6_Entities._Store;
using Assets._Game._Scripts._6_Entities._Store._Products;
using Assets._Game._Scripts._6_Entities._Units._Base;
using UnityEngine;

namespace Assets._Game._Scripts._6_Entities._Units._PrebuilderDesktop {
    public class PrebuilderDesktop : BaseUnitGame {
        public GameObject AvailableIndicator;

        [Header("Тип продукта - ОБЯЗАТЕЛЬНО ЗАПОЛНИТЬ")]
        [SerializeField] private ProductStoreType _productStoreType;
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
        public bool IsActive = true;

        public int Cost {
            get => _cost;
            set => _cost = value;
        }

        public ProductStoreType ProductStoreType {
            get => _productStoreType;
            set => _productStoreType = value;
        }

        public GameMode GameMode {
            get => _gameMode;
            set => _gameMode = value;
        }

        public bool IsDesktopPurchased = false;

        private void Awake() {
            _view = GetComponentInChildren<UIPrebuilderView>();
            // Game.Instance.RegisterPrebuilder(this);

        }
        // Use this for initialization
        public override void Construct(GameMode gameMode, DataMode_ dataMode) {

            GameMode =gameMode;
            _dataMode = dataMode;
            _view = GetComponentInChildren<UIPrebuilderView>();
            _viewModel = new PrebuilderViewModel(this, _view, _uiMode);
            ViewModel = _viewModel;
            _view.Construct(_viewModel);
            _gameMode.OnChangedStatsOrMoney  += UpdateOnChangeStatsOrMoney;
            Cost = GameMode.DataMode.GetProductUpgradeSO(ProductStoreType).Upgrades[0].Cost;

            UpdateOnChangeStatsOrMoney();
        }

        private void UpdateOnChangeStatsOrMoney() {
            UpdateViewAvailabilityIndicator();
        }

        private void UpdateViewAvailabilityIndicator() {
            AvailableIndicator.SetActive(IsActive && GameMode.Coins >= GameMode.DataMode.GetProductUpgradeSO(ProductStoreType).Upgrades[0].Cost);
        }

        public void OnButtonBuyDesktop() {
            GameMode.OnButtonBuyDesktop(this);
        }

        public void PurchasedDesktopSetBool() {
            IsDesktopPurchased = true;
        }

        public void SetDeactivateChildTransform(bool value) {
            // предполагая, что 'prebuilder' это ваш GameObject
            foreach (Transform child in this.transform) {
                child.gameObject.SetActive(value);
                IsActive = value;
            }
        }



    }


}