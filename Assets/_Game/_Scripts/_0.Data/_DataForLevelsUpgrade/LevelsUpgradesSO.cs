using System.Collections.Generic;
using UnityEngine;



namespace Assets._Game._Scripts._0.Data._DataForLevelsUpgrade
{
    [CreateAssetMenu(fileName = "LevelsUpgradesSO", menuName = "Game/LevelsUpgradesSO")]
    public class LevelsUpgradesSO : ScriptableObject
    {
        public Dictionary<int, LevelUpgrade> LevelUpgrades => _levelsUpgrades;

        private Dictionary<int, LevelUpgrade> _levelsUpgrades = new Dictionary<int, LevelUpgrade>()
        {
            /// Ключ - номер текущего уроыня

            {
                1, new LevelUpgrade(
                    new List<UpgradeOption>
                    {
                        new UpgradeOption(2, 0), // Изначально
                        new UpgradeOption(3, 20),
                        new UpgradeOption(4, 250)
                    },
                    new List<UpgradeOption>
                    {
                        new UpgradeOption(1, 0), // Изначально
                        new UpgradeOption(2, 35)
                    },
                    new ProductBoost(1.3f, 350), // на 30% быстрее

                    new SpeedBoost(1.2f, 500) // на 20% быстрее
                )
            },
            {
                2, new LevelUpgrade(
                    new List<UpgradeOption>

                    {
                        new UpgradeOption(2, 0),
                        new UpgradeOption(3, 20),
                        new UpgradeOption(4, 250),
                        new UpgradeOption(5, 2500),
                        new UpgradeOption(6, 5000)
                    },

                    new List<UpgradeOption>
                    {
                        new UpgradeOption(1, 0),
                        new UpgradeOption(2, 35),
                        new UpgradeOption(3, 500),
                        new UpgradeOption(4, 1000)
                    },
                    new ProductBoost(1.3f, 3500),

                    new SpeedBoost(1.2f, 5000)
                )
            },
            {
                3, new LevelUpgrade(

                    new List<UpgradeOption>
                    {
                        new UpgradeOption(2, 0),
                        new UpgradeOption(3, 20),
                        new UpgradeOption(4, 250),
                        new UpgradeOption(5, 2500),
                        new UpgradeOption(6, 5000),
                        new UpgradeOption(7, 10000),
                        new UpgradeOption(8, 50000)
                    },

                    new List<UpgradeOption>
                    {
                        new UpgradeOption(1, 0),
                        new UpgradeOption(2, 35),
                        new UpgradeOption(3, 500),
                        new UpgradeOption(4, 1000),
                        new UpgradeOption(5, 5000),
                        new UpgradeOption(6, 10000)
                    },
                    new ProductBoost(1.3f, 10500),

                    new SpeedBoost(1.2f, 15000)
                )
            },

            {
                4, new LevelUpgrade(

                    new List<UpgradeOption>
                    {
                        new UpgradeOption(2, 0),
                        new UpgradeOption(3, 20),
                        new UpgradeOption(4, 250),
                        new UpgradeOption(5, 2500),
                        new UpgradeOption(6, 5000),
                        new UpgradeOption(7, 10000),
                        new UpgradeOption(8, 50000),
                        new UpgradeOption(9, 100000),
                        new UpgradeOption(10, 500000)
                    },

                    new List<UpgradeOption>
                    {
                        new UpgradeOption(1, 0),
                        new UpgradeOption(2, 35),
                        new UpgradeOption(3, 500),
                        new UpgradeOption(4, 1000),
                        new UpgradeOption(5, 5000),
                        new UpgradeOption(6, 10000),
                        new UpgradeOption(7, 50000),
                        new UpgradeOption(8, 100000)
                    },
                    new ProductBoost(1.3f, 50000),

                    new SpeedBoost(1.2f, 55000)
                )
            },
        };

    }
}