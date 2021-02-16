﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Appearer : MonoBehaviour
{
	public float appearAfter = -1f;
	public float hideDelay;
    public bool silent;

    public TMP_Text text;
    private Vector3 size;

    // Start is called before the first frame update
    void Start()
    {
	    var t = transform;
	    size = t.localScale;
        t.localScale = Vector3.zero;

		if (appearAfter >= 0)
			Invoke(nameof(Show), appearAfter);
    }

    public void Show()
    {
        if(!silent)
        {
            // AudioManager.Instance.PlayEffectAt(16, Vector3.zero, 0.336f);
            // AudioManager.Instance.PlayEffectAt(17, Vector3.zero, 0.329f);
        }

        gameObject.SetActive(true);
		Tweener.Instance.ScaleTo(transform, size, 0.3f, 0f, TweenEasings.BounceEaseOut);
    }

    public void Hide()
	{
        CancelInvoke(nameof(Show));

        if(!silent)
        {
            // AudioManager.Instance.PlayEffectAt(16, Vector3.zero, 0.336f);
            // AudioManager.Instance.PlayEffectAt(17, Vector3.zero, 0.329f);
        }

		Tweener.Instance.ScaleTo(transform, Vector3.zero, 0.2f, 0f, TweenEasings.QuadraticEaseOut);
		Invoke(nameof(DisableObject), 0.3f);
	}

    private void DisableObject()
    {
	    gameObject.SetActive(false);
    }

    public void HideWithDelay()
	{
		Invoke(nameof(Hide), hideDelay);
	}

    public void ShowWithText(string t, float delay)
    {
        if (text)
            text.text = t;

        Invoke(nameof(Show), delay);
    }
}
