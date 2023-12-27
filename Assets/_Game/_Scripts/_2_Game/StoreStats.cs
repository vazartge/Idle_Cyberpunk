using System;
using System.Collections.Generic;
using Assets._Game._Scripts._0.Data._DataForLevelsUpgrade;
using Assets._Game._Scripts._6_Entities._Store;
using UnityEngine;

namespace Assets._Game._Scripts._2_Game {
    // Главная структура для хранения текущего состояния игры для сохранений и геймплея 
    [Serializable]
    public class StoreStats
    {
        [SerializeField] private GameStats gameStats = new GameStats();
        [SerializeField] private LevelUpgrade levelUpgrade = new LevelUpgrade(); // без начального значения
        [SerializeField] private List<PrebuilderStats> prebuilderStats = new List<PrebuilderStats>();
        [SerializeField] private List<DesktopStats> desktopStatsList = new List<DesktopStats>();
        [SerializeField] private List<SceneStat> sceneStatsList = new List<SceneStat>();


        public StoreStats()
        {
        }
        public StoreStats(GameStats gameStats,
            LevelUpgrade levelUpgrade,
            List<PrebuilderStats> prebuilderStats,
            List<DesktopStats> desktopStatsList,
            List<SceneStat> sceneStatsList
            ) {
            GameStats=gameStats;
            LevelUpgrade=levelUpgrade;
            PrebuilderStats=prebuilderStats;
            DesktopStatsList=desktopStatsList;
            SceneStatsList=sceneStatsList;
            
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

      

        public List<SceneStat> SceneStatsList
        {
            get => sceneStatsList;
            set => sceneStatsList = value;
        }

        public GameStats GameStats
        {
            get => gameStats;
            set => gameStats = value;
        }
    }
}
