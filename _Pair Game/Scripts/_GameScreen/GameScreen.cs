using UnityEngine;

public class GameScreen : ScreenBase
{
    [SerializeField] GameObject m_AreYouSureYouWantToQuit_Modal;

    public void OnClick_Resume()
    {
        m_AreYouSureYouWantToQuit_Modal.SetActive(false);
        GameManager.Instance.IsPaused = false;
    }

    public void OnClick_Quit()
    {
        GameManager.Instance.IsPaused = true;

        if (m_AreYouSureYouWantToQuit_Modal.activeInHierarchy)
            return;

        if(GameManager.Instance == null)
        {
            ChangeScreen(Screen.Home);
            return;
        }

        if(GameManager.Instance.UserInteractedWithCard)
        {
            m_AreYouSureYouWantToQuit_Modal.SetActive(true);
            return;
        }

        ChangeScreen(Screen.Home);
    }

    public void OnClick_ForceQuit()
    {
        ChangeScreen(Screen.Home);
    }
}
