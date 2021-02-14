using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dude : MonoBehaviour
{
    public Rigidbody2D body, arm;
    public float direction = 1f;
    public Face face;
    public Transform ball;

    private void Start()
    {
        face.lookTarget = ball;
    }

    public void Move(float dir)
    {
        var velocity = body.velocity;
        body.velocity = Vector2.MoveTowards(velocity, new Vector2(dir * 10f, velocity.y), 1f);
    }

    public void Jump()
    {
        body.AddForce(Vector2.up * 100f, ForceMode2D.Impulse);
    }

    // ReSharper disable Unity.PerformanceAnalysis
    public void Swing()
    {
        arm.AddForce(Vector2.right * 200f * direction, ForceMode2D.Impulse);
        body.AddForce(Vector2.left * 200f * direction, ForceMode2D.Impulse);
    }
}
