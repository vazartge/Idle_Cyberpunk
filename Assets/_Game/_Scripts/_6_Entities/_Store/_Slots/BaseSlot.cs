using Assets._Game._Scripts._6_Entities._Units._Base;
using System.Collections;
using UnityEngine;

namespace Assets._Game._Scripts._6_Entities._Store._Slots {
    public abstract class BaseSlot : MonoBehaviour, ISlot {

        public int ID { get; private set; }

        private IUnitGame _unit;

        public bool IsOccupied {
            get { return _unit != null; }
        }

        public IUnitGame Unit {
            get { return _unit; }
            set {
                _unit = value;
                // Дополнительная логика при изменении значения, если требуется
            }
        }
    }
}