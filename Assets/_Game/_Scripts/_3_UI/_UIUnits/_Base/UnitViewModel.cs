﻿namespace Assets._Game._Scripts._3_UI._UIUnits._Base {
    public abstract class UnitViewModel : IUiUnitViewModel {
        public UiUnitView View { get; set; }
        public bool IsOpenedWindow;
        public virtual void OnAnyInputControllerEvent() {
            CheckWindow();
        }

        public virtual void ShowWindow()
        {
         
        }
       

        public virtual void HideWindow()
        {
          
        }
        public virtual void CheckWindow() { }
    }
}