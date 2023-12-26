using Assets._Game._Scripts._3_UI._UIUnits._Base;
using Assets._Game._Scripts._5_Managers;
using System;
using System.Linq;
using Assets._Game._Scripts._2_Game;
using UnityEngine;

namespace Assets._Game._Scripts._6_Entities._Units._Desktop
{
    public class DesktopViewModel: UnitViewModel  
    {
        public  DesktopUnit _desktop;
        private UIDesktopView _view;
        
        
        private UIMode _uiMode;
        private string _productName;
        private int _incomeValue;
        private float _progressStarsValue;
        private bool _windowOpenedByTouch;
        private int _maxStars;

       

        public DesktopViewModel(DesktopUnit desktopUnit, UIDesktopView view)
        {
            _desktop = desktopUnit;
            _view = view;
            _uiMode = _desktop.GameMode.UiMode;
            _maxStars = _desktop.GameMode.DataMode.GetMaxStarsForProductType(_desktop.ProductStoreType);
            _view.Construct(this, _maxStars);
            _view.Canvas.worldCamera = _desktop.GameMode.UiCamera;
        }

       
        public void UpdateOnChangeMoney()
        {
            var isButtonEnabled = _desktop.GameMode.DataMode.GameLevel >= _desktop.GameMode.DataMode.GetProductUpgradeSO(_desktop.ProductStoreType)
                .Upgrades[_desktop.Level].OpeningAtLevel;// проверка соответствует ли уровень игры уровню прокачки отдельного стола
            _productName = _uiMode.GetStringNameByProductType(_desktop.ProductStoreType);
            _incomeValue = _desktop.GameMode.DataMode.GetProductUpgradeSO(_desktop.ProductStoreType)
                .Upgrades[_desktop.Level-1].IncomeMoney;
            if (Game.Instance.StoreStats.GameStats.PurchasedIncreaseProfit)
            {
                _incomeValue *= 2;
            }

            if (_desktop.GameMode.EconomyAndUpgrade.IsBoostedFromRewarded)
            {
                _incomeValue *= 2;
            }
            _progressStarsValue = CalculateProgressToNextStar();
            _view.UpdateOnChangeMoney(_desktop.Cost, _desktop.Level, _desktop.GameMode.Coins
                , _productName, _incomeValue, _desktop.GameMode.DataMode.GetProductUpgradeSO(_desktop.ProductStoreType).Upgrades[_desktop.Level-1].Stars, _progressStarsValue, isButtonEnabled);

        }


        public override void ShowWindow()
        {
            // if(IsOpenedWindow) return;
            _uiMode.OpenNewViewModel(this);
            _view.ShowWindow();
            _desktop.UpdateOnChangeStatsOrMoney();
           
            // IsOpenedWindow = true;

        }
        public override void HideWindow() {
            _view.HideWindow();
        }
        // public override void HideWindow()
        // {
        //    // if (!IsOpenedWindow) return;
        //     View.HideWindow();
        //     //IsOpenedWindow = false;
        // }

        public void OnButtonUpgrade()
        {
            _desktop.OnButtonUpgradeDesktop();
        }

        public float CalculateProgressToNextStar() {
            var currentLevel = _desktop.Level; // Текущий уровень стола
            var upgradesData = _desktop.GameMode.DataMode.GetProductUpgradeSO(_desktop.ProductStoreType).Upgrades;

            // Находим текущее улучшение
            var currentUpgrade = upgradesData.FirstOrDefault(upgrade => upgrade.Level == currentLevel);
            if (currentUpgrade == null) {
                Debug.LogError("Текущее улучшение не найдено.");
                return 0f;
            }

            // Находим все улучшения с текущим количеством звезд
            var sameStarsUpgrades = upgradesData.Where(upgrade => upgrade.Stars == currentUpgrade.Stars).ToArray();
            var totalSameStarsUpgrades = sameStarsUpgrades.Length;

            // Находим позицию текущего улучшения среди улучшений с тем же количеством звезд
            var currentUpgradePosition = Array.IndexOf(sameStarsUpgrades, currentUpgrade);

            // Расчет прогресса
            float progress = (float)(currentUpgradePosition) / (totalSameStarsUpgrades - 1);

            return Mathf.Clamp01(progress);
        }



    }
}