#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;


//
//
// ��������� �� ����� ����� � ������� ����������
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
             // ��������� ���� � ������� �������� �����
             EditorPrefs.SetString(LastScenePrefKey, EditorSceneManager.GetActiveScene().path);

             // ������� ���� � ����� ��������� �����
             string startUpScene = "Assets/_Game/_Scenes/Boot.unity";

             if (EditorSceneManager.GetActiveScene().path != startUpScene) {
                 if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo()) {
                     EditorSceneManager.OpenScene(startUpScene);
                 }
             }
         } else if (state == PlayModeStateChange.EnteredEditMode) {
             // ������������ � ��������� �������� ����� ����� ��������� ����
             string lastScene = EditorPrefs.GetString(LastScenePrefKey, string.Empty);
             if (!string.IsNullOrEmpty(lastScene)) {
                 EditorSceneManager.OpenScene(lastScene);
             }
         }
     }
}
#endif