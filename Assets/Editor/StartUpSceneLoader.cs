#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;


//
//
// Запускает из любой сцены и обратно возвращает
//
//
[InitializeOnLoad]
public class StartUpSceneLoader {
     
     private const string LastScenePrefKey = "LastOpenedScene";

     static StartUpSceneLoader() {
         EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
     }

     private static void OnPlayModeStateChanged(PlayModeStateChange state) {
         if (state == PlayModeStateChange.ExitingEditMode) {
             // Сохраняем путь к текущей активной сцене
             EditorPrefs.SetString(LastScenePrefKey, EditorSceneManager.GetActiveScene().path);

             // Укажите путь к вашей стартовой сцене
             string startUpScene = "Assets/_Game/_Scenes/Boot.unity";

             if (EditorSceneManager.GetActiveScene().path != startUpScene) {
                 if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo()) {
                     EditorSceneManager.OpenScene(startUpScene);
                 }
             }
         } else if (state == PlayModeStateChange.EnteredEditMode) {
             // Возвращаемся к последней активной сцене после остановки игры
             string lastScene = EditorPrefs.GetString(LastScenePrefKey, string.Empty);
             if (!string.IsNullOrEmpty(lastScene)) {
                 EditorSceneManager.OpenScene(lastScene);
             }
         }
     }
}
#endif