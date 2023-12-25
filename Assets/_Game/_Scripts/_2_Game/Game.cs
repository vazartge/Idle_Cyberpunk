using Assets._Game._Scripts._0.Data._DataForLevelsUpgrade;
using Assets._Game._Scripts._5_Managers;
using Assets._Game._Scripts._6_Entities._Store;
using DG.Tweening;
using System.Collections.Generic;
using System.Linq;
using Assets._Game._Scripts._0.Data;
using UnityEngine;
using UnityEngine.SceneManagement;


namespace Assets._Game._Scripts._2_Game {
    public class Game : MonoBehaviour {
        [SerializeField] private ReferencesData _referencesData;
        public static Game Instance;

        
        private StoreStatsService _storeStatsService;
        // [SerializeField] private StoreStats _storeStats;
        // public LevelsUpgradesSO levelsUpgradesSO;
        // [SerializeField] private int Level = 1;
        public bool IsDataLoaded;
        // public bool IsNewLevel;

        // public List<PrebuilderDesktop> prebuilders = new List<PrebuilderDesktop>();

        private bool isPaused;
        private int _countRegister = 0;
        public bool IsInitializedGoogleService;

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

        public GameMode GameMode {
            get => _referencesData.GameMode;
            set => _referencesData.GameMode = value;
        }

        public UIMode UIMode {
            get => _referencesData.UiMode;
            set => _referencesData.UiMode = value;
        }

        public DataMode_ DataMode {
            get => _referencesData.DataMode;
            set => _referencesData.DataMode = value;
        }

        public StoreStats StoreStats {
            get => _referencesData.StoreStats;
            set => _referencesData.StoreStats = value;
        }

        public int CountRegister {
            get => _referencesData.CountRegister;
            set => _referencesData.CountRegister = value;
        }

        public LevelsUpgradesSO LevelsUpgradesSo {
            get => _referencesData.LevelsUpgradesSO;
            set => _referencesData.LevelsUpgradesSO = value;
        }

        private AudioSource AudioSource
        {
            get=> _referencesData.AudioSource;
            set=> _referencesData.AudioSource = value;
        }

        private void Awake() {
            if (Instance != null) {
                if (Instance != this) {
                    Destroy(this.gameObject);
                }
                return;
            }
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
            Application.targetFrameRate = 30; // оставить только в Boot

            AudioSource = GetComponent<AudioSource>();
            AudioSource.clip = _referencesData.BackGroundMusicClip;


        }

        private void Start() {

            // InitializeGame();
            InitializeGame();
           

        }
        public void OnIAPInitialized() {
            Debug.Log("Initialized IAP");
            IsInitializedGoogleService = true;
        }

        private void InitializeGame() {

            _storeStatsService = new StoreStatsService();

            //  ClearPrebuildersList();
            // if (Application.platform == RuntimePlatform.Android) {
            //     StoreStats = LoadGame(); // Загрузка или создание StoreStats
            //     IAPManager.Instance.RestorePurchases();
            // } else {
            //     StoreStats = LoadGame(); // Загрузка или создание StoreStats
            //     LoadLevel();
            // }
            StoreStats = LoadGame(); // Загрузка или создание StoreStats
            CheckMusic();
            LoadLevel();


        }




        private void LoadLevel() {


            SceneManager.LoadScene(StoreStats.GameStats.LevelGame); // Загрузка соответствующей сцены
                                                                    // SceneManager.LoadScene(Level); // Загрузка соответствующей сцены

        }

        public void RegisterGameMode(GameMode gameMode) {
            GameMode = gameMode;
            CountRegister++;
            CheckCountRegister();
        }
        public void RegisterUIMode(UIMode uiMode) {
            UIMode= uiMode;
            CountRegister++;
            CheckCountRegister();
        }

        public void RegisterDataMode_(DataMode_ dataMode) {
            DataMode = dataMode;
            CountRegister++;
            CheckCountRegister();
        }
        private void CheckCountRegister() {
            Debug.Log($"CountRegister = {CountRegister}");
            if (CountRegister >= 3) {
                Debug.Log("Game Start And Registered");
                DataMode.Construct(GameMode, UIMode);
                GameMode.Construct(DataMode, UIMode);
                CountRegister = 0;

            }
        }

        public void OnSaveGameButton() {
            SaveGame();
        }

        public void OnButtonLoadGame() {

            SceneManager.LoadScene("Boot");
            Debug.Log($"CountRegister = {CountRegister}");
            InitializeGame();
        }
        private void SaveGame() {
            if (GameMode == null) return;
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
                newStoreStats.LevelUpgrade = GetLevelUpgradeForLevel(newStoreStats.GameStats.LevelGame);
                Debug.Log($"GetLevelUpgradeForLevel(newStoreStats.LevelGame) = null {GetLevelUpgradeForLevel(newStoreStats.GameStats.LevelGame)==null}");
                Debug.Log($"StoreStats.LevelUpgrade == null {newStoreStats.LevelUpgrade == null}");
                return newStoreStats;
            }
        }
        private LevelUpgrade GetLevelUpgradeForLevel(int level) {
            // предполагая, что у тебя есть доступ к экземпляру LevelsUpgradesSO
            if (LevelsUpgradesSo.LevelUpgrades.TryGetValue(level, out LevelUpgrade levelUpgrade)) {
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

            // var coins = StoreStats.Coins;
            // var levelGame = StoreStats.LevelGame;
            //
            // StoreStats = null;
            // StoreStats = new StoreStats();
            // StoreStats.Coins = coins;
            // StoreStats.LevelGame = levelGame;
            StoreStats.GameStats.Coins += 10;
            StoreStats.GameStats.SpeedMoveSeller = 5f;
            StoreStats.GameStats.SpeedMoveSeller = 5f;
            StoreStats.GameStats.ProductionSpeed = 2f;
            StoreStats.GameStats.TakingOrder = 2f;
            StoreStats.DesktopStatsList.Clear();
            StoreStats.PrebuilderStats.Clear();
            ChangeLevel(StoreStats.GameStats.LevelGame);


            SceneManager.LoadScene(StoreStats.GameStats.LevelGame); // Загрузка соответствующей сцены


            // LoadLevel();

        }

        private void ChangeLevel(int newLevel) {
            var levelUpgrade = GetLevelUpgradeForLevel(newLevel);

            StoreStats.LevelUpgrade = levelUpgrade;

        }

        public void CollectDataForSave() {


            var prebuilderData = new List<PrebuilderStats>();
            var prebuilders = GameMode.GetPrebuildersList();
            if (prebuilders != null) {
                foreach (var prebuilder in prebuilders) {
                    if (prebuilder != null) {
                        var stats = new PrebuilderStats(
                            prebuilder.gameObject.transform.position,
                            prebuilder.ProductStoreType,
                            prebuilder.RotationAngleZ
                        );
                        prebuilderData.Add(stats);
                        Debug.Log(
                            $"stats.ProductStoreType ={stats.ProductStoreType}, stats.RotationAngleZ = {stats.RotationAngleZ}");
                    } else {
                        Debug.Log("Prebuilder == null");
                    }

                }

                StoreStats.PrebuilderStats = prebuilderData;
            } else {
                StoreStats.PrebuilderStats.Clear();
            }
            //---------------------------------


            if (GameMode.Store.GetDesktopUnitsList() != null) {
                var desktopStatsList = new List<DesktopStats>();
                foreach (var desktop in GameMode.Store.GetDesktopUnitsList()) {
                    if (desktop != null) {
                        var stats = new DesktopStats(
                            desktop.gameObject.transform.position,
                            desktop.RotationAngleZ,
                            desktop.ProductStoreType,
                            desktop.Level,
                            desktop.IsAdditionalDesktop,
                            desktop.IsUpgradedForLevel
                        );
                        desktopStatsList.Add(stats);
                        Debug.Log(
                            $"desktop.ProductStoreType = {desktop.ProductStoreType} , desktop.Level = {desktop.Level} ,  desktop.AdditionalDesktop = {desktop.IsAdditionalDesktop}, desktop.IsUpgradedForLevel = {desktop.IsUpgradedForLevel} ");

                    } else {
                        Debug.Log("Desktop == null");
                    }


                }

                StoreStats.DesktopStatsList = desktopStatsList;


                Debug.Log($"Saving StoreStats: Coins = {StoreStats.GameStats.Coins}, LevelGame = {StoreStats.GameStats.LevelGame}, SpeedMoveCustomer = {StoreStats.GameStats.SpeedMoveCustomer}, SpeedMoveSeller = {StoreStats.GameStats.SpeedMoveSeller}, ProductionSpeed = {StoreStats.GameStats.ProductionSpeed}, TakingOrder = {StoreStats.GameStats.TakingOrder}, IsMusic = {StoreStats.GameStats.IsPlayingMusic}");

                // ... добавление данных от других сущностей ...
            } else {
                StoreStats.DesktopStatsList.Clear();
            }


            CollectSceneStatsForSave();
        }

        private void CollectSceneStatsForSave() {
            var nameScene = SceneManager.GetActiveScene().name;

            if (StoreStats.SceneStatsList == null) {
                StoreStats.SceneStatsList = new List<SceneStat>();
            }

            var currentStat = StoreStats.SceneStatsList.FirstOrDefault(stat => stat.NameScene == nameScene);
            if (currentStat != null) return;

            var stat = new SceneStat {
                NameScene = nameScene,
                IsOpened = true
            };
            StoreStats.SceneStatsList.Add(stat);
            Debug.Log($" StoreStats.SceneStatsList.Add(stat) Name = {stat.NameScene}");
        }

        // Вызывается при выходе из приложения
        private void OnApplicationQuit() {
            SaveGame();
        }

        // Вызывается при паузе приложения (например, при сворачивании на мобильном устройстве)
        private void OnApplicationPause(bool pauseStatus) {

            SaveGame();

        }


        public void OnRewardedButtonFor5LevelsUpgrade() {
            Debug.Log("Start RewardedFor5LevelsUpgrade");
        }

        public void OnRewardedButtonForBoostProduction() {
            Debug.Log("Start RewardedForBoosProduction");
        }


        #region IAP Purchase

        public void OnButtonPurchaseNOADS() {
            //#if UNITY_EDITOR
            Debug.Log("Try Purchase NOADS");
            //  Game.Instance.StoreStats.PurchasedDisabledAds = true;
            //   Game.Instance.SaveGame();

            //#endif
            //#if !UNITY_EDITOR && PLATFORM_ANDROID
            IAPManager.Instance.BuyDisableADS();
            //#endif
        }

        public void OnButtonPurchaseIncrease2xProfit() {
            //#if UNITY_EDITOR
            Debug.Log("Try Purchase PurchaseIncrease2xProfit");
            //Game.Instance.StoreStats.PurchasedIncreaseProfit = true;
            //  Game.Instance.SaveGame();

            //#endif
            //#if !UNITY_EDITOR && PLATFORM_ANDROID
            IAPManager.Instance.BuyIncreaseProfit();
            //#endif
        }
        // public void OnSuccessPurchasedDisabledADS() {
        //     StoreStats.PurchasedDisabledAds = true;
        // }
        // public void OnSuccessPurchasedIncreaseProfit() {
        //     StoreStats.PurchasedIncreaseProfit = true;
        // }

        public void OnPurchasesRestored(bool success) {
            if (success) {
                Debug.Log($"OnPurchasesRestored(bool success) = {success}");

                // Здесь можно вызвать методы, которые обновят игровой интерфейс или состояние игры в соответствии с восстановленными покупками.
                // Например, если реклама была отключена, обновить UI, чтобы отразить это.
            } else {
                Debug.Log($"OnPurchasesRestored(bool success) = {success}");
                // Обработать случай, если покупки не были восстановлены.
                // Например, показать сообщение пользователю.
            }

            //SceneManager.sceneLoaded += OnSceneLoaded; // Подписка на событие загрузки сцены
            LoadLevel();
        }
        #endregion

        #region Settings Music

        public void PlayMusic() {
            if (!AudioSource.isPlaying) {
                AudioSource.Play();
                StoreStats.GameStats.IsPlayingMusic = true;
            }


        }
        public void PauseMusic() {
            if (AudioSource.isPlaying) {
                AudioSource.Pause();
                StoreStats.GameStats.IsPlayingMusic = false;
            }

        }
        private void CheckMusic() {
            if (!StoreStats.GameStats.IsPlayingMusic) {
                PauseMusic();
            } else {
                PlayMusic();
            }
        }
        public bool IsMusicPlaying() {
            return AudioSource.isPlaying;
        }
        #endregion



        public bool GetSceneStatForLevel() {
            if (StoreStats.SceneStatsList == null) {
                return false;
            }

            return StoreStats.SceneStatsList.Any(stat => stat.NameScene == SceneManager.GetActiveScene().name);

        }
    }
}