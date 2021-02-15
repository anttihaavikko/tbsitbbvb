using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Scorer : MonoBehaviour
{
    public List<Dude> dudes;
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

        if (other.gameObject.CompareTag("Ball"))
        {
            triggering = true;
            Invoke(nameof(NextRound), 0.75f);
            return;
        }

        var d = other.GetComponentInParent<Dude>();
        if (!d) return;
        
        if (dudes.Contains(d))
        {
            d.ReturnHome();
        }
    }

    private void NextRound()
    {
        ballTrail.Stop();
        scoreDisplay.UpdateScores(playerMulti, opponentMulti);
        var starter = dudes.OrderByDescending(d => Mathf.Abs(d.body.position.x)).First();
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
