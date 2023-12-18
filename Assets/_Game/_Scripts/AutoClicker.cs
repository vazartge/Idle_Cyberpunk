using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections.Generic;

public class AutoClicker : MonoBehaviour {
    public float clickInterval = 0.1f; // Интервал между имитацией кликов
    private float timer;
    private bool isAutoClicking = false;

    void Update() {
        // Проверяем, зажата ли клавиша Ctrl и нажата ли левая кнопка мыши
        if ((Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl)) && Input.GetMouseButton(0)) {
            timer += Time.deltaTime;

            if (timer >= clickInterval) {
                timer = 0f;
                PerformClick();
            }
        } else {
            timer = 0f; // Сброс таймера, если кнопка мыши не нажата
        }
    }

    private void PerformClick() {
        PointerEventData pointerEventData = new PointerEventData(EventSystem.current);
        pointerEventData.position = Input.mousePosition;

        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointerEventData, results);

       
            if (results[0].gameObject.GetComponent<Button>() != null) {
            results[0].gameObject.GetComponent<Button>().onClick.Invoke();
                Debug.Log("Clicked Button: " + results[0].gameObject.name);
            } 
    }
}