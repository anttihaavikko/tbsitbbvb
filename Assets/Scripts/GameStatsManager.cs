using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameStatsManager : MonoBehaviour
{
    private GameStats data;
    private List<Dude> touchers;

    private void Awake()
    {
        touchers = new List<Dude>();
            
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

    public void AddToucher(Dude d)
    {
        if(!touchers.Contains(d)) touchers.Add(d);
    }

    public bool WasSolo()
    {
        return touchers.Count <= 1;
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

    public void MarkTutorialDone()
    {
        data.tutorialDone = true;
        Save();
    }

    public void CheckFive()
    {
        if (data.wins == 5)
        {
            CompleteChallenge(12);
        }
    }
}

[System.Serializable]
public class GameStats
{
    public bool tutorialDone;
    public int wins;
    public int losses;
    public List<Challenge> completed;
    public List<Challenge> recentlyCompleted;
    public string id;

    public GameStats()
    {
        wins = 0;
        losses = 0;
        completed = new List<Challenge>();
        recentlyCompleted = new List<Challenge>();
        id = Guid.NewGuid().ToString();
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
        "Receive a special hit",
        "Win under two minutes",
        "Win a flawless match",
        "Win solo", // 10
        "Win without swinging",
        "Win 5 matches"
    };
    
    public int index;
    public DateTime completed;

    public Challenge(int i)
    {
        index = i;
        completed = DateTime.Now;
    }
}