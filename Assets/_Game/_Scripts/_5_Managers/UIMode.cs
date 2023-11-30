using Assets._Game._Scripts._2_Game;
using Unity.VisualScripting;
using UnityEngine;

namespace Assets._Game._Scripts._5_Managers {
    public class UIMode : MonoBehaviour {
        private DataMode _dataMode;

        private GameMode _gameMode;


        public void Construct(DataMode dataMode, GameMode gameMode) {
            _dataMode = dataMode;
            _gameMode = gameMode;
            BeginPlay();
        }
        private void BeginPlay() {

        }



    }
}