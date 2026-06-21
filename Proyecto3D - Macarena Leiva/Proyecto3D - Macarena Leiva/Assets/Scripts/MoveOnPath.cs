using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveOnPath : MonoBehaviour
{
    public EditorPath pathToFollow;
    public int currentWayPointID = 0;
    public float speed = 1.0f;
    public float rotationSpeed = 5.0f;
    public string pathName;

    private float reachDistance = 1.0f;
    private Vector3 last_position;
    private Vector3 current_position;

    void Start()
    {
        last_position = transform.position;
    }

    void Update()
    {
        float distance = Vector3.Distance( pathToFollow.path_objs[currentWayPointID].position, transform.position );
        transform.position = Vector3.MoveTowards( transform.position, pathToFollow.path_objs[currentWayPointID].position, Time.deltaTime*speed );

        var rotation = Quaternion.LookRotation( pathToFollow.path_objs[currentWayPointID].position - transform.position );
        transform.rotation = Quaternion.Slerp( transform.rotation, rotation, Time.deltaTime*rotationSpeed );
        if( distance <= reachDistance )
            currentWayPointID++;
        if( currentWayPointID >= pathToFollow.path_objs.Count )
            currentWayPointID = 0;
    }
}
