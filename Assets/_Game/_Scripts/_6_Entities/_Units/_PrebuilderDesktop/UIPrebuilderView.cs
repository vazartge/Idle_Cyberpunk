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
        
        [SerializeField] private PrebuilderViewModel _viewModel;
        [SerializeField] private TMP_Text _typeProduct;
        [SerializeField] private TMP_Text _cost;
        [SerializeField] private Button _buyButton;
        private UICameraScript _uiCamera;

        // Создание цвета
        Color activeColor = new Color(0.761f, 0.957f, 0.980f, 1.0f);
        public void Construct(PrebuilderViewModel viewModel)
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
                _typeProduct.text =
                    _viewModel.UiMode.GetStringNameByProductType(_viewModel.PrebuilderDesktop.ProductStoreType);
                _cost.color = Color.red;
                _buyButton.interactable = false;
            }
            else
            {
                _cost.color = activeColor;
                _buyButton.interactable = true;
            }
            _cost.text = NumberFormatterService.FormatNumber(cost);
        }

        public override void HideWindow()
        {
            if(_uiWindow == null) return;
            _uiWindow.SetActive(false);
        }

        public void OnButtonBuy()
        {
            
            _viewModel.OnButtonBuyDesktop();
        }
    }
}