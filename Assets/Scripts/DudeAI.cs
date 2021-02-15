using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class DudeAI : MonoBehaviour
{
    public Dude dude;
    public Transform checkPoint;
    public LayerMask ballMask;
    public Rigidbody2D ball;

    private float swingCooldown;
    private float homePos;
    private float armLength;

    private void Start()
    {
        homePos = dude.body.position.x;
        armLength = dude.GetStat(Stat.ArmLength);
    }

    private void Update()
    {
        TrySwing();

        swingCooldown -= Time.deltaTime;

        MoveHome();

        TryJump();
        FollowBall();

        if (IsStuck())
        {
            // ReSharper disable once Unity.PerformanceCriticalCodeInvocation
            Invoke(nameof(FixIfStuck), 0.5f);
        }
    }

    private void MoveHome()
    {
        var diff = dude.body.position.x - homePos;
        if (diff > 1f) dude.Move(-1f);
        if (diff < -1f) dude.Move(1f);
    }

    private void TrySwing()
    {
        var dist = 1.5f * dude.GetStat(Stat.ArmLength);
        var ballFound = Physics2D.OverlapCircle(checkPoint.position, dist, ballMask);
        
        if (!ballFound || !(swingCooldown <= 0f) || !(Random.value < 0.1f)) return;
        
        swingCooldown = 0.5f;
        dude.Swing();
    }

    private void TryJump()
    {
        var diff = dude.body.position.x - ball.position.x;
        var dist = Mathf.Abs(diff);
        var ballFound = Physics2D.OverlapCircle(checkPoint.position, 10f, ballMask);
        if (ballFound && ball.velocity.magnitude < 5f && dist < 2f && Random.value < 0.1f)
        {
            dude.Jump();
        }
    }

    private bool IsStuck()
    {
        var dist = 2f * dude.GetStat(Stat.ArmLength);
        return ball.velocity.magnitude < 0.2f && ball.position.y > dude.body.position.y ? 
            Physics2D.OverlapCircle(checkPoint.position, dist, ballMask) : 
            false;
    }

    private void FixIfStuck()
    {
        if (IsStuck())
        {
            dude.Jump();
        }
    }

    private void FollowBall()
    {
        const float correction = 0.5f;
        var diff = dude.body.position.x - ball.position.x - ball.velocity.x * correction;
        var dist = Mathf.Abs(diff);
        if (dist > 0.5f && dist < 4f)
        {
            dude.Move(-Mathf.Sign(diff));
        }
    }
}
