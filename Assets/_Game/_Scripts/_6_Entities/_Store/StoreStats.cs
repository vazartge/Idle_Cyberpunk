using Assets._Game._Scripts._5_Managers;

namespace Assets._Game._Scripts._6_Entities._Store {
    public class StoreStats {
        private GameMode _gameMode;
        private Store _store;
        private long _money = 10;
        private int _level = 1;

        public long Money => _money;
        public int Level => _level;

        public StoreStats(Store store, GameMode gameMode)
        {
            _store = store;
            _gameMode = gameMode;
            _gameMode.ChangedMoney();
        }

        public bool AddMoney(long amount)
        {
            _money += amount;
            _gameMode.ChangedMoney();
            return true;
        }

        public bool RemoveMoney(long amount)
        {
            if (_money >= 0)
            {
                _money -= amount;
                _gameMode.ChangedMoney();
                return true;
            }
            return false;
        }

        public bool AddLevelPlayer()
        {
            _gameMode.ChangeLevel();
            return true;
        }


    }
}
