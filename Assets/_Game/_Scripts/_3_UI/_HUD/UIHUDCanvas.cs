using Assets._Game._Scripts._4_Services;
using Assets._Game._Scripts._5_Managers;
using TMPro;
using UnityEngine;

namespace Assets._Game._Scripts._3_UI
{
    public class UIHUDCanvas : MonoBehaviour
    {
        private UIMode _uiMode;
        [SerializeField] private TMP_Text _moneyText;

        public void Construct(UIMode uiMode)
        {
            _uiMode = uiMode;
          
            
        }

        public void UpdateUIHUD(long money)
        {
            Debug.Log(money);
            _moneyText.text = NumberFormatterService.FormatNumber(money);
            
        }
    }
}