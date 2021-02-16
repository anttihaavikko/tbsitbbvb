using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundCollider : MonoBehaviour
{
    public Transform emitPoint;
    
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Ground") && other.relativeVelocity.magnitude > 3f)
        {
            EffectManager.Instance.AddEffect(1, emitPoint.position);
        }
    }
}
