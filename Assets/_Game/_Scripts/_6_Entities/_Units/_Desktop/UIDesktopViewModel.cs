using Assets._Game._Scripts._3_UI._UIUnits._Base;
using Assets._Game._Scripts._5_Managers;
using System;
using System.Linq;
using UnityEngine;

namespace Assets._Game._Scripts._6_Entities._Units._Desktop
{
    public class UIDesktopViewModel: UIUnitViewModel  
    {
        private  DesktopUnit _desktopModel;
       
        private UIMode _uiMode;
        private string _productName;
        private int _incomeValue;
        private float _progressStarsValue;
        private bool _windowOpenedByTouch;

       

        public UIDesktopViewModel(DesktopUnit desktopModelUnit, UIDesktopView view)
        {
            _desktopModel = desktopModelUnit;
            View = view;
            _uiMode = _desktopModel.GameMode.UiMode;
            View.Construct(this);
            View.Canvas.worldCamera = _desktopModel.GameMode.UiCamera;
        }

       
        public void UpdateOnChangeMoney()
        {
            _productName = _uiMode.GetStringNameByProductType(_desktopModel.ProductType);
            _incomeValue = _desktopModel.GameMode.DataMode.GetProductUpgradeSO(_desktopModel.ProductType)
                .Upgrades[_desktopModel.Level].IncomeMoney;
            _progressStarsValue = CalculateProgressToNextStar();
            View.UpdateOnChangeMoney(_desktopModel.Cost, _desktopModel.Level, _desktopModel.Money
                , _productName, _incomeValue, _desktopModel.GameMode.DataMode.GetProductUpgradeSO(_desktopModel.ProductType).Upgrades[_desktopModel.Level-1].Stars, _progressStarsValue);

        }

        public void CheckWindow()
        {
            
        }

        public override void ShowWindow()
        {
           // if(IsOpenedWindow) return;
            View.ShowWindow();
            _desktopModel.UpdateOnChangeMoney();
           // IsOpenedWindow = true;
            
        }

        public override void HideWindow()
        {
           // if (!IsOpenedWindow) return;
            View.HideWindow();
            //IsOpenedWindow = false;
        }

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

            Debug.Log($"Прогресс до следующей звезды: {progress} (Текущий уровень: {currentLevel}, Уровни до след. звезды: {levelsToNextStar}, Пройдено уровней: {completedLevels})");
            return progress;
        }





    }
}