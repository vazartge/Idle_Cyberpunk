using Assets._Game._Scripts._3_UI._UIUnits._Base;
using Assets._Game._Scripts._5_Managers;

namespace Assets._Game._Scripts._6_Entities._Units._PrebuilderDesktop
{
    public class PrebuilderViewModel: UnitViewModel
    {
        public PrebuilderDesktop PrebuilderDesktop;
       // public UiUnitView View => _view;
        private UIPrebuilderView _view;
        public UIMode UiMode;
       

        public PrebuilderViewModel(PrebuilderDesktop prebuilderDesktop, UIPrebuilderView view, UIMode uiMode) {
            PrebuilderDesktop=prebuilderDesktop;
            _view = view;
            UiMode = uiMode;
        }

    

        public override void ShowWindow()
        {
            if(_view == null) return;
            var cost = PrebuilderDesktop.Cost;
            var enough = PrebuilderDesktop.GameMode.EconomyAndUpgrade.Coins >= cost;
            PrebuilderDesktop.GameMode.UiMode.OpenNewViewModel(this);
            _view.ShowWindow(cost, enough);
        }

        public override void HideWindow()
        {
            _view.HideWindow();
        }

        public void OnButtonBuyDesktop()
        {
            PrebuilderDesktop.OnButtonBuyDesktop();
        }
    }
}