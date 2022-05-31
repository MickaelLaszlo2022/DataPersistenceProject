using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainManager : MonoBehaviour
{
    public Brick BrickPrefab;
    public int LineCount = 6;
    public Rigidbody Ball;

    public Text ScoreText;
    public GameObject GameOverText;
    public Text BestScoreText;
    
    private bool m_Started = false;
    private int m_Points;
    
    private bool m_GameOver = false;

    
    // Start is called before the first frame update
    void Start()
    {
        const float step = 0.6f;
        int perLine = Mathf.FloorToInt(4.0f / step);
        
        int[] pointCountArray = new [] {1,1,2,2,5,5};
        for (int i = 0; i < LineCount; ++i)
        {
            for (int x = 0; x < perLine; ++x)
            {
                Vector3 position = new Vector3(-1.5f + step * x, 2.5f + i * 0.3f, 0);
                var brick = Instantiate(BrickPrefab, position, Quaternion.identity);
                brick.PointValue = pointCountArray[i];
                brick.onDestroyed.AddListener(AddPoint);
            }
        }

        BestScoreText.text = "Best score : ";
        if (ScoreManager.Manager.HighScores.Count > 0)
            BestScoreText.text += ScoreManager.Manager.HighScores[0].Points + " (" + ScoreManager.Manager.HighScores[0].PlayerName + ")";
        else
            BestScoreText.text += " None";

        // Just to update the initial display of the player's name
        AddPoint(0);
    }

    private void Update()
    {
        if (!m_Started)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                m_Started = true;
                float randomDirection = Random.Range(-1.0f, 1.0f);
                Vector3 forceDir = new Vector3(randomDirection, 1, 0);
                forceDir.Normalize();

                Ball.transform.SetParent(null);
                Ball.AddForce(forceDir * 2.0f, ForceMode.VelocityChange);
            }
        }
        else if (m_GameOver)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }

        // Escape to back to Menu" works even in ongoing game
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene("Menu");
        }
    }

    void AddPoint(int point)
    {
        m_Points += point;
        ScoreText.text = $"Score : {m_Points}";
        if ((ScoreManager.Manager.CurrentPlayerName != null) && (ScoreManager.Manager.CurrentPlayerName != ""))
        {
            ScoreText.text += " (" + ScoreManager.Manager.CurrentPlayerName + ")";
        }
    }

    public void GameOver()
    {
        // Load menu scene to entry player's name if it is a new Highscore, otherwise, stay here in "game over" state.
        if (!ScoreManager.Manager.CreateNewScore(m_Points))
        {
            m_GameOver = true;
            GameOverText.SetActive(true);
        }
    }
}
