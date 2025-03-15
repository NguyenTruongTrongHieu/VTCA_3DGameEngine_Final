using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Rendering;

public class HumamAudio : MonoBehaviour
{
    [SerializeField] private bool male;
    [SerializeField] private bool isHostage;
    [SerializeField] private bool stopPlaySFXHostage;
    private NavMeshAgent agent;
    private AudioSource audioSource;

    private bool isMoving;

    private float delayTime = 0f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        audioSource = GetComponent<AudioSource>();

        stopPlaySFXHostage = false;

        if (isHostage)
        {
            audioSource.loop = true;
            if (male)
            {
                audioSource.clip = AudioManager.audioInstance.sfxSounds.Find(x => x.name == "ScaredMale").audioClip;
                audioSource.Play();
            }
            else
            {
                audioSource.clip = AudioManager.audioInstance.sfxSounds.Find(x => x.name == "ScaredFemale").audioClip;
                audioSource.Play();
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (GameState.gameStateInstance.currentGameState != GameState.State.playing)
        {
            return;
        }

        if (agent.velocity.magnitude > 0f)
        {
            isMoving = true;
        }
        else
        {
            isMoving = false;
        }

        if (isHostage)
        {
            if (stopPlaySFXHostage)
            {
                return;
            }

            AddSoundThanksForHostage();
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Street") && isMoving)
        {
            AudioManager.audioInstance.PlaySFX("FootstepInStreet");
        }

        if (collision.gameObject.CompareTag("Floor") && isMoving)
        {
            AudioManager.audioInstance.PlaySFX("FootstepInFloor");
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Street") && isMoving)
        {
            //SetDelayTimeForSound("FootstepInStreet", 0.05f);
            if (!audioSource.isPlaying)
            {
                audioSource.PlayOneShot(AudioManager.audioInstance.sfxSounds.Find(x => x.name == "FootstepOnStreet").audioClip);
            }
            //AudioManager.audioInstance.PlaySFX("FootstepInStreet");
        }

        if (other.gameObject.CompareTag("Floor") && isMoving)
        {
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

    void AddSoundThanksForHostage()
    {
        bool isRescue = this.GetComponent<Hostage>().isRescue;
        if (isRescue)
        {
            if (male)
            {
                audioSource.PlayOneShot(AudioManager.audioInstance.sfxSounds.Find(x => x.name == "ThanksMale").audioClip);
            }
            else
            {
                audioSource.PlayOneShot(AudioManager.audioInstance.sfxSounds.Find(x => x.name == "ThanksFemale").audioClip);
            }
            stopPlaySFXHostage = true;
        }
    }
}
