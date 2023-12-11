using Assets._Game._Scripts._0.Data._DataForLevelsUpgrade;
using Assets._Game._Scripts._4_Services;
using Assets._Game._Scripts._5_Managers;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Assets._Game._Scripts._3_UI._UpgradeButton {
    public class UpgradeButton : MonoBehaviour {

        public TMP_Text UpgradeNameText;
        public TMP_Text PriceText;
        public Button ButtonComponent;
        private UIMode _uiMode;
        private UpgradeSeller _upgradeSeller;
        private UpgradeCustomer _upgradeCustomer;
        private ProductBoost _productBoost;
        private SpeedBoost _speedBoost;
        private IUpgradeItem _upgradeItem;
        public void Initialized(UIMode uiMode)
        {
            _uiMode = uiMode;
        }
        public void SetupTextsButton(string upgradeName, int cost, bool enoughCoins) {
            UpgradeNameText.text = upgradeName;
            PriceText.text = NumberFormatterService.FormatNumber(cost);
            PriceText.color = enoughCoins ? Color.black : Color.red;
            ButtonComponent.interactable = enoughCoins;
        }
        public void UpdateButtonState(long currentCoins) {
            bool enoughCoins = false;
            if (_upgradeSeller != null) {
                enoughCoins = _upgradeSeller.Price <= currentCoins;
                ButtonComponent.interactable = enoughCoins && !_upgradeSeller.IsPurchased;
            } else if (_productBoost != null) {
                enoughCoins = _productBoost.Price <= currentCoins;
                ButtonComponent.interactable = enoughCoins && !_productBoost.IsPurchased;
            } else if (_speedBoost != null) {
                enoughCoins = _speedBoost.Price <= currentCoins;
                ButtonComponent.interactable = enoughCoins && !_speedBoost.IsPurchased;
            }

            PriceText.color = enoughCoins ? Color.black : Color.red;
        }

        // Для обработки нажатия на кнопку UpgradeSeller
        public void InitializeUpgrade(UpgradeSeller upgradeSeller) {
            _upgradeSeller = upgradeSeller;
            SetupTextsButton(upgradeSeller.Name, upgradeSeller.Price, upgradeSeller.Price<=_uiMode.GameMode.Coins);
            ButtonComponent.onClick.AddListener(() => _uiMode.OnBuyUpgradeSeller(_upgradeSeller));
            
        }
        public void InitializeUpgrade(UpgradeCustomer upgradeCustomer) {
            _upgradeCustomer = upgradeCustomer;
            SetupTextsButton(upgradeCustomer.Name, upgradeCustomer.Price, upgradeCustomer.Price<=_uiMode.GameMode.Coins);
            ButtonComponent.onClick.AddListener(() => _uiMode.OnBuyUpgradeCustomer(_upgradeCustomer));
            
        }

        // Для обработки нажатия на кнопку ProductBoost
        public void InitializeUpgrade(ProductBoost productBoost) {
            _productBoost = productBoost;
            SetupTextsButton(productBoost.Name, productBoost.Price,productBoost.Price <=_uiMode.GameMode.Coins);
            ButtonComponent.onClick.AddListener(() => _uiMode.OnBuyProductBoost(_productBoost));
        }

        // Для обработки нажатия на кнопку SpeedBoost
        public void InitializeUpgrade(SpeedBoost speedBoost) {
            _speedBoost = speedBoost;
            SetupTextsButton(speedBoost.Name, speedBoost.Price, speedBoost.Price<=_uiMode.GameMode.Coins);
            ButtonComponent.onClick.AddListener(() => _uiMode.OnBuySpeedBoost(_speedBoost));
        }

    }
}