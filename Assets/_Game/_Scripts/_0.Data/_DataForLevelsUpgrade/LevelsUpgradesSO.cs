using Assets._Game._Scripts._0.Data._DataForLevelsUpgrade;
using System.Collections.Generic;
using UnityEngine;



namespace Assets._Game._Scripts._0.Data._DataForLevelsUpgrade{


    [CreateAssetMenu(fileName = "LevelsUpgradesSO", menuName = "Game/LevelsUpgradesSO")]
    public class LevelsUpgradesSO : ScriptableObject
    {
        public Dictionary<int, LevelUpgrade> LevelUpgrades => _levelsUpgrades;

        public Dictionary<int, LevelUpgrade> _levelsUpgrades = new Dictionary<int, LevelUpgrade>()
        {
            /// Ключ - номер текущего уроыня

            {
                1, new LevelUpgrade(
                    new List<UpgradeCustomer>
                    {
                       // new UpgradeCustomer("Второй покупатель", 2, 0, false), // Изначально
                        new UpgradeCustomer("Третий покупатель",3, 45, false),
                        new UpgradeCustomer("Четвертый покупатель", 4, 150, false)
                    },
                    new List<UpgradeSeller>
                    {   
                      //  new UpgradeSeller("Первый продавец", 1, 0, false), // Изначально
                        new UpgradeSeller("Второй продавец", 2, 40, false)
                    },
                    new ProductBoost("Ускорение производства 30%", 1.3f, 350, false), // на 30% быстрее

                    new SpeedBoost("Ускорение перемещения продавцов на 20%", 1.2f, 400, false) // на 20% быстрее
                )
            },
            {
                2, new LevelUpgrade(
                    new List<UpgradeCustomer>

                    {
                      //  new UpgradeCustomer("Второй покупатель", 2, 0, false),
                        new UpgradeCustomer("Третий покупатель", 3, 45, false),
                        new UpgradeCustomer("Четвертый покупатель", 4, 150, false),
                        new UpgradeCustomer("Пятый покупатель", 5, 900, false),
                        new UpgradeCustomer("Шестой покупатель", 6, 4000, false)
                    },

                    new List<UpgradeSeller>
                    {
                     //   new UpgradeSeller("Первый продавец", 1, 0, false),
                        new UpgradeSeller("Второй продавец", 2, 100, false),
                        new UpgradeSeller("Третий продавец", 3, 1000, false),
                        new UpgradeSeller("Четвертый продавец", 4, 5000, false)
                    },
                    new ProductBoost("Ускорение производства 30%", 1.3f, 3500, false),

                    new SpeedBoost("Ускорение перемещения продавцов на 20%", 1.2f, 4000, false)
                )
            },
            {
                3, new LevelUpgrade(

                    new List<UpgradeCustomer>
                    {
                      //  new UpgradeCustomer("Второй покупатель", 2, 0, false),
                        new UpgradeCustomer("Третий покупатель", 3, 100, false),
                        new UpgradeCustomer("Четвертый покупатель", 4, 1000, false),
                        new UpgradeCustomer("Пятый покупатель", 5, 2000, false),
                        new UpgradeCustomer("Шестой покупатель", 6, 5000, false),
                        new UpgradeCustomer("Седьмой покупатель", 7, 35000, false),
                        new UpgradeCustomer("Восьмой покупатель",8, 80000, false)
                    },

                    new List<UpgradeSeller>
                    {
                     //   new UpgradeSeller("Второй продавец", 1, 0, false),
                        new UpgradeSeller("Третий продавец", 2, 500, false),
                        new UpgradeSeller("Четвертый продавец", 3, 1500, false),
                        new UpgradeSeller("Пятый продавец", 4, 10000, false),
                        new UpgradeSeller("Шестой продавец", 5, 50000, false),
                        new UpgradeSeller("Седьмой продавец", 6, 110000, false)
                    },
                    new ProductBoost("Ускорение производства 30%", 1.3f, 20000, false),

                    new SpeedBoost("Ускорение перемещения продавцов на 20%", 1.2f, 25000, false)
                )
            },

            {
                4, new LevelUpgrade(

                    new List<UpgradeCustomer>
                    {
                     //   new UpgradeCustomer("Второй покупатель",2, 0, false),
                        new UpgradeCustomer("Третий покупатель",3, 2000, false),
                        new UpgradeCustomer("Четвертый покупатель", 4, 5000, false),
                        new UpgradeCustomer("Пятый покупатель", 5, 10000, false),
                        new UpgradeCustomer("Шестой покупатель", 6, 40000, false),
                        new UpgradeCustomer("Седьмой покупатель", 7, 90000, false),
                        new UpgradeCustomer("Восьмой покупатель", 8, 200000, false),
                        new UpgradeCustomer("Девятый покупатель", 9, 2000000, false),
                        new UpgradeCustomer("Десятый покупатель", 10, 19000000, false)
                    },

                    new List<UpgradeSeller>
                    {
                      //  new UpgradeSeller("Перый продавец", 1, 0, false),
                        new UpgradeSeller("Второй продавец", 2, 3000, false),
                        new UpgradeSeller("Третий продавец", 3, 10000, false),
                        new UpgradeSeller("Четвертый продавец", 4, 50000, false),
                        new UpgradeSeller("Пятый продавец  ", 5, 100000, false),
                        new UpgradeSeller("Шестой продавец", 6, 300000, false),
                        new UpgradeSeller("Седьмой продавец", 7, 2500000, false),
                        new UpgradeSeller("Восьмой продавец", 8, 20000000, false)
                    },
                    new ProductBoost("Ускорение производства 30%", 1.3f, 400000, false),

                    new SpeedBoost("Ускорение перемещения продавцов на 20%", 1.2f, 450000, false)
                )
            },
        };

    }
}

// [System.Serializable]
// public class LevelData {
//     public int LevelNumber;
//     public LevelUpgrade LevelUpgradeData;
// }
//
// [CreateAssetMenu(fileName = "LevelsUpgradesSO", menuName = "Game/LevelsUpgradesSO")]
// public class LevelsUpgradesSO : ScriptableObject {
//     public List<LevelData> LevelsData = new List<LevelData>();
//
//     // Метод для получения данных об улучшении для определенного уровня
//     public LevelUpgrade GetLevelUpgrade(int level) {
//         foreach (var data in LevelsData) {
//             if (data.LevelNumber == level) {
//                 return data.LevelUpgradeData;
//             }
//         }
//         return null; // или какую-то логику обработки ошибок
//     }
// }