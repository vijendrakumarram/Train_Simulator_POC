using UnityEngine;
using UnityEngine.EventSystems;

public class MouseLook : MonoBehaviour
{
    public float mouseSensitivity = 100f;
    public Transform playerBody; // Reference to the object to rotate horizontally (usually the player or camera parent)

    float xRotation = 0f;

    //void Start()
    //{
    //    Cursor.lockState = CursorLockMode.Locked; // Lock the cursor in the center of the screen
    //}

    void Update()
    {
        if (IsPointerOverUI()) return;

        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f); // Prevent looking too far up/down

        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);     // Vertical look (camera)
        playerBody.Rotate(Vector3.up * mouseX);                            // Horizontal look (player body)
    }

    bool IsPointerOverUI()
    {
#if UNITY_ANDROID || UNITY_IOS
        // Touch-based check
        if (Input.touchCount > 0)
            return EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId);
        else
            return false;
#else
        // Mouse-based check
        return EventSystem.current.IsPointerOverGameObject();
#endif
    }

}
