using Assets._Game._Scripts._5_Managers;
using Unity.VisualScripting;
using UnityEngine;

namespace Assets._Game._Scripts._0.Data {
    [CreateAssetMenu(fileName = "ReferecesData", menuName = "Game/Data/ReferencesData")]

    public class ReferencesData: ScriptableObject 
    {
        public DataMode_ DataMode;
        public UIMode UiMode;
        public GameMode GameMode;
    }
}
