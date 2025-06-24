using ScreenUtils.Manager;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TrainController : MonoBehaviour
{
    [Header("Train Settings")]
    public float acceleration = 5f;
    public float brakeForce = 10f;
    public float maxSpeed = 200f;

    [Header("Stop Logic")]
    public Vector3 targetPosition;
    public float stopThreshold = 300f;
    public float stopSnapDistance = 1f;

    [Header("Audio")]
    public float minPitch = 0.5f;
    public float maxPitch = 2f;

    [Header("Speedometer")]
    public RectTransform needle;
    public RectTransform speedLabelTemplate;
    public Text speedText;
    public float maxNeedleAngle = -20f;
    public float zeroNeedleAngle = 230f;

    [Header("UI Prompt")]
    public TextMeshProUGUI journeyEndPromptText;

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

        if (journeyEndPromptText != null)
            journeyEndPromptText.text = "";
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

            if (GameController.Instance.isManualMode)
            {
                HandleInput();
            }
            else
            {
                if (!isSlowingDown)
                    AutoAccelerate(distanceToTarget);
                else
                    AutoBrake(distanceToTarget);
            }

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

    void AutoAccelerate(float distanceToTarget)
    {
        if (!isSlowingDown && currentSpeed < maxSpeed)
        {
            currentSpeed += acceleration * Time.deltaTime;
            currentSpeed = Mathf.Clamp(currentSpeed, 0f, maxSpeed);
        }
    }

    void AutoBrake(float distanceToTarget)
    {
        float t = Mathf.InverseLerp(stopThreshold, stopSnapDistance, distanceToTarget);
        float minApproachSpeed = Mathf.Lerp(0.5f, 5f, t);

        if (distanceToTarget > stopSnapDistance)
        {
            currentSpeed -= brakeForce * Time.deltaTime;
            currentSpeed = Mathf.Clamp(currentSpeed, minApproachSpeed, maxSpeed);
        }
        else
        {
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
            currentSpeed -= brakeForce * Time.deltaTime;
            currentSpeed = Mathf.Max(0f, currentSpeed);

            if (currentSpeed <= 0.1f && !GameController.Instance.isManualMode)
            {
                currentSpeed = 0f;
                hasJourneyEnded = true;
                canStartJourney = false;
                GameController.Instance.EndJoureny();
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

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("JourneyEndTrigger") && !hasJourneyEnded)
        {
            currentSpeed = 0f;
            hasJourneyEnded = true;
            canStartJourney = false;

            if (journeyEndPromptText != null)
                journeyEndPromptText.text = "";

            GameController.Instance.EndJoureny();
        }
    }
}
