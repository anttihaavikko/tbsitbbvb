using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundCollider : MonoBehaviour
{
    public Transform emitPoint;
    
    private void OnCollisionEnter2D(Collision2D other)
    {
        var mag = Mathf.Abs(other.relativeVelocity.y);
        if (!other.gameObject.CompareTag("Ground")) return;
        
        if (mag > 3f)
        {
            EffectManager.Instance.AddEffect(1, emitPoint.position);
        }
                

        if (mag > 5f)
        {
            var position = emitPoint.position;
            AudioManager.Instance.PlayEffectAt(24, position, 0.465f);
            AudioManager.Instance.PlayEffectAt(23, position, 0.335f);
            AudioManager.Instance.PlayEffectAt(17, position, 0.171f);
            AudioManager.Instance.PlayEffectAt(20, position, 0.106f);
            AudioManager.Instance.PlayEffectAt(6, position, 0.294f);
        }
    }
}
