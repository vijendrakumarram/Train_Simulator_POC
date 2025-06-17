using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;

class ScenesWindow : EditorWindow
{
    static string baseSceneFolder = "Assets/_My Assets/Scenes/";
    private static ScenesWindow window;
    private static Dictionary<string, string> allScenes = new Dictionary<string, string>();
    static readonly GUILayoutOption playButtonLayout = GUILayout.Width(30);

    [MenuItem("Utils/Show Scenes Window")]
    static void Init()
    {
        // Get existing open window or if none, make a new one:
        window = (ScenesWindow)GetWindow(typeof(ScenesWindow));
        window.Show();
        ReloadScenesList();
    }

    private void OnEnable() => ReloadScenesList();

    private void GameNotPlaying()
    {
        string active = SceneManager.GetActiveScene().name;
        foreach (KeyValuePair<string, string> scene in allScenes)
        {
            if (scene.Key == active)
            {
                GUILayout.Space(5);
                GUILayout.BeginHorizontal();
                GUILayout.Label(scene.Key, EditorStyles.toolbarButton);
                if (GUILayout.Button("▶", playButtonLayout)) EditorApplication.isPlaying = true;
                GUILayout.EndHorizontal();
                GUILayout.Space(5);
                continue;
            }

            
            GUILayout.BeginHorizontal();
            if (GUILayout.Button(scene.Key))
            {
                EditorSceneManager.SaveScene(SceneManager.GetActiveScene());
                EditorSceneManager.OpenScene(scene.Value);
            }

            if (GUILayout.Button("▶", playButtonLayout))
            {
                EditorSceneManager.SaveScene(SceneManager.GetActiveScene());
                EditorSceneManager.OpenScene(scene.Value);
                EditorApplication.isPlaying = true;
            }
            GUILayout.EndHorizontal();
        }
    }

    private void GamePlaying()
    {
        string active = SceneManager.GetActiveScene().name;
        foreach (KeyValuePair<string, string> scene in allScenes)
        {
            if (scene.Key == active)
            {
                GUILayout.Space(5);
                GUILayout.Label(scene.Key, EditorStyles.toolbarButton);
                GUILayout.Space(5);
                continue;
            }

            if (GUILayout.Button(scene.Key)) SceneManager.LoadScene(scene.Value);
        }
    }
    
    public void OnGUI()
    {
        if (GUILayout.Button("Refresh List")) ReloadScenesList();
        GUILayout.Space(20);
        if (Application.isPlaying) GamePlaying();
        else GameNotPlaying();
        
    }

    public static void ReloadScenesList()
    {
        allScenes.Clear();
        foreach (UnityEditor.EditorBuildSettingsScene S in EditorBuildSettings.scenes)
        {
            if (S.enabled)
            {
                string name = S.path.Substring(S.path.LastIndexOf('/') + 1);
                name = name.Substring(0, name.Length - 6);
                allScenes.Add(name, S.path);
            }
        }
    }
}