using Assets._Game._Scripts._3_UI._UIUnits._Base;
using UnityEngine;

namespace Assets._Game._Scripts._6_Entities._Units._Base {
    public abstract class BaseUnitGame : MonoBehaviour, IUnitGame/*, IUnitTouchable*/{
        public UnitViewModel ViewModel { get;  set; }
        public string ID;
      // public virtual void Construct(GameMode gameMode, DataMode_ dataMode){}


        // public void OnClick() {
        //     ViewModel.ShowWindow();
        // }



    }
}