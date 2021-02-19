using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialGutter : MonoBehaviour
{
    public Tutorial tutorial;
    private bool triggering;
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (triggering) return;
        if (!other.gameObject.CompareTag("Ball")) return;
        
        triggering = true;
        Invoke(nameof(Next), 0.75f);
    }

    private void Next()
    {
        tutorial.NextSpot();
        Invoke(nameof(EnableTrigger), 0.2f);
    }

    private void EnableTrigger()
    {
        triggering = false;
    }
}
