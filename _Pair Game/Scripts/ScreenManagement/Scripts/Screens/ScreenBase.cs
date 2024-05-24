using UnityEngine;

public class ScreenBase : MonoBehaviour
{
    protected void ChangeScreen(Screen newState)
    {
        ScreenManager.Instance.ChangeScreen(newState);
    }
}
