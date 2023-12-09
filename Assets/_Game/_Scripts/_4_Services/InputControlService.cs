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
            // �������� ��������� ��� ����������� ���� �����
#if UNITY_EDITOR || UNITY_STANDALONE
            // ��������� ����� ���� ��� ��������� Unity � ��
            HandleMouseInput();
#else
        // ��������� ����� ���������� ������ ��� ��������� ���������
        HandleTouchInput();
#endif
            if (Input.touchCount <= 0 && !IsAnyMouseButtonPressed()) {
                _canClick = true;
            }
        }

        private void HandleMouseInput() {
            
            if (Input.GetMouseButtonDown(0) && _canClick) // ���������, �������� �� ������ ����
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
                if (touch.phase == TouchPhase.Began) // ���������, ������ �� ������������ �����
                {
                    ProcessInput(touch.position);
                    _canClick = false;
                }
            }

        
        }

        private void ProcessInput(Vector2 screenPosition) {
            // // ���������� ������ ��� raycast
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
            // // ������ ��� �������� ����������� raycast
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

            // ���� canvas �� ������, ��������� ������� �� �����
            //   if (canvas == null) {

            // _gameMode.UiMode.TouchInput(touchable/*, null*/);
            // } else {
            //     // ���� canvas ������, ������������ ������� �� UI
            //     _gameMode.UiMode.TouchInput(null, canvas);
            // }
        }


    }
}
