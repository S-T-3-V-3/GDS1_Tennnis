using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHit : MonoBehaviour
{
    private BallBehaviour ballBehaviour;
    private float xDirectionModifier;

    //Taken in inspector window
    public GameObject playerModel;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<BallBehaviour>() == null) return;
        {
            GameManager.Instance.AudioManager.GetComponent<AudioManager>().PlaySound("Hit");

            ballBehaviour = collision.gameObject.GetComponent<BallBehaviour>();

            if(transform.position.z < 0)
            {
                xDirectionModifier = playerModel.transform.localRotation.y;
            }
            else
            {
                xDirectionModifier = playerModel.transform.localRotation.y * 3.5f / 8.5f;
            }


            ballBehaviour.ReturnBall(this.transform, xDirectionModifier, GetComponent<PlayerController>().currentTeam);
        }
    }
}
