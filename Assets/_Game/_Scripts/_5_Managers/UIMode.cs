using System;
using Assets._Game._Scripts._2_Game;
using Unity.VisualScripting;
using UnityEngine;

namespace Assets._Game._Scripts._5_Managers {
    public class UIMode : MonoBehaviour
    {

        
        private DataMode _dataMode;
        private GameMode _gameMode;

        public GameMode GameMode
        {
            get => _gameMode;
            set => _gameMode = value;
        }


        public void Construct(DataMode dataMode, GameMode gameMode) {
            _dataMode = dataMode;
            GameMode = gameMode;
            GameMode.OnChangedMoney += GameModeOnOnChangedMoney;
            GameMode.OnChangedLevelPlayer += GameModeOnOnChangedLevelPlayer;
            BeginPlay();
        }

        private void GameModeOnOnChangedLevelPlayer()
        {

        }

        private void GameModeOnOnChangedMoney()
        {
            
        }

        private void BeginPlay() {

        }



    }
}