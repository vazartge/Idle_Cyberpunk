using Assets._Game._Scripts._3_UI;
using Assets._Game._Scripts._3_UI._UIUnits._Base;
using Assets._Game._Scripts._4_Services;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets._Game._Scripts._6_Entities._Units._PrebuilderDesktop
{
    public class UIPrebuilderView: UiUnitView {
        [SerializeField] private GameObject _uiWindow;
        
        [SerializeField] private UIPrebuilderViewModel _viewModel;

        [SerializeField] private TMP_Text _cost;
        [SerializeField] private Button _buyButton;
        private UICameraScript _uiCamera;


        public void Construct(UIPrebuilderViewModel viewModel)
        {
            _viewModel = viewModel;
            _uiCamera = GameObject.FindObjectOfType<UICameraScript>();
            _uiWindow.GetComponentInChildren<Canvas>().worldCamera = _uiCamera.GetComponentInChildren<Camera>();
        }

        public void ShowWindow(int cost, bool enough)
        {
            _uiWindow.SetActive(true);
            if (!enough)
            {
                _cost.color = Color.red;
                _buyButton.interactable = false;
            }
            else
            {
                _cost.color = Color.black;
                _buyButton.interactable = true;
            }
            _cost.text = NumberFormatterService.FormatNumber(cost);
        }

        public override void HideWindow()
        {
            _uiWindow.SetActive(false);
        }

        public void OnButtonBuy()
        {
            
            _viewModel.OnButtonBuyDesktop();
        }
    }
}