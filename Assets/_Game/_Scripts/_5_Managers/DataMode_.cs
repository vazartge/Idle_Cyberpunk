using System.Collections.Generic;
using System.Linq;
using Assets._Game._Scripts._0.Data;
using Assets._Game._Scripts._0.Data._Base;
using Assets._Game._Scripts._0.Data._DataForLevelsUpgrade;
using Assets._Game._Scripts._0.Data._DataForUpgrade;
using Assets._Game._Scripts._0.Data._SpritesForPersons;
using Assets._Game._Scripts._2_Game;
using Assets._Game._Scripts._6_Entities._Store._Products;
using UnityEngine;

namespace Assets._Game._Scripts._5_Managers {
    // Основной класс хранения всех ресурсов
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


   

    public class DataMode_ : MonoBehaviour {
        [SerializeField] private ResourceData ResData;
        [SerializeField] private CharacterSpritesDataSO _characterSpritesDataSo;
 
        [SerializeField] private LevelsUpgradesSO _levelsUpgrades;
        public Dictionary<ProductStoreType, BaseUpgradeSO> _dataForUpgradeDesktopsMap;
        [SerializeField] private IronHeartUpgradeSO _ironHeartUpgradeSO;
        [SerializeField] private MechanicalEyeUpgradeSO _mechanicalEyeUpgradeSo;
        [SerializeField] private NeurochipUpgradeSO _neurochipUpgradeSo;
        [SerializeField] private RoboticArmUpgradeSO _roboticArmUpgradeSO;

       
        [SerializeField] private GameObject _desktopPrefab;

        [SerializeField] private GameObject _prebuilderDesktopPrefab;
        [SerializeField] private GameObject _starForTooltipPrafab;

       // [SerializeField] private GameMode _gameMode;
       // [SerializeField] private UIMode _uiMode;
        private List<LevelInfo> _levels;

        public Dictionary<ProductStoreType, BaseUpgradeSO> DataForUpgradeDesktopsMap => _dataForUpgradeDesktopsMap;
        public Dictionary<ProductStoreType, int> MaxStarsOnLevelMap;
        public LevelsUpgradesSO LevelsUpgrades => _levelsUpgrades;

        public GameObject PrefabsForCreateDesktop => _desktopPrefab;
        public GameObject PrefabsForCreatePrebuilderDesktop => _prebuilderDesktopPrefab;
        public int GameLevel => Game.Instance.StoreStats.GameStats.LevelGame; //GameMode.GameLevel;
        public List<LevelInfo> Levels => _levels;

        public CharacterSpritesDataSO CharacterSpritesDataSo => _characterSpritesDataSo;

        public GameMode GameMode
        {
            get => Game.Instance.GameMode;
            set => Game.Instance.GameMode = value;
        }

        public UIMode UiMode
        {
            get => Game.Instance.UIMode;
            set => Game.Instance.UIMode = value;
        }

        private void Awake() {
            Game.Instance.DataMode = this;
        }

        private void Start() {
            
            //словарь данных для обновления столов 
            _dataForUpgradeDesktopsMap = new Dictionary<ProductStoreType, BaseUpgradeSO>()
            {
                { ProductStoreType.IronHeartProduct , _ironHeartUpgradeSO},
                { ProductStoreType.MechanicalEyeProduct , _mechanicalEyeUpgradeSo},
                { ProductStoreType.NeurochipProduct , _neurochipUpgradeSo},
                { ProductStoreType.RoboticArmProduct , _roboticArmUpgradeSO},
            };
            // Словарь для определения количества звезд в тултипе
            MaxStarsOnLevelMap =  new Dictionary<ProductStoreType, int>();
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
      
        
        public void UpdateMaxStarsOnLevel() {
            foreach (var kvp in _dataForUpgradeDesktopsMap) {//Определение максимального количества звезд для текущего уровня
                MaxStarsOnLevelMap[kvp.Key] = kvp.Value.GetMaxStarsForLevel(GameLevel);
            }
        }
        public int GetMaxStarsForProductType(ProductStoreType productStoreType) { // Получение максимального количества звезд уже в конкретный тултип по типу продукта
            if (MaxStarsOnLevelMap.TryGetValue(productStoreType, out int maxStars)) {
                return maxStars;
            }
            return 0; // Возвращаем 0, если в словаре нет записи для данного типа продукта
        }


        public Sprite GetIconByProductType(ProductStoreType productStoreType) { // Получение иконки по типу продукта
            var productInfo = ResData.ProductsInfo.FirstOrDefault(p => p.ProductStoreType.ToString() == productStoreType.ToString());
            return productInfo != null ? productInfo.ProductIcon : ResData.BaseIcon;
        }


        public BaseUpgradeSO GetProductUpgradeSO(ProductStoreType productStoreType) { // Получение данных для прокачивания стола по типу продукта
            return DataForUpgradeDesktopsMap[productStoreType];
        }


        public GameObject GetPrefabForDesktop() { // Получение префаба стола
            return PrefabsForCreateDesktop;
        }
      

    }
}