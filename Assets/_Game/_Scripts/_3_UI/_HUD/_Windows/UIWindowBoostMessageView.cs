using Assets._Game._Scripts._3_UI._UIUnits._Base;
using TMPro;
using UnityEngine;

namespace Assets._Game._Scripts._3_UI._HUD._Windows {
    public class UIWindowBoostMessageView : MonoBehaviour, IUiUnitView {

        public TMP_Text TextMessage;
        public void UpdateUI(string messageText) {
            TextMessage.text = messageText;
        }

        public void ShowWindow() {
           
            Debug.Log("Active Upgrade wWindow");
            gameObject.SetActive(true);
        }
        public void HideWindow() {
            Debug.Log("Disactive Upgrade wWindow");
            gameObject.SetActive(false);
        }
    }
}