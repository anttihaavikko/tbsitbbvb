using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Appearer : MonoBehaviour
{
	public float appearAfter = -1f;
	public float hideDelay;
    public bool silent;
    public bool randomizeAngle;
    public float maxAngle = 5f;
    public Transform soundPos;

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
	    if (randomizeAngle)
	    {
			transform.rotation = Quaternion.Euler(new Vector3(0, 0, Random.Range(-maxAngle, maxAngle)));    
	    }
	    
        if(!silent)
        {
	        var p = soundPos ? soundPos.position : transform.position;
	        DoSound();
        }

        gameObject.SetActive(true);
		Tweener.Instance.ScaleTo(transform, size, 0.3f, 0f, TweenEasings.BounceEaseOut);
    }

    private void DoSound(float volume = 1f)
    {
	    var p = soundPos ? soundPos.position : transform.position;
	    AudioManager.BaseSound(p, volume);
    }

    public void Hide()
	{
        CancelInvoke(nameof(Show));

        if(!silent)
        {
	        DoSound(0.4f);
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

    public void ShowAfter()
    {
	    Invoke(nameof(Show), hideDelay);
    }
}
