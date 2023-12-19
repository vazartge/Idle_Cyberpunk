using Assets._Game._Scripts._0.Data._DataForLevelsUpgrade;
using Assets._Game._Scripts._4_Services;
using Assets._Game._Scripts._5_Managers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets._Game._Scripts._3_UI._UpgradeButton {
    public class UpgradeButton : MonoBehaviour {

        public TMP_Text UpgradeNameText;
        public TMP_Text PriceText;
        public Button ButtonComponent;
        private UIMode _uiMode;
        
        public IUpgradeItem _upgradeItem;

        private void OnEnable()
        {
            UpdateButtonState(_uiMode.GameMode.Coins);
        }
        public void Initialize(IUpgradeItem upgradeItem, UIMode uiMode, bool hasDesktops) {
            _uiMode = uiMode;
            _upgradeItem = upgradeItem;

            SetupTextsButton(upgradeItem.Name, upgradeItem.Price, upgradeItem.Price <= _uiMode.GameMode.Store.Stats.Coins);

            // Дополнительная логика для кнопок UpgradeCustomer
            if (upgradeItem is UpgradeCustomer && !hasDesktops) {
                ButtonComponent.interactable = false; // Делаем кнопку неактивной, если это UpgradeCustomer и нет столов
            } else {
                ButtonComponent.interactable = true;
            }

            ButtonComponent.onClick.AddListener(() => uiMode.OnBuyUpgradeItem(_upgradeItem));
        }

        public void SetupTextsButton(string upgradeName, int cost, bool enoughCoins, bool hasDesktops = true) {
            UpgradeNameText.text = upgradeName;
            PriceText.text = NumberFormatterService.FormatNumber(cost);
            PriceText.color = enoughCoins ? Color.black : Color.red;

            // Дополнительная проверка для UpgradeCustomer
            if (_upgradeItem is UpgradeCustomer && !hasDesktops) {
                ButtonComponent.interactable = false;
            } else {
                ButtonComponent.interactable = enoughCoins;
            }
        }

        public void UpdateButtonState(long currentCoins) {
            // Проверка, есть ли рабочие столы
            bool hasDesktops = _uiMode.GameMode.HasDesktops();

            // Проверка, достаточно ли монет и не куплен ли уже данный элемент
            bool enoughCoins = _upgradeItem.Price <= currentCoins;
            bool isPurchased = _upgradeItem.IsPurchased;

           
            
            ButtonComponent.interactable = enoughCoins && !isPurchased;
            

            // Обновление цвета текста цены
            PriceText.color = enoughCoins ? Color.black : Color.red;

            // Если элемент - UpgradeCustomer и нет рабочих столов, кнопка неактивна
            if (_upgradeItem is UpgradeCustomer && !hasDesktops) {
                ButtonComponent.interactable = false;
            }
        }

        // Добавьте этот метод, чтобы проверить, инициализирован ли _upgradeItem
        public bool HasInitializedItem() {
            return _upgradeItem != null;
        }
       // Метод для деактивации кнопки
        public void DeactivateButton() {
            gameObject.SetActive(false);
        }
    }
}