using System.Collections.Generic;
using UnityEngine;



namespace Assets._Game._Scripts._0.Data._DataForLevelsUpgrade{


    [CreateAssetMenu(fileName = "LevelsUpgradesSO", menuName = "Game/LevelsUpgradesSO")]
    public class LevelsUpgradesSO : ScriptableObject
    {
        public Dictionary<int, LevelUpgrade> LevelUpgrades => _levelsUpgrades;

        private Dictionary<int, LevelUpgrade> _levelsUpgrades = new Dictionary<int, LevelUpgrade>()
        {
            /// Ключ - номер текущего уроыня

            {
                1, new LevelUpgrade(
                    new List<UpgradeCustomer>
                    {
                        new UpgradeCustomer("Второй покупатель", 2, 0, false), // Изначально
                        new UpgradeCustomer("Третий покупатель",3, 20, false),
                        new UpgradeCustomer("Четвертый покупатель", 4, 250, false)
                    },
                    new List<UpgradeSeller>
                    {   
                        new UpgradeSeller("Первый продавец", 1, 0, false), // Изначально
                        new UpgradeSeller("Второй продавец", 2, 35, false)
                    },
                    new ProductBoost("Ускорение производства 30%", 1.3f, 350, false), // на 30% быстрее

                    new SpeedBoost("Ускорение перемещения продавцов на 20%", 1.2f, 500, false) // на 20% быстрее
                )
            },
            {
                2, new LevelUpgrade(
                    new List<UpgradeCustomer>

                    {
                        new UpgradeCustomer("Второй покупатель", 2, 0, false),
                        new UpgradeCustomer("Третий покупатель", 3, 20, false),
                        new UpgradeCustomer("Четвертый покупатель", 4, 250, false),
                        new UpgradeCustomer("Пятый покупатель", 5, 2500, false),
                        new UpgradeCustomer("Шестой покупатель", 6, 5000, false)
                    },

                    new List<UpgradeSeller>
                    {
                        new UpgradeSeller("Первый продавец", 1, 0, false),
                        new UpgradeSeller("Второй продавец", 2, 35, false),
                        new UpgradeSeller("Третий продавец", 3, 500, false),
                        new UpgradeSeller("Четвертый продавец", 4, 1000, false)
                    },
                    new ProductBoost("Ускорение производства 30%", 1.3f, 3500, false),

                    new SpeedBoost("Ускорение перемещения продавцов на 20%", 1.2f, 5000, false)
                )
            },
            {
                3, new LevelUpgrade(

                    new List<UpgradeCustomer>
                    {
                        new UpgradeCustomer("Второй покупатель", 2, 0, false),
                        new UpgradeCustomer("Третий покупатель", 3, 20, false),
                        new UpgradeCustomer("Четвертый покупатель", 4, 250, false),
                        new UpgradeCustomer("Пятый покупатель", 5, 2500, false),
                        new UpgradeCustomer("Шестой покупатель", 6, 5000, false),
                        new UpgradeCustomer("Седьмой покупатель", 7, 10000, false),
                        new UpgradeCustomer("Восьмой покупатель",8, 50000, false)
                    },

                    new List<UpgradeSeller>
                    {
                        new UpgradeSeller("Второй продавец", 1, 0, false),
                        new UpgradeSeller("Третий продавец", 2, 35, false),
                        new UpgradeSeller("Четвертый продавец", 3, 500, false),
                        new UpgradeSeller("Пятый продавец", 4, 1000, false),
                        new UpgradeSeller("Шестой продавец", 5, 5000, false),
                        new UpgradeSeller("Седьмой продавец", 6, 10000, false)
                    },
                    new ProductBoost("Ускорение производства 30%", 1.3f, 10500, false),

                    new SpeedBoost("Ускорение перемещения продавцов на 20%", 1.2f, 15000, false)
                )
            },

            {
                4, new LevelUpgrade(

                    new List<UpgradeCustomer>
                    {
                        new UpgradeCustomer("Второй покупатель",2, 0, false),
                        new UpgradeCustomer("Третий покупатель",3, 20, false),
                        new UpgradeCustomer("Четвертый покупатель", 4, 250, false),
                        new UpgradeCustomer("Пятый покупатель", 5, 2500, false),
                        new UpgradeCustomer("Шестой покупатель", 6, 5000, false),
                        new UpgradeCustomer("Седьмой покупатель", 7, 10000, false),
                        new UpgradeCustomer("Восьмой покупатель", 8, 50000, false),
                        new UpgradeCustomer("Девятый покупатель", 9, 100000, false),
                        new UpgradeCustomer("Десятый покупатель", 10, 500000, false)
                    },

                    new List<UpgradeSeller>
                    {
                        new UpgradeSeller("Перый продавец", 1, 0, false),
                        new UpgradeSeller("Второй продавец", 2, 35, false),
                        new UpgradeSeller("Третий продавец", 3, 500, false),
                        new UpgradeSeller("Четвертый продавец", 4, 1000, false),
                        new UpgradeSeller("Пятый продавец  ", 5, 5000, false),
                        new UpgradeSeller("Шестой продавец", 6, 10000, false),
                        new UpgradeSeller("Седьмой продавец", 7, 50000, false),
                        new UpgradeSeller("Восьмой продавец", 8, 100000, false)
                    },
                    new ProductBoost("Ускорение производства 30%", 1.3f, 50000, false),

                    new SpeedBoost("Ускорение перемещения продавцов на 20%", 1.2f, 55000, false)
                )
            },
        };

    }
}