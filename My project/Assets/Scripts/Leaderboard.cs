using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Leaderboard : MonoBehaviour 
{
    // Singleton instance
    private static Leaderboard _instance;

    // Public accessor for the instance
    public static Leaderboard Instance 
    {
        get 
        {
            if (_instance == null) 
            {
                Debug.LogError("Leaderboard instance is null. Ensure it's added to the scene.");
            }
            return _instance;
        }
    }

    // UI elements for leaderboard
    [SerializeField] private TextMeshProUGUI namesText;
    [SerializeField] private TextMeshProUGUI scoresText;
    [SerializeField] private TextMeshProUGUI mistakesText;
    [SerializeField] private TextMeshProUGUI levelsText;
    [SerializeField] private Animator animator;
    [SerializeField] private TextMeshProUGUI description;

    private const int MaxEntries = 10;
    private List<LeaderboardEntry> entries = new List<LeaderboardEntry>();

    private void Awake() 
    {
        // Enforce Singleton behavior
        if (_instance == null) 
        {
            _instance = this;
        } 
        else if (_instance != this) 
        {
            Destroy(gameObject); // Ensure only one instance exists
        }

        LoadEntries();

        LeaderboardEntry playerEntry = GameManager.Instance.GetLeaderboardEntry();
        description.text = $"You scored {playerEntry.score}, reached level {playerEntry.level}, and made {playerEntry.mistakes} mistakes. Enter your name to save your score:";
    }

    private void Start() 
    {
        UpdateDisplay();
    }

    public void UpdatePlayerName(string playerName) 
    {
        GameManager.Instance.playerName = playerName;
        SavePlayerScore();
        UpdateDisplay();
    }

    public void SavePlayerScore() 
    {
        LeaderboardEntry newEntry = GameManager.Instance.GetLeaderboardEntry();
        entries.Add(newEntry);

        // Sort scores in descending order and keep the top 10
        entries = entries.OrderByDescending(e => e.score).Take(MaxEntries).ToList();
        SaveEntries();
    }

    private void UpdateDisplay() 
    {
        namesText.text = string.Join("\n", entries.Select(e => e.name));
        scoresText.text = string.Join("\n", entries.Select(e => e.score));
        mistakesText.text = string.Join("\n", entries.Select(e => e.mistakes));
        levelsText.text = string.Join("\n", entries.Select(e => e.level));
    }

    public void SubmitScore() 
    {
        SavePlayerScore();
        CloseInput();
    }

    public void CloseInput() 
    {
        animator.SetBool("isOpen", false);
    }

    private void SaveEntries() 
    {
        string json = JsonUtility.ToJson(new EntryWrapper { Items = entries });
        PlayerPrefs.SetString("Leaderboard", json);
        PlayerPrefs.Save();
    }

    private void LoadEntries() 
    {
        string json = PlayerPrefs.GetString("Leaderboard", "{}");
        entries = JsonUtility.FromJson<EntryWrapper>(json)?.Items ?? new List<LeaderboardEntry>();
    }
}

[Serializable]
public class LeaderboardEntry 
{
    public string name;
    public int score;
    public int mistakes;
    public int level;

    public LeaderboardEntry(string playerName, int scoreValue, int mistakesMade, int levelReached) 
    {
        name = playerName;
        score = scoreValue;
        mistakes = mistakesMade;
        level = levelReached;
    }
}

[Serializable]
public class EntryWrapper 
{
    public List<LeaderboardEntry> Items = new List<LeaderboardEntry>();
}
