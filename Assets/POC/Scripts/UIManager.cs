using ScreenUtils.Manager;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [Header("Buttons")]
    public Button quitButton;

    private const string UIPrefKey = "UIVisible";

    void Start()
    {
        bool wasVisible = PlayerPrefs.GetInt(UIPrefKey, 1) == 1;
        if (!wasVisible)
            ScreenManager.ShowScreen(ScreenUtils.Screen.InstructionsScreen);

        quitButton?.onClick.AddListener(ShowQuitPopup);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ScreenManager.ShowScreen(ScreenUtils.Screen.YesNoScreen);
        }

        if (Input.GetKeyDown(KeyCode.I))
        {
            ScreenManager.ShowScreen(ScreenUtils.Screen.InstructionsScreen);
        }
    }

    public void ShowQuitPopup()
    {
        ScreenManager.ShowScreen(ScreenUtils.Screen.YesNoScreen);
    }

    public void QuitGame()
    {
        Debug.Log("Quitting game...");
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
