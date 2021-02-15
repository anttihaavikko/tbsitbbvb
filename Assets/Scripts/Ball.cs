using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    public EffectCamera cam;
    public float effectLimit = 15f;

    private float stopCooldown;

    private void OnCollisionEnter2D(Collision2D other)
    {
        var mag = other.relativeVelocity.magnitude;
        if (mag > effectLimit)
        {
            cam.BaseEffect(other.relativeVelocity.magnitude * 0.01f);
            
            if (other.gameObject.CompareTag("Hand") && mag > 20f && stopCooldown <= 0f && other.rigidbody.velocity.magnitude > 12f)
            {
                stopCooldown = 0.5f;
                Time.timeScale = 0f;
                this.StartCoroutine(() => Time.timeScale = 1f, 1f / 60f);
            }
        }

        if (mag > 10f)
        {
            EffectManager.Instance.AddEffect(0, other.contacts[0].point);
        }
    }

    private void Update()
    {
        stopCooldown -= Time.unscaledDeltaTime;
    }
}
