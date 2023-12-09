using Assets._Game._Scripts._5_Managers;

namespace Assets._Game._Scripts._6_Entities._Store {
    public class StoreStats {
        private GameMode _gameMode;
        private Store _store;
        private int _levelStore =1;

        public long Money { get; private set; } = 1000;

        public int LevelStore => _levelStore;

        public StoreStats(Store store, GameMode gameMode)
        {
            _store = store;
            _gameMode = gameMode;
            _gameMode.ChangedStatsOrMoney();
            _gameMode.InitializedStoreStats();
        }

        public bool AddMoney(long amount)
        {
            Money += amount;
            _gameMode.ChangedStatsOrMoney();
            return true;
        }

        public bool RemoveMoney(long amount)
        {
            if (Money >= 0)
            {
                Money -= amount;
                _gameMode.ChangedStatsOrMoney();
                return true;
            }
            return false;
        }

        public bool AddLevelStore()
        {
            // _gameMode.ChangeLevel();

            _gameMode.ChangedStatsOrMoney();
            return true;
        }


    }
}
