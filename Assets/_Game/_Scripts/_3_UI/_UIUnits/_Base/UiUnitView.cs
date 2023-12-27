using UnityEngine;

namespace Assets._Game._Scripts._3_UI._UIUnits._Base {
    // View используется в тултипах столов и пребилдеров
    public abstract class UiUnitView : MonoBehaviour, IUiUnitView {
        
        public virtual void HideWindow() { }
        
    }
}