using Assets._Game._Scripts._5_Managers;
using Assets._Game._Scripts._6_Entities._Store;
using DG.Tweening;
using Newtonsoft.Json;
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


        private void Awake() {
            Instance = this;
            DontDestroyOnLoad(this);
            Application.targetFrameRate = 30; // оставить только в Boot
            
        }

        private void Start()
        {
            SceneManager.sceneLoaded += OnSceneLoaded; // Подписка на событие загрузки сцены
            LoadLevel();
        }
        private void LoadLevel() {
            _storeStats = LoadGame(); // Загрузка или создание StoreStats
            int levelToLoad = _storeStats.LevelGame; // Получение уровня из StoreStats
            SceneManager.LoadScene(levelToLoad); // Загрузка соответствующей сцены
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            SetupComponents();

        }
        public void SetupComponents()
        {
           
            _gameMode = FindObjectOfType<GameMode>();
            _uiMode = FindObjectOfType<UIMode>();
            _dataMode = FindObjectOfType<DataMode_>();
            _storeStats = LoadGame();
            _dataMode.Construct(_gameMode, _uiMode);
            _gameMode.Construct(_dataMode, _uiMode, _storeStats);
           
            
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
                return _storeStatsService.LoadFromJson(json);
            } else {
                return new StoreStats(); // Возвращает новый экземпляр с начальными значениями
            }
        }
        // В классе Game
        public void NextLevelStart()
        {
            ChangeLevel(_gameMode.Store.Stats.LevelGame);
            SceneManager.LoadScene(_gameMode.Store.Stats.LevelGame); // Загрузка соответствующей сцены

        }
        private void ChangeLevel(int newLevel) {
            LevelUpgrade levelUpgrade = _dataMode.GetLevelUpgradeForLevel(newLevel);
            if (levelUpgrade != null) {
                // Предполагается, что у тебя уже есть экземпляр StoreStats
                // Обновляем только LevelUpgrade в существующем StoreStats
                _storeStats.LevelUpgrade = levelUpgrade;

                
            }
        }


    }

    public class StoreStatsService
    {
        public string SaveToJson(StoreStats storeStats)
        {
            return JsonConvert.SerializeObject(storeStats);
        }


        public StoreStats LoadFromJson(string json)
        {
            return JsonConvert.DeserializeObject<StoreStats>(json);
        }
    }

}