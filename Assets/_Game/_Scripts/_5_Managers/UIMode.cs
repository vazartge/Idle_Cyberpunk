using System.Collections.Generic;
using Assets._Game._Scripts._0.Data;
using Assets._Game._Scripts._3_UI;
using Assets._Game._Scripts._3_UI._HUD;
using Assets._Game._Scripts._3_UI._UIUnits._Base;
using Assets._Game._Scripts._6_Entities._Units._Base;
using Assets._Game._Scripts._6_Entities._Units._Desktop;
using UnityEngine;
using UnityEngine.UIElements;

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

        public Dictionary<ProductType, string> ProductTypeAndNameMap;

        private UIUnitViewModel _currentUnitViewModel;
        [SerializeField] private GameObject _closeWindowsButton;

        public GameMode GameMode {
            get => _gameMode;
            set => _gameMode = value;

        }


        public void Construct(DataMode_ dataMode, GameMode gameMode) {

            ProductTypeAndNameMap = new Dictionary<ProductType, string>
            {
                {ProductType.MechanicalEyeProduct, "Механический глаз"},
                // остальные заполнить
            };
            _dataMode = dataMode;
            GameMode = gameMode;


            _hudCanvas = GetComponent<UIHUDCanvas>();

            BeginPlay();
        }

        private void BeginPlay() {
            _economyAndUpgrade = GameMode.EconomyAndUpgrade;
            _hudCanvas.Construct(this);
            GameMode.OnChangedStatsOrMoney += UpdateOnChangedStatsOrMoney;
            // GameMode.OnChangedLevelPlayer += UpdateOnChangedLevelPlayer;
            UpdateOnChangedStatsOrMoney();
        }

        public void SetCurrentViewModel(UIUnitViewModel viewModel) {
            
            if (_currentUnitViewModel != null ) {
                _currentUnitViewModel.HideWindow();
            }
            _closeWindowsButton.SetActive(true);
            // Задаем новый _currentUnitViewModel
            _currentUnitViewModel = viewModel;

            // // Если viewModel не null, показываем окно
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
            _hudCanvas.UpdateUIHUD(_gameMode.EconomyAndUpgrade.Money);
        }


        public string GetStringNameByProductType(ProductType productType) {
            return ProductTypeAndNameMap.GetValueOrDefault(productType);
        }

        public void TouchInput(IUnitTouchable touchable/*, Canvas canvas)*/) {
            // Debug.Log($"canvas == null   {canvas == null}");
            // if (touchable == null && _currentUnitViewModel!=null && canvas != null && _currentUnitViewModel.View.Canvas == canvas) {
            //    // _currentUnitViewModel?.HideWindow();//вот это место закрывает мой ui текущего объекта
            //    // _currentUnitViewModel = null;
            //    Debug.Log("!!!!!!!!!!!!");
            //     return;
            // }

            //touchable.OnTouch();
            // if (touchable!=null) {
            //     BaseUnitGame touchBaseUnitGame = touchable as BaseUnitGame;
            //     var viewModel = touchBaseUnitGame.ViewModel;
            //     if (viewModel != _currentUnitViewModel) {
            //         _currentUnitViewModel?.HideWindow();
            //         _currentUnitViewModel = viewModel;
            //         _currentUnitViewModel?.ShowWindow();
            //         return;
            //     }
            // }
            //  _currentUnitViewModel?.HideWindow();
            // _currentUnitViewModel = null;


        }
    }
}