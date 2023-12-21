using Assets._Game._Scripts._0.Data._DataForLevelsUpgrade;
using Assets._Game._Scripts._5_Managers;
using Assets._Game._Scripts._6_Entities._Store;
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
        [SerializeField] private StoreStats _storeStats;
        public LevelsUpgradesSO levelsUpgradesSO;
        [SerializeField] private int Level = 1;
        public bool IsDataLoaded { get; private set; }

        // public List<PrebuilderDesktop> prebuilders = new List<PrebuilderDesktop>();

        private bool isPaused;
        private int _countRegister = 0;

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

        public StoreStats StoreStats {
            get => Game.Instance._storeStats;
            set => Game.Instance._storeStats = value;
        }

        public int CountRegister
        {
            get => Game.Instance._countRegister;
            set => Game.Instance._countRegister = value;
        }


        private void Awake() {
            if (Instance != null && Instance != this) {
                Destroy(this.gameObject);
            }
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
            Application.targetFrameRate = 30; // оставить только в Boot


        }

        private void Start()
        {
            InitializeGame();
        }

        private void InitializeGame() {

            this._storeStatsService = new StoreStatsService();

            //  ClearPrebuildersList();

            StoreStats = LoadGame(); // Загрузка или создание StoreStats
            //SceneManager.sceneLoaded += OnSceneLoaded; // Подписка на событие загрузки сцены
            LoadLevel();

        }


        private void LoadLevel() {

            int levelToLoad = StoreStats.LevelGame; // Получение уровня из StoreStats

            SceneManager.LoadScene(levelToLoad); // Загрузка соответствующей сцены
                                                 // SceneManager.LoadScene(Level); // Загрузка соответствующей сцены

        }

        public void RegisterGameMode(GameMode gameMode) {
            _gameMode = gameMode;
            CountRegister++;
            CheckCountRegister();
        }
        public void RegisterUIMode(UIMode uiMode) {
            _uiMode= uiMode;
            CountRegister++;
            CheckCountRegister();
        }

        public void RegisterDataMode_(DataMode_ dataMode) {
            _dataMode = dataMode;
            CountRegister++;
            CheckCountRegister();
        }
        private void CheckCountRegister() {
            Debug.Log($"CountRegister = {CountRegister}");
            if (CountRegister >= 3) {
                Debug.Log("Game Start And Registered");
                _dataMode.Construct(_gameMode, _uiMode);
                _gameMode.Construct(_dataMode, _uiMode, StoreStats);
                CountRegister = 0;
            }
        }

        public void OnSaveGameButton() {
            SaveGame();
        }

        public void OnButtonLoadGame()
        {
            
            SceneManager.LoadScene("Boot");
            Debug.Log($"CountRegister = {CountRegister}");
            InitializeGame();
        }
        private void SaveGame() {
            CollectDataForSave();
            Debug.Log("Save Game!");
            _storeStatsService = new StoreStatsService();
            string json = _storeStatsService.SaveToJson(StoreStats);
            PlayerPrefs.SetString("StoreStats", json);
            PlayerPrefs.Save();
        }

        private StoreStats LoadGame() {

            Debug.Log("Load Game!");
            string json = PlayerPrefs.GetString("StoreStats", "");
            if (!string.IsNullOrEmpty(json)) {
                IsDataLoaded = true;
                return _storeStatsService.LoadFromJson(json);
            } else {
                // Создать новый StoreStats и загрузить LevelUpgrade из LevelsUpgradesSO
                IsDataLoaded = false;
                var newStoreStats = new StoreStats();
                newStoreStats.LevelUpgrade = GetLevelUpgradeForLevel(newStoreStats.LevelGame);
                Debug.Log($"GetLevelUpgradeForLevel(newStoreStats.LevelGame) = null {GetLevelUpgradeForLevel(newStoreStats.LevelGame)==null}");
                Debug.Log($"StoreStats.LevelUpgrade == null {newStoreStats.LevelUpgrade == null}");
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
         
            var coins = StoreStats.Coins;
            var levelGame = StoreStats.LevelGame;
            StoreStats = new StoreStats();
            StoreStats.Coins = coins;
            StoreStats.LevelGame = levelGame;
            ChangeLevel(levelGame);

            SaveGame();
            SceneManager.LoadScene(StoreStats.LevelGame); // Загрузка соответствующей сцены
            LoadLevel();

        }

        private void ChangeLevel(int newLevel) {
            var levelUpgrade = GetLevelUpgradeForLevel(newLevel);

            StoreStats.LevelUpgrade = levelUpgrade;

        }

        public void CollectDataForSave() {

            _gameMode = FindObjectOfType<GameMode>();


            var prebuilders = _gameMode.GetPrebuildersList();
            //   if (prebuilders != null) {
            var prebuilderData = prebuilders.Select(p => new PrebuilderStats(p.ProductType, p.RotationAngleZ, p.IsActive, p.IsDesktopPurchased/*, p.transform.position*/)).ToList();
            StoreStats.PrebuilderStats = prebuilderData;
            // }
            //  else
            //   {
            Debug.Log($"_gameMode.GetPrebuildersList() == null {_gameMode.GetPrebuildersList() == null}");
            //   }

            //---------------------------------



            //if (_gameMode.Store.GetDesktopUnitsList() != null) {
            var desktopStatsList = new List<DesktopStats>();
            foreach (var desktop in _gameMode.Store.GetDesktopUnitsList()) {
                Debug.Log($"_gameMode.Store.GetDesktopUnitsList() == null {_gameMode.Store.GetDesktopUnitsList() == null}");
                if (desktop._mainDesktop.CurDesktopType == DesktopType.main) {
                    var stats = new DesktopStats(
                        /*desktop.transform.position,*/
                        desktop._mainDesktop.ProductType,
                        desktop._mainDesktop.Level,
                        desktop._mainDesktop.CurDesktopType,
                        desktop._mainDesktop.IsAdditionalDesktop,
                        desktop._mainDesktop.IsUpgradedForLevel
                    );
                    desktopStatsList.Add(stats);
                    Debug.Log($"desktop.ProductType = {desktop._mainDesktop.ProductType} , desktop.Level = {desktop._mainDesktop.Level} , desktop._mainDesktop.CurDesktopType = {desktop._mainDesktop.CurDesktopType} , desktop._mainDesktop.AdditionalDesktop = {desktop._mainDesktop.IsAdditionalDesktop}, desktop._mainDesktop.IsUpgradedForLevel = {desktop._mainDesktop.IsUpgradedForLevel} ");
                }
            }
            StoreStats.DesktopStatsList = desktopStatsList;


            Debug.Log($"_gameMode.Store.GetDesktopUnitsList() == null  {_gameMode.Store.GetDesktopUnitsList() == null}");



            Debug.Log($"Saving StoreStats: Coins = {StoreStats.Coins}, LevelGame = {StoreStats.LevelGame}, SpeedMoveCustomer = {StoreStats.SpeedMoveCustomer}, SpeedMoveSeller = {StoreStats.SpeedMoveSeller}, ProductionSpeed = {StoreStats.ProductionSpeed}, TakingOrder = {StoreStats.TakingOrder}");

            // ... добавление данных от других сущностей ...
        }

        // Вызывается при выходе из приложения
        private void OnApplicationQuit() {
            SaveGame();
        }

        // Вызывается при паузе приложения (например, при сворачивании на мобильном устройстве)
        private void OnApplicationPause(bool pauseStatus) {
             if (pauseStatus) {
                 SaveGame();
             }
        }


    }
}