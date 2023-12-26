using UnityEngine;
using UnityEngine.EventSystems;

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