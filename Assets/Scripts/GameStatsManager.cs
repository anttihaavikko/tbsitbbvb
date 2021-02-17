using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameStatsManager : MonoBehaviour
{
    private GameStats data;

    private void Awake()
    {
        data = new GameStats();
        Load();
    }

    public void Save()
    {
        var json = JsonUtility.ToJson(data);
        PlayerPrefs.SetString("GameStats", json);
    }

    private void Load()
    {
        const string key = "GameStats";
        if (!PlayerPrefs.HasKey(key)) return;
        var json = PlayerPrefs.GetString(key);
        data = JsonUtility.FromJson<GameStats>(json);
    }

    public void Clear()
    {
        PlayerPrefs.DeleteKey("GameStats");
    }

    public GameStats GetData()
    {
        return data;
    }

    public string GetRoundName()
    {
        string[] names =
        {
            "FIRST MATCH",
            "SECOND MATCH",
            "THIRD MATCH",
            "FOURTH MATCH",
            "FIFTH MATCH",
            "SIXTH MATCH",
            "SEVENTH MATCH",
            "EIGHTH MATCH",
            "NINTH MATCH",
            "MATCH TEN"
        };

        var r = data.wins + data.losses;
        return r < names.Length - 1 ? names[r] : "MATCH " + (r + 1);
    }
    
    public void CompleteChallenge(int index)
    {
        var alreadyCompleted = data.completed.Any(c => c.index == index);
        var alreadyRecentlyCompleted = data.recentlyCompleted.Any(c => c.index == index);
        
        if (alreadyCompleted || alreadyRecentlyCompleted) return;
        
        data.recentlyCompleted.Add(new Challenge(index));
    }
}

[System.Serializable]
public class GameStats
{
    public int wins;
    public int losses;
    public List<Challenge> completed;
    public List<Challenge> recentlyCompleted;

    public GameStats()
    {
        wins = 0;
        losses = 0;
        completed = new List<Challenge>();
        recentlyCompleted = new List<Challenge>();
    }
}

[System.Serializable]
public class Challenge
{
    public static readonly string[] Names =
    {
        "Win a match",
        "Hit a special",
        "Serve an ace",
        "Win a 15+ point match",
        "Win a 5+ min match",
        "Fault!", // 5
        "Hit a curve ball",
        "Receive a special hit"
    };
    
    public int index;
    public DateTime completed;

    public Challenge(int i)
    {
        index = i;
        completed = DateTime.Now;
    }
}