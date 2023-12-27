using Assets._Game._Scripts._0.Data._DataForLevelsUpgrade;
using Assets._Game._Scripts._5_Managers;
using Assets._Game._Scripts._6_Entities._Store;
using DG.Tweening;
using System.Collections.Generic;
using System.Linq;
using Assets._Game._Scripts._0.Data;
using Assets._Game._Scripts._6_Entities._Units._Desktop;
using UnityEngine;
using UnityEngine.SceneManagement;


namespace Assets._Game._Scripts._2_Game {
    // Главный класс, который управляет игрой

    public class Game : MonoBehaviour {


        #region FIELDS
        public static Game Instance; // Синглтон данного класс
        public float adInterval = 10f; // Интервал в секундах для показа рекламы межстарничной
        public float rewardedADSInterval = 10f; // интервал для реварад, чтобы не было множественных нажатий кнопок
        public bool IsDataLoaded; // Есть ли файл сохранения или в превый раз игра запускается
        public bool IsInitializedGoogleService;
        public LevelsUpgradesSO levelsUpgradesSO; //Структура для получения данных об улучшениях на уровне и применении в системе сохранений
        public AudioClip BackGroundMusicClip; // Музыкальный клип - один

        [SerializeField] private ReferencesData _referencesData; //В данный момент не используется - для хранения ссылок на другие объекты
        private float timerRewardedADS; // Таймер ревард рекламы
        private float timerInterstitialADS; // Таймер межстраничной рекламы
        private bool wasPlayingMusic; // Временный флаг для сохранения текущего проигрывания музыки при паузе, чтобы вернуть это значения при продолжении игры
        private StoreStatsService _storeStatsService; // Сервис по сериализации сохранения в JSON
        private bool isPaused; // Есть ли пауза игры
        private DesktopUnit _desktopForReward; // Ссылка на временный объект стола для вознаграждения - если null, то ревард на Увеличение прибыли, если not null, то прокачка этого стола
        private GameMode gameMode; // Игровой режим
        private UIMode uiMode; // UI режим
        private DataMode_ dataMode; // Данные
        private StoreStats storeStats; // Данные для сохранений
        private AudioSource audioSource; // Проигрыватель звуков в игре - один
        private bool isRewardedADSReady; // Проверка готовности реварда

        #endregion

        #region PROPERTIES

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
            get => gameMode;
            set => gameMode = value;
        }

        public UIMode UIMode {
            get => uiMode;
            set => uiMode = value;
        }

        public DataMode_ DataMode {
            get => dataMode;
            set => dataMode = value;
        }

        public StoreStats StoreStats {
            get => storeStats;
            set => storeStats = value;
        }

        public LevelsUpgradesSO LevelsUpgradesSo {
            get => levelsUpgradesSO;
            set => levelsUpgradesSO = value;
        }

        private AudioSource AudioSource {
            get => audioSource;
            set => audioSource = value;
        }
        public bool IsInitializedAppodeal { get; set; }

        public bool IsRewardedAdsReady {
            get => isRewardedADSReady;
            set => isRewardedADSReady = value;
        }

        public bool IsRewardedIncreaseProfit2X {
            get => isRewardedIncreaseProfit2x;
            set => isRewardedIncreaseProfit2x = value;
        }

        #endregion private bool isFirstInterstitialADS;

        #region NOT USED FIELDS

        private bool isStartBannerADS;
        private int _countRegister = 0;
        private bool IsGameModeRegistered;
        private bool IsUIModeRegistered;
        private bool IsDataModeRegistered;
        private bool IsRewardedDesktopTooltip5LevelsAdd; // Не используется, но для получения реварда за просмотр из тултипа стола
        private bool isRewardedIncreaseProfit2x;// Не используется, но для получения реварда за просмотр из кнопки HUD увеличения прибыли
        #endregion

        #region AWAKE START UPDATE
        
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
            AudioSource.clip = BackGroundMusicClip;

