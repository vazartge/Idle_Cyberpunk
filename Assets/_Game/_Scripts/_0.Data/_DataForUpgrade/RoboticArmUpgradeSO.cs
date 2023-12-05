using Assets._Game._Scripts._0.Data._Base;
using UnityEngine;

namespace Assets._Game._Scripts._0.Data._DataForUpgrade
{
    [CreateAssetMenu(fileName = "RoboticArmUpgradeData", menuName = "Game/RoboticArmDataForUpgrade")]
    public class RoboticArmUpgradeSO: BaseUpgradeSO {
         public override BaseDataForUpgrade[] Upgrades => _upgrades;

        [SerializeField]
        private RoboticArmDataForUpgrade[] _upgrades = new RoboticArmDataForUpgrade[]
        {
            new RoboticArmDataForUpgrade { Stars = 0, Level = 1, Cost = 10, IncomeMoney = 3, OpeningAtLevel = 1, Events = "" },
            new RoboticArmDataForUpgrade { Stars = 0, Level = 2, Cost = 3, IncomeMoney = 4, OpeningAtLevel = 1, Events = "" },
            new RoboticArmDataForUpgrade { Stars = 0, Level = 3, Cost = 4, IncomeMoney = 5, OpeningAtLevel = 1, Events = "" },
            new RoboticArmDataForUpgrade { Stars = 0, Level = 4, Cost = 6, IncomeMoney = 6, OpeningAtLevel = 1, Events = "" },
            new RoboticArmDataForUpgrade { Stars = 0, Level = 5, Cost = 8, IncomeMoney = 7, OpeningAtLevel = 1, Events = "" },
            new RoboticArmDataForUpgrade { Stars = 0, Level = 6, Cost = 11, IncomeMoney = 8, OpeningAtLevel = 1, Events = "" },
            new RoboticArmDataForUpgrade { Stars = 0, Level = 7, Cost = 14, IncomeMoney = 9, OpeningAtLevel = 1, Events = "" },
            new RoboticArmDataForUpgrade { Stars = 0, Level = 8, Cost = 18, IncomeMoney = 10, OpeningAtLevel = 1, Events = "" },
            new RoboticArmDataForUpgrade { Stars = 0, Level = 9, Cost = 23, IncomeMoney = 11, OpeningAtLevel = 1, Events = "" },
            new RoboticArmDataForUpgrade { Stars = 0, Level = 10, Cost = 29, IncomeMoney = 12, OpeningAtLevel = 1, Events = "" },
            new RoboticArmDataForUpgrade { Stars = 1, Level = 11, Cost = 37, IncomeMoney = 27, OpeningAtLevel = 1, Events = "Additional table opens" },
            new RoboticArmDataForUpgrade { Stars = 1, Level = 12, Cost = 47, IncomeMoney = 30, OpeningAtLevel = 1, Events = "" },
            new RoboticArmDataForUpgrade { Stars = 1, Level = 13, Cost = 60, IncomeMoney = 33, OpeningAtLevel = 1, Events = "" },
            new RoboticArmDataForUpgrade { Stars = 1, Level = 14, Cost = 76, IncomeMoney = 36, OpeningAtLevel = 1, Events = "" },
            new RoboticArmDataForUpgrade { Stars = 1, Level = 15, Cost = 96, IncomeMoney = 39, OpeningAtLevel = 1, Events = "" },
            new RoboticArmDataForUpgrade { Stars = 1, Level = 16, Cost = 121, IncomeMoney = 43, OpeningAtLevel = 1, Events = "" },
            new RoboticArmDataForUpgrade { Stars = 1, Level = 17, Cost = 153, IncomeMoney = 47, OpeningAtLevel = 1, Events = "" },



        };
    }
}