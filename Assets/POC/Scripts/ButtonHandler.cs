using UnityEngine;
using UnityEngine.UI;

public class ButtonHandler : MonoBehaviour
{
    [SerializeField] private string sceneName = string.Empty;
    [SerializeField] public Button playButton;

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
