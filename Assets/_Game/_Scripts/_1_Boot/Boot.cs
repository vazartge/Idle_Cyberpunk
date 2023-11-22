using UnityEngine;

namespace Assets._Game._Scripts._1_Boot
{
    public class Boot : MonoBehaviour
    {
        private void Awake()
        {
            Application.targetFrameRate = 60;
        }

        public void Initialize()
        {
            // Инициализационная логика здесь
            Debug.Log("Boot sequence initialized zenject.");
        }
    }
}