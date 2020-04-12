using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHit : MonoBehaviour
{
    public AudioSource audioSource;
    private BallBehaviour ballBehaviour;
    private float xDirectionModifier;

    //Taken in inspector window
    public GameObject playerModel;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if(playerModel.transform.position.z < 0)
        {
            Debug.Log("Player 1: " + playerModel.transform.localRotation);
        }
        else
        {
            Debug.Log("Player 2: " + playerModel.transform.localRotation);
        }     
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<BallBehaviour>() == null) return;
        {
            audioSource.Play();

            ballBehaviour = collision.gameObject.GetComponent<BallBehaviour>();

            if(transform.position.z < 0)
            {
                xDirectionModifier = playerModel.transform.localRotation.y;
            }
            else
            {
                xDirectionModifier = playerModel.transform.localRotation.y - 0.7f;
            }


            ballBehaviour.ReturnBall(this.transform, xDirectionModifier);
        }
    }
}
