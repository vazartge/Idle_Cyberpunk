﻿using Assets._Game._Scripts._0.Data._Base;
using UnityEngine;

namespace Assets._Game._Scripts._0.Data._DataForUpgrade
{
    [CreateAssetMenu(fileName = "MechanicalEyeUpgradeData", menuName = "Game/MechanicalEyeDataForUpgrade")]
    public class MechanicalEyeUpgradeSO : BaseUpgradeSO {
        public override BaseDataForUpgrade[] Upgrades => _upgrades;

        [SerializeField]
        private MechanicalEyeDataForUpgrade[] _upgrades = new MechanicalEyeDataForUpgrade[]
        {
        new MechanicalEyeDataForUpgrade { Stars = 0, Level = 1, Cost = 10, IncomeMoney = 3, OpeningAtLevel = 1, Events = "" },
        new MechanicalEyeDataForUpgrade { Stars = 0, Level = 2, Cost = 3, IncomeMoney = 4, OpeningAtLevel = 1, Events = "" },
        new MechanicalEyeDataForUpgrade { Stars = 0, Level = 3, Cost = 4, IncomeMoney = 5, OpeningAtLevel = 1, Events = "" },
        new MechanicalEyeDataForUpgrade { Stars = 0, Level = 4, Cost = 6, IncomeMoney = 6, OpeningAtLevel = 1, Events = "" },
        new MechanicalEyeDataForUpgrade { Stars = 0, Level = 5, Cost = 8, IncomeMoney = 7, OpeningAtLevel = 1, Events = "" },
        new MechanicalEyeDataForUpgrade { Stars = 0, Level = 6, Cost = 11, IncomeMoney = 8, OpeningAtLevel = 1, Events = "" },
        new MechanicalEyeDataForUpgrade { Stars = 0, Level = 7, Cost = 14, IncomeMoney = 9, OpeningAtLevel = 1, Events = "" },
        new MechanicalEyeDataForUpgrade { Stars = 0, Level = 8, Cost = 18, IncomeMoney = 10, OpeningAtLevel = 1, Events = "" },
        new MechanicalEyeDataForUpgrade { Stars = 0, Level = 9, Cost = 23, IncomeMoney = 11, OpeningAtLevel = 1, Events = "" },
        new MechanicalEyeDataForUpgrade { Stars = 0, Level = 10, Cost = 29, IncomeMoney = 12, OpeningAtLevel = 1, Events = "" },
        new MechanicalEyeDataForUpgrade { Stars = 1, Level = 11, Cost = 37, IncomeMoney = 27, OpeningAtLevel = 1, Events = "Additional table opens" },
        new MechanicalEyeDataForUpgrade { Stars = 1, Level = 12, Cost = 47, IncomeMoney = 30, OpeningAtLevel = 1, Events = "" },
        new MechanicalEyeDataForUpgrade { Stars = 1, Level = 13, Cost = 60, IncomeMoney = 33, OpeningAtLevel = 1, Events = "" },
        new MechanicalEyeDataForUpgrade { Stars = 1, Level = 14, Cost = 76, IncomeMoney = 36, OpeningAtLevel = 1, Events = "" },
        new MechanicalEyeDataForUpgrade { Stars = 1, Level = 15, Cost = 96, IncomeMoney = 39, OpeningAtLevel = 1, Events = "" },
        new MechanicalEyeDataForUpgrade { Stars = 1, Level = 16, Cost = 121, IncomeMoney = 43, OpeningAtLevel = 1, Events = "" },
        new MechanicalEyeDataForUpgrade { Stars = 1, Level = 17, Cost = 153, IncomeMoney = 47, OpeningAtLevel = 1, Events = "" },
        new MechanicalEyeDataForUpgrade { Stars = 1, Level = 18, Cost = 193, IncomeMoney = 51, OpeningAtLevel = 1, Events = "" },
        new MechanicalEyeDataForUpgrade { Stars = 1, Level = 19, Cost = 244, IncomeMoney = 56, OpeningAtLevel = 1, Events = "" },
        new MechanicalEyeDataForUpgrade { Stars = 1, Level = 20, Cost = 308, IncomeMoney = 124, OpeningAtLevel = 1, Events = "" },
        new MechanicalEyeDataForUpgrade { Stars = 1, Level = 21, Cost = 389, IncomeMoney = 134, OpeningAtLevel = 1, Events = "" },
        new MechanicalEyeDataForUpgrade { Stars = 1, Level = 22, Cost = 491, IncomeMoney = 145, OpeningAtLevel = 1, Events = "" },
        new MechanicalEyeDataForUpgrade { Stars = 1, Level = 23, Cost = 619, IncomeMoney = 157, OpeningAtLevel = 1, Events = "" },
        new MechanicalEyeDataForUpgrade { Stars = 1, Level = 24, Cost = 780, IncomeMoney = 170, OpeningAtLevel = 1, Events = "" },
        new MechanicalEyeDataForUpgrade { Stars = 2, Level = 25, Cost = 983, IncomeMoney = 374, OpeningAtLevel = 1, Events = "" },
        new MechanicalEyeDataForUpgrade { Stars = 2, Level = 26, Cost = 1180, IncomeMoney = 394, OpeningAtLevel = 1, Events = "" },
        new MechanicalEyeDataForUpgrade { Stars = 2, Level = 27, Cost = 1420, IncomeMoney = 474, OpeningAtLevel = 2, Events = "" },
        new MechanicalEyeDataForUpgrade { Stars = 2, Level = 28, Cost = 1700, IncomeMoney = 567, OpeningAtLevel = 2, Events = "" },
        new MechanicalEyeDataForUpgrade { Stars = 2, Level = 29, Cost = 2040, IncomeMoney = 680, OpeningAtLevel = 2, Events = "" },
        new MechanicalEyeDataForUpgrade { Stars = 2, Level = 30, Cost = 2450, IncomeMoney = 817, OpeningAtLevel = 2, Events = "" },
        new MechanicalEyeDataForUpgrade { Stars = 2, Level = 31, Cost = 2940, IncomeMoney = 980, OpeningAtLevel = 2, Events = "" },
        new MechanicalEyeDataForUpgrade { Stars = 2, Level = 32, Cost = 3530, IncomeMoney = 1180, OpeningAtLevel = 2, Events = "" },
        new MechanicalEyeDataForUpgrade { Stars = 2, Level = 33, Cost = 4240, IncomeMoney = 1410, OpeningAtLevel = 2, Events = "" },
        new MechanicalEyeDataForUpgrade { Stars = 2, Level = 34, Cost = 5090, IncomeMoney = 1700, OpeningAtLevel = 2, Events = "" },
        new MechanicalEyeDataForUpgrade { Stars = 2, Level = 35, Cost = 6110, IncomeMoney = 2040, OpeningAtLevel = 2, Events = "" },
        new MechanicalEyeDataForUpgrade { Stars = 2, Level = 36, Cost = 7330, IncomeMoney = 2440, OpeningAtLevel = 2, Events = "" },
        new MechanicalEyeDataForUpgrade { Stars = 2, Level = 37, Cost = 8800, IncomeMoney = 2930, OpeningAtLevel = 2, Events = "" },
        new MechanicalEyeDataForUpgrade { Stars = 2, Level = 38, Cost = 10600, IncomeMoney = 3530, OpeningAtLevel = 2, Events = "" },
        new MechanicalEyeDataForUpgrade { Stars = 2, Level = 39, Cost = 12700, IncomeMoney = 4230, OpeningAtLevel = 2, Events = "" },
        new MechanicalEyeDataForUpgrade { Stars = 2, Level = 40, Cost = 15200, IncomeMoney = 5070, OpeningAtLevel = 2, Events = "" },
        new MechanicalEyeDataForUpgrade { Stars = 2, Level = 41, Cost = 18200, IncomeMoney = 6070, OpeningAtLevel = 2, Events = "" },
        new MechanicalEyeDataForUpgrade { Stars = 2, Level = 42, Cost = 21800, IncomeMoney = 7270, OpeningAtLevel = 2, Events = "" },
        new MechanicalEyeDataForUpgrade { Stars = 2, Level = 43, Cost = 26200, IncomeMoney = 8730, OpeningAtLevel = 2, Events = "" },
        new MechanicalEyeDataForUpgrade { Stars = 2, Level = 44, Cost = 31400, IncomeMoney = 10500, OpeningAtLevel = 2, Events = "" },
        new MechanicalEyeDataForUpgrade { Stars = 2, Level = 45, Cost = 37700, IncomeMoney = 12600, OpeningAtLevel = 2, Events = "" },
        new MechanicalEyeDataForUpgrade { Stars = 2, Level = 46, Cost = 45200, IncomeMoney = 15100, OpeningAtLevel = 2, Events = "" },
        new MechanicalEyeDataForUpgrade { Stars = 2, Level = 47, Cost = 54200, IncomeMoney = 18100, OpeningAtLevel = 2, Events = "" },
        new MechanicalEyeDataForUpgrade { Stars = 2, Level = 48, Cost = 65000, IncomeMoney = 21700, OpeningAtLevel = 2, Events = "" },
        new MechanicalEyeDataForUpgrade { Stars = 2, Level = 49, Cost = 78000, IncomeMoney = 26000, OpeningAtLevel = 2, Events = "" },
        new MechanicalEyeDataForUpgrade { Stars = 3, Level = 50, Cost = 93600, IncomeMoney = 31200, OpeningAtLevel = 2, Events = "" },
        new MechanicalEyeDataForUpgrade { Stars = 3, Level = 51, Cost = 102000, IncomeMoney = 31900, OpeningAtLevel = 3, Events = "" },
        new MechanicalEyeDataForUpgrade { Stars = 3, Level = 52, Cost = 111000, IncomeMoney = 34700, OpeningAtLevel = 3, Events = "" },
        new MechanicalEyeDataForUpgrade { Stars = 3, Level = 53, Cost = 121000, IncomeMoney = 37800, OpeningAtLevel = 3, Events = "" },
        new MechanicalEyeDataForUpgrade { Stars = 3, Level = 54, Cost = 132000, IncomeMoney = 41300, OpeningAtLevel = 3, Events = "" },
        new MechanicalEyeDataForUpgrade { Stars = 3, Level = 55, Cost = 144000, IncomeMoney = 45000, OpeningAtLevel = 3, Events = "" },
        new MechanicalEyeDataForUpgrade { Stars = 3, Level = 56, Cost = 157000, IncomeMoney = 49100, OpeningAtLevel = 3, Events = "" },
        new MechanicalEyeDataForUpgrade { Stars = 3, Level = 57, Cost = 171000, IncomeMoney = 53400, OpeningAtLevel = 3, Events = "" },
        new MechanicalEyeDataForUpgrade { Stars = 3, Level = 58, Cost = 186000, IncomeMoney = 58100, OpeningAtLevel = 3, Events = "" },
        new MechanicalEyeDataForUpgrade { Stars = 3, Level = 59, Cost = 203000, IncomeMoney = 63400, OpeningAtLevel = 3, Events = "" },
        new MechanicalEyeDataForUpgrade { Stars = 3, Level = 60, Cost = 221000, IncomeMoney = 69100, OpeningAtLevel = 3, Events = "" },
        new MechanicalEyeDataForUpgrade { Stars = 3, Level = 61, Cost = 241000, IncomeMoney = 75300, OpeningAtLevel = 3, Events = "" },
        new MechanicalEyeDataForUpgrade { Stars = 3, Level = 62, Cost = 263000, IncomeMoney = 82200, OpeningAtLevel = 3, Events = "" },
        new MechanicalEyeDataForUpgrade { Stars = 3, Level = 63, Cost = 287000, IncomeMoney = 89700, OpeningAtLevel = 3, Events = "" },
        new MechanicalEyeDataForUpgrade { Stars = 3, Level = 64, Cost = 313000, IncomeMoney = 97800, OpeningAtLevel = 3, Events = "" },
        new MechanicalEyeDataForUpgrade { Stars = 3, Level = 65, Cost = 341000, IncomeMoney = 107000, OpeningAtLevel = 3, Events = "" },
        new MechanicalEyeDataForUpgrade { Stars = 3, Level = 66, Cost = 372000, IncomeMoney = 116000, OpeningAtLevel = 3, Events = "" },
        new MechanicalEyeDataForUpgrade { Stars = 3, Level = 67, Cost = 405000, IncomeMoney = 127000, OpeningAtLevel = 3, Events = "" },
        new MechanicalEyeDataForUpgrade { Stars = 3, Level = 68, Cost = 441000, IncomeMoney = 138000, OpeningAtLevel = 3, Events = "" },
        new MechanicalEyeDataForUpgrade { Stars = 3, Level = 69, Cost = 481000, IncomeMoney = 150000, OpeningAtLevel = 3, Events = "" },
        new MechanicalEyeDataForUpgrade { Stars = 3, Level = 70, Cost = 524000, IncomeMoney = 164000, OpeningAtLevel = 3, Events = "" },
        new MechanicalEyeDataForUpgrade { Stars = 3, Level = 71, Cost = 571000, IncomeMoney = 173000, OpeningAtLevel = 3, Events = "" },
        new MechanicalEyeDataForUpgrade { Stars = 3, Level = 72, Cost = 622000, IncomeMoney = 188000, OpeningAtLevel = 3, Events = "" },
        new MechanicalEyeDataForUpgrade { Stars = 3, Level = 73, Cost = 678000, IncomeMoney = 205000, OpeningAtLevel = 3, Events = "" },
        new MechanicalEyeDataForUpgrade { Stars = 3, Level = 74, Cost = 739000, IncomeMoney = 224000, OpeningAtLevel = 3, Events = "" },
        new MechanicalEyeDataForUpgrade { Stars = 4, Level = 75, Cost = 806000, IncomeMoney = 244000, OpeningAtLevel = 3, Events = "" },
        new MechanicalEyeDataForUpgrade { Stars = 4, Level = 76, Cost = 846000, IncomeMoney = 256000, OpeningAtLevel = 4, Events = "" },
        new MechanicalEyeDataForUpgrade { Stars = 4, Level = 77, Cost = 888000, IncomeMoney = 269000, OpeningAtLevel = 4, Events = "" },
        new MechanicalEyeDataForUpgrade { Stars = 4, Level = 78, Cost = 932000, IncomeMoney = 282000, OpeningAtLevel = 4, Events = "" },
        new MechanicalEyeDataForUpgrade { Stars = 4, Level = 79, Cost = 979000, IncomeMoney = 297000, OpeningAtLevel = 4, Events = "" },
        new MechanicalEyeDataForUpgrade { Stars = 4, Level = 80, Cost = 1030000, IncomeMoney = 312000, OpeningAtLevel = 4, Events = "" },
        new MechanicalEyeDataForUpgrade { Stars = 4, Level = 81, Cost = 1080000, IncomeMoney = 327000, OpeningAtLevel = 4, Events = "" },
        new MechanicalEyeDataForUpgrade { Stars = 4, Level = 82, Cost = 1130000, IncomeMoney = 342000, OpeningAtLevel = 4, Events = "" },
        new MechanicalEyeDataForUpgrade { Stars = 4, Level = 83, Cost = 1190000, IncomeMoney = 361000, OpeningAtLevel = 4, Events = "" },
        new MechanicalEyeDataForUpgrade { Stars = 4, Level = 84, Cost = 1250000, IncomeMoney = 379000, OpeningAtLevel = 4, Events = "" },
        new MechanicalEyeDataForUpgrade { Stars = 4, Level = 85, Cost = 1310000, IncomeMoney = 397000, OpeningAtLevel = 4, Events = "" },
        new MechanicalEyeDataForUpgrade { Stars = 4, Level = 86, Cost = 1380000, IncomeMoney = 418000, OpeningAtLevel = 4, Events = "" },
        new MechanicalEyeDataForUpgrade { Stars = 4, Level = 87, Cost = 1450000, IncomeMoney = 439000, OpeningAtLevel = 4, Events = "" },
        new MechanicalEyeDataForUpgrade { Stars = 4, Level = 88, Cost = 1520000, IncomeMoney = 461000, OpeningAtLevel = 4, Events = "" },
        new MechanicalEyeDataForUpgrade { Stars = 4, Level = 89, Cost = 1600000, IncomeMoney = 485000, OpeningAtLevel = 4, Events = "" },
        new MechanicalEyeDataForUpgrade { Stars = 4, Level = 90, Cost = 1680000, IncomeMoney = 509000, OpeningAtLevel = 4, Events = "" },
        new MechanicalEyeDataForUpgrade { Stars = 4, Level = 91, Cost = 1760000, IncomeMoney = 533000, OpeningAtLevel = 4, Events = "" },
        new MechanicalEyeDataForUpgrade { Stars = 4, Level = 92, Cost = 1850000, IncomeMoney = 561000, OpeningAtLevel = 4, Events = "" },
        new MechanicalEyeDataForUpgrade { Stars = 4, Level = 93, Cost = 1940000, IncomeMoney = 588000, OpeningAtLevel = 4, Events = "" },
        new MechanicalEyeDataForUpgrade { Stars = 4, Level = 94, Cost = 2040000, IncomeMoney = 618000, OpeningAtLevel = 4, Events = "" },
        new MechanicalEyeDataForUpgrade { Stars = 4, Level = 95, Cost = 2140000, IncomeMoney = 648000, OpeningAtLevel = 4, Events = "" },
        new MechanicalEyeDataForUpgrade { Stars = 4, Level = 96, Cost = 2250000, IncomeMoney = 682000, OpeningAtLevel = 4, Events = "" },
        new MechanicalEyeDataForUpgrade { Stars = 4, Level = 97, Cost = 2360000, IncomeMoney = 715000, OpeningAtLevel = 4, Events = "" },
        new MechanicalEyeDataForUpgrade { Stars = 4, Level = 98, Cost = 2480000, IncomeMoney = 752000, OpeningAtLevel = 4, Events = "" },
        new MechanicalEyeDataForUpgrade { Stars = 4, Level = 99, Cost = 2600000, IncomeMoney = 788000, OpeningAtLevel = 4, Events = "" },
        new MechanicalEyeDataForUpgrade { Stars = 5, Level = 100, Cost = 2730000, IncomeMoney = 827000, OpeningAtLevel = 4, Events = "" },
        new MechanicalEyeDataForUpgrade { Stars = 5, Level = 101, Cost = 0, IncomeMoney = 0, OpeningAtLevel = 5, Events = "" },
        };
    }

}