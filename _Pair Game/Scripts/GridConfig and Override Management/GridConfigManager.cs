using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class Config
{
    public bool IsLocked = true;
}

/// <summary>
/// Definitions of different grids are defined here, this is
/// data is used on the homescreen and is used throughout as 'levels'
/// dictionary/levels is sequential
/// </summary>
public class GridConfigManager : Singleton<GridConfigManager>
{
    public GridConfigDictionary GridConfigurations;
   
    [Header("Selected")]
    public Vector2 SelectedGridSize = new Vector2(3, 2);
}
