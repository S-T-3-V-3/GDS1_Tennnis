using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBoundry : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        //Sorting layer 8 = Player boundries, 9 = ball
        Physics.IgnoreLayerCollision(8, 9);
    }
}
