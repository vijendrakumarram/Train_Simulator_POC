using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.UI;
using TMPro;

public class SceneLoaderWithDelay : MonoBehaviour
{
    public Slider loadingSlider;   
    public TextMeshProUGUI percentageText;   
    public float minimumLoadTime = 2f;

    private static SceneLoaderWithDelay _instance;

    public static SceneLoaderWithDelay Instance { get { return _instance; } }

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
    }

    private void Start()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        percentageText.text = "";
        loadingSlider.value = 0;
        loadingSlider.gameObject.SetActive(false);

        Debug.Log("Loaded Scene: " + scene.name);
    }

    public void LoadSceneWithDelay(string sceneName)
    {
        StartCoroutine(LoadSceneRoutine(sceneName));
    }

    private IEnumerator LoadSceneRoutine(string sceneName)
    {
        if (loadingSlider)
        {
            loadingSlider.gameObject.SetActive(true);
            loadingSlider.value = 0;
        }

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);
        asyncLoad.allowSceneActivation = false;

        float timer = 0f;

        while (!asyncLoad.isDone)
        {
            float progress = Mathf.Clamp01(asyncLoad.progress / 0.9f);

            float fillAmount = Mathf.Clamp01(Mathf.Min(progress, timer / minimumLoadTime));
            DisplaySlider(fillAmount);

            timer += Time.deltaTime;

            if (asyncLoad.progress >= 0.9f && timer >= minimumLoadTime)
            {
                asyncLoad.allowSceneActivation = true;
                DisplaySlider(1f);
            }

            yield return null;
        }
    }

    private void DisplaySlider(float amount)
    {
        loadingSlider.value = amount;
        int percent = Mathf.RoundToInt(amount * 100f);
        percentageText.text = $"{percent}%";      
    }
}
