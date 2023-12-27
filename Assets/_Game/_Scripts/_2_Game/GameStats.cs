using System;
using UnityEngine;

namespace Assets._Game._Scripts._2_Game {
    // Структура для хранения текущего состояния игры для сохранений и геймплея 
    [Serializable]
    public class GameStats {

        [SerializeField] private long coins = 10;
        [SerializeField] private int levelGame = 1;
        [SerializeField] private float speedMoveCustomer = 5f;
        [SerializeField] private float speedMoveSeller = 5f;
        [SerializeField] private float productionSpeed = 2f;
        [SerializeField] private float takingOrder = 2f;
        [SerializeField] private bool purchasedDisabledADS = false;
        [SerializeField] private bool purchasedIncreaseProfit = false;

        [SerializeField] private bool isPlayingMusic = true;

        public GameStats(){}
        public GameStats(long coins, 
            int levelGame, 
            float speedMoveCustomer, 
            float speedMoveSeller, 
            float productionSpeed, 
            float takingOrder, 
            bool purchasedDisabledADS, 
            bool purchasedIncreaseProfit,
            bool isPlayingMusic) {
            Coins=coins;
            LevelGame=levelGame;
            SpeedMoveCustomer=speedMoveCustomer;
            SpeedMoveSeller=speedMoveSeller;
            ProductionSpeed=productionSpeed;
            TakingOrder=takingOrder;
            PurchasedDisabledAds=purchasedDisabledADS;
            PurchasedIncreaseProfit=purchasedIncreaseProfit;
            IsPlayingMusic=isPlayingMusic;
            
        }

        public long Coins {
            get => coins;
            set => coins = value;
        }

        public int LevelGame {
            get => levelGame;
            set => levelGame = value;
        }

        public float SpeedMoveCustomer {
            get => speedMoveCustomer;
            set => speedMoveCustomer = value;
        }

        public float SpeedMoveSeller {
            get => speedMoveSeller;
            set => speedMoveSeller = value;
        }

        public float ProductionSpeed {
            get => productionSpeed;
            set => productionSpeed = value;
        }

        public float TakingOrder {
            get => takingOrder;
            set => takingOrder = value;
        }
        public bool PurchasedDisabledAds {
            get => purchasedDisabledADS;
            set => purchasedDisabledADS = value;
        }

        public bool PurchasedIncreaseProfit {
            get => purchasedIncreaseProfit;
            set => purchasedIncreaseProfit = value;
        }
        public bool IsPlayingMusic {
            get => isPlayingMusic;
            set => isPlayingMusic = value;
        }
    }
}
