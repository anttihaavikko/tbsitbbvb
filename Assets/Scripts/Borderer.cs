using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ReSharper disable once IdentifierTypo
public class Borderer : MonoBehaviour
{
    public Transform target;
    public float width = 0.1f;

    public void Fix()
    {
        var lossyScale = target.lossyScale;
        var sign = Mathf.Sign(lossyScale.x);
        var x = (lossyScale.x + width * sign) / lossyScale.x;
        var y = (lossyScale.y + width) / lossyScale.y;
        transform.localScale = new Vector3(x, y, 1);
    }
}
