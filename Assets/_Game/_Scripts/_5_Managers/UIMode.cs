using System.Collections.Generic;
using Assets._Game._Scripts._0.Data;
using Assets._Game._Scripts._3_UI;
using Assets._Game._Scripts._3_UI._HUD;
using Assets._Game._Scripts._3_UI._UIUnits._Base;
using Assets._Game._Scripts._6_Entities._Units._Base;
using UnityEngine;

namespace Assets._Game._Scripts._5_Managers {
    public class UIMode : MonoBehaviour {
        public EconomyService Economy {
            get => _economy;
            set => _economy = value;
        }

        private UIHUDCanvas _hudCanvas;

        private DataMode_ _dataMode;

        private GameMode _gameMode;

        private EconomyService _economy;

        public Dictionary<ProductType, string> ProductTypeAndNameMap;

        private UIUnitViewModel _currentUnitViewModel;


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
            _economy = GameMode.Economy;
            _hudCanvas.Construct(this);
            GameMode.OnChangedMoney += UpdateOnChangedMoney;
            GameMode.OnChangedLevelPlayer += UpdateOnChangedLevelPlayer;
            UpdateOnChangedMoney();
        }

        public void OnAnyInputControllerEvent() {

        }

        public void UpdateOnChangedLevelPlayer()
        {
            
        }

    

        public void UpdateOnChangedMoney() {
            _hudCanvas.UpdateUIHUD(_gameMode.Economy.Money);
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
            if (touchable!=null) {
                BaseUnitGame touchBaseUnitGame = touchable as BaseUnitGame;
                var viewModel = touchBaseUnitGame.ViewModel;
                if (viewModel != _currentUnitViewModel) {
                    _currentUnitViewModel?.HideWindow();
                    _currentUnitViewModel = viewModel;
                    _currentUnitViewModel?.ShowWindow();
                    return;
                }
            }
          //  _currentUnitViewModel?.HideWindow();
            _currentUnitViewModel = null;


        }
    }
}