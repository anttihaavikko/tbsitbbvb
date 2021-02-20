using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using Random = UnityEngine.Random;

public class IntroSplash : MonoBehaviour
{
    public TextWithBackground heading, vs, home, opponent;
    public List<Appearer> appearers; 

    private void Start()
    {
        // HideAndRotate(heading.transform);
        // HideAndRotate(home.transform);
        // HideAndRotate(vs.transform);
        // HideAndRotate(opponent.transform);
    }
    
    public void SetPlayerNames(Dude d1, Dude d2)
    {
        var name1 = d1.GetName();
        var name2 = d2.GetName();
        home.SetText("<color=#" + ColorUtility.ToHtmlStringRGB(d1.GetColor()) + ">" + name1 + "</color>" + " & " + "<color=#" + ColorUtility.ToHtmlStringRGB(d2.GetColor()) + ">" + name2 + "</color>");
    }

    public void SetOpponentNames(Color c1, Color c2)
    {
        var rand = new System.Random();
        var textInfo = new CultureInfo("en-US", false).TextInfo;
        var name1 = Namer.GenerateName(rand);
        var name2 = Namer.GenerateName(rand);
        opponent.SetText("<color=#" + ColorUtility.ToHtmlStringRGB(c1) + ">" + name1 + "</color>" + " & " + "<color=#" + ColorUtility.ToHtmlStringRGB(c2) + ">" + name2 + "</color>");
    }

    public void Show()
    {
        appearers.ForEach(a => a.ShowAfter());
        Invoke(nameof(Hide), 4.5f);
    }

    private void AnimateIn(Transform t, float delay)
    {
        Tweener.Instance.ScaleTo(t, Vector3.one, 0.3f, delay, TweenEasings.BounceEaseOut);
        this.StartCoroutine(() => AudioManager.BaseSound(Vector3.zero), delay + 0.6f);
    }

    private void Hide()
    {
        appearers.ForEach(a => a.HideWithDelay());
    }

    private static void HideAndRotate(Transform t)
    {
        t.localScale = Vector3.zero;
        t.Rotate(new Vector3(0, 0, Random.Range(-5f, 5f)));
    }

    public void SetHeading(string roundName)
    {
        heading.SetText(roundName);
    }
}
