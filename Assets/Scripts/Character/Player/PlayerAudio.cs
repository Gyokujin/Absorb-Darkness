using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAudio : CharacterAudio
{
    public enum PlayerSound
    {
        Footstep, Sprint, Rolling, Backstep, Attack1, Attack2, Drink, Recovery
    }

    [Header("Parameter")]
    private bool onFootstepPlaying;
    private bool onSprintPlaying;

    [Header("Audio")]
    public AudioClip[] playerClips;
    private AudioSource footstepAudio;
    private AudioSource sprintAudio;

    public void PlayFootstepSFX()
    {
        if (!onFootstepPlaying)
        {
            onFootstepPlaying = true;
            footstepAudio = characterAudios[AudioAllocation()];
            footstepAudio.clip = playerClips[(int)PlayerSound.Footstep];
            footstepAudio.loop = true;
            footstepAudio.Play();
        }
    }

    public void StopFootstepSFX()
    {
        if (onFootstepPlaying)
        {
            onFootstepPlaying = false;
            footstepAudio.loop = false;
        }
    }

    public void PlaySprintSFX()
    {
        if (!onSprintPlaying)
        {
            onSprintPlaying = true;
            sprintAudio = characterAudios[AudioAllocation()];
            sprintAudio.clip = playerClips[(int)PlayerSound.Sprint];
            sprintAudio.Play();
        }
    }

    public void StopSprintSFX()
    {
        if (onSprintPlaying)
        {
            onSprintPlaying = false;
            sprintAudio.loop = false;
        }
    }
}