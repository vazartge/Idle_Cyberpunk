using System;
using UnityEngine;

namespace Assets._Game._Scripts._2_Game {
    // Структура для хранения текущего состояния игры для сохранений и геймплея - по ней определяется удаление пребилдеров на сцене и создание их из сохранения, если 
    // сцена уже была сохранена
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
