using Assets._Game._Scripts._3_UI._UIUnits._Base;
using Assets._Game._Scripts._4_Services;
using Assets._Game._Scripts._5_Managers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets._Game._Scripts._3_UI._HUD._Windows
{
    public class UIWindowNextLevelView : MonoBehaviour, IUiUnitView{

        public Button NextLevelViewButton;
        public TMP_Text _costNextLevelOnButton;
        private UIMode _mode;
        Color activeColor = new Color(0.6666f, 0.9568f, 0.9803f, 1.0f);
        public void Construct(UIMode mode)
        {
            _mode = mode;
            NextLevelViewButton.onClick.AddListener(() => _mode.OnNextLevelButton());
        }

        public void UpdateUI()
        {
            bool enoughMoney = _mode.GetEnoughMoney();
            int cost = _mode.GetCostBuyNextLevel();
            if (enoughMoney)
            {
                NextLevelViewButton.interactable = true;
                _costNextLevelOnButton.color = activeColor;
            }
            else
            {
                NextLevelViewButton.interactable = false;
                _costNextLevelOnButton.color = Color.red;
            }

            _costNextLevelOnButton.text = NumberFormatterService.FormatNumber(cost).ToString();
        }
        

        public void ShowWindow()
        {
            UpdateUI();
            Debug.Log("Active Upgrade wWindow");
            gameObject.SetActive(true);
        }
        public void HideWindow()
        {
            Debug.Log("Disactive Upgrade wWindow");
            gameObject.SetActive(false);
        }
    }
}