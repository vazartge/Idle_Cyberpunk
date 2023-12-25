using System;
using UnityEngine;

namespace Assets._Game._Scripts._2_Game {
    [Serializable]
    public class SceneStat
    {
        [SerializeField] private string nameScene;
        [SerializeField] private bool isOpened;

        public SceneStat(){}

        public SceneStat(string nameScene, bool isOpened)
        {
            nameScene = nameScene;
            isOpened = isOpened;
        }

        public string NameScene
        {
            get => nameScene;
            set => nameScene = value;
        }

        public bool IsOpened
        {
            get => isOpened;
            set => isOpened = value;
        }
    }
}
