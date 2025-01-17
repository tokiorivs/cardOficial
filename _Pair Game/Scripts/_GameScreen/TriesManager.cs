﻿using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Counter for the number times a user does a try
/// </summary>
public class TriesManager : MonoBehaviour
{
    [SerializeField] Text m_TriesCountText;

    /// <summary>
    /// The amount of tries user has made 
    /// </summary>
    public int Tries { get; private set; } = 0;

    /// <summary>
    /// Call this method, to increment tries counter
    /// </summary>
    public void UserTried()
    {
        Tries++;

        UpdateTriesUICounter();
    }

    public void Reset()
    {
        Tries = 0;

        UpdateTriesUICounter();
    }

    private void UpdateTriesUICounter()
    {
        m_TriesCountText.text = "TRIES: " + Tries.ToString();
    }
}
