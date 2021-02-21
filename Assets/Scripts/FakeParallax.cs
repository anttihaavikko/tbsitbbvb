using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FakeParallax : MonoBehaviour
{
    public Transform cam;
    
    private Vector3 previousCamPos;

    private void Awake()
    {
        previousCamPos = cam.position;
    }

    private void Update()
    {
        var t = transform;
        var p = t.position;
        var camPos = cam.position;
        var amt = (previousCamPos - camPos) * -p.z;
        var nextPos = p + new Vector3(amt.x * 1.5f, amt.y, 0);
        transform.position = Vector3.Lerp(p, nextPos, Time.deltaTime);
        previousCamPos = camPos;
    }
}
