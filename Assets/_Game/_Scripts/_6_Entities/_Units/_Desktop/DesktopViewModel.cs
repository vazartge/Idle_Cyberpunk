using Assets._Game._Scripts._3_UI._UIUnits._Base;
using Assets._Game._Scripts._5_Managers;
using System;
using System.Linq;
using UnityEngine;

namespace Assets._Game._Scripts._6_Entities._Units._Desktop
{
    public class DesktopViewModel: UnitViewModel  
    {
        public  DesktopUnit _desktopModel;
        private UIDesktopView _view;
        
        
        private UIMode _uiMode;
        private string _productName;
        private int _incomeValue;
        private float _progressStarsValue;
        private bool _windowOpenedByTouch;
        private int _maxStars;

       

        public DesktopViewModel(DesktopUnit desktopModelUnit, UIDesktopView view)
        {
            _desktopModel = desktopModelUnit;
            _view = view;
            _uiMode = _desktopModel.GameMode.UiMode;
            _maxStars = _desktopModel.GameMode.DataMode.GetMaxStarsForProductType(_desktopModel.ProductType);
            _view.Construct(this, _maxStars);
            _view.Canvas.worldCamera = _desktopModel.GameMode.UiCamera;
        }

       
        public void UpdateOnChangeMoney()
        {
            var isButtonEnabled = _desktopModel.GameMode.DataMode.GameLevel >= _desktopModel.GameMode.DataMode.GetProductUpgradeSO(_desktopModel.ProductType)
                .Upgrades[_desktopModel.Level].OpeningAtLevel;// проверка соответствует ли уровень игры уровню прокачки отдельного стола
            _productName = _uiMode.GetStringNameByProductType(_desktopModel.ProductType);
            _incomeValue = _desktopModel.GameMode.DataMode.GetProductUpgradeSO(_desktopModel.ProductType)
                .Upgrades[_desktopModel.Level].IncomeMoney;
            _progressStarsValue = CalculateProgressToNextStar();
            _view.UpdateOnChangeMoney(_desktopModel.Cost, _desktopModel.Level, _desktopModel.Money
                , _productName, _incomeValue, _desktopModel.GameMode.DataMode.GetProductUpgradeSO(_desktopModel.ProductType).Upgrades[_desktopModel.Level-1].Stars, _progressStarsValue, isButtonEnabled);

        }


        public override void ShowWindow()
        {
            // if(IsOpenedWindow) return;
            _uiMode.OpenNewViewModel(this);
            _view.ShowWindow();
            _desktopModel.UpdateOnChangeStatsOrMoney();
           
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
            _desktopModel.OnButtonUpgradeDesktop();
        }

        public float CalculateProgressToNextStar() {
            var currentLevel = _desktopModel.Level;
            var upgradesData = _desktopModel.GameMode.DataMode.GetProductUpgradeSO(_desktopModel.ProductType).Upgrades;

            // Находим текущее улучшение и следующее улучшение по звездам
            var currentUpgrade = upgradesData.FirstOrDefault(upgrade => upgrade.Level == currentLevel);
            var nextStarUpgrade = upgradesData.FirstOrDefault(upgrade => upgrade.Stars > currentUpgrade.Stars);

            if (nextStarUpgrade == null) {
                Debug.Log("Следующее улучшение не найдено, текущий уровень - максимальный");
                return 1f;
            }

            // Ищем индекс первого улучшения с текущим количеством звезд
            int firstLevelWithCurrentStar = Array.FindIndex(upgradesData, upgrade => upgrade.Stars == currentUpgrade.Stars);
            if (firstLevelWithCurrentStar == -1) {
                Debug.LogError("Ошибка при поиске первого уровня с текущим количеством звезд");
                return 0f;
            }

            // Расчет прогресса
            int levelsToNextStar = nextStarUpgrade.Level - upgradesData[firstLevelWithCurrentStar].Level;
            int completedLevels = currentLevel - upgradesData[firstLevelWithCurrentStar].Level;
            float progress = completedLevels / (float)levelsToNextStar;

          //  Debug.Log($"Прогресс до следующей звезды: {progress} (Текущий уровень: {currentLevel}, Уровни до след. звезды: {levelsToNextStar}, Пройдено уровней: {completedLevels})");
            return progress;
        }





    }
}