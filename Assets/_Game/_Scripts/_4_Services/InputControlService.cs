using Assets._Game._Scripts._5_Managers;
using UnityEngine;

namespace Assets._Game._Scripts._4_Services
{
    // Не используется
    public class InputControlService
    {
        private GameMode _gameMode;

        private bool _canClick = true;

        public InputControlService(GameMode gameMode)
        {
            _gameMode = gameMode;
        }
        public void UpdateInputControl() {
            // Проверка платформы для определения типа ввода
#if UNITY_EDITOR || UNITY_STANDALONE
            // Обработка ввода мыши для редактора Unity и ПК
            HandleMouseInput();
#else
        // Обработка ввода сенсорного экрана для мобильных устройств
        HandleTouchInput();
#endif
            if (Input.touchCount <= 0 && !IsAnyMouseButtonPressed()) {
                _canClick = true;
            }
        }

        private void HandleMouseInput() {
            
            if (Input.GetMouseButtonDown(0) && _canClick) // Проверяем, отпущена ли кнопка мыши
            {
                ProcessInput(Input.mousePosition);
                _canClick = false;
            }


        }
        private bool IsAnyMouseButtonPressed() {
            return Input.GetMouseButton(0) || Input.GetMouseButton(1) || Input.GetMouseButton(2);
        }
        private void HandleTouchInput() {
            if (Input.touchCount > 0 && _canClick) {
                Touch touch = Input.GetTouch(0);
                if (touch.phase == TouchPhase.Began) // Проверяем, поднял ли пользователь палец
                {
                    ProcessInput(touch.position);
                    _canClick = false;
                }
            }

        
        }

        private void ProcessInput(Vector2 screenPosition) {
           
        }


    }
}
