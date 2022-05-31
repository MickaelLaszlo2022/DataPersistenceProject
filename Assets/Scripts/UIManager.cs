using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class UIManager : MonoBehaviour
{
    public TMP_Text hsName1;
    public TMP_Text hsName2;
    public TMP_Text hsName3;
    public TMP_Text hsName4;
    public TMP_Text hsName5;

    public TMP_Text hsScore1;
    public TMP_Text hsScore2;
    public TMP_Text hsScore3;
    public TMP_Text hsScore4;
    public TMP_Text hsScore5;

    public TMP_InputField hsNewPlayerName;

    // Start is called before the first frame update
    void Start()
    {
        //hsName3.SetText("Player 3 name");
        //hsScore3.SetText("000");

        UpdateScoreList();

        ActivateNewNameEntry();
    }

    private void UpdateScoreList()
    {
        for (int i = 0; i < ScoreManager.Manager.HighScores.Count; i++)
        {
            switch (i)
            {
                case 0: FillLine(ScoreManager.Manager.HighScores[i], hsName1, hsScore1); break;
                case 1: FillLine(ScoreManager.Manager.HighScores[i], hsName2, hsScore2); break;
                case 2: FillLine(ScoreManager.Manager.HighScores[i], hsName3, hsScore3); break;
                case 3: FillLine(ScoreManager.Manager.HighScores[i], hsName4, hsScore4); break;
                case 4: FillLine(ScoreManager.Manager.HighScores[i], hsName5, hsScore5); break;
                default: break;
            }
        }
    }

    private void FillLine(ScoreManager.Score score, TMP_Text hsName, TMP_Text hsScore)
    {
        hsName.text = score.PlayerName;
        hsScore.text = score.Points.ToString();
    }

    private void ActivateNewNameEntry()
    {
        if (ScoreManager.Manager.NewHighScore != null)
        {
            hsNewPlayerName.gameObject.SetActive(true);
            switch (ScoreManager.Manager.NewHighScore.Rank)
            {
                case 1: InitEntryComponent(hsName1); break;
                case 2: InitEntryComponent(hsName2); break;
                case 3: InitEntryComponent(hsName3); break;
                case 4: InitEntryComponent(hsName4); break;
                case 5: InitEntryComponent(hsName5); break;
                default: break;
            }
        }
        else
        {
            hsNewPlayerName.gameObject.SetActive(false);
        }
    }

    private void InitEntryComponent(TMP_Text nameComponent)
    {
        hsNewPlayerName.gameObject.GetComponent<RectTransform>().anchoredPosition = nameComponent.gameObject.GetComponent<RectTransform>().anchoredPosition;
        hsNewPlayerName.text = ScoreManager.Manager.CurrentPlayerName;
        EventSystem.current.SetSelectedGameObject(hsNewPlayerName.gameObject);
    }

    // Update is called once per frame
    //void Update()
    //{
    //hsName5.SetText(hsNewPlayerName.text);
    //}

    public void StartNewGame()
    {
        if (IsScreenValid())
        {
            if (hsNewPlayerName.gameObject.activeInHierarchy)
            {
                ScoreManager.Manager.ValidateNewScore(hsNewPlayerName.text);
            }
            SceneManager.LoadScene("Main");
        }
    }

    public void Exit()
    {
        if (IsScreenValid())
        {
            if (hsNewPlayerName.gameObject.activeInHierarchy)
            {
                ScoreManager.Manager.ValidateNewScore(hsNewPlayerName.text);
            }
            ScoreManager.Manager.SaveScores();
#if UNITY_EDITOR
            EditorApplication.ExitPlaymode();
#else
            Application.Quit();
#endif
        }
    }

    private bool IsScreenValid()
    {
        bool result = ((!hsNewPlayerName.gameObject.activeInHierarchy) || (hsNewPlayerName.text != ""));
        if (!result)
            Debug.Log("Please enter the player's name !");
        return result;
    }
}
