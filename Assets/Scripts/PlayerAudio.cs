using UnityEngine;

public class PlayerAudio : MonoBehaviour
{
    private CharacterController characterController;

    private bool isMoving;
    private bool isSprinting;

    private float delayTime = 0f; 

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
        if (collision.gameObject.CompareTag("Street") && isMoving)
        {
            Debug.Log("street");
            AudioManager.audioInstance.PlaySFX("FootstepInStreet");
        }

        if (collision.gameObject.CompareTag("Floor") && isMoving)
        {
            Debug.Log("floor");
            AudioManager.audioInstance.PlaySFX("FootstepInFloor");
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Street") && isMoving)
        {
            Debug.Log("street");
            //SetDelayTimeForSound("FootstepInStreet", 0.05f);
            if (!AudioManager.audioInstance.sfxSource.isPlaying)
            {
                AudioManager.audioInstance.PlaySFX("FootstepInStreet");
            }
            //AudioManager.audioInstance.PlaySFX("FootstepInStreet");
        }
        else if (other.gameObject.CompareTag("Street") && isSprinting)
        {
            Debug.Log("street");
            //SetDelayTimeForSound("FootstepInStreet", 0.01f);
            if (!AudioManager.audioInstance.sfxSource.isPlaying)
            {
                AudioManager.audioInstance.PlaySFX("FootstepOnStreet");
            }
        }

        if (other.gameObject.CompareTag("Floor") && isMoving)
        {
            Debug.Log("floor");
            //SetDelayTimeForSound("FootstepInFloor", 0.05f);
            if (!AudioManager.audioInstance.sfxSource.isPlaying)
            {
                AudioManager.audioInstance.PlaySFX("FootstepInFloor");
            }
            //AudioManager.audioInstance.PlaySFX("FootstepInFloor");
        }
        else if (other.gameObject.CompareTag("Floor") && isSprinting)
        {
            Debug.Log("floor");
            //SetDelayTimeForSound("FootstepInFloor", 1f);
            if (!AudioManager.audioInstance.sfxSource.isPlaying)
            {
                AudioManager.audioInstance.PlaySFX("FootstepOnFloor");
            }
        }
    }

    void SetDelayTimeForSound(string soundName, float time)
    {
        if (delayTime < time)
        {
            delayTime += Time.deltaTime;
        }
        else
        {
            AudioManager.audioInstance.PlaySFX(soundName);
            delayTime = 0f;
        }
    }
}
