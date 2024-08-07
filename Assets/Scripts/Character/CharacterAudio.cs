using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAudio : MonoBehaviour
{
    public enum CharacterSound
    {
        Hit, Die
    }

    [Header("Audio")]
    [SerializeField][Range(0, 1)]
    private float audioVolume;
    [SerializeField]
    private int characterCh;
    private int characterIndex;

    [Header("Sound Clip")]
    public AudioClip[] characterClips;
    protected AudioSource[] characterAudios;

    void Awake()
    {
        Init();
    }

    void Init()
    {
        GameObject characterAudio = new ("Character Audio");
        characterAudio.transform.parent = transform;
        characterAudio.transform.localPosition = Vector3.zero;
        characterAudios = new AudioSource[characterCh];

        for (int i = 0; i < characterCh; i++)
        {
            characterAudios[i] = characterAudio.AddComponent<AudioSource>();
            characterAudios[i].playOnAwake = false;
            characterAudios[i].volume = audioVolume;
        }
    }

    public void PlaySFX(AudioClip audioClip)
    {
        int loopIndex = AudioAllocation();
        characterAudios[loopIndex].clip = audioClip;
        characterAudios[loopIndex].loop = false;
        characterAudios[loopIndex].Play();
    }

    protected int AudioAllocation()
    {
        for (int i = 0; i < characterAudios.Length; i++)
        {
            int loopIndex = (i + characterIndex) % characterAudios.Length;

            if (characterAudios[loopIndex].isPlaying)
                continue;

            characterIndex = loopIndex;
            break;
        }

        return characterIndex;
    }
}