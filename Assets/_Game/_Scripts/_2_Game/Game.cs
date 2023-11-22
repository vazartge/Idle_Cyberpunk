using Assets._Game._Scripts._5_Managers;
using UnityEngine;


namespace Assets._Game._Scripts._2_Game {
    public class Game : MonoBehaviour {
        public Game Instance;
        public UIManager UiManager;
        public GameMode GameMode;

        private void Awake() {
            Instance = this;

        }

        private void Start() {
            UiManager = FindObjectOfType<UIManager>();
            GameMode = FindObjectOfType<GameMode>();
            UiManager.Construct(this);
            GameMode.Construct(this);
            Debug.Log("Game Start");
        }

    }
}