using UnityEngine;

public class MouseMovement : MonoBehaviour
{
    public float mouseSensitivity = 500.0f;

    float xRotation = 0.0f;
    float yRotation = 0.0f;

    public float topClamp = -90.0f;
    public float bottomClamp = 90.0f;

    public GameObject playerHead;

    void Start()
    {
        // Lock the cursor to the center of the screen and hide it
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        if (GameOverManager.overInstance.isOver)
        {
            return;
        }

        // Ensure the cursor is locked
        if (Cursor.lockState != CursorLockMode.Locked)
        {
            Cursor.lockState = CursorLockMode.Locked;
        }

        // Get mouse X and Y axis values
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        // Rotate around the X axis (looking up and down)
        xRotation -= mouseY;
        // Clamp the up and down rotation
        xRotation = Mathf.Clamp(xRotation, topClamp, bottomClamp);

        // Rotate around the Y axis (looking left and right)
        yRotation += mouseX;

        // Rotate the camera
        transform.localRotation = Quaternion.Euler(0.0f, yRotation, 0.0f);
        playerHead.transform.localRotation = Quaternion.Euler(xRotation, 0.0f, 0.0f);
    }
}