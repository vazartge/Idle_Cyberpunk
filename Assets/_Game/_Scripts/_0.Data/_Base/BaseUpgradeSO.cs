using System;
using UnityEngine;

namespace Assets._Game._Scripts._0.Data._Base {
    public abstract class BaseUpgradeSO: ScriptableObject
    {
        public abstract BaseDataForUpgrade[] Upgrades { get; }
        // Метод получения количества звезд для UI тултипа стола
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
