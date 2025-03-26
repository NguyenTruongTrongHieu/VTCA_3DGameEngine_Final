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
        if (other.CompareTag("Player") && !isOpen) // Chỉ hiển thị nếu cửa chưa mở
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
            float targetRotation = isOpen ? -90f : 0f;
            float newRotation = Mathf.MoveTowardsAngle(transform.eulerAngles.y, targetRotation, rotationSpeed * Time.deltaTime);
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, newRotation, transform.eulerAngles.z);

            if (Mathf.Approximately(newRotation, targetRotation))
            {
                isRotating = false;
                if (isOpen)
                {
                    interactButton.SetActive(false); // Ẩn nút khi cửa mở xong
                }
            }
        }

        // Chỉ mở cửa khi nhấn phím E, người chơi ở trong vùng trigger và cửa chưa mở
        if (Input.GetKeyDown(KeyCode.E) && isPlayerInTrigger && !isOpen)
        {
            isOpen = true;
            isRotating = true;
        }
    }
}
