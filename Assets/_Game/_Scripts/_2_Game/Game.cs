using Assets._Game._Scripts._0.Data._DataForLevelsUpgrade;
using Assets._Game._Scripts._5_Managers;
using Assets._Game._Scripts._6_Entities._Store;
using Assets._Game._Scripts._6_Entities._Units._Desktop;
using Assets._Game._Scripts._6_Entities._Units._PrebuilderDesktop;
using DG.Tweening;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;


namespace Assets._Game._Scripts._2_Game {
    public class Game : MonoBehaviour {
        public static Game Instance;
        private GameMode _gameMode;
        private UIMode _uiMode;
        private DataMode_ _dataMode;
        private StoreStatsService _storeStatsService;
        private StoreStats _storeStats;
        public LevelsUpgradesSO levelsUpgradesSO;
        [SerializeField] private int Level = 1;
        public bool IsDataLoaded { get; private set; }

       // public List<PrebuilderDesktop> prebuilders = new List<PrebuilderDesktop>();

        private bool isPaused;
        public bool IsPaused {
            get => isPaused;
            set {
                isPaused = value;
                if (isPaused) {
                    DOTween.PauseAll(); // Пауза всех анимаций
                    // Или DOTween.Pause("yourId"); для паузы конкретной анимации с идентификатором
                } else {
                    DOTween.PlayAll(); // Продолжение всех анимаций
                    // Или DOTween.Play("yourId"); для продолжения конкретной анимации с идентификатором
                }
            }
        }

        public GameMode GameMode => _gameMode;

        public UIMode UiMode => _uiMode;

        public DataMode_ DataMode => _dataMode;

        public StoreStats StoreStats => _storeStats;


        private void Awake() {
            Instance = this;
            DontDestroyOnLoad(this);
            Application.targetFrameRate = 30; // оставить только в Boot
            _storeStatsService = new StoreStatsService();

        }

        private void Start() {
            SceneManager.sceneLoaded += OnSceneLoaded; // Подписка на событие загрузки сцены
            LoadLevel();
        }
        private void LoadLevel()
        {
          //  ClearPrebuildersList();
            _storeStats = LoadGame(); // Загрузка или создание StoreStats
            int levelToLoad = StoreStats.LevelGame; // Получение уровня из StoreStats
            
            SceneManager.LoadScene(levelToLoad); // Загрузка соответствующей сцены
           // SceneManager.LoadScene(Level); // Загрузка соответствующей сцены
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
            SetupComponents();

        }
        public void SetupComponents() {

            _gameMode = FindObjectOfType<GameMode>();
            _uiMode = FindObjectOfType<UIMode>();
            _dataMode = FindObjectOfType<DataMode_>();
           // _storeStats = LoadGame();
            _dataMode.Construct(_gameMode, _uiMode);
            _gameMode.Construct(_dataMode, _uiMode, StoreStats);


            OnStartNewScene();
        }

        private void OnStartNewScene() {

            Debug.Log("Game Start");
        }
      
        private void SaveGame(StoreStats storeStats) {
            string json = _storeStatsService.SaveToJson(storeStats);
            PlayerPrefs.SetString("StoreStats", json);
            PlayerPrefs.Save();
        }

        private StoreStats LoadGame() {
            string json = PlayerPrefs.GetString("StoreStats", "");
            if (!string.IsNullOrEmpty(json)) {
                IsDataLoaded = true;
                return _storeStatsService.LoadFromJson(json);
            } else {
                // Создать новый StoreStats и загрузить LevelUpgrade из LevelsUpgradesSO
                IsDataLoaded = false;
                var newStoreStats = new StoreStats();
                newStoreStats.LevelUpgrade = GetLevelUpgradeForLevel(newStoreStats.LevelGame);
                return newStoreStats;
            }
        }
        private LevelUpgrade GetLevelUpgradeForLevel(int level) {
            // предполагая, что у тебя есть доступ к экземпляру LevelsUpgradesSO
            if (levelsUpgradesSO.LevelUpgrades.TryGetValue(level, out LevelUpgrade levelUpgrade)) {
                return levelUpgrade;
            } else {
                // Обработка случая, когда нет данных для уровня
                Debug.LogError($"No LevelUpgrade found for level {level}");
                return null;
            }
        }



        // В классе Game
        public void NextLevelStart() {
            IsPaused = true;
            DOTween.CompleteAll();
            ChangeLevel(_gameMode.Store.Stats.LevelGame);
            SaveGame(StoreStats);
            SceneManager.LoadScene(_gameMode.Store.Stats.LevelGame); // Загрузка соответствующей сцены
            LoadGame();

        }
        private void ChangeLevel(int newLevel) {
            var levelUpgrade = GetLevelUpgradeForLevel(newLevel);
            if (levelUpgrade != null) {
                StoreStats.LevelUpgrade = levelUpgrade;
            }
        }
        // public void RegisterPrebuilder(PrebuilderDesktop prebuilder) {
        //     if (!prebuilders.Contains(prebuilder)) {
        //         prebuilders.Add(prebuilder);
        //     }
        // }
        // public void ClearPrebuildersList() {
        //     prebuilders.Clear();
        // }
        public void CollectDataForSave()
        {
            var prebuilders = _gameMode.GetPrebuildersList(); //FindObjectsOfType<PrebuilderDesktop>();
            var prebuilderData = prebuilders.Select(p => new PrebuilderStats(p.ProductType, p.RotationAngleZ, p.IsActive, p.IsDesktopPurchased, p.transform.position)).ToList();
            StoreStats.PrebuilderStats = prebuilderData;
            //---------------------------------
            
            var desktopStatsList = new List<DesktopStats>();
            foreach (var desktop in GameMode.Store.GetDesktopUnitsList()) {
                if (desktop._desktopType == DesktopUnit.DesktopType.main) {
                    var stats = new DesktopStats(
                        desktop.transform.position,
                        desktop.ProductType,
                        desktop.Level,
                        desktop._desktopType == DesktopUnit.DesktopType.main,
                        desktop.IsUpgradedForLevel,
                        desktop.AdditionalDesktop != null
                    );
                    desktopStatsList.Add(stats);
                }
            }
            StoreStats.DesktopStatsList = desktopStatsList;
            // ... добавление данных от других сущностей ...
        }
       



    }
}