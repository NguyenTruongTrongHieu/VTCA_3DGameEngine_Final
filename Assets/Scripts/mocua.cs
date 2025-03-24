using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class mocua : MonoBehaviour
{
    public float rotationSpeed = 50f; // Tốc độ quay
    private bool isRotating = false;
    private bool isOpen = false;
    private bool isPlayerInTrigger = false; // Kiểm tra người chơi có trong vùng trigger không
    public GameObject interactButton; // Tham chiếu đến UI Button
    void Start()
    {
        interactButton.SetActive(false); // Ẩn button khi game bắt đầu
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInTrigger = true;
            interactButton.SetActive(true); // Hiện button khi người chơi vào vùng trigger
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInTrigger = false;
            interactButton.SetActive(false); // Ẩn button khi rời khỏi vùng trigger
        }
    }

    void Update()
    {
        if (isRotating)
        {
            float targetRotation = isOpen ? 0f : -90f;
            float newRotation = Mathf.MoveTowardsAngle(transform.eulerAngles.y, targetRotation, rotationSpeed * Time.deltaTime);
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, newRotation, transform.eulerAngles.z);

            if (Mathf.Approximately(newRotation, targetRotation))
            {
                isRotating = false;
            }
        }

        // Chỉ mở cửa khi nhấn phím E và người chơi đang ở trong vùng trigger
        if (Input.GetKeyDown(KeyCode.E) && isPlayerInTrigger)
        {
            isOpen = !isOpen;
            isRotating = true;
        }
    }
}
