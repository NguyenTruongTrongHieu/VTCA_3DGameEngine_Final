using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager audioInstance;

    public AudioSource audioSource;
    public AudioSource sfxSource;

    public List<Sound> musicSounds;
    public List<Sound> sfxSounds;

    private void Awake()
    {
        if (audioInstance == null)
        {
            audioInstance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        { 
            Destroy(gameObject);
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayMusic(string name)
    {
        var music = musicSounds.Find( x => x.name == name);

        if (music == null)
        {
            Debug.Log("Khong tim thay am thanh");
            return;
        }

        audioSource.clip = music.audioCLip;
        audioSource.loop = true;
        audioSource.Play();
    }

    public void PlaySFX(string name)
    {
        var soundEffect = sfxSounds.Find(x => x.name == name);

        if (soundEffect == null)
        {
            Debug.Log("Khong tim thay am thanh");
            return;
        }

        sfxSource.PlayOneShot(soundEffect.audioCLip);
    }
}
