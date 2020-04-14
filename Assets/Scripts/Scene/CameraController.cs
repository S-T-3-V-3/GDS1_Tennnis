using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    Transform worldAlignedTransform;
    public Vector3 forwardVector;
    public Vector3 rightVector;

    void Update()
    {
        worldAlignedTransform.position.Set(gameObject.transform.position.x,gameObject.transform.position.y,gameObject.transform.position.z);
        worldAlignedTransform.eulerAngles = new Vector3(0f,gameObject.transform.eulerAngles.y,0f);
        forwardVector = worldAlignedTransform.forward;
        rightVector = worldAlignedTransform.right;
    }
}
