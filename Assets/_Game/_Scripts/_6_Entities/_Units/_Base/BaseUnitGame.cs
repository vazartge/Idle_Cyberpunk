using System.Collections;
using Assets._Game._Scripts._3_UI._UIUnits._Base;
using Assets._Game._Scripts._5_Managers;
using UnityEngine;

namespace Assets._Game._Scripts._6_Entities._Units._Base {
    public abstract class BaseUnitGame : MonoBehaviour, IUnitGame, IUnitTouchable
    {
        public UIUnitViewModel ViewModel { get;  set; }
        public string ID;

        public virtual void Construct(GameMode gameMode, DataMode_ dataMode){}
        // Сделаем OnTouch виртуальным
        public virtual void OnTouch() {
            OnTouchAction();
        }

        protected virtual void OnTouchAction() {
            Debug.Log($"Touch unit info: ID {ID}, name {this.name}");
        }
    }
}