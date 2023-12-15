using System.Collections;
using Assets._Game._Scripts._3_UI._UIUnits._Base;
using Assets._Game._Scripts._5_Managers;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Assets._Game._Scripts._6_Entities._Units._Base {
    public abstract class BaseUnitGame : MonoBehaviour, IUnitGame/*, IUnitTouchable*/{
        public UnitViewModel ViewModel { get;  set; }
        public string ID;
        public GameMode GameMode { get; }
        public virtual void Construct(GameMode gameMode, DataMode_ dataMode){}


        public void OnClick() {
            ViewModel.ShowWindow();
        }



    }
}