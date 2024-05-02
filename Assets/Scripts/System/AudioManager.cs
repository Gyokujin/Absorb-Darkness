using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SystemSound
{
    GameSystem, Click, Interact1, Interact2, PickUp
}

public enum PlayerActionSound
{
    Attack1, Attack2, Rolling, Backstep
}

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance = null;

    [Header("System")]
    public AudioClip[] systemClips;
    [SerializeField]
    private float systemVolume;
    [SerializeField]
    private int systemCh;
    private int systemIndex;
    private AudioSource[] systemAudios;

    [Header("Action")]
    public AudioClip[] actionClips;
    [SerializeField]
    private float actionVolume;
    [SerializeField]
    private int actionCh;
    private int actionIndex;
    private AudioSource[] actionAudios;

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
        actionAudios = new AudioSource[actionCh];

        for (int i = 0; i < actionCh; i++)
        {
            actionAudios[i] = actionAudio.AddComponent<AudioSource>();
            actionAudios[i].playOnAwake = false;
            actionAudios[i].volume = actionVolume;
        }
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

    public void PlayActionSFX(AudioClip audioClip)
    {
        for (int i = 0; i < actionAudios.Length; i++)
        {
            int loopIndex = (i + actionIndex) % actionAudios.Length;

            if (actionAudios[loopIndex].isPlaying)
                continue;

            actionIndex = loopIndex;
            actionAudios[loopIndex].clip = audioClip;
            actionAudios[loopIndex].Play();
            break;
        }
    }
}