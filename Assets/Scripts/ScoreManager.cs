using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Manager; // First and unique ScoreManager object that has been created.

    public List<Score> HighScores;

    public string CurrentPlayerName;

    public Score NewHighScore = null;

    public const int NB_MAX_HIGHSCORES = 5;

    private string saveFilePath;

    private bool isListModified = false;

    // Start is called before the first frame update
    public void Awake()
    {
        if (Manager == null)
        {
            Manager = this;
            NewHighScore = null;
            saveFilePath = Application.persistentDataPath + "/savedScores.json";
            Debug.Log("saveFilePath = " + saveFilePath);
            LoadScores();
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    [System.Serializable]
    public class Scores
    {
        public List<Score> scoreList;
    }

    [System.Serializable]
    public class Score
    {
        public int Rank;
        public string PlayerName;
        public int Points;
    }

    // Returns true if it ios a new HighScore
    public bool CreateNewScore(int points)
    {
        int scoreRank = 1; //Best rank is 1 (corresponds to index 0 in the score list)
        for (int i=0; i< HighScores.Count; i++)
        {
            if (points >= HighScores[i].Points)
                break;
            else
                scoreRank++;
        }

        if (scoreRank > NB_MAX_HIGHSCORES)
        {
            return false;
        }
        else
        {
            isListModified = true;

            NewHighScore = new Score();
            NewHighScore.Points = points;
            NewHighScore.PlayerName = CurrentPlayerName;
            NewHighScore.Rank = scoreRank;
            HighScores.Insert(scoreRank-1, NewHighScore);
            // Update the rank of the lower highscores
            for (int i = scoreRank; i < HighScores.Count; i++)
            {
                HighScores[i].Rank++;
            }

            // Delete the lowest highscore if we now have more than the max number of scores.
            if (HighScores.Count > NB_MAX_HIGHSCORES)
                HighScores.RemoveAt(HighScores.Count - 1);

            SceneManager.LoadScene("Menu");
            return true;
        }
    }

    public void ValidateNewScore(string newPlayerName)
    {
        CurrentPlayerName = newPlayerName;
        NewHighScore.PlayerName = CurrentPlayerName; // Updates the score in the list
        NewHighScore = null; // Indicates there is no new score to complete anymore (previous one is still referenced in the list)
    }

    private void LoadScores()
    {
        if (File.Exists(saveFilePath))
        {
            string json = File.ReadAllText(saveFilePath);
            Scores scores = JsonUtility.FromJson<Scores>(json);
            if ((scores != null) && (scores.scoreList != null))
                HighScores = scores.scoreList;
            else
            {
                HighScores = new List<Score>();
            }
        }
    }

    public void SaveScores()
    {
        if (isListModified)
        {
            Scores scores = new Scores();
            scores.scoreList = HighScores;
            string jsonString = JsonUtility.ToJson(scores);
            File.WriteAllText(saveFilePath, jsonString);
            isListModified = false;
            Debug.Log("Scores are saved");
        }
        else
            Debug.Log("No need to save scores");
    }
}
