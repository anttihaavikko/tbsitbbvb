using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomMirror : MonoBehaviour
{
    private void Start()
    {
        var t = transform;
        var scale = t.localScale;
        scale = new Vector3(scale.x * Random.value < 0.5f ? 1 : -1, scale.y, scale.z);
        t.localScale = scale;
        t.position += Vector3.left * Random.Range(-3f, 3f);
        t.Rotate(new Vector3(0, 0, Random.Range(-5f, 5f)));
    }
}
