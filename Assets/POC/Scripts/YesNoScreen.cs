using ScreenUtils.Manager;
using UnityEngine;
using UnityEngine.UI;

public class YesNoScreen : MonoBehaviour
{
    public Button yesButton;
    public Button noButton;

    void Start()
    {
        yesButton?.onClick.AddListener(QuitGame);
        noButton?.onClick.AddListener(HideQuitPopup);
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

    public void HideQuitPopup()
    {
        ScreenManager.HideScreen(ScreenUtils.Screen.YesNoScreen);
    }
}
