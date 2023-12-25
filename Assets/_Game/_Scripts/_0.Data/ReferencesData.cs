using Assets._Game._Scripts._0.Data._DataForLevelsUpgrade;
using Assets._Game._Scripts._2_Game;
using Assets._Game._Scripts._5_Managers;
using UnityEngine;

namespace Assets._Game._Scripts._0.Data {
    [CreateAssetMenu(fileName = "ReferecesData", menuName = "Game/Data/ReferencesData")]

    public class ReferencesData: ScriptableObject 
    {
        public DataMode_ DataMode;
        public UIMode UiMode;
        public GameMode GameMode;
        public StoreStats StoreStats;
        public LevelsUpgradesSO LevelsUpgradesSO;

        public IAPManager IAPManager;
        public ADSAppodeal AdsAppodeal;

        public int CountRegister=0;

        public AudioClip BackGroundMusicClip;
        public AudioSource AudioSource;

    }
}
