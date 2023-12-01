using Assets._Game._Scripts._6_Entities._Units._Desktop;
using UnityEngine;

namespace Assets._Game._Scripts._3_UI._UIUnits._Base {
    public abstract class UIUnitViewModel : IUiUnitViewModel {
        public UIDesktopView View {
            get => _view;
            set => _view = value;
        }
        protected UIDesktopView _view;
        public bool IsOpenedWindow;
        public virtual void OnAnyInputControllerEvent() {
            CheckWindow();
        }

        public virtual void ShowWindow() { }
        public virtual void HideWindow() { }
        public virtual void CheckWindow() { }
    }
}