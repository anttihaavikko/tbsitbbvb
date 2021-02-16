using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class Dude : MonoBehaviour
{
    public Rigidbody2D body, arm;
    public float direction = 1f;
    public Face face;
    public Transform ball;
    public Transform bodyVisual;
    public Color shadowColor;
    public List<SpriteRenderer> skinSprites, skinDarkSprites;
    public List<SpriteRenderer> shirtSprites, shirtDarkSprites;
    public List<SpriteRenderer> pantsSprites, pantsDarkSprites;
    public Transform groundCheck;
    public List<Borderer> borders;
    public Animator anim;
    public BonusMenu bonusMenu;

    private Stats stats;
    private Vector2 startBodyPos, startArmPos;
    private bool canMove = true;

    private void Awake()
    {
        stats = new Stats();
        face.lookTarget = ball;

        // for (var i = 0; i < 7; i++)
        // {
        //     stats.AddRandom();
        //     stats.AddColor(BonusColor.Top, Color.HSVToRGB(Random.value, 0.25f, 1f));
        // }

        UpdateVisuals();
        Colorize();
    }

    private void Start()
    {
        startBodyPos = body.position;
        startArmPos = arm.position;
    }

    private void Update()
    {
        if (!canMove && bonusMenu)
        {
            bonusMenu.transform.position = body.position;
        }
    }

    private void Colorize()
    {
        var skinColor = stats.GetColor(BonusColor.Top);
        var c1 = stats.GetColor(BonusColor.Middle);
        var c2 =stats.GetColor(BonusColor.Bottom);
        skinSprites.ForEach(s => s.color = skinColor);
        skinDarkSprites.ForEach(s => s.color = skinColor * shadowColor);
        shirtSprites.ForEach(s => s.color = c1);
        shirtDarkSprites.ForEach(s => s.color = c1 * shadowColor);
        pantsSprites.ForEach(s => s.color = c2);
        pantsDarkSprites.ForEach(s => s.color = c2 * shadowColor);
    }

    private void UpdateVisuals()
    {
        arm.transform.localScale = new Vector3(1f, stats.Get(Stat.ArmLength), 1f);
        bodyVisual.localScale = new Vector3(1f, stats.Get(Stat.Height), 1f);
        
        borders.ForEach(b => b.Fix());
    }

    public void Move(float dir)
    {
        if (!canMove) return;
        
        var velocity = body.velocity;
        body.velocity = Vector2.MoveTowards(velocity, new Vector2(dir * 7f * stats.Get(Stat.Speed), velocity.y), 1f);
    }

    public void Jump()
    {
        if (!canMove) return;
        
        if (Mathf.Abs(body.velocity.y) > 0.5f) return;
        if (!Physics2D.OverlapCircle(groundCheck.position, 0.1f)) return;
        
        // DisableAnimation();
        
        body.velocity = new Vector2(body.velocity.x, 0f);
        body.AddForce(Vector2.up * (100f * stats.Get(Stat.Jump)), ForceMode2D.Impulse);
    }

    // ReSharper disable Unity.PerformanceAnalysis
    public void Swing()
    {
        if (!canMove) return;
        
        // DisableAnimation();
        var force = stats.Get(Stat.Strength);
        arm.AddForce(Vector2.right * 150f * direction * force, ForceMode2D.Impulse);
        body.AddForce(Vector2.left * 150f * direction * force, ForceMode2D.Impulse);
    }

    private void DisableAnimation()
    {
        anim.enabled = false;
        CancelInvoke(nameof(ResumeAnimation));
        Invoke(nameof(ResumeAnimation), 0.5f);
    }

    private void ResumeAnimation()
    {
        anim.enabled = true;
    }

    public float GetStat(Stat stat)
    {
        return stats.Get(stat);
    }

    public void ReturnHome()
    {
        body.position = startBodyPos;
        arm.position = startArmPos;
    }
    
    public void ShowMenu()
    {
        canMove = false;

        if (bonusMenu)
        {
            bonusMenu.transform.position = body.position;
            bonusMenu.gameObject.SetActive(true);

            this.StartCoroutine(() => bonusMenu.Populate(this), 0.1f);
        }
    }

    public void ApplyBonus(Bonus b, int multiplier = 1)
    {
        stats.Add(b.firstStat, b.firstAmount * multiplier);
        if (b.secondAmount != 0)
        {
            stats.Add(b.secondStat, b.secondAmount * multiplier);
        }

        if (multiplier > 0)
        {
            stats.AddColor(b.colorType, b.color);
        }
        else
        {
            stats.PopColor(b.colorType);
        }
        
        UpdateVisuals();
        Colorize();
    }
}

