using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball_Launch : MonoBehaviour
{
    public Rigidbody ballRigidbody;
    public float launchPower = 5f;

    // Start is called before the first frame update
    void Start()
    {
        ballRigidbody.velocity = new Vector3(9f, 0, launchPower);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
