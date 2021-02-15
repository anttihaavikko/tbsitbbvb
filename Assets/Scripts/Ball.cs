using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    public EffectCamera cam;
    public float effectLimit = 15f;

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.relativeVelocity.magnitude > effectLimit)
        {
            cam.BaseEffect(other.relativeVelocity.magnitude * 0.01f);
        }
    }
}
