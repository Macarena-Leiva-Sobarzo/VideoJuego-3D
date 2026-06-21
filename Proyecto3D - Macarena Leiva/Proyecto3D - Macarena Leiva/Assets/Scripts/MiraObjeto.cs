using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiraObjeto : MonoBehaviour
{
    public Transform target;

    void LateUpdate()
    {
        transform.LookAt( target.position );
    }
}
