using UnityEngine;

namespace Assets._Game._Scripts._0.Data._Base {
    public abstract class BaseUpgradeSO: ScriptableObject
    {
        public abstract BaseDataForUpgrade[] Upgrades { get; }
       
    }
}
