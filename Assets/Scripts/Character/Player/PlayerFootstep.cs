using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFootstep : MonoBehaviour
{
    [Header("Audios")]
    [SerializeField]
    private AudioSource moveAudio;
    [SerializeField]
    private AudioSource splintAudio;

    public void PlayMoveSFX()
    {
        if (!moveAudio.isPlaying)
        {
            moveAudio.Play();
        }
    }

    public void PlaySplintSFX()
    {
        if (!splintAudio.isPlaying)
        {
            splintAudio.Play();
        }
    }
}