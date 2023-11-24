using Assets._Game._Scripts._6_Entities._Store._Slots;
using Assets._Game._Scripts._6_Entities._Units._Base;
using UnityEngine;

public abstract class BaseSlot : MonoBehaviour, ISlot {
    public int ID { get; private set; }
    private IUnitGame _unit;

    public bool IsOccupied => _unit != null;

    public IUnitGame Unit {
        get => _unit;
        set => _unit = value;
    }
}