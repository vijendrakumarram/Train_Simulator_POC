using ScreenUtils.Manager;
using UnityEngine;
using UnityEngine.UI;

public class InstructionsView : MonoBehaviour
{
    public Button closeButton;

    private const string UIPrefKey = "UIVisible";

    void Start()
    {
        closeButton?.onClick.AddListener(CloseUIPanel);     
    }

    public void CloseUIPanel()
    {
        PlayerPrefs.SetInt(UIPrefKey, 1);
        PlayerPrefs.Save();
        ScreenManager.HideScreen(ScreenUtils.Screen.InstructionsScreen);
    }
}
