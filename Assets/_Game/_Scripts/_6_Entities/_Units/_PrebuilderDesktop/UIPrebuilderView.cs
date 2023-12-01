using Assets._Game._Scripts._3_UI._UIUnits._Base;
using Assets._Game._Scripts._4_Services;
using Assets._Game._Scripts._5_Managers;
using TMPro;
using UnityEngine;

namespace Assets._Game._Scripts._6_Entities._Units._PrebuilderDesktop
{
    public class UIPrebuilderView: UiUnitView {
        [SerializeField] private GameObject _uiWindow;
        
        [SerializeField] private UIPrebuilderViewModel _viewModel;

        [SerializeField] private TMP_Text _cost;


        public void Construct(UIPrebuilderViewModel viewModel)
        {
            _viewModel = viewModel;
        }

        public void ShowWindow(int cost)
        {
            _uiWindow.SetActive(true);
            _cost.text = NumberFormatterService.FormatNumber(cost);
        }

        public void OnButtonBuy()
        {
            
            _viewModel.OnButtonBuyDesktop();
        }
    }
}