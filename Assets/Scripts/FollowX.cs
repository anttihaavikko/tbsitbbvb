using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowX : MonoBehaviour
{
    public Transform target;

    private void Update()
    {
        var t = transform;
        var position = t.position;
        t.position = new Vector3(target.position.x, position.y, position.z);
    }
}
