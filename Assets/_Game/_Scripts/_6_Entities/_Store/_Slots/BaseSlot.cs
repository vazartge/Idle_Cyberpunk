using Assets._Game._Scripts._5_Managers;
using Assets._Game._Scripts._6_Entities._Store._Products;
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

        public virtual ProductType ProductType { get; set; }

        public bool IsOccupied => _unit != null;

        public IUnitGame Unit {
            get => _unit;
            set => _unit = value;
        }

        [SerializeField]protected ProductType _productType;
        
        [SerializeField] int _id;
        private IUnitGame _unit;
    }
}