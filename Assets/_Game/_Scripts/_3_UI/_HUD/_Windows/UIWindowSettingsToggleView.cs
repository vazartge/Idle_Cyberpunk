using Assets._Game._Scripts._2_Game;
using Assets._Game._Scripts._3_UI._UIUnits._Base;
using UnityEngine;
using UnityEngine.UI;


namespace Assets._Game._Scripts._3_UI._HUD._Windows {
    public class UIWindowSettingsToggleView : MonoBehaviour, IUiUnitView
    {

        public Toggle bgmToggle; // Ссылка на компонент Toggle

        public RectTransform checkmarkTransform; // Присвойте это в редакторе
        public Vector2 onPosition; // Позиция Checkmark, когда Toggle включен
        public Vector2 offPosition; // Позиция Checkmark, когда Toggle выключен
        public float animationSpeed = 0.1f; // Скорость анимации

        private Vector2 targetPosition;
        public Image backgroundImage; // Добавь ссылку на компонент Image фона Toggle в редакторе Unity

        public Color onColor = Color.green; // Цвет для включенного состояния
        public Color offColor = Color.gray; // Цвет для выключенного состояния


        // Start вызывается перед первым кадром обновления
        private void Start()
        {
            bgmToggle.onValueChanged.AddListener(OnToggleChanged);
            OnToggleChanged(bgmToggle.isOn);
        }

        private void OnToggleChanged(bool isOn) {
            if (gameObject.activeInHierarchy) {
                targetPosition = isOn ? onPosition : offPosition;
                backgroundImage.color = isOn ? onColor : offColor; // Изменение цвета фона

                // Управление музыкой
                if (isOn) {
                    Game.Instance.PlayMusic();
                } else {
                    Game.Instance.PauseMusic();
                }
            }
        }

        private void Update() {
            if (checkmarkTransform.anchoredPosition != targetPosition) {
                checkmarkTransform.anchoredPosition = Vector2.Lerp(checkmarkTransform.anchoredPosition, targetPosition, animationSpeed * Time.deltaTime);
            }
        }

        public void ShowWindow() {
            Debug.Log("Active Upgrade Window");
            gameObject.SetActive(true);

            // Проверяем, играет ли музыка
            bool isMusicPlaying = Game.Instance.IsMusicPlaying();
            bgmToggle.isOn = isMusicPlaying;
            backgroundImage.color = isMusicPlaying ? onColor : offColor; // Устанавливаем цвет фона
            checkmarkTransform.anchoredPosition = isMusicPlaying ? onPosition : offPosition;
        }

        public void HideWindow() {
            Debug.Log("Disactive Upgrade wWindow");
            gameObject.SetActive(false);
        }
    }
}