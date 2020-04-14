using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip[] audioClips;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void PlaySound(string soundEffect)
    {
        switch (soundEffect)
        {
            case "Bounce":
                audioSource.clip = audioClips[0];
                break;
            case "Hit":
                audioSource.clip = audioClips[1];
                break;
            case "Score":
                audioSource.clip = audioClips[2];
                break;
            case "Powerup":
                audioSource.clip = audioClips[3];
                break;
            case "PowerShot":
                audioSource.clip = audioClips[4];
                break;
        }
        audioSource.Play();
    }
}
