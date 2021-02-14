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

    private Stats stats;

    private void Start()
    {
        stats = new Stats();
        face.lookTarget = ball;

        for (var i = 0; i < 5; i++)
        {
            stats.AddRandom();
        }

        UpdateVisuals();
        stats.PrintStats();
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
    Test,
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