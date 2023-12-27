using Assets._Game._Scripts._5_Managers;
using UnityEngine;

namespace Assets._Game._Scripts._4_Services
{
    // �� ������������
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
           
        }


    }
}
