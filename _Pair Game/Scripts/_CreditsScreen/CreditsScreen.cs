using UnityEngine;

public class CreditsScreen : ScreenBase
{
    public void OnClick_Back()
    {
        ChangeScreen(Screen.Home);
    }

    public void OnClick_OpenDeveloperURL()
    {
        Application.OpenURL("http://www.glenhunter.co.uk");
    }

    public void OnClick_OpenFreePikURL()
    {
        Application.OpenURL("https://www.flaticon.com/authors/freepik");
    }

    public void OnClick_OpenFlatIconURL()
    {
        Application.OpenURL("https://www.flaticon.com");
    }
}
