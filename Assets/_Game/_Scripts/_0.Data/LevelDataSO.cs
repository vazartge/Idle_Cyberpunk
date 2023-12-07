using UnityEngine;

namespace Assets._Game._Scripts._0.Data
{
    [CreateAssetMenu(fileName = "LevelData", menuName = "Game ResData/LevelStore ResData")]
    public class LevelDataSO : ScriptableObject {
        public LevelData[] levels;
    }
    // Пример
    [System.Serializable]
    public class LevelData {
        public int level;
        public float price;
        public float priceIncreasePercent;
        public float earnings;
        public float earningsIncreasePercent;
        public AdditionalTable[] additionalTables;
    }

    [System.Serializable]
    public class AdditionalTable {
        public int unlockAtLevel;
        public float unlockPrice;
        public float earningsBonus;
        public float bonusIncreasePercent;
    }
}