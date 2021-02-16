using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class IntroSplash : MonoBehaviour
{
    public TextWithBackground heading, vs, home, opponent;

    private void Start()
    {
        HideAndRotate(heading.transform);
        HideAndRotate(home.transform);
        HideAndRotate(vs.transform);
        HideAndRotate(opponent.transform);
    }
    
    public void SetPlayerNames(Color c1, Color c2)
    {
        var name1 = "Jim";
        var name2 = "Bob";
        home.SetText("<color=#" + ColorUtility.ToHtmlStringRGB(c1) + ">" + name1 + "</color>" + " & " + "<color=#" + ColorUtility.ToHtmlStringRGB(c2) + ">" + name2 + "</color>");
    }

    public void SetOpponentNames(Color c1, Color c2)
    {
        var rand = new System.Random();
        var name1 = Namer.GenerateName(rand);
        var name2 = Namer.GenerateName(rand);
        opponent.SetText("<color=#" + ColorUtility.ToHtmlStringRGB(c1) + ">" + name1 + "</color>" + " & " + "<color=#" + ColorUtility.ToHtmlStringRGB(c2) + ">" + name2 + "</color>");
    }

    public void Show()
    {
        Tweener.Instance.ScaleTo(heading.transform, Vector3.one, 0.3f, 0f, TweenEasings.BounceEaseOut);
        Tweener.Instance.ScaleTo(home.transform, Vector3.one, 0.3f, 0.2f, TweenEasings.BounceEaseOut);
        Tweener.Instance.ScaleTo(vs.transform, Vector3.one, 0.3f, 0.4f, TweenEasings.BounceEaseOut);
        Tweener.Instance.ScaleTo(opponent.transform, Vector3.one, 0.3f, 0.6f, TweenEasings.BounceEaseOut);
        
        Invoke(nameof(Hide), 5f);
    }

    private void Hide()
    {
        Tweener.Instance.ScaleTo(heading.transform, Vector3.zero, 0.2f, 0f, TweenEasings.QuadraticEaseIn);
        Tweener.Instance.ScaleTo(home.transform, Vector3.zero, 0.2f, 0.1f, TweenEasings.QuadraticEaseIn);
        Tweener.Instance.ScaleTo(vs.transform, Vector3.zero, 0.2f, 0.2f, TweenEasings.QuadraticEaseIn);
        Tweener.Instance.ScaleTo(opponent.transform, Vector3.zero, 0.2f, 0.3f, TweenEasings.QuadraticEaseIn);
    }

    private static void HideAndRotate(Transform t)
    {
        t.localScale = Vector3.zero;
        t.Rotate(new Vector3(0, 0, Random.Range(-5f, 5f)));
    }
}
