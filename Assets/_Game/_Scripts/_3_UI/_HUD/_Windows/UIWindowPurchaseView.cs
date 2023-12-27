using Assets._Game._Scripts._2_Game;
using Assets._Game._Scripts._3_UI._UIUnits._Base;
using UnityEngine;

namespace Assets._Game._Scripts._3_UI._HUD._Windows
{
    // Окно покупок
    public class UIWindowPurchaseView : MonoBehaviour, IUiUnitView
    {
        public GameObject NOADSButton;
        public GameObject Profitx2Button;
        public void ShowWindow() {

            Debug.Log("Active Upgrade wWindow");
            gameObject.SetActive(true);
            UpdateState();
           
        }

        public void UpdateState()
        {
            if (Game.Instance.StoreStats.GameStats.PurchasedDisabledAds) NOADSButton.SetActive(false);
            if (Game.Instance.StoreStats.GameStats.PurchasedIncreaseProfit) Profitx2Button.SetActive(false);
            if (!NOADSButton.activeSelf && !Profitx2Button.activeSelf) gameObject.SetActive(false);
        }

        public void HideWindow() {
            Debug.Log("Disactive Upgrade wWindow");
            gameObject.SetActive(false);
        }
    }
}