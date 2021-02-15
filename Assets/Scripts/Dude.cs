using System.Collections.Generic;
using System.Linq;
using UnityEngine;

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

    private Stats stats;

    private void Start()
    {
        stats = new Stats();
        face.lookTarget = ball;

        for (var i = 0; i < 7; i++)
        {
            stats.AddRandom();
        }

        UpdateVisuals();
        stats.PrintStats();
        Colorize();
    }

    private void Colorize()
    {
        var skinColor = Color.HSVToRGB(Random.value, 0.25f, 1f);
        var c1 = Color.HSVToRGB(Random.value, 0.25f, 1f);
        var c2 = Color.HSVToRGB(Random.value, 0.25f, 1f);
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
    }

    public void Move(float dir)
    {
        var velocity = body.velocity;
        body.velocity = Vector2.MoveTowards(velocity, new Vector2(dir * 7f * stats.Get(Stat.Speed), velocity.y), 1f);
    }

    public void Jump()
    {
        if (!Physics2D.OverlapCircle(groundCheck.position, 0.01f)) return;
        
        body.velocity = new Vector2(body.velocity.x, 0f);
        body.AddForce(Vector2.up * (100f * stats.Get(Stat.Jump)), ForceMode2D.Impulse);
    }

    // ReSharper disable Unity.PerformanceAnalysis
    public void Swing()
    {
        var force = stats.Get(Stat.Strength);
        arm.AddForce(Vector2.right * 150f * direction * force, ForceMode2D.Impulse);
        body.AddForce(Vector2.left * 150f * direction * force, ForceMode2D.Impulse);
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

public class Stats
{
    private readonly int[] data;

    public Stats()
    {
        var statCount = System.Enum.GetNames(typeof(Stat)).Length;
        data = Enumerable.Repeat(1, statCount).ToArray();
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
}