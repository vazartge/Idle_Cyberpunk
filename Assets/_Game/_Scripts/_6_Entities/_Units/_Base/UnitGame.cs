using System.Collections;
using UnityEngine;

namespace Assets._Game._Scripts._6_Entities._Units._Base {
    public abstract class UnitGame : MonoBehaviour, IUnitGame, IUnitTouchable
    {

        public string ID;
        // Сделаем OnTouch виртуальным
        public virtual void OnTouch() {
            OnTouchAction();
        }

        protected virtual void OnTouchAction() {
            Debug.Log($"Touch unit info: ID {ID}, name {this.name}");
        }
    }
}