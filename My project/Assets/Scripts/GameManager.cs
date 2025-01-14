using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager 
{
    // Singleton instance for global access
    private static GameManager _instance;

    // Player details and progress
    public string playerName = "Player 1";
    public int mistakes { get; private set; } = 0; 
    public int lives { get; private set; } = 3;
    public int currentLevel { get; set; } = 1;

    // Singleton instance retrieval
    public static GameManager Instance 
    {
        get 
        {
            if (_instance == null) 
            {
                _instance = new GameManager();
            }
            return _instance;
        }
    }

    // Resets the internal game state
    public void ResetGame() 
    {
        mistakes = 0;
        lives = 3;
        currentLevel = 1;
    }

    // Reduces lives, handles game over, or reloads the level
    public void HandleMistake() 
    {
        mistakes++;

        if (lives > 0) 
        {
            lives--;
        }

        if (lives <= 0) 
        {
            EndGame();
        }
        else 
        {
            ReloadLevel();
        }
    }

    // Prepares the game for the next level
    public void LoadNextLevel() 
    {
        currentLevel++;
        ReloadLevel();
    }

    // Reloads the current level (logic to reload would be added in LevelManager)
    public void ReloadLevel() 
    {
        LevelManager.Instance.ReloadLevel();
    }

    // Handles end-game scenario
    public void EndGame() 
    {
        SceneManager.LoadScene("GameOver");
    }

    // Calculates score based on performance
    public LeaderboardEntry GetLeaderboardEntry() 
    {
        int score = (int)(20000 * currentLevel / Math.Sqrt((double)mistakes + 1)); // +1 avoids division by zero
        return new LeaderboardEntry(playerName, score, mistakes, currentLevel);
    }

   //Adds the player’s score to the leaderboard
    public void AddScoreToLeaderboard() 
    {
        Leaderboard.Instance.SavePlayerScore();
    }

}
