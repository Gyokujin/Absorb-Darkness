using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance = null;

    public enum SystemSound
    {
        DoorOpen, FogEntrance, EnemyDieGlow, EnemyDieFlash, EnemyDissolve
    }

    public enum UISound
    {
        GameUI, Click, Interact1, Interact2, Pickup, Victory, FieldUI
    }

    [Header("BGM")]
    [SerializeField]
    [Range(0, 1)]
    private float bgmVolume;
    private AudioSource bgmAudio;

    [Header("System")]
    public AudioClip[] systemClips;
    [SerializeField]
    [Range(0, 1)]
    private float systemVolume;
    [SerializeField]
    private int systemCh;
    private int systemIndex;
    private AudioSource[] systemAudios;

    [Header("UI")]
    public AudioClip[] uiClips;
    [SerializeField]
    [Range(0, 1)]
    private float uiVolume;
    [SerializeField]
    private int uiCh;
    private int uiIndex;
    private AudioSource[] uiAudios;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        Init();
    }

    void Init()
    {
        // BGM
        bgmAudio = new GameObject("Bgm Audio").AddComponent<AudioSource>();
        bgmAudio.transform.parent = transform;
        bgmAudio.playOnAwake = true;
        bgmAudio.loop = true;
        bgmAudio.volume = bgmVolume;

        // System
        GameObject systemAudio = new("System Audio");
        systemAudio.transform.parent = transform;
        systemAudios = new AudioSource[systemCh];

        for (int i = 0; i < systemCh; i++)
        {
            systemAudios[i] = systemAudio.AddComponent<AudioSource>();
            systemAudios[i].playOnAwake = false;
            systemAudios[i].volume = systemVolume;
        }

        // UI
        GameObject uiAudio = new("UI Audio");
        uiAudio.transform.parent = transform;
        uiAudios = new AudioSource[uiCh];

        for (int i = 0; i < uiCh; i++)
        {
            uiAudios[i] = uiAudio.AddComponent<AudioSource>();
            uiAudios[i].playOnAwake = false;
            uiAudios[i].volume = uiVolume;
        }
    }

    public void PlayBGM(AudioClip audioClip)
    {
        bgmAudio.Stop();
        bgmAudio.clip = audioClip;
        bgmAudio.Play();
    }

    public void MuteBGM()
    {
        bgmAudio.Stop();
        bgmAudio.clip = null;
    }

    public void PlaySystemSFX(AudioClip audioClip)
    {
        for (int i = 0; i < systemAudios.Length; i++)
        {
            int loopIndex = (i + systemIndex) % systemAudios.Length;

            if (systemAudios[loopIndex].isPlaying)
                continue;

            systemIndex = loopIndex;
            systemAudios[loopIndex].clip = audioClip;
            systemAudios[loopIndex].Play();
            break;
        }
    }

    public void PlayUISFX(AudioClip audioClip)
    {
        for (int i = 0; i < uiAudios.Length; i++)
        {
            int loopIndex = (i + uiIndex) % uiAudios.Length;

            if (uiAudios[loopIndex].isPlaying)
                continue;

            uiIndex = loopIndex;
            uiAudios[loopIndex].clip = audioClip;
            uiAudios[loopIndex].Play();
            break;
        }
    }
}