using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scorer : MonoBehaviour
{
    public Dude starter;
    public Rigidbody2D ball;
    public ScoreDisplay scoreDisplay;
    public int playerMulti, opponentMulti;

    private bool triggering;
    private ParticleSystem ballTrail;

    private void Start()
    {
        ballTrail = ball.gameObject.GetComponentInChildren<ParticleSystem>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (triggering) return;
        
        triggering = true;
        Invoke(nameof(NextRound), 0.75f);
    }

    private void NextRound()
    {
        ballTrail.Stop();
        scoreDisplay.UpdateScores(playerMulti, opponentMulti);
        ball.position = new Vector2(starter.body.position.x, 5f);
        ball.velocity = Vector2.zero;
        triggering = false;
        Invoke(nameof(ReactivateBallTrail), 0.2f);
    }

    private void ReactivateBallTrail()
    {
        ballTrail.Play();
    }
}
