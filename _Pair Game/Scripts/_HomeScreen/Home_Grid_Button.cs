using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Home_Grid_Button : MonoBehaviour
{
    [SerializeField] Color32 m_PrimaryColor;
    [SerializeField] Color32 m_Text_Unlocked_Color;

    [SerializeField] Sprite m_Unlocked;
    [SerializeField] Sprite m_Locked;

    [SerializeField] Image m_Button_Image;

    [SerializeField] Button m_Button;
    [SerializeField] Text m_Text;

    [SerializeField] byte m_CompleteFade = 50;

    public void Setup(int _row, int _column, bool _isCurrentLevel, bool _locked, UnityAction _onClickListener)
    {
        m_Button_Image.color = m_PrimaryColor;

        m_Text.text = _row + "x" + _column;

        // THIS IS ALL JUST VISUALS
        // Show locked icon setup
        if (_locked)
        {
            Setup_VisualCard_Locked(m_PrimaryColor);
        }
        // Show icon unlocked but... current level icon
        else if(_isCurrentLevel)
        {
            Setup_VisualCard_NotLocked_ButIsCurrentLevel();
           
        }
        // Show unlocked icon card but not current level
        else
        {
            Setup_VisualCard_NotLocked_NotCurrentLevel(m_PrimaryColor);
        }

        // Setup onclicks...
        m_Button.onClick.RemoveAllListeners();
        m_Button.onClick.AddListener(_onClickListener);


    }

    private void Setup_VisualCard_Locked(Color32 textColor)
    {
        m_Button_Image.sprite = m_Locked;
        m_Text.color = textColor;
        m_Button.interactable = false;
    }

    private void Setup_VisualCard_NotLocked_ButIsCurrentLevel()
    {
        m_Button_Image.sprite = null;
        m_Text.color = m_Text_Unlocked_Color;
    }

    private void Setup_VisualCard_NotLocked_NotCurrentLevel(Color32 randomColor)
    {
        m_Button_Image.sprite = null;

        Color32 fadedBGColor = randomColor;
        fadedBGColor.a = m_CompleteFade;

        m_Button_Image.color = fadedBGColor;
        m_Text.color = m_Text_Unlocked_Color;
    }
}
