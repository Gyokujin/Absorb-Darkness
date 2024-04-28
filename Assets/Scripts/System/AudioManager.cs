using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerActionSound
{
    Attack1, Attack2
}

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance = null;

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
        GameObject actionAudio = new GameObject("Action Audio");
        actionAudio.transform.parent = transform;
        actionAudios = new AudioSource[actionCh];

        for (int k = 0; k < actionCh; k++)
        {
            actionAudios[k] = actionAudio.AddComponent<AudioSource>();
            actionAudios[k].playOnAwake = false;
            actionAudios[k].volume = actionVolume;
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