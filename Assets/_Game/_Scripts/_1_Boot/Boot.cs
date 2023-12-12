using Assets._Game._Scripts._2_Game;
using Assets._Game._Scripts._5_Managers;
using UnityEngine;

namespace Assets._Game._Scripts._1_Boot
{
    public class Boot : MonoBehaviour
    {
       
        private void Awake()
        {
           // Application.targetFrameRate = 30;
        }

        public void Start()
        {
            Initialize();
        }
        public void Initialize()
        {
            // Инициализационная логика здесь
           

        }
    }
}