using ScreenUtils.Manager;
using UnityEngine;
using UnityEngine.Audio;

public class TrainSoundManager : MonoBehaviour
{
    public TrainController trainController;

    public AudioSource bgAudio;
    public AudioSource idleAudio;
    public AudioSource moveAudio;
    public AudioSource hornAudio;

    public AudioMixer trainMixer;

    public float transitionSpeed = 3f;

    private bool hasBgFinished = false;
    private bool isHornPlaying = false;

    private float targetIdleVolume = 0f;
    private float targetMoveVolume = -80f;
    private float targetBGVolume = 0f;

    private float originalIdleVol;
    private float originalMoveVol;
    private float originalBGVol;

    void Start()
    {
        // Start background music
        bgAudio.Play();

        // Initialize mixer volumes
        trainMixer.SetFloat("IdleVolume", -80f);
        trainMixer.SetFloat("MoveVolume", -80f);
        trainMixer.SetFloat("BGVolume", 0f); // Full volume by default

        // Ensure sound sources start stopped
        idleAudio.Stop();
        moveAudio.Stop();
        hornAudio.Stop();
    }

    void Update()
    {
        // Wait until background music ends to allow train movement
        if (!hasBgFinished && !bgAudio.isPlaying)
        {
            hasBgFinished = true;
            idleAudio.Play();
            trainController.CanStartTrain = true;
            ScreenManager.ShowScreen(ScreenUtils.Screen.StartEngine);
        }

        if (hasBgFinished && trainController != null && trainController.CanStartJourney)
        {
            float speed = trainController.CurrentSpeedNormalized();

            // Manage movement sound
            if (speed > 0.01f)
            {
                if (!moveAudio.isPlaying)
                    moveAudio.Play();
            }
            else
            {
                if (moveAudio.isPlaying)
                    moveAudio.Stop();
            }

            // Only update target volumes if not ducking
            if (!isHornPlaying && !IsRestoringVolumes())
            {
                targetIdleVolume = Mathf.Lerp(0f, -80f, speed);
                targetMoveVolume = Mathf.Lerp(-80f, 0f, speed * 5f);
                targetBGVolume = 0f;
            }
        }

        // Horn trigger
        if (Input.GetKeyDown(KeyCode.H) && !hornAudio.isPlaying && trainController.CanStartJourney)
        {
            hornAudio.Play();
            isHornPlaying = true;

            // Save current volumes before ducking
            trainMixer.GetFloat("IdleVolume", out originalIdleVol);
            trainMixer.GetFloat("MoveVolume", out originalMoveVol);
            trainMixer.GetFloat("BGVolume", out originalBGVol);

            // Clamp and reduce volumes by ~6dB
            trainMixer.SetFloat("IdleVolume", Mathf.Clamp(originalIdleVol - 6f, -80f, 0f));
            trainMixer.SetFloat("MoveVolume", Mathf.Clamp(originalMoveVol - 6f, -80f, 0f));
            trainMixer.SetFloat("BGVolume", Mathf.Clamp(originalBGVol - 6f, -80f, 0f));
        }

        // Horn ended — start restoring
        if (isHornPlaying && !hornAudio.isPlaying)
        {
            isHornPlaying = false;
        }

        // Gradually restore volumes
        if (!isHornPlaying && trainController.CanStartJourney)
        {
            SmoothVolume("IdleVolume", targetIdleVolume);
            SmoothVolume("MoveVolume", targetMoveVolume);
            SmoothVolume("BGVolume", targetBGVolume);
        }
    }

    void SmoothVolume(string param, float target)
    {
        float current;
        trainMixer.GetFloat(param, out current);
        float newVolume = Mathf.Lerp(current, target, Time.deltaTime * transitionSpeed);
        trainMixer.SetFloat(param, newVolume);
    }

    bool IsRestoringVolumes()
    {
        float idle, move, bg;
        trainMixer.GetFloat("IdleVolume", out idle);
        trainMixer.GetFloat("MoveVolume", out move);
        trainMixer.GetFloat("BGVolume", out bg);

        return Mathf.Abs(idle - targetIdleVolume) > 0.5f ||
               Mathf.Abs(move - targetMoveVolume) > 0.5f ||
               Mathf.Abs(bg - targetBGVolume) > 0.5f;
    }
}
