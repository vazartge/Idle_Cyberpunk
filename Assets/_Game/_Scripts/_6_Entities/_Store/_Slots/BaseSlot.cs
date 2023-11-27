using Assets._Game._Scripts._6_Entities._Units._Base;
using UnityEngine;

namespace Assets._Game._Scripts._6_Entities._Store._Slots
{
    public abstract class BaseSlot : MonoBehaviour, ISlot {
        public int ID
        {
            get => _id;
            private set => _id = value;
        }

        private IUnitGame _unit;
        [SerializeField] int _id;

        public bool IsOccupied => _unit != null;

        public IUnitGame Unit {
            get => _unit;
            set => _unit = value;
        }
    }
}