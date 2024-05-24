using UnityEngine;
using System.Linq;
using System.Collections.Generic;

/// <summary>
/// Responsible for unlocking/locking
/// </summary>
public class GridUnlockManager : Singleton<GridUnlockManager>
{
    private const string m_PrefsLabel = "GridUnlockData";

    public int CurrentLevelIndex { get; private set; }

    public bool DeleteData = false;

    public void Load_SavedData()
    {
        if (PlayerPrefs.HasKey(m_PrefsLabel))
        {
            GridConfigDictionary config = JsonUtility.FromJson<GridConfigDictionary>(PlayerPrefs.GetString(m_PrefsLabel));
            GridConfigManager.Instance.GridConfigurations = config;
        }

        CurrentLevelIndex = DetermineLevel(GridConfigManager.Instance.GridConfigurations);
    }

    public void CompletedCurrentLevel()
    {
        GridConfigDictionary dictionary = GridConfigManager.Instance.GridConfigurations;

        Vector2 currentGrid = GridConfigManager.Instance.SelectedGridSize;
        List<Vector2> gridKeys = dictionary.Keys.ToList();


        for (int i = 0; i < gridKeys.Count; i++)
        {
            if(gridKeys[i] == currentGrid)
            {
                if(i + 1 < gridKeys.Count && dictionary[gridKeys[i + 1]].IsLocked)
                {
                    dictionary[gridKeys[i + 1]].IsLocked = false;
                    break;
                }
            }
        }

        GridConfigManager.Instance.GridConfigurations = dictionary;

        SaveData();

    }

    private int DetermineLevel(GridConfigDictionary _dictionary)
    {
        int currentLevel = -1;

        foreach (KeyValuePair<Vector2, Config> gridConfig in _dictionary)
        {
            if (!gridConfig.Value.IsLocked)
                currentLevel++;
        }

        return currentLevel;
    }

    private void Update()
    {
        if(DeleteData)
        {
            DeleteData = false;
            PlayerPrefs.DeleteKey(m_PrefsLabel);
        }
    }

    private void SaveData()
    {
        string gridConfigJson = JsonUtility.ToJson(GridConfigManager.Instance.GridConfigurations);

        PlayerPrefs.SetString(m_PrefsLabel, gridConfigJson);
    }


}
