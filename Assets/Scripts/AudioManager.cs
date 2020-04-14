using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip[] audioClips;

    //Stops sounds after scoring
    public bool stopSounds = false;
    //TODO: RESET STOP SOUNDS AFTER A SCORE HAS OCCURED

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void PlaySound(string soundEffect)
    {
        if (!stopSounds)
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
                    stopSounds = true;
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
}
