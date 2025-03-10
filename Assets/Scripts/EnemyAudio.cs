using UnityEngine;
using UnityEngine.AI;

public class EnemyAudio : MonoBehaviour
{
    private NavMeshAgent agent;
    private AudioSource audioSource;

    private bool isMoving;

    private float delayTime = 0f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (agent.velocity.magnitude > 0f)
        {
            isMoving = true;
        }
        else
        {
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
            if (!audioSource.isPlaying)
            {
                audioSource.PlayOneShot(AudioManager.audioInstance.sfxSounds.Find(x => x.name == "FootstepOnStreet").audioClip);
            }
            //AudioManager.audioInstance.PlaySFX("FootstepInStreet");
        }

        if (other.gameObject.CompareTag("Floor") && isMoving)
        {
            Debug.Log("floor");
            //SetDelayTimeForSound("FootstepInFloor", 0.05f);
            if (!audioSource.isPlaying)
            {
                audioSource.PlayOneShot(AudioManager.audioInstance.sfxSounds.Find(x => x.name == "FootstepOnFloor").audioClip);
            }
            //AudioManager.audioInstance.PlaySFX("FootstepInFloor");
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
