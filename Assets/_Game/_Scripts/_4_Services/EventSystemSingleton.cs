using UnityEngine;
using UnityEngine.EventSystems;

namespace Assets._Game._Scripts._4_Services
{
    // Appodeal создает несколько EventSystem , что вызывает Предупреждения в консоли Unity. Этот скрипт уничтожает остальные экземплярыы EventSystem на сцене
    public class EventSystemSingleton : MonoBehaviour {

        private static EventSystemSingleton instance;

        private void Awake() {
            if (instance == null) {
                instance = this;
                DontDestroyOnLoad(gameObject);
            } else if (instance != this) {
                Destroy(gameObject);
            }

            // Удаление лишних EventSystem
            var eventSystems = FindObjectsOfType<EventSystem>();
            foreach (var es in eventSystems) {
                if (es != GetComponent<EventSystem>()) {
                    Destroy(es.gameObject);
                }
            }
        }
    }
}