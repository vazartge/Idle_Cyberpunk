using System;
using System.Collections.Generic;
using Assets._Game._Scripts._0.Data._DataForLevelsUpgrade;
using Assets._Game._Scripts._6_Entities._Store;
using UnityEngine;

namespace Assets._Game._Scripts._2_Game {
    [Serializable]
    public class StoreStats
    {

        [SerializeField] private long coins = 10;
        [SerializeField] private int levelGame = 1;
        [SerializeField] private float speedMoveCustomer = 5f;
        [SerializeField] private float speedMoveSeller = 5f;
        [SerializeField] private float productionSpeed = 2f;
        [SerializeField] private float takingOrder = 2f;
        [SerializeField] private bool purchasedDisabledADS = false;
        [SerializeField] private bool purchasedIncreaseProfit = false;

        [SerializeField] private bool isPlayingMusic = true;
        


        [SerializeField] private LevelUpgrade levelUpgrade = new LevelUpgrade(); // без начального значения
        [SerializeField] private List<PrebuilderStats> prebuilderStats = new List<PrebuilderStats>();
        [SerializeField] private List<DesktopStats> desktopStatsList = new List<DesktopStats>();
        [SerializeField] private List<SceneStat> sceneStatsList = new List<SceneStat>();


        public long Coins
        {
            get => coins;
            set => coins = value;
        }

        public int LevelGame
        {
            get => levelGame;
            set => levelGame = value;
        }

        public float SpeedMoveCustomer
        {
            get => speedMoveCustomer;
            set => speedMoveCustomer = value;
        }

        public float SpeedMoveSeller
        {
            get => speedMoveSeller;
            set => speedMoveSeller = value;
        }

        public float ProductionSpeed
        {
            get => productionSpeed;
            set => productionSpeed = value;
        }

        public float TakingOrder
        {
            get => takingOrder;
            set => takingOrder = value;
        }

        public LevelUpgrade LevelUpgrade
        {
            get => levelUpgrade;
            set => levelUpgrade = value;
        }
        public List<PrebuilderStats> PrebuilderStats {
            get => prebuilderStats;
            set => prebuilderStats = value;
        }

        public List<DesktopStats> DesktopStatsList
        {
            get => desktopStatsList;
            set => desktopStatsList = value;
        }

        public bool PurchasedDisabledAds
        {
            get => purchasedDisabledADS;
            set => purchasedDisabledADS = value;
        }

        public bool PurchasedIncreaseProfit
        {
            get => purchasedIncreaseProfit;
            set => purchasedIncreaseProfit = value;
        }

        public List<SceneStat> SceneStatsList
        {
            get => sceneStatsList;
            set => sceneStatsList = value;
        }

        public bool IsPlayingMusic
        {
            get => isPlayingMusic;
            set => isPlayingMusic = value;
        }
    }
}
