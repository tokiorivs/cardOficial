using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Game_TextShowDisplayCardName : MonoBehaviour
{
    [SerializeField] private Text m_Text;
    [SerializeField] float m_FadeSpeed = 1f;

    private bool m_ShowingText = false;
    private bool m_Animate = false;

    private Color32 m_OriginalColor;
    private Color32 m_FadedColor;
    private Color32 m_TargetColor;

    private void OnEnable()
    {
        m_OriginalColor = m_Text.color;
        m_FadedColor = new Color32(m_OriginalColor.r, m_OriginalColor.g, m_OriginalColor.b, 0);

        m_TargetColor = m_FadedColor;

        m_Text.color = m_FadedColor;
    }

    private void Update()
    {
        if(m_Animate)
        {
            m_Text.color = Color.Lerp(m_Text.color, m_TargetColor, m_FadeSpeed);

            if (m_ShowingText && ColorIsEqual(m_Text.color, m_OriginalColor))
            {
                StartCoroutine(Delay(.5f, () => { Hide(); }));
            }

            if(!m_ShowingText && ColorIsEqual(m_Text.color, m_FadedColor))
            {
                m_Animate = false;
            }
        }
    }

    /// <summary>
    /// Generally accesed from external script such as gamemanager.cs
    /// </summary>
    /// <param name="text"></param>
    public void Show(string text)
    {
        m_Animate = true;
        m_ShowingText = true;
        m_TargetColor = m_OriginalColor;

        StopAllCoroutines();
        m_Text.color = m_FadedColor;
        m_Text.text = text.Replace("-1", "").ToUpper();
    }

    private void Hide()
    {
        m_ShowingText = false;
        m_TargetColor = m_FadedColor;
    }

    private bool ColorIsEqual(Color32 color1, Color32 color2)
    {
        if(
            color1.r != color2.r ||
            color1.g != color2.g ||
            color1.b != color2.b ||
            color1.a != color2.a)
        {
            return false;
        }

        return true;
    }

    private IEnumerator Delay(float seconds, UnityAction complete)
    {
        yield return new WaitForSeconds(seconds);
        complete();
    }
}
