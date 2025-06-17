using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TrainController : MonoBehaviour
{
    [Header("Train Settings")]
    public float acceleration = 5f;
    public float brakeForce = 10f;
    public float maxSpeed = 200f;

    [Header("Audio")]
    public float minPitch = 0.5f;
    public float maxPitch = 2f;

    [Header("Speedometer")]
    public RectTransform needle;
    public RectTransform speedLabelTemplate;
    public Text speedText;
    public float maxNeedleAngle = -20f;
    public float zeroNeedleAngle = 230f;
    public int labelCount = 10;
    public float labelRadius = 120f;

    private float currentSpeed = 0f;

    public Vector3 targetPosition;

    public bool canMoveTrain = false;

    void Start()
    {
        canMoveTrain = false;

        if (speedLabelTemplate != null)
        {
            speedLabelTemplate.gameObject.SetActive(false);
        }
    }

    void Update()
    {
        if (!canMoveTrain) return;
        HandleInput();
        MoveTrain();
        UpdateSpeedometer();
        EndGame();
    }

    void EndGame()
    {
        if (this.transform.position.x >= targetPosition.x)
        {
            SceneManager.LoadScene(2);
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

    void MoveTrain()
    {
        transform.Translate(Vector3.left * currentSpeed * Time.deltaTime);
    }

    void UpdateSpeedometer()
    {
        float normalizedSpeed = currentSpeed / maxSpeed;

        // Update needle rotation
        if (needle != null)
        {
            float angle = Mathf.Lerp(zeroNeedleAngle, maxNeedleAngle, normalizedSpeed);
            needle.localEulerAngles = new Vector3(0, 0, angle);
        }

        // Update speed text
        if (speedText != null)
            speedText.text = "Speed: " + Mathf.RoundToInt(currentSpeed) + " km/h";
    }

    public float CurrentSpeedNormalized()
    {
        return currentSpeed / maxSpeed;
    }
}
