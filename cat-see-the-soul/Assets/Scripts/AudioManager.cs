using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    static AudioManager Instance;

    public AudioSource bgmAudioSource;
    public AudioSource hitAudioSource;
    public AudioSource healAudioSource;
    public AudioSource deadAudioSource;
    public AudioSource damagedAudioSource;
    public AudioSource victoryAudioSource;
    public AudioSource uiAudioSource;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            bgmAudioSource.Play();
            DontDestroyOnLoad(this);
        }
    }

    public static void PlayHit()
    {
        if (Instance == null)
            return;
        Instance.hitAudioSource.Play();
    }

    public static void PlayHeal()
    {
        if (Instance == null)
            return;
        Instance.healAudioSource.Play();
    }

    public static void PlayDead()
    {
        if (Instance == null)
            return;
        Instance.deadAudioSource.Play();
    }

    public static void PlayDamaged()
    {
        if (Instance == null)
            return;
        Instance.damagedAudioSource.Play();
    }

    public static void PlayVictory()
    {
        if (Instance == null)
            return;
        Instance.victoryAudioSource.Play();
    }

    public static void PlayUI()
    {
        if (Instance == null)
            return;
        Instance.uiAudioSource.Play();
    }
}
