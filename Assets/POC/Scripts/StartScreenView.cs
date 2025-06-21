using UnityEngine;
using UnityEngine.UI;

public class StartScreenView : MonoBehaviour
{
    [SerializeField] private string sceneName = string.Empty;
    public Button playButton;

    private void OnEnable()
    {
        playButton.onClick.AddListener(OnButtonClicked);
    }

    private void OnDisable()
    {
        playButton.onClick.RemoveListener(OnButtonClicked);
    }

    private void OnButtonClicked()
    {
        playButton.gameObject.SetActive(false);
        SceneLoaderWithDelay.Instance.LoadSceneWithDelay(sceneName);
    }
}
