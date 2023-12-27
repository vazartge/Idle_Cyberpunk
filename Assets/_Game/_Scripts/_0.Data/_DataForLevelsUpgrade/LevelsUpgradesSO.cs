using System.Collections.Generic;
using UnityEngine;



namespace Assets._Game._Scripts._0.Data._DataForLevelsUpgrade{

    // Структура используется для определения Апгрейда магазина, также в сохранениях StoreStats для сохранения купленных улучшений для текущего уровня
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
                        new UpgradeCustomer("Второй покупатель", 2, 0, true), // Изначально
                        new UpgradeCustomer("Третий покупатель",1, 45, false),
                        new UpgradeCustomer("Четвертый покупатель", 1, 150, false)
                    },
                    new List<UpgradeSeller>
                    {   
                        new UpgradeSeller("Первый продавец", 1, 0, true), // Изначально
                        new UpgradeSeller("Второй продавец", 1, 40, false)
                    },
                    new ProductBoost("Ускорение производства 30%", 1.3f, 350, false), // на 30% быстрее

                    new SpeedBoost("Ускорение перемещения продавцов на 20%", 1.2f, 400, false) // на 20% быстрее
                )
            },
            {
                2, new LevelUpgrade(
                    new List<UpgradeCustomer>

                    {
                        new UpgradeCustomer("Второй покупатель", 2, 0, true),
                        new UpgradeCustomer("Третий покупатель", 1, 45, false),
                        new UpgradeCustomer("Четвертый покупатель", 1, 150, false),
                        new UpgradeCustomer("Пятый покупатель", 1, 900, false),
                        new UpgradeCustomer("Шестой покупатель", 1, 4000, false)
                    },

                    new List<UpgradeSeller>
                    {
                        new UpgradeSeller("Первый продавец", 1, 0, true),
                        new UpgradeSeller("Второй продавец", 1, 100, false),
                        new UpgradeSeller("Третий продавец", 1, 1000, false),
                        new UpgradeSeller("Четвертый продавец", 1, 5000, false)
                    },
                    new ProductBoost("Ускорение производства 30%", 1.3f, 3500, false),

                    new SpeedBoost("Ускорение перемещения продавцов на 20%", 1.2f, 4000, false)
                )
            },
            {
                3, new LevelUpgrade(

                    new List<UpgradeCustomer>
                    {
                        new UpgradeCustomer("Второй покупатель", 2, 0, true),
                        new UpgradeCustomer("Третий покупатель", 1, 100, false),
                        new UpgradeCustomer("Четвертый покупатель", 1, 1000, false),
                        new UpgradeCustomer("Пятый покупатель", 1, 2000, false),
                        new UpgradeCustomer("Шестой покупатель", 1, 5000, false),
                        new UpgradeCustomer("Седьмой покупатель", 1, 35000, false),
                        new UpgradeCustomer("Восьмой покупатель",1, 80000, false)
                    },

                    new List<UpgradeSeller>
                    {
                        new UpgradeSeller("Второй продавец", 1, 0, true),
                        new UpgradeSeller("Третий продавец", 1, 500, false),
                        new UpgradeSeller("Четвертый продавец", 1, 1500, false),
                        new UpgradeSeller("Пятый продавец", 1, 10000, false),
                        new UpgradeSeller("Шестой продавец", 1, 50000, false),
                        new UpgradeSeller("Седьмой продавец", 1, 110000, false)
                    },
                    new ProductBoost("Ускорение производства 30%", 1.3f, 20000, false),

                    new SpeedBoost("Ускорение перемещения продавцов на 20%", 1.2f, 25000, false)
                )
            },

            {
                4, new LevelUpgrade(

                    new List<UpgradeCustomer>
                    {
                        new UpgradeCustomer("Второй покупатель",2, 0, true),
                        new UpgradeCustomer("Третий покупатель",1, 2000, false),
                        new UpgradeCustomer("Четвертый покупатель", 1, 5000, false),
                        new UpgradeCustomer("Пятый покупатель", 1, 10000, false),
                        new UpgradeCustomer("Шестой покупатель", 1, 40000, false),
                        new UpgradeCustomer("Седьмой покупатель", 1, 90000, false),
                        new UpgradeCustomer("Восьмой покупатель", 1, 200000, false),
                        new UpgradeCustomer("Девятый покупатель", 1, 2000000, false),
                        new UpgradeCustomer("Десятый покупатель", 1, 19000000, false)
                    },

                    new List<UpgradeSeller>
                    {
                        new UpgradeSeller("Перый продавец", 1, 0, true),
                        new UpgradeSeller("Второй продавец", 1, 3000, false),
                        new UpgradeSeller("Третий продавец", 1, 10000, false),
                        new UpgradeSeller("Четвертый продавец", 1, 50000, false),
                        new UpgradeSeller("Пятый продавец  ", 1, 100000, false),
                        new UpgradeSeller("Шестой продавец", 1, 300000, false),
                        new UpgradeSeller("Седьмой продавец", 1, 2500000, false),
                        new UpgradeSeller("Восьмой продавец", 1, 20000000, false)
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