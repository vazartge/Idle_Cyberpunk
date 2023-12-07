using Assets._Game._Scripts._2_Game;
using Assets._Game._Scripts._5_Managers;
using Assets._Game._Scripts._6_Entities._Store;
using Assets._Game._Scripts._6_Entities._Units._Desktop._Base;
using UnityEngine;

namespace Assets._Game._Scripts._6_Entities._Units._Desktop {
    public class DesktopUnit : DesktopBaseUnitBase {

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

        [SerializeField] private GameMode _gameMode;
        //[SerializeField] private UIMode _uiMode;
        [SerializeField] private ProductType _productType;
        [SerializeField] private Order _order;
        [SerializeField] private UIDesktopViewModel _viewModel;
        [SerializeField] private UIDesktopView _view;
        [SerializeField] private GameObject _additionalDesktopGO;

        [SerializeField] private SpriteRenderer _spriteIconProductType;
        [SerializeField] private bool _isAdditionalDesktop;

        private EconomyAndUpgradeService _economyAndUpgrade;
        private DesktopUnit _additionalDesktop;
        private long _cost;
        private int _level = 1;


        public void Construct(GameMode gameMod, ProductType type) {

            GameMode =gameMod;
            ProductType = type;
            _gameMode.OnChangedStatsOrMoney += UpdateOnChangeStatsOrMoney;
            _view = GetComponentInChildren<UIDesktopView>();
            _viewModel = new UIDesktopViewModel(this, _view);
            ViewModel = _viewModel;
            _economyAndUpgrade = _gameMode.EconomyAndUpgrade;

            _spriteIconProductType.sprite = GameMode.DataMode.GetIconByProductType(ProductType);
            if (!_isAdditionalDesktop)
            {
                _additionalDesktop = _additionalDesktopGO.GetComponent<DesktopUnit>();
            }
            
        }


        public void UpdateOnChangeStatsOrMoney() {
            //if(_viewModel.IsOpenedWindow) return;
            SetCost();
            _viewModel.UpdateOnChangeMoney();
        }


        public void OnButtonUpgradeDesktop() {

            var isSuccess = GameMode.OnButtonUpgradeDesktop(this);
            //UpdateOnChangeStatsOrMoney();
        }

        public void UpgradeLevelUp() {
            Level++;
            //UpdateOnChangeStatsOrMoney();
        }
       
        public GameObject GetAdditionalDesktopGO()
        {
            return _additionalDesktopGO;
        }

        public DesktopUnit GetAdditionalDesktopScript() {
            return _additionalDesktop;
        }
        private void SetupAdditionalDesktop(ProductType productType, GameMode gameMode)
        {
            ProductType = productType;
            GameMode = gameMode;
        }

        private void SetCost() {
            Cost = _economyAndUpgrade.SetCostBuyProductAndLevel(Level+1, ProductType);

        }
        protected override void OnTouchAction() {
            // _viewModel.ShowWindow();
            // UpdateOnChangeStatsOrMoney();

        }


      
    }
}
