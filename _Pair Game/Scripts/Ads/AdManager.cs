using UnityEngine;
using UnityEngine.Advertisements;


public class AdManager : Singleton<AdManager>
{

    /// <summary>
    /// Used to record how many times an ad start, want to do this to limit
    /// the amount of ads shown at the end of every game
    /// </summary>
    private int m_RequestCount = 0;

    [SerializeField] public string m_GoogleGameID = "";
    [SerializeField] public string m_AppleGameID = "";

    private void Start()
    {

#if UNITY_ANDROID && UNITY_ADS
        if(string.IsNullOrEmpty(m_GoogleGameID))
        {
            Debug.Log("Google GameID is null or empty, please locate AdManager.cs ensure m_AppleGameID has the appropiate gameid");
            return;
        }

        Advertisement.Initialize(m_GoogleGameID);

#endif

#if UNITY_IOS && UNITY_ADS
        if (string.IsNullOrEmpty(m_AppleGameID))
        {
            Debug.Log("Apple GameID is null or empty, please locate AdManager.cs ensure m_AppleGameID has the appropiate gameid");
            return;
        }
        Advertisement.Initialize(m_AppleGameID);
#endif
    }

    public void ShowVideo()
    {
#if UNITY_ADS
        m_RequestCount++;

        // show ad every 2 times
        if(m_RequestCount % 2 != 0)
        {
            return;
        }

        string placementId = "video";

        if(Advertisement.IsReady(placementId))
        {
            Advertisement.Show(placementId);
        }
    
#else
        Debug.Log("It appears Unity Ads is not enabled/installed");
#endif
    }
}