public enum Stat
{
    Height,
    ArmLength,
    Strength,
    Jump,
    Speed,
    Spin
}

[System.Serializable]
public class Stats
{
    public int[] data;
    public Stack<Triple> skin, shirt, pants;

    public Stats()
    {
        var statCount = System.Enum.GetNames(typeof(Stat)).Length;
        data = Enumerable.Repeat(1, statCount).ToArray();
        skin = new Stack<Triple>();
        shirt = new Stack<Triple>();
        pants = new Stack<Triple>();
    }

    public Stack<Triple> GetColorList(BonusColor bc)
    {
        return bc switch
        {
            BonusColor.Top => skin,
            BonusColor.Middle => shirt,
            BonusColor.Bottom => pants,
            _ => new Stack<Triple>()
        };
    }

    public Color GetColor(BonusColor slot)
    {
        var list = GetColorList(slot);

        if (!list.Any()) return Color.white;

        var hue = 0f;
        list.ToList().ForEach(triple =>
        {
            Color.RGBToHSV(triple.ToColor(), out var h, out _, out _);
            hue += h;
        });

        hue /= list.Count;
        return Color.HSVToRGB(hue, 0.25f, 1f);
    }

    public void AddColor(BonusColor slot, Color color)
    {
        var list = GetColorList(slot);
        list.Push(Triple.FromColor(color));
    }
    
    public void PopColor(BonusColor slot)
    {
        GetColorList(slot).Pop();
    }

    public void Add(Stat stat, int amount)
    {
        data[(int) stat] += amount;
    }

    public void AddRandom()
    {
        var stat = Random.Range(0, System.Enum.GetNames(typeof(Stat)).Length);
        Add((Stat)stat, Random.Range(1, 4));
    }

    public float Get(Stat stat)
    {
        var s = (int) stat;
        var value = data[s];
        var mod = Mathf.Pow(0.99f, value);
        // Debug.Log("Applying decay: " + s + " => " + mod);
        return 1f + 0.1f * mod * value;
    }

    public void PrintStats()
    {
        var values = new List<string>();
        for (var i = 0; i < System.Enum.GetNames(typeof(Stat)).Length; i++)
        {
            var stat = (Stat) i;
            values.Add(System.Enum.GetName(typeof(Stat), stat) + ": " + Get(stat));
        }
        Debug.Log(string.Join(", ", values));
    }

    public static Stat GetRandom()
    {
        return (Stat) Random.Range(0, System.Enum.GetNames(typeof(Stat)).Length);
    }
}

[System.Serializable]
public class Triple
{
    public Triple(float aa, float bb, float cc)
    {
        a = aa;
        b = bb;
        c = cc;
    }

    public static Triple FromVector(Vector3 vector)
    {
        return new Triple(vector.x, vector.y, vector.z);
    }

    public static Triple FromColor(Color color)
    {
        return new Triple(color.r, color.g, color.b);
    }

    public Vector3 ToVector3()
    {
        return new Vector3(a, b, c);
    }

    public Color ToColor()
    {
        return new Color(a, b, c);
    }

    public bool IsBlackish()
    {
        return a < 0.1f && b < 0.1f && c < 0.1f;
    }

    public float a, b, c;
}