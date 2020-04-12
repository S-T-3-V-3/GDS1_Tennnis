using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball_Collisions : MonoBehaviour
{
    public AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Floor")
        {
            audioSource.Play();
        }
    }
}
