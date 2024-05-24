using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Timer, which is used for counting the duration of a game session 
/// </summary>
public class TimerManager : MonoBehaviour
{
    [SerializeField] Text m_TimerText;

    public bool IsPaused = false;

    /// <summary>
    /// Total time elasped from start to finish of a current game sessions
    /// </summary>
    public float TotalTimeInSeconds { get; private set; } = 0;

    private bool m_Count = false;
   

    private void Update()
    {
        if(m_Count && !IsPaused)
        {
            TotalTimeInSeconds += Time.deltaTime;
            UpdateTimer(TotalTimeInSeconds);
        }
    }

    public void StartTimer()
    {
        m_Count = true;
    }

    public void StopTimer()
    {
        m_Count = false;
    }

    public void ResetTimer()
    {
        TotalTimeInSeconds = 0;
    }

    private void UpdateTimer(float totalSeconds)
    {
        string minutes = Mathf.Floor(totalSeconds / 60).ToString();
        string seconds = Mathf.Floor(totalSeconds % 60).ToString("00");

        m_TimerText.text = "TIME: " + string.Format("{0}:{1}", minutes, seconds);
    }
}
