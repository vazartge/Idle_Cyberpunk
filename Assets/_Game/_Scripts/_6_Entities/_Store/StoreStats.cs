using Assets._Game._Scripts._5_Managers;

namespace Assets._Game._Scripts._6_Entities._Store {
    public class StoreStats {
        private GameMode _gameMode;
        private Store _store;
        private long _money = 1000;
        private int _levelStore =1;

        public long Money => _money;
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
            _money += amount;
            _gameMode.ChangedStatsOrMoney();
            return true;
        }

        public bool RemoveMoney(long amount)
        {
            if (_money >= 0)
            {
                _money -= amount;
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
