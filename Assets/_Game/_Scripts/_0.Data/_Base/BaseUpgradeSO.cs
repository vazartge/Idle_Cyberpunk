using System;
using System.Linq;
using Assets._Game._Scripts._2_Game;
using UnityEngine;

namespace Assets._Game._Scripts._0.Data._Base {
    public abstract class BaseUpgradeSO: ScriptableObject
    {
        public abstract BaseDataForUpgrade[] Upgrades { get; }
        public int GetMaxStarsForLevel(int level) {
            int maxStars = 0;
            foreach (var upgrade in Upgrades) {
                if (upgrade.OpeningAtLevel <= level) {
                    maxStars = Math.Max(maxStars, upgrade.Stars);
                }
            }
            return maxStars;
        }





    }
}
