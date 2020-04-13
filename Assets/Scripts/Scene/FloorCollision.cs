using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorCollision : MonoBehaviour
{
    //Set in inspector window
    public ScoreboardManager scoreboardManager;
    public bool isInnerCourt;

    public AudioSource audioSource; 

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<BallBehaviour>() == null) return;
        {
            audioSource.Play();
            scoreboardManager.AddBounceCounter(isInnerCourt, collision.gameObject.transform.position.z);
        }
    }
}
