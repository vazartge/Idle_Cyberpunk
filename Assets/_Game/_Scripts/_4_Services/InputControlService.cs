using Assets._Game._Scripts._5_Managers;
using Assets._Game._Scripts._6_Entities._Units._Base;
using UnityEngine;

namespace Assets._Game._Scripts._4_Services
{
    public class InputControlService
    {
        private GameMode _gameMode;

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
        }

        private void HandleMouseInput() {
            
            if (Input.GetMouseButtonUp(0)) // Проверяем, отпущена ли кнопка мыши
            {
                ProcessInput(Input.mousePosition);
            }
        }

        private void HandleTouchInput() {
            if (Input.touchCount > 0) {
                Touch touch = Input.GetTouch(0);
                if (touch.phase == TouchPhase.Ended) // Проверяем, поднял ли пользователь палец
                {
                    ProcessInput(touch.position);
                }
            }
        }

        private void ProcessInput(Vector2 screenPosition) {
            Vector3 worldPosition = Camera.main.ScreenToWorldPoint(screenPosition);
            RaycastHit2D hit = Physics2D.Raycast(worldPosition, Vector2.zero);

            if (hit.collider != null) {
                IUnitTouchable touchable = hit.collider.gameObject.GetComponentInParent<IUnitTouchable>();
                
                if (touchable != null) {
                   
                    touchable.OnTouch();
                }
            }
        }
    }
}
