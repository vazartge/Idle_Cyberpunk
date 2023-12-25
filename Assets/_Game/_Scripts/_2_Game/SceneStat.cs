using System;

namespace Assets._Game._Scripts._2_Game {
    [Serializable]
    public class SceneStat
    {
        public string NameScene;
        public bool IsOpened;

        public SceneStat(){}

        public SceneStat(string nameScene, bool isOpened)
        {
            NameScene = nameScene;
            IsOpened = isOpened;
        }
    }
}
