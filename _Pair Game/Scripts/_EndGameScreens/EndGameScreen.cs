using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class EndGameScreen : ScreenBase
{
    [SerializeField] Text m_EndGameMessage_Text;
    [SerializeField] GameObject m_ScoresPrefab;
    [SerializeField] Transform m_ScoresPrefab_Parent;

    private void OnEnable()
    {
        float? totalTimeInSeconds = ScoreManager.Instance.Time;
        int? triesCount = ScoreManager.Instance.Tries;

        if(totalTimeInSeconds == null)
        {
            Debug.LogError("totalTimeInSeconds is null, GameManager.cs should be calling ScoreManager.Instance.UserScored");
            return;
        }

        if(triesCount == null)
        {
            Debug.LogError("triesCount is null, GameManager.cs should be calling ScoreManager.Instance.UserScored");
            return;
        }

        // TIME TEXT
        int minutes = (int)Mathf.Floor((float)totalTimeInSeconds / 60);
        string seconds = Mathf.Floor((float)totalTimeInSeconds % 60).ToString("00");


        SetupBestScores();

        if(minutes < 1)
        {
            m_EndGameMessage_Text.text = string.Format("THIS GAME TOOK YOU <color=#19AD78>{0} TRIES</color> & <color=#19AD78>{1} SECONDS</color> TO COMPLETE", triesCount, seconds);
        }
        else if(minutes == 1)
        {
            m_EndGameMessage_Text.text = string.Format("THIS GAME TOOK YOU <color=#19AD78>{0} TRIES</color>, <color=#19AD78>{1} MINUTE</color> & <color=#19AD78>{2} SECONDS</color> TO COMPLETE", triesCount, minutes, seconds);
        }
        else
        {
            m_EndGameMessage_Text.text = string.Format("THIS GAME TOOK YOU <color=#19AD78>{0} TRIES</color>, <color=#19AD78>{1} MINUTES</color> & <color=#19AD78>{2} SECONDS</color> TO COMPLETE", triesCount, minutes, seconds);
        }
        
    }

    private void SetupBestScores()
    {
        IEnumerable<KeyValuePair<int, float>> top5Scores = ScoreManager.Instance.GetTopFiveScores(GridConfigManager.Instance.SelectedGridSize);

        foreach(KeyValuePair<int, float> score in top5Scores)
        {
            GameObject scorePrefab = Instantiate(m_ScoresPrefab, m_ScoresPrefab_Parent, false);
            scorePrefab.GetComponent<EndGame_BestScoresPrefab>().Setup(score.Key, score.Value);
        }
    }

    public void OnClick_Retry()
    {
        ChangeScreen(Screen.Game);
    }

    public void OnClick_Continue()
    {
        ChangeScreen(Screen.Home);
    }
}
