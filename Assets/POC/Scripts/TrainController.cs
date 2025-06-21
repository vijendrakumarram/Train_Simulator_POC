using DG.Tweening;
using ScreenUtils.Manager;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TrainController : MonoBehaviour
{
    [Header("Train Settings")]
    public float acceleration = 5f;
    public float brakeForce = 10f;
    public float maxSpeed = 200f;

    [Header("Stop Logic")]
    public Vector3 targetPosition;
    public float stopThreshold = 300f;         // Start braking this far before target
    public float stopSnapDistance = 1f;        // Snap when this close to stop

    [Header("Audio")]
    public float minPitch = 0.5f;
    public float maxPitch = 2f;

    [Header("Speedometer")]
    public RectTransform needle;
    public RectTransform speedLabelTemplate;
    public Text speedText;
    public float maxNeedleAngle = -20f;
    public float zeroNeedleAngle = 230f;

    public Image image;

    private float currentSpeed = 0f;
    private int moveDirection = 1;

    public bool CanStartJourney => canStartJourney;
    public bool CanStartTrain { get => canStartEngine; set { canStartEngine = value; } }
    public bool HasJourneyEnded => hasJourneyEnded;

    private bool canStartJourney = false;
    private bool canStartEngine = false;
    private bool hasJourneyEnded = false;
    private bool isSlowingDown = false;

    void Start()
    {
        canStartJourney = false;

        moveDirection = targetPosition.x < transform.position.x ? -1 : 1;

        if (speedLabelTemplate != null)
            speedLabelTemplate.gameObject.SetActive(false);
    }

    void Update()
    {
        if (canStartEngine && Input.GetKeyDown(KeyCode.S) && !canStartJourney)
        {
            ScreenManager.HideScreen(ScreenUtils.Screen.StartEngine);
            canStartJourney = true;
        }

        if (canStartJourney && !hasJourneyEnded)
        {
            float distanceToTarget = Mathf.Abs(transform.position.x - targetPosition.x);

            if (distanceToTarget <= stopThreshold)
                isSlowingDown = true;

            if (!isSlowingDown)
                HandleInput();
            else
                AutoBrake(distanceToTarget);

            MoveTrain();
            UpdateSpeedometer();
            CheckStopPoint(distanceToTarget);
        }
    }

    void HandleInput()
    {
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
            currentSpeed += acceleration * Time.deltaTime;

        if (Input.GetKey(KeyCode.Space) || Input.GetKey(KeyCode.DownArrow))
            currentSpeed -= brakeForce * Time.deltaTime;

        currentSpeed = Mathf.Clamp(currentSpeed, 0f, maxSpeed);
    }

    void AutoBrake(float distanceToTarget)
    {
        // Dynamically adjust min speed based on how close we are
        float t = Mathf.InverseLerp(stopThreshold, stopSnapDistance, distanceToTarget);
        //float minApproachSpeed = Mathf.Lerp(0.05f, 5f, t); // Gets closer to 0 as we approach
        float minApproachSpeed = Mathf.Lerp(0.5f, 5f, t); // Gets closer to 0 as we approach

        if (distanceToTarget > stopSnapDistance)
        {
            currentSpeed -= brakeForce * Time.deltaTime;
            currentSpeed = Mathf.Clamp(currentSpeed, minApproachSpeed, maxSpeed);
        }
        else
        {
            // Final stopping logic
            currentSpeed -= brakeForce * Time.deltaTime;
            currentSpeed = Mathf.Max(0f, currentSpeed);
        }
    }

    void MoveTrain()
    {
        Vector3 direction = new Vector3(moveDirection, 0f, 0f) * -1f;
        transform.Translate(direction * currentSpeed * Time.deltaTime);
    }

    void CheckStopPoint(float distanceToTarget)
    {
        if (distanceToTarget <= stopSnapDistance)
        {
            // Gradually reduce to full stop
            currentSpeed -= brakeForce * Time.deltaTime;
            currentSpeed = Mathf.Max(0f, currentSpeed);

            // Once it's nearly stopped, snap to final position
            if (currentSpeed <= 0.1f)
            {
                //transform.position = targetPosition;
                currentSpeed = 0f;
                hasJourneyEnded = true;
                canStartJourney = false;
                
                image.DOFade(1f, 1)
                    .OnComplete(() => SceneManager.LoadScene(2));

                Debug.Log("✅ Train reached and smoothly stopped at final target position.");
            }
        }
    }

    void UpdateSpeedometer()
    {
        float normalizedSpeed = currentSpeed / maxSpeed;

        if (needle != null)
        {
            float angle = Mathf.Lerp(zeroNeedleAngle, maxNeedleAngle, normalizedSpeed);
            needle.localEulerAngles = new Vector3(0, 0, angle);
        }

        if (speedText != null)
            speedText.text = "Speed: " + Mathf.RoundToInt(currentSpeed) + " km/h";
    }

    public float CurrentSpeedNormalized()
    {
        return currentSpeed / maxSpeed;
    }
}
