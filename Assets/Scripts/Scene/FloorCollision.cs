using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorCollision : MonoBehaviour
{
    //Set in inspector window
    public bool isInnerCourt;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<BallBehaviour>() == null) return;
        {
            GameManager.Instance.AudioManager.GetComponent<AudioManager>().PlaySound("Bounce");
            collision.gameObject.GetComponent<BallBehaviour>().AddBounceCounter(isInnerCourt, collision.gameObject.transform.position.z);
        }
    }
}
