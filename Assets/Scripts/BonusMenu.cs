using System;
using System.Collections;
using System.Collections.Generic;
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
        
        for (var i = 0; i < 3; i++)
        {
            if (!doublePrefab) return;
            
            var bonus = Instantiate(doublePrefab, container);
            bonus.Setup(new Bonus
            {
                firstStat = Stats.GetRandom(),
                firstAmount = Random.Range(1, 3),
                secondStat = Stats.GetRandom(),
                secondAmount = Random.Range(1, 3),
                color = Color.red,
                colorType = BonusColor.Top
            });
            
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
        }

        selected = !selected;
        selectionArrow.gameObject.SetActive(!selected);
    }

    private void SelectNext(int direction)
    {
        if (selected) Toggle();
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
}

public enum BonusColor
{
    Top,
    Middle,
    Bottom
}