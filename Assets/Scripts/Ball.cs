using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class Ball : MonoBehaviour
{
    public EffectCamera cam;
    public float effectLimit = 15f;
    public Rigidbody2D body;

    private float stopCooldown;

    private void OnCollisionEnter2D(Collision2D other)
    {
        var mag = other.relativeVelocity.magnitude;

        if (mag > 3f)
        {
            var amount = Mathf.Clamp(mag * 0.02f, 0f, 0.25f);
            var normal = other.contacts[0].normal;
            var x = normal.x < normal.y ? 1f + amount : 1f - amount;
            var y = normal.x > normal.y ? 1f + amount : 1f - amount;

            transform.parent.localScale = new Vector3(x, y, 1);   
        }

        if (mag > effectLimit)
        {
            cam.BaseEffect(other.relativeVelocity.magnitude * 0.01f);
            EffectManager.Instance.AddEffect(0, body.position);
            
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
        var t = transform;
        var parent = t.parent;
        parent.localScale = Vector3.MoveTowards(parent.localScale, Vector3.one, Time.deltaTime * 5f);
        stopCooldown -= Time.unscaledDeltaTime;
        parent.position = body.position;
        t.localPosition = Vector3.zero;
    }
}
