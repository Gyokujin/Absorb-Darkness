using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance = null;

    public enum GameBGM
    {
        Stage0Field, Stage0Boss
    }

    public enum SystemSound
    {
        GameSystem, Click, Interact1, Interact2, PickUp, FogEntrance
    }

    [Header("BGM")]
    public AudioClip[] bgmClips;
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

    [Header("Player Action")]
    public AudioClip[] playerActionClips;
    [SerializeField]
    [Range(0, 1)]
    private float playerActionVolume;
    [SerializeField]
    private int playerActionCh;
    private int playerActionIndex;
    private AudioSource[] playerActionAudios;

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
        GameObject systemAudio = new GameObject("System Audio");
        systemAudio.transform.parent = transform;
        systemAudios = new AudioSource[systemCh];

        for (int i = 0; i < systemCh; i++)
        {
            systemAudios[i] = systemAudio.AddComponent<AudioSource>();
            systemAudios[i].playOnAwake = false;
            systemAudios[i].volume = systemVolume;
        }

        // Action
        GameObject actionAudio = new GameObject("Action Audio");
        actionAudio.transform.parent = transform;
        playerActionAudios = new AudioSource[playerActionCh];

        for (int i = 0; i < playerActionCh; i++)
        {
            playerActionAudios[i] = actionAudio.AddComponent<AudioSource>();
            playerActionAudios[i].playOnAwake = false;
            playerActionAudios[i].volume = playerActionVolume;
        }
    }

    public void PlayBGM(AudioClip audioClip)
    {
        bgmAudio.Stop();
        bgmAudio.clip = audioClip;
        bgmAudio.Play();
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

    public void PlayPlayerActionSFX(AudioClip audioClip)
    {
        for (int i = 0; i < playerActionAudios.Length; i++)
        {
            int loopIndex = (i + playerActionIndex) % playerActionAudios.Length;

            if (playerActionAudios[loopIndex].isPlaying)
                continue;

            playerActionIndex = loopIndex;
            playerActionAudios[loopIndex].clip = audioClip;
            playerActionAudios[loopIndex].Play();
            break;
        }
    }
}