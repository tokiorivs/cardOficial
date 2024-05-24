using UnityEngine;
using System.Collections.Generic;

public class HomeScreen : ScreenBase
{

    [SerializeField] Transform m_Grid_Button_Parent;
    [SerializeField] GameObject m_Grid_Button_Prefab;

    private void OnEnable()
    {
        GridUnlockManager.Instance.Load_SavedData();

        Setup_SelectionGrid();
    }

    private void Setup_SelectionGrid()
    {
        GridConfigDictionary gridLayoutDictionary = GridConfigManager.Instance.GridConfigurations;

        if(gridLayoutDictionary == null)
        {
            Debug.LogError("Could not find grid config dictionary");
            return;
        }

        int i = 0;

        foreach(KeyValuePair<Vector2, Config> grid in gridLayoutDictionary)
        {
            GameObject menuGridItem = Instantiate(m_Grid_Button_Prefab, m_Grid_Button_Parent, false);

            Home_Grid_Button gridButtonScript = menuGridItem.GetComponent<Home_Grid_Button>();
       
            gridButtonScript.Setup(
                (int)grid.Key.x,
                (int)grid.Key.y,
                (GridUnlockManager.Instance.CurrentLevelIndex == i) ? true : false,
                grid.Value.IsLocked,
                () =>
            {
                GridConfigManager.Instance.SelectedGridSize = new Vector2(grid.Key.x, grid.Key.y);

                AdManager.Instance.ShowVideo();

                ScreenManager.Instance.ChangeScreen(Screen.Game);
            });

            i++;
        }
    }

    /// <summary>
    /// Go to credits screen
    /// </summary>
    public void OnClick_Credits()
    {
        ChangeScreen(Screen.Credits);
    }

    public void OpenUrl(string privacyPolicyURL)
    {
        if(string.IsNullOrEmpty(privacyPolicyURL))
        {
            Debug.LogError("Privacy policy does not exist (privacyPolicyURL=nullorempty)");
            return;
        }

        Application.OpenURL(privacyPolicyURL);
    }
}
