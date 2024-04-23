using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CharacterSound 
{
    Detect, Attack, Hit, Die
}

[RequireComponent(typeof(AudioSource))]
public class CharacterAudio : MonoBehaviour
{
    [Header("Audio")]
    [SerializeField]
    [Range(0, 1)]
    private float audioVolume;
    [SerializeField]
    private int enemyCh;
    private int enemyIndex;

    [Header("Sound Clip")]
    public AudioClip[] audioClips;
    private AudioSource[] enemyAudios;

    void Awake()
    {
        Init();
    }

    void Init()
    {
        GameObject enemyAudio = new GameObject("EnemyAudio");
        enemyAudio.transform.parent = transform;
        enemyAudios = new AudioSource[enemyCh];

        for (int k = 0; k < enemyCh; k++)
        {
            enemyAudios[k] = enemyAudio.AddComponent<AudioSource>();
            enemyAudios[k].playOnAwake = false;
            enemyAudios[k].volume = audioVolume;
        }
    }

    public void PlaySFX(AudioClip audioClip)
    {
        for (int i = 0; i < enemyAudios.Length; i++)
        {
            int loopIndex = (i + enemyIndex) % enemyAudios.Length;

            if (enemyAudios[loopIndex].isPlaying)
                continue;

            enemyIndex = loopIndex;
            enemyAudios[loopIndex].clip = audioClip;
            enemyAudios[loopIndex].Play();
            break;
        }
    }
}