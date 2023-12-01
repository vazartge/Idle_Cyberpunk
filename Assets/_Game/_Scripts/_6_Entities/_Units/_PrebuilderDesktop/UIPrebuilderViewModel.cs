using Assets._Game._Scripts._3_UI._UIUnits._Base;
using TMPro;
using UnityEngine;

namespace Assets._Game._Scripts._6_Entities._Units._PrebuilderDesktop
{
    public class UIPrebuilderViewModel: UIUnitViewModel
    {
        private PrebuilderDesktop _prebuilderDesktop;
        private UIPrebuilderView _view;
       
        public UIPrebuilderViewModel(PrebuilderDesktop prebuilderDesktop, UIPrebuilderView view) {
            _prebuilderDesktop=prebuilderDesktop;
            _view = view;
            
        }

        public void ShowWindow()
        {
            _view.ShowWindow(_prebuilderDesktop.Cost);
        }

        public void OnButtonBuyDesktop()
        {
            _prebuilderDesktop.OnButtonBuyDesktop();
        }
    }
}