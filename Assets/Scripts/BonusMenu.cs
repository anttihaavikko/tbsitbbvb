using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class BonusMenu : MonoBehaviour
{
    public Transform selectionArrow;
    public BonusTile singlePrefab, doublePrefab;
    public Transform container;
    public KeyCode up, down;
    public KeyCode left, right;
    public Transform help;
    public Appearer appearer;

    private List<BonusTile> bonuses;
    private int current;
    private bool selected;
    private Dude dude;
    private bool locked;

    private void Start()
    {
        bonuses = new List<BonusTile>();
    }

    public void Populate(Dude d)
    {
        dude = d;

        var amount = Mathf.Clamp(3 + dude.GetRawStat(Stat.Extras), 2, 6);
        
        for (var i = 0; i < amount; i++)
        {
            if (!doublePrefab) return;

            var b = Bonus.GetRandom(dude);
            var bonus = Instantiate(b.secondAmount != 0 ? doublePrefab : singlePrefab, container);
            bonus.Setup(b);
            
            bonuses.Add(bonus);
        }

        current = bonuses.Count - 1;
        
        Canvas.ForceUpdateCanvases();
        container.gameObject.SetActive(false);
        container.gameObject.SetActive(true);
    }

    private void Update()
    {
        if (locked) return;
        
        if (Input.GetKeyDown(up))
        {
            SelectNext(-1);
        }

        if (Input.GetKeyDown(down))
        {
            SelectNext(1);
        }
        
        if (Input.GetKeyDown(left))
        {
            Toggle();
        }

        if (Input.GetKeyDown(right))
        {
            Toggle();
        }
    }

    private void Toggle()
    {
        if (!bonuses.Any()) return;
        
        if (selected)
        {
            dude.ApplyBonus(bonuses[current].GetBonus(), -1);
            bonuses.ForEach(d => d.dimmer.SetActive(false));
            bonuses[current].transform.localScale = Vector3.one;
        }
        else
        {
            var bonus = bonuses[current]; 
            bonuses.ForEach(d => d.dimmer.SetActive(true));
            bonus.dimmer.SetActive(false);
            bonus.transform.localScale = Vector3.one * 1.05f;
            dude.ApplyBonus(bonus.GetBonus());
            dude.SayNice();
        }

        selected = !selected;
        selectionArrow.gameObject.SetActive(!selected);
    }

    private void SelectNext(int direction)
    {
        if (!bonuses.Any()) return;
        if (selected) Toggle();
        AudioManager.Instance.PlayEffectAt(5, selectionArrow.position, 0.408f);
        AudioManager.Instance.PlayEffectAt(8, selectionArrow.position, 1f);
        current = (current + direction + bonuses.Count) % bonuses.Count;
        FocusArrow();
    }

    private void FocusArrow()
    {
        var p = selectionArrow.position;
        p.y = bonuses[current].transform.position.y;
        selectionArrow.position = p;
    }

    public void Mirror()
    {
        MirrorTransform(transform);
        MirrorTransform(container);
        MirrorTransform(help);
    }

    private static void MirrorTransform(Transform t)
    {
        var v = t.localScale;
        t.localScale = new Vector3(-1f * v.x, v.y, v.z);
    }

    public void Lock()
    {
        locked = true;
    }

    public bool Selected()
    {
        return selected;
    }
}

public class Bonus
{
    public Stat firstStat, secondStat;
    public int firstAmount, secondAmount;
    public Color color;
    public BonusColor colorType;
    
    public static Bonus GetRandom(Dude dude)
    {
        var first = 1;
        var second = 0;
        
        if (Random.value < 0.2f)
        {
            first = 2;
            second = 0;
        }
        
        if (Random.value < 0.1f)
        {
            first = 1;
            second = 1;
        }

        if (Random.value < 0.05f)
        {
            first = 3;
            second = -1;
        }
        
        var boon = Stats.GetRandom();
        return new Bonus
        {
            firstStat = boon,
            firstAmount = first,
            secondStat = dude.GetBane(boon),
            secondAmount = second,
            color = Color.HSVToRGB(Random.value, 0.25f, 1f),
            colorType = (BonusColor) Random.Range(0, 3)
        };
    }
}

public enum BonusColor
{
    Top,
    Middle,
    Bottom
}