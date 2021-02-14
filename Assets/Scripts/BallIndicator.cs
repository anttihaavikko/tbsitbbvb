using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallIndicator : MonoBehaviour
{
    public Transform ball;
    public float limit;
    public SpriteRenderer sprite;
    
    private void Update()
    {
        var ballPos = ball.position;
        var t = transform;
        sprite.enabled = ballPos.y > limit;
        t.position = new Vector3(ballPos.x, t.position.y, 0);
    }
}
