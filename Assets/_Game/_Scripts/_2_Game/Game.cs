using Assets._Game._Scripts._5_Managers;
using UnityEngine;


namespace Assets._Game._Scripts._2_Game {
    public class Game : MonoBehaviour {
        public static Game Instance;
        public GameMode GameMode;
        public UIMode UiMode;
        public DataMode_ DataMode;
        public bool IsPaused { get; set; }

        private void Awake() {
            Instance = this;
            DontDestroyOnLoad(this);
            Application.targetFrameRate = 30; // оставить только в Boot
        }

        private void Start()
        {
            Construct();
        }
        public void Construct()
        {
           
            GameMode = FindObjectOfType<GameMode>();
            UiMode = FindObjectOfType<UIMode>();
            DataMode = FindObjectOfType<DataMode_>();
            DataMode.Construct(GameMode, UiMode);
            GameMode.Construct(DataMode, UiMode);
           
            
            OnStartNewScene();
        }

        private void OnStartNewScene() {
            
            Debug.Log("Game Start");
        }

    }
}