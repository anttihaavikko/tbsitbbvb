using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public class ChallengeTile : MonoBehaviour
{
    public TMP_Text textActual, textBg;

    public static readonly Color White = new Color(1f, 1f, 1f, 0.5f);

    public void SetText(int index, GameStats stats)
    {
        var challenge = Challenge.Names[index];
        textBg.text = "<mark=#ffffff padding='30, 170, 10, 10'>" + challenge + "</mark>";
        textBg.transform.Rotate(new Vector3(0, 0, Random.Range(-4f, 4f)));
        textBg.gameObject.SetActive(false);
        textActual.text = challenge;

        ActivateIfDone(index, stats);
        AnimateIfJustDone(index, stats);
    }

    private void AnimateIfJustDone(int index, GameStats stats)
    {
        if (stats.recentlyCompleted.All(c => c.index != index)) return;
        var s = textBg.transform.localScale;
        textBg.transform.localScale = new Vector3(0f, s.y, s.z);
        textBg.gameObject.SetActive(true);
        this.StartCoroutine(() => AnimateIn(s), 2f + Random.Range(0f, 0.4f));
    }

    private void AnimateIn(Vector3 s)
    {
        Tweener.Instance.ScaleTo(textBg.transform, s, 0.75f, 0, TweenEasings.BackEaseOut);
        Tweener.Instance.ColorTo(textBg, White, 0.5f, 0, TweenEasings.BackEaseOut);
        Tweener.Instance.ColorTo(textActual, White, 0.5f, 0, TweenEasings.BackEaseOut);
    }

    private void ActivateIfDone(int index, GameStats stats)
    {
        if (stats.completed.Any(c => c.index == index))
        {
            textBg.gameObject.SetActive(true);
            textBg.color = White;
            textActual.color = White;
        }
    }
}
