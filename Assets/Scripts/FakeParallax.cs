using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FakeParallax : MonoBehaviour
{
    public Transform cam;
    
    private Vector3 previousCamPos;

    private void Start()
    {
        previousCamPos = cam.position;
    }

    // Update is called once per frame
    void Update()
    {
        var amt = (previousCamPos - cam.position) * -transform.position.z;
        var nextPos = transform.position + new Vector3(amt.x * 1.5f, amt.y, 0);
        transform.position = Vector3.Lerp(transform.position, nextPos, Time.deltaTime);
        previousCamPos = cam.position;
    }
}
