using UnityEngine;
using UnityEngine.EventSystems;

public class MouseLook : MonoBehaviour
{
    public float mouseSensitivity = 100f;
    public Transform playerBody;

    public float minVerticalAngle = -60f;
    public float maxVerticalAngle = 60f;

    public float minHorizontalAngle = -90f;
    public float maxHorizontalAngle = 90f;

    private float xRotation = 0f;
    private float yRotation = 0f;

    private Quaternion initialBodyRotation;

    void Start()
    {
        initialBodyRotation = playerBody.rotation;
    }

    void Update()
    {
        if (IsPointerOverUI()) return;

        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        // Vertical
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, minVerticalAngle, maxVerticalAngle);
        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        // Horizontal
        yRotation += mouseX;
        yRotation = Mathf.Clamp(yRotation, minHorizontalAngle, maxHorizontalAngle);
        playerBody.localRotation = Quaternion.Euler(0f, yRotation, 0f);
    }

    bool IsPointerOverUI()
    {
#if UNITY_ANDROID || UNITY_IOS
        if (Input.touchCount > 0)
            return EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId);
        else
            return false;
#else
        return EventSystem.current.IsPointerOverGameObject();
#endif
    }
}
