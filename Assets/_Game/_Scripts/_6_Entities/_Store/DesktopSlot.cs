using UnityEngine;
using System.Collections;

namespace Assets._Game._Scripts._6_Entities._Store {
    public class DesktopSlot : MonoBehaviour {
        public bool IsOccupied { get; private set; }
        public float assemblyTime = 4.0f; // Время сбора заказа

        // Use this for initialization
        void Start() {
            IsOccupied = false;
        }

        // Метод для начала обработки заказа
        public IEnumerator ProcessOrder(Order order, System.Action onOrderCompleted) {
            IsOccupied = true;

            // Здесь можно добавить логику обработки заказа, например создание товара
            // ...

            // Имитация времени на сбор заказа
            yield return new WaitForSeconds(assemblyTime);

            // Вызов callback-функции по завершении сбора заказа
            onOrderCompleted?.Invoke();

            IsOccupied = false;
        }
    }
}