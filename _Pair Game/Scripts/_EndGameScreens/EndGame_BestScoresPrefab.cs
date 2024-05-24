using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Prefab script that is responsible for showing scores previously earned on a particular grid
/// </summary>
public class EndGame_BestScoresPrefab : MonoBehaviour
{
    [SerializeField] Text m_TimeText;
    [SerializeField] Text m_TriesText;

    public void Setup(int triesCount, float timeInSeconds)
    {
        UpdateTimeText(timeInSeconds);
        UpdateTriesText(triesCount);
    }

    private void UpdateTimeText(float timeInSeconds)
    {
        // TIME TEXT
        string minutes = Mathf.Floor(timeInSeconds / 60).ToString();
        string seconds = Mathf.Floor(timeInSeconds % 60).ToString("00");

        m_TimeText.text = string.Format("TIME: {0}:{1}", minutes, seconds);
    }

    private void UpdateTriesText(float triesCount)
    {
        // TRIES TEXT
        m_TriesText.text = string.Format("TRIES: {0}", triesCount);
    }
}
