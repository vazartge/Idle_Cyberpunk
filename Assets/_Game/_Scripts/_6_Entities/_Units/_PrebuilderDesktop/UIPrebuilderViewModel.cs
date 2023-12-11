using Assets._Game._Scripts._3_UI;
using Assets._Game._Scripts._3_UI._UIUnits._Base;
using TMPro;
using UnityEngine;

namespace Assets._Game._Scripts._6_Entities._Units._PrebuilderDesktop
{
    public class UIPrebuilderViewModel: UIUnitViewModel
    {
        private PrebuilderDesktop _prebuilderDesktop;
        public UiUnitView View => _view;
        private UIPrebuilderView _view;
       

        public UIPrebuilderViewModel(PrebuilderDesktop prebuilderDesktop, UIPrebuilderView view) {
            _prebuilderDesktop=prebuilderDesktop;
            _view = view;
            
        }

    

        public override void ShowWindow()
        {
            var cost = _prebuilderDesktop.Cost;
            var enough = _prebuilderDesktop.GameMode.EconomyAndUpgrade.Coins >= cost;
            _prebuilderDesktop.GameMode.UiMode.SetCurrentViewModel(this);
            _view.ShowWindow(cost, enough);
        }

        public override void HideWindow()
        {
            _view.HideWindow();
        }

        public void OnButtonBuyDesktop()
        {
            _prebuilderDesktop.OnButtonBuyDesktop();
        }
    }
}