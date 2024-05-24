using UnityEngine;

public class ScreenManager : Singleton<ScreenManager>
{
    
    [SerializeField] ScreenObjectDictionary m_Screens = null;

    [SerializeField] Transform m_CanvasParent;

    [SerializeField] Screen m_StartupScreen = Screen.Home;

    private GameObject m_CurrentScreen;
  

    private void Start()
    {
        // Setup screen
        ChangeScreen(m_StartupScreen);
    }

    // Reset is called when the user hits the Reset button in the Inspector's context menu or when adding the component the first time.
    // This function is only called in editor mode. Reset is most commonly used to give good default values in the Inspector.
    private void Reset()
    {
        m_Screens = new ScreenObjectDictionary();
    }

    public void ChangeScreen(Screen newScreen)
    {
        if(m_CurrentScreen != null)
        {
            Destroy(m_CurrentScreen);
        }

        if(!m_Screens.ContainsKey(newScreen))
        {
            Debug.LogError("Could not find "+ newScreen.ToString() + " screen key/enum within Screen Manager inspector window");
            return;
        }

        if(m_Screens[newScreen] == null)
        {
            Debug.LogError("Could not find prefab for " + newScreen.ToString() + " screen you can assign the empty field through the Screen Manager inspector window");
            return;
        }

        m_CurrentScreen = Instantiate(m_Screens[newScreen], m_CanvasParent, false);
    }
}
