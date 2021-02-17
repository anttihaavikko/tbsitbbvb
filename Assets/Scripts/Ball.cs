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
    public GameObject super;
    public ParticleSystem superStars;
    public GameStatsManager gameStats;

    private float stopCooldown;
    private float homingAmount, homingDirection;
    private Dude lastHit;

    private void Start()
    {
        superStars.Stop();
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        var mag = other.relativeVelocity.magnitude;
        var dude = other.gameObject.GetComponentInParent<Dude>();
        
        if ((other.gameObject.CompareTag("Ground") || other.gameObject.CompareTag("Net")) && mag > 7f)
        {
            EffectManager.Instance.AddEffect(1, other.contacts[0].point);
        }

        if (!other.gameObject.CompareTag("Hand") && !other.gameObject.CompareTag("Net"))
        {
            ClearHoming();   
        }

        if (mag > 3f)
        {
            var amount = Mathf.Clamp(mag * 0.02f, 0f, 0.25f);
            var normal = other.contacts[0].normal;
            var x = normal.x < normal.y ? 1f + amount : 1f - amount;
            var y = normal.x > normal.y ? 1f + amount : 1f - amount;

            transform.parent.localScale = new Vector3(x, y, 1);   
        }

        if (homingAmount > 0f && other.gameObject.CompareTag("Hand") && dude && dude != lastHit)
        {
            gameStats.CompleteChallenge(7);
        }

        if (mag > effectLimit)
        {
            cam.BaseEffect(other.relativeVelocity.magnitude * 0.01f);
            EffectManager.Instance.AddEffect(0, body.position);
            
            if (other.gameObject.CompareTag("Hand"))
            {
                if (other.relativeVelocity.magnitude > 5f)
                {
                    EffectManager.Instance.AddEffect(2, other.contacts[0].point);
                }

                if (mag > 20f && stopCooldown <= 0f && other.rigidbody.velocity.magnitude > 12f)
                {
                    body.angularVelocity = 500f * (0.5f + dude.GetRawStat(Stat.Spin) * 0.25f) * dude.direction;
                    stopCooldown = 0.5f;
                    Time.timeScale = 0f;
                    this.StartCoroutine(() => Time.timeScale = 1f, 1f / 60f);

                    if (body.angularVelocity > 550f && dude.direction > 0)
                    {
                        gameStats.CompleteChallenge(6);
                    }

                    if (dude && dude.partner == lastHit)
                    {
                        if (dude.direction > 0)
                        {
                            gameStats.CompleteChallenge(1);
                        }
                        AddHoming(dude.GetRawStat(Stat.Super) * 1f, dude.direction);
                    }
                }

                lastHit = dude;
                CancelInvoke(nameof(ResetLastTouch));
                Invoke(nameof(ResetLastTouch), dude.GetStat(Stat.Super));
            }
        }

        if (mag > 10f)
        {
            EffectManager.Instance.AddEffect(0, other.contacts[0].point);
        }
    }

    private void ResetLastTouch()
    {
        lastHit = null;
    }

    private void Update()
    {
        var t = transform;
        var parent = t.parent;
        parent.localScale = Vector3.MoveTowards(parent.localScale, Vector3.one, Time.deltaTime * 5f);
        stopCooldown -= Time.unscaledDeltaTime;
        parent.position = body.position;
        t.localPosition = Vector3.zero;

        if (homingAmount > 0f && SameSign(body.position.x, homingDirection))
        {
            body.AddForce(Vector2.down * (10f * homingAmount), ForceMode2D.Force);
        }
        
        if (homingAmount > 0f && body.velocity.magnitude < 5f)
        {
            ClearHoming();
        }
    }

    private static bool SameSign(float num1, float num2)
    {
        return num1 < 0 == num2 < 0;
    }

    private void AddHoming(float amount, float direction)
    {
        homingAmount = amount;
        homingDirection = direction;
        super.SetActive(amount > 0f);
        if (amount > 0f)
        {
            superStars.Play();
        }
        else
        {
            superStars.Stop();
        }
    }

    private void ClearHoming()
    {
        AddHoming(0f, 0f);
    }

    public Dude LastToucher()
    {
        return lastHit;
    }
}
