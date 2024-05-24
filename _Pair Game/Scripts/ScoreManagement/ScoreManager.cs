using UnityEngine;
using System.Linq;
using System.Collections.Generic;

/// <summary>
/// Simple score manager that records users top 3 scores
/// </summary>
public class ScoreManager : Singleton<ScoreManager>
{
    const string m_Label = "scoreData";

    /// <summary>
    /// int tries, float time
    /// </summary>
    private GridScoreDictionary m_ScoresDictionary;

    /// <summary>
    /// Mainly used to show total time in seconds of the last game session 
    /// </summary>
    public float? Time { get; private set; } = null;

    /// <summary>
    /// Mainly used to show total tries count from the last game session
    /// </summary>
    public int? Tries { get; private set; } = null;

    private void Start()
    {
        LoadScores();
    }

    public void UserScored(Vector2 _gridSize, int _triesCount, float _time)
    {
        Time = _time;
        Tries = _triesCount;

        if(m_ScoresDictionary == null)
        {
            m_ScoresDictionary = new GridScoreDictionary();
        }

        if(!m_ScoresDictionary.ContainsKey(_gridSize))
        {
            m_ScoresDictionary.Add(_gridSize, new ScoreDictionary
            {
                {_triesCount, _time }
            });
        }
        else if(m_ScoresDictionary[_gridSize].ContainsKey(_triesCount))
        {
            if(_time < m_ScoresDictionary[_gridSize][_triesCount])
            {
                m_ScoresDictionary[_gridSize][_triesCount] = _time;
            }
        }
        else
        {
            m_ScoresDictionary[_gridSize].Add(_triesCount, _time);
        }

        SaveScore();
    }

    public IEnumerable<KeyValuePair<int, float>> GetTopFiveScores(Vector2 _gridSize)
    {

        var scoresDictionary = from pair in m_ScoresDictionary[_gridSize]
                                       orderby pair.Key ascending
                                       select pair;

        return scoresDictionary.Take(5);
    }

    private void LoadScores()
    {
        if(PlayerPrefs.HasKey(m_Label))
        {
            m_ScoresDictionary = JsonUtility.FromJson<GridScoreDictionary>(PlayerPrefs.GetString(m_Label));
        }
    }

    private void SaveScore()
    {
        string json = JsonUtility.ToJson(m_ScoresDictionary);

        PlayerPrefs.SetString(m_Label, json);
    }
}
