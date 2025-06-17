using UnityEditor.SceneManagement;
using UnityEditor;
using UnityEngine.SceneManagement;

class EditorScriptsLauncher : EditorWindow
{
    [MenuItem("Scene/Play/Menu _%h")]
    public static void RunMainScene()
    {
        EditorSceneManager.SaveScene(SceneManager.GetActiveScene());
        EditorSceneManager.OpenScene("Assets/POC/DevelopmentScene/Menu.unity");
        EditorApplication.isPlaying = true;
    }
}