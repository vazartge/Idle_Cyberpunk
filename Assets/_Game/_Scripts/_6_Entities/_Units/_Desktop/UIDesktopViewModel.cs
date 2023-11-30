using Assets._Game._Scripts._3_UI._UIUnits._Base;
using Assets._Game._Scripts._5_Managers;

namespace Assets._Game._Scripts._6_Entities._Units._Desktop
{
    public class UIDesktopViewModel: UIUnitViewModel  
    {
        private  DesktopUnit _desktopModel;
        private  UIDesktopView _view;
        
        
        

        public UIDesktopViewModel(DesktopUnit desktopModelUnit, UIDesktopView view)
        {
            _desktopModel = desktopModelUnit;
            _view = view;
            _uiMode = _desktopModel.GameMode.UiMode;
           
        }

        private void UpdateOnChangeMoney()
        {
            
            _view.UpdateOnChangeMoney();

        }

        public void ShowWindow()
        {
            
        }

        public void OnButtonUpgrade()
        {
            _desktopModel.OnButtonUpgradeDesktop();
        }
    }
}