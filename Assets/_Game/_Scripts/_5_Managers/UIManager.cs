using Assets._Game._Scripts._2_Game;
using UnityEngine;

namespace Assets._Game._Scripts._5_Managers {
    public class UIManager : MonoBehaviour {
        private Game _game;

        // Start is called before the first frame update
        void Start() {
        }
        public void Construct(Game game) {
            _game = game;
        }
        // Update is called once per frame
        void Update() {
        }


    }
}