using System.Collections.Generic;
using System.Linq;
using Assets._Game._Scripts._0.Data;
using Assets._Game._Scripts._0.Data._Base;
using Assets._Game._Scripts._0.Data._DataForLevelsUpgrade;
using Assets._Game._Scripts._0.Data._DataForUpgrade;
using Assets._Game._Scripts._2_Game;

using UnityEngine;

namespace Assets._Game._Scripts._5_Managers {
    public class LevelInfo {
        public int Level { get; set; }
        public long? CoinsAtStart { get; set; }
        public int NumberOfTables { get; set; }

        public LevelInfo(int level, int? coinsAtStart, int numberOfTables) {
            Level = level;
            CoinsAtStart = coinsAtStart;
            NumberOfTables = numberOfTables;
        }
    }


    public enum ProductType {
        MechanicalEyeProduct,
        RoboticArmProduct,
        IronHeartProduct,
        NeurochipProduct
    }

    public class DataMode_ : MonoBehaviour {
        [SerializeField] private ResourceData ResData;

        [SerializeField] private LevelsUpgradesSO _levelsUpgrades;
        public Dictionary<ProductType, BaseUpgradeSO> _dataForUpgradeDesktopsMap;
        [SerializeField] private IronHeartUpgradeSO _ironHeartUpgradeSO;
        [SerializeField] private MechanicalEyeUpgradeSO _mechanicalEyeUpgradeSo;
        [SerializeField] private NeurochipUpgradeSO _neurochipUpgradeSo;
        [SerializeField] private RoboticArmUpgradeSO _roboticArmUpgradeSO;

       
        [SerializeField] private GameObject _desktopPrefab;

        [SerializeField] private GameObject _prebuilderDesktopPrefab;
        [SerializeField] private GameObject _starForTooltipPrafab;

        [SerializeField] private GameMode _gameMode;
        [SerializeField] private UIMode _uiMode;
        private List<LevelInfo> _levels;

        public Dictionary<ProductType, BaseUpgradeSO> DataForUpgradeDesktopsMap => _dataForUpgradeDesktopsMap;
        public Dictionary<ProductType, int> MaxStarsOnLevelMap;
        public LevelsUpgradesSO LevelsUpgrades => _levelsUpgrades;

        public GameObject PrefabsForCreateDesktop => _desktopPrefab;
        public GameObject PreafabsForCreatePrebuilderDesktop => _prebuilderDesktopPrefab;
        public int GameLevel => Game.Instance.GameLevel;
        public List<LevelInfo> Levels => _levels;


        private void Awake() {
            //словарь данных для обновления столов 
            _dataForUpgradeDesktopsMap = new Dictionary<ProductType, BaseUpgradeSO>()
            {
                { ProductType.IronHeartProduct , _ironHeartUpgradeSO},
                { ProductType.MechanicalEyeProduct , _mechanicalEyeUpgradeSo},
                { ProductType.NeurochipProduct , _neurochipUpgradeSo},
                { ProductType.RoboticArmProduct , _roboticArmUpgradeSO},
            };
            // Словарь для определения количества звезд в тултипе
            MaxStarsOnLevelMap =  new Dictionary<ProductType, int>();
            UpdateMaxStarsOnLevel();
            // Инициализация уровней
            _levels = new List<LevelInfo>
            {
                new LevelInfo(level: 1, 10, 2),
                new LevelInfo(2, null, 4),
                new LevelInfo(3, null, 6),
                new LevelInfo(4, null, 8)
            };


        }
        public void Construct(GameMode gameMode, UIMode uiMode) {
            _gameMode = gameMode;
            _uiMode = uiMode;
            Debug.Log(_levelsUpgrades.LevelUpgrades[1].Buyers[0]);

        }

        public long GetCoinsForStartLevel(int currentLevel) {
            return Levels[currentLevel].CoinsAtStart ?? 0;
        }
        public void UpdateMaxStarsOnLevel() {
            foreach (var kvp in _dataForUpgradeDesktopsMap) {
                MaxStarsOnLevelMap[kvp.Key] = kvp.Value.GetMaxStarsForLevel(GameLevel);
            }
        }
        public int GetMaxStarsForProductType(ProductType productType) {
            if (MaxStarsOnLevelMap.TryGetValue(productType, out int maxStars)) {
                return maxStars;
            }
            return 0; // Возвращаем 0, если в словаре нет записи для данного типа продукта
        }

        // public GameObject GetStarPrefab()
        // {
        //     return _starForTooltipPrafab;
        // }

        public Sprite GetIconByProductType(ProductType productType) {
            var productInfo = ResData.ProductsInfo.FirstOrDefault(p => p.ProductType.ToString() == productType.ToString());
            return productInfo != null ? productInfo.ProductIcon : ResData.BaseIcon;
        }


        public BaseUpgradeSO GetProductUpgradeSO(ProductType productType) {
            return DataForUpgradeDesktopsMap[productType];
        }

        // public GameObject GetPrefabForPrebuilderDesktop() {
        //     return PreafabsForCreatePrebuilderDesktop;
        // }

        public GameObject GetPrefabForDesktop() {
            return PrefabsForCreateDesktop;
        }
    }
}