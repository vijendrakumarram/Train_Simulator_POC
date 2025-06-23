using UnityEngine;
using ScreenUtils.Manager;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    [Header("Component References")]
    public TrainController trainController;
    public TrainSoundManager soundManager;
    public MaterialManager materialManager;
    public NPCManager npcManager;

    [Header("UI Controls")]
    public Toggle manualToggle;
    public Toggle autoToggle;

    public static GameController Instance { get; private set; }

    public bool isManualMode = false;

    public Image image;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        Init();

        // Add toggle listeners
        manualToggle.onValueChanged.AddListener(OnManualToggleChanged);
        autoToggle.onValueChanged.AddListener(OnAutoToggleChanged);

        // Set default toggle states at start
        SetModeFromUI();
    }

    private void Init()
    {
        if (soundManager != null)
            soundManager.PlayAnnouncement();

        if (npcManager != null)
            npcManager.SpawnNPC();
    }

    public void OnCompleteAnnouncement()
    {
        if (trainController != null)
        {
            trainController.CanStartTrain = true;
            ScreenManager.ShowScreen(ScreenUtils.Screen.StartEngine);
        }

        if (materialManager != null)
        {
            materialManager.UpdateSignalMaterial();
        }
    }

    public void EndJoureny()
    {
        image.DOFade(1f, 1)
            .OnComplete(() => SceneManager.LoadScene(2));
        Debug.Log("✅ Train reached and smoothly stopped at final target position.");
    }

    private void OnManualToggleChanged(bool isOn)
    {
        if (isOn)
        {
            isManualMode = true;
            if (autoToggle.isOn) autoToggle.isOn = false;
            Debug.Log("Switched to Manual Mode");
        }
    }

    private void OnAutoToggleChanged(bool isOn)
    {
        if (isOn)
        {
            isManualMode = false;
            if (manualToggle.isOn) manualToggle.isOn = false;
            Debug.Log("Switched to Auto Mode");
        }
    }

    private void SetModeFromUI()
    {
        autoToggle.SetIsOnWithoutNotify(true);
        manualToggle.SetIsOnWithoutNotify(false);
        isManualMode = false;
    }
}
