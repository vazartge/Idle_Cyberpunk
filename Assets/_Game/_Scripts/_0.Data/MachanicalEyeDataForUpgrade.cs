using UnityEngine;

namespace Assets._Game._Scripts._0.Data
{
    [System.Serializable]
    public class MachanicalEyeDataForUpgrade {

        [SerializeField] private int _stars;
        public int Stars { get => _stars; set => _stars = value; }
        [SerializeField] private int _level;
        public int Level { get => _level; set => _level = value; }
        [SerializeField] private int _cost;
        public int Cost { get => _cost; set => _cost = value; }
        [SerializeField] private int _incomeMoney;
        public int IncomeMoney { get => _incomeMoney; set => _incomeMoney = value; }
        [SerializeField] private int _openingAtLevel;
        public int OpeningAtLevel { get => _openingAtLevel; set => _openingAtLevel = value; }
        [SerializeField] private string _events;
        public string Events { get => _events; set => _events = value; }
       
    }
}