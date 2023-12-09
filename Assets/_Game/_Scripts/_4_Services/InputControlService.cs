using Assets._Game._Scripts._5_Managers;
using Assets._Game._Scripts._6_Entities._Units._Base;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Assets._Game._Scripts._4_Services
{
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
            // // Подготовка данных для raycast
            // PointerEventData pointerData = new PointerEventData(EventSystem.current) {
            //     position = screenPosition
            // };
            // Vector3 worldPosition = Camera.main.ScreenToWorldPoint(screenPosition);
            // RaycastHit2D hit = Physics2D.Raycast(worldPosition, Vector2.zero);
            // IUnitTouchable touchable = null;
            // if (hit.collider != null) {
            //     touchable = hit.collider.gameObject.GetComponentInParent<IUnitTouchable>();
            //     if (touchable != null) {
            //         touchable.OnTouch();
            //     }
            // }


            //----------------------------------------
            // // Список для хранения результатов raycast
            // List<RaycastResult> results = new List<RaycastResult>();
            // EventSystem.current.RaycastAll(pointerData, results);
            //
            // Canvas canvas = null;
            // foreach (var result in results) {
            //     if (result.gameObject.GetComponent<Canvas>() != null) {
            //         canvas = result.gameObject.GetComponent<Canvas>();
            //         break;
            //     }
            // }

            // Если canvas не найден, проверяем объекты на сцене
            //   if (canvas == null) {

            // _gameMode.UiMode.TouchInput(touchable/*, null*/);
            // } else {
            //     // Если canvas найден, обрабатываем нажатие на UI
            //     _gameMode.UiMode.TouchInput(null, canvas);
            // }
        }


    }
}
