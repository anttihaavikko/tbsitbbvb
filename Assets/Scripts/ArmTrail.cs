using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmTrail : MonoBehaviour
{
    public void Show(Transform container)
    {
        var t = transform;
        t.localPosition = Vector3.zero;
        t.localScale = Vector3.one;
        t.localRotation = Quaternion.Euler(Vector3.zero);
        Tweener.Instance.ScaleTo(t, Vector3.zero, 0.2f, 0f, TweenEasings.QuadraticEaseIn);
        Invoke(nameof(ReturnToPool), 0.3f);
        t.parent = container;
    }

    private void ReturnToPool()
    {
        EffectManager.Instance.ReturnToPool(this);
    }
}