            timerInterstitialADS = adInterval; // Сбрасываем таймер
            timerRewardedADS = rewardedADSInterval;
        }



        private void Start() {

            // InitializeGame();
            InitializeGame();


        }

        private void Update() {
            InterstitialTimerADS();
            RewardedTimer();
            //StartBanner();
        }


        #endregion

        #region INITIALIZATIONS

        public void OnIAPInitialized() { // Сервис покупок IAP
            Debug.Log("Initialized IAP");
            IsInitializedGoogleService = true;
        }

        private void InitializeGame() { // Начало игры

            _storeStatsService = new StoreStatsService();

            StoreStats = LoadGame(); // Загрузка или создание StoreStats
            CheckMusic();
            LoadLevel();


        }

        public void AppodealInitialized() { 
            Debug.Log("AppodealInitialized");

            IsInitializedAppodeal = true;
            IsRewardedAdsReady = true;
            StartBanner();
            // StartADS();
        }

        #endregion

        #region LOAD DATA



        private void LoadLevel() { // Загрузка уровня


            SceneManager.LoadScene(StoreStats.GameStats.LevelGame); // Загрузка соответствующей сцены
                                                                    // SceneManager.LoadScene(Level); // Загрузка соответствующей сцены
                                                                    //  DataModeConstruct();
            Invoke("FindTags", 2f);


        }

        private void FindTags() { // Получение ссылок на объекты на сцене, другими способами ссылки ломались почему-то

            DataMode = GameObject.FindGameObjectWithTag("DataMode").GetComponentInChildren<DataMode_>();
            GameMode = GameObject.FindGameObjectWithTag("GameMode").GetComponentInChildren<GameMode>();
            UIMode = GameObject.FindGameObjectWithTag("UIMode").GetComponentInChildren<UIMode>();
            Debug.Log($"DataMode == null {DataMode == null}");
            Debug.Log($"GameMode == null {GameMode == null}");
            Debug.Log($"UIMode == null {UIMode == null}");
        }

        

        public void OnButtonLoadGame() { // Для загрузки сохранения из любого места

            SceneManager.LoadScene("Boot");

            InitializeGame();
        }



        private StoreStats LoadGame() { // Загрузка данных сохранения из PlayerPrefs или если такого нет, то создание нового

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
        private LevelUpgrade GetLevelUpgradeForLevel(int level) { // Получение данных для текущего уровня игры из  LevelsUpgradesSO
            // предполагая, что у тебя есть доступ к экземпляру LevelsUpgradesSO
            if (LevelsUpgradesSo.LevelUpgrades.TryGetValue(level, out LevelUpgrade levelUpgrade)) {
                return levelUpgrade;
            } else {
                // Обработка случая, когда нет данных для уровня
                Debug.LogError($"No LevelUpgrade found for level {level}");
                return null;
            }
        }

        #endregion

        #region NEXT LEVEL

        
        // В классе Game
        public void NextLevelStart() { // Переход на следующий уровень - удаление информации из сохранения , кроме денег и номера уровня

            IsPaused = true;
            DOTween.CompleteAll();

           
            StoreStats.GameStats.Coins += 10;
            StoreStats.GameStats.SpeedMoveSeller = 5f;
            StoreStats.GameStats.SpeedMoveSeller = 5f;
            StoreStats.GameStats.ProductionSpeed = 2f;
            StoreStats.GameStats.TakingOrder = 2f;
            StoreStats.DesktopStatsList.Clear();
            StoreStats.PrebuilderStats.Clear();
            if (StoreStats.GameStats.LevelGame >= 5) {
                GameOver();
            }
            ChangeLevel(StoreStats.GameStats.LevelGame);


            SceneManager.LoadScene(StoreStats.GameStats.LevelGame); // Загрузка соответствующей сцены

            

        }

        private void GameOver() { // Конец игры при достижении 5го уровня
            PauseGame();
            UIMode.GameOverWindowGO.SetActive(true);
        }

        private void ChangeLevel(int newLevel) { // Получение данных для текущего уровня игры из  LevelsUpgradesSO при переходе на новый уровень
            var levelUpgrade = GetLevelUpgradeForLevel(newLevel);

            StoreStats.LevelUpgrade = levelUpgrade;

        }

        
        #endregion

        #region SAVE DATA
        public void OnSaveGameButton() {
            SaveGame();
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

        public void CollectDataForSave() { // Сборщик данных при сохранении: статы пребилдеров, столов и прокачки магазина


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
        public bool GetSceneStatForLevel() {
            if (StoreStats.SceneStatsList == null) {
                return false;
            }

            return StoreStats.SceneStatsList.Any(stat => stat.NameScene == SceneManager.GetActiveScene().name);

        }
        // Вызывается при выходе из приложения
        private void OnApplicationQuit() {
            if (GameMode != null) {
                SaveGame();
            }
        }

        // Вызывается при паузе приложения (например, при сворачивании на мобильном устройстве)
        private void OnApplicationPause(bool pauseStatus) {
            if (GameMode != null) {
                SaveGame();
            }


        }
        
        #endregion
        
        #region IAP Purchase

        public void OnButtonPurchaseNOADS() {
            Debug.Log("Try Purchase NOADS");
            IAPManager.Instance.BuyDisableADS();
        }

        public void OnButtonPurchaseIncrease2xProfit() {
            Debug.Log("Try Purchase PurchaseIncrease2xProfit");
            IAPManager.Instance.BuyIncreaseProfit();
        }

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

        #region SETTINGS

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
        public void PauseGame() {
            isPaused = true;
            Time.timeScale = 0;
            if (AudioSource.isPlaying) {
                wasPlayingMusic = true;
                AudioSource.Pause();
            } else {
                wasPlayingMusic = false;
            }
        }

        public void UnPauseGame() {
            isPaused = false;
            Time.timeScale = 1;
            if (wasPlayingMusic) {
                AudioSource.Play();
            }
        }

        public bool GetIsRewardedADSReady() {
            return IsRewardedAdsReady;
        }
        public void OnExitApplicationButton() { 
            StoreStats = null;
            // Очистка PlayerPrefs
            PlayerPrefs.DeleteAll();
            PlayerPrefs.Save();

            Application.Quit();
        }

        public void OnPlayGameAgain() {

            StoreStats = null;
            // Очистка PlayerPrefs
            PlayerPrefs.DeleteAll();
            PlayerPrefs.Save();

            SceneManager.LoadScene("Boot");


        }
        #endregion
        
        #region ADS APPODEAL
        public void UpdateADSState() {
            if (StoreStats.GameStats.PurchasedDisabledAds) {
                ADSAppodeal.Instance.HideBanners();
                ADSAppodeal.Instance.DestroyBanner();
            }
        }
        public void OnRewardedButtonFor5LevelsUpgrade(DesktopUnit desktop) {
            Debug.Log("Start RewardedFor5LevelsUpgrade");
            IsRewardedAdsReady = false;
            IsRewardedDesktopTooltip5LevelsAdd = true;
            IsRewardedIncreaseProfit2X = false;
            _desktopForReward = desktop;
            ADSAppodeal.Instance.ShowRewardedAds();

        }

        public void OnRewardedButtonForBoostProduction() {
            Debug.Log("Try OnRewardedButtonForBoostProduction");
            IsRewardedAdsReady = false;
            IsRewardedIncreaseProfit2X = true;
            IsRewardedDesktopTooltip5LevelsAdd = false;
            
            ADSAppodeal.Instance.ShowRewardedAds();
            
        }

        public void ErrorLoadRewardedVideo() {

        }

        public void FiledShowRewardedVideo() {

        }
        public void OnSuccesRewarded() {
            IsRewardedAdsReady = true;
            timerRewardedADS = rewardedADSInterval;


            Debug.Log($"rewardedVideoFinished");
            if (/*IsRewardedDesktopTooltip5LevelsAdd*/_desktopForReward!=null) {
                // Логика для desktopTooltip5LevelsAdd
                Debug.Log("5 levels UP");
                _desktopForReward.UpgradeLevelUp(1);
                _desktopForReward = null;
            } else {

                /*if (IsRewardedIncreaseProfit2X) {*/
                Debug.Log("Boost UP");
                GameMode.Store.StartBoostProductionCoroutine();
                // }
            }


            IsRewardedDesktopTooltip5LevelsAdd = false;
            IsRewardedIncreaseProfit2X = false;
        }

        private void InterstitialTimerADS() {
            if (!IsInitializedAppodeal) return;
            if (StoreStats == null || StoreStats.GameStats == null) return;
            if (StoreStats.GameStats.PurchasedDisabledAds) return;
            // FirstInterstialADS();

            // Обновляем таймер каждый кадр
            timerInterstitialADS -= Time.deltaTime;

            // Проверяем, не пора ли показать рекламу
            if (timerInterstitialADS <= 0f) {
                Debug.Log("Try Show Interstitial ADS");
                ADSAppodeal.Instance.ShowInterstitialADS();
                timerInterstitialADS = adInterval; // Сбрасываем таймер
            }

        }
        private void RewardedTimer() {
            if (!IsInitializedAppodeal) return;
            if (StoreStats == null || StoreStats.GameStats == null) return;
            // Проверяем, не пора ли показать рекламу
            timerRewardedADS -=Time.deltaTime;
            if (timerRewardedADS <= 0f) {
                IsRewardedAdsReady = true;
                timerRewardedADS = rewardedADSInterval; // Сбрасываем таймер
            }
        }

        public void OnFailedReward() {
            IsRewardedDesktopTooltip5LevelsAdd = false;
            IsRewardedIncreaseProfit2X = false;
        }

        private void StartBanner() {

            ADSAppodeal.Instance.ShowBanner();

        }


        #endregion
        
    }
}