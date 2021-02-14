using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DudeAI : MonoBehaviour
{
    public Dude dude;
    public Transform checkPoint;
    public LayerMask ballMask;
    public Rigidbody2D ball;

    private float swingCooldown;
    private float homePos;

    private void Start()
    {
        homePos = dude.body.position.x;
    }

    private void Update()
    {
        // print("Ball velocity: " + ball.velocity.magnitude);
        var ballFound = Physics2D.OverlapCircle(checkPoint.position, 2f, ballMask);
        if (ballFound && swingCooldown <= 0f)
        {
            swingCooldown = 0.5f;
            dude.Swing();
        }

        swingCooldown -= Time.deltaTime;

        var diff = dude.body.position.x - homePos;
        if(diff > 1f) dude.Move(-1f);
        if(diff < -1f) dude.Move(1f);
    }
}
