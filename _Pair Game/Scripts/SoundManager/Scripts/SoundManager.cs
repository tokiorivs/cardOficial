using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Sounds
{
    CardPlaced,
    Ding,
    Buzz,
}

public enum BackgroundMusic
{
    HappyMusic,
}

/// <summary>
/// Dictionary of sounds that can be played
/// </summary>
public class SoundManager : Singleton<SoundManager>
{
    [SerializeField] AudioSource m_Music_AudioSource;
    [SerializeField] AudioSource m_Sound_AudioSource;

    [SerializeField] MusicDictionary m_MusicDictionary;
    [SerializeField] SoundDictionary m_SoundDictionary;

    private void Start()
    {
        PlayBackgroundMusic(BackgroundMusic.HappyMusic);
    }

    public void Play_CardPlaced()
    {
        PlaySound(Sounds.CardPlaced, 1f);
    }

    public void Play_CardsMatched()
    {
         StartCoroutine(Delay(.025f, () =>
         {
             PlaySound(Sounds.Ding);
         }));
    }

    private IEnumerator Delay(float seconds, System.Action callback)
    {
        yield return new WaitForSeconds(seconds);
        callback();
    }

    private void PlaySound(Sounds sound, float volume = 1)
    {
        if (!m_SoundDictionary.ContainsKey(sound))
        {
            Debug.LogError("m_SoundDictionary dictionary, does not contain 'KEY' for Sounds." + sound);
            return;
        }

        if (!m_SoundDictionary[sound])
        {
            Debug.LogError("m_SoundDictionary dictionary, does not contain a 'VALUE' for Sounds." + sound);
            return;
        }

        m_Sound_AudioSource.PlayOneShot(m_SoundDictionary[sound], volume);
    }

    private void PlayBackgroundMusic(BackgroundMusic bgMusic)
    {
        if (!m_MusicDictionary.ContainsKey(bgMusic))
        {
            Debug.LogError("m_MusicDictionary dictionary, does not contain 'KEY' for BackgroundMusic." + bgMusic);
            return;
        }

        if (!m_MusicDictionary[bgMusic])
        {
            Debug.LogError("m_MusicDictionary dictionary, does not contain a 'VALUE' for BackgroundMusic." + bgMusic);
            return;
        }

        m_Music_AudioSource.clip = m_MusicDictionary[bgMusic];
        m_Music_AudioSource.Play();
    }
}
