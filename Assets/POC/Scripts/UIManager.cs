using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [Header("UI Elements")]
    public GameObject uiPanel;
    public GameObject quitPopup;

    [Header("Buttons")]
    public Button quitButton;
    public Button closeButton;
    public Button yesButton;
    public Button noButton;

    private const string UIPrefKey = "UIVisible";

    void Start()
    {
        // Restore panel visibility
        bool wasVisible = PlayerPrefs.GetInt(UIPrefKey, 1) == 1;
        uiPanel.SetActive(wasVisible);

        // Hide popup by default
        if (quitPopup != null) quitPopup.SetActive(false);

        // Hook up buttons
        quitButton?.onClick.AddListener(ShowQuitPopup);
        closeButton?.onClick.AddListener(CloseUIPanel);
        yesButton?.onClick.AddListener(QuitGame);
        noButton?.onClick.AddListener(HideQuitPopup);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (quitPopup != null && !quitPopup.activeSelf)
                ShowQuitPopup();
            else
                HideQuitPopup();
        }
    }

    public void ShowUI()
    {
        uiPanel.SetActive(true);
        PlayerPrefs.SetInt(UIPrefKey, 1);
        PlayerPrefs.Save();
    }

    public void HideUI()
    {
        uiPanel.SetActive(false);
        PlayerPrefs.SetInt(UIPrefKey, 0);
        PlayerPrefs.Save();
    }

    public void CloseUIPanel()
    {
        uiPanel.SetActive(false);
        PlayerPrefs.SetInt(UIPrefKey, 0);
        PlayerPrefs.Save();
    }

    public void ShowQuitPopup()
    {
        if (quitPopup != null)
        {
            quitPopup.SetActive(true);
           
        }
    }

    public void HideQuitPopup()
    {
        if (quitPopup != null)
        {
            quitPopup.SetActive(false);
        }
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
