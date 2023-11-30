using Assets._Game._Scripts._3_UI._UIUnits._Base;
using Assets._Game._Scripts._5_Managers;
using UnityEngine;

namespace Assets._Game._Scripts._6_Entities._Units._PrebuilderDesktop
{
    public class UIPrebuilderView: UiUnitView {
        [SerializeField] private GameObject _uiWindow;
        
        [SerializeField] private UIPrebuilderViewModel _viewModel;


        public void Construct(UIPrebuilderViewModel viewModel)
        {
            _viewModel = viewModel;
        }

        public void ShowWindow()
        {
            _uiWindow.SetActive(true);
        }

        public void OnButtonBuy()
        {
            
            _viewModel.OnButtonBuyDesktop();
        }
    }
}