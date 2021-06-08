using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    public Transform observeObject;

    // Update is called once per frame
    void Update()
    {
        if (observeObject != null) {
            transform.position = new Vector3(observeObject.position.x, transform.position.y, transform.position.z);
        }
    }
}
