using UnityEngine;

public class PlayerAudio : MonoBehaviour
{
    private CharacterController characterController;

    private bool isMoving;
    private bool isSprinting;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        characterController = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (characterController.velocity.magnitude > 0f)
        {
            if (Input.GetKey(KeyCode.LeftShift))
            {
                isSprinting = true;
                isMoving = false;
            }
            else
            {
                isMoving = true;
                isSprinting = false;
            }
        }
        else
        {
            isSprinting = false;
            isMoving = false;
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Street"))
        {
            Debug.Log("street");
            AudioManager.audioInstance.PlaySFX("FootstepInStreet");
        }

        if (collision.gameObject.CompareTag("Floor"))
        {
            Debug.Log("floor");
            AudioManager.audioInstance.PlaySFX("FootstepInFloor");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Street"))
        {
            Debug.Log("street");
            AudioManager.audioInstance.PlaySFX("FootstepInStreet");
        }

        if (other.gameObject.CompareTag("Floor"))
        {
            Debug.Log("floor");
            AudioManager.audioInstance.PlaySFX("FootstepInFloor");
        }
    }
}
