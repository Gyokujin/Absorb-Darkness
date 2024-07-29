using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAudio : CharacterAudio
{
    private bool onFootstepPlaying;
    private bool onSprintPlaying;

    private AudioSource footstepAudio;
    private AudioSource sprintAudio;

    public enum PlayerSound
    {
        Footstep, Sprint, Rolling, Backstep, Attack1, Attack2, Drink, Recovery
    }

    public AudioClip[] playerClips;

    public void PlayFootstepSFX()
    {
        if (!onFootstepPlaying)
        {
            onFootstepPlaying = true;
            int loofIndex = AudioAllocation();

            footstepAudio = characterAudios[loofIndex];
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
            int loofIndex = AudioAllocation();

            sprintAudio = characterAudios[loofIndex];
            sprintAudio.clip = playerClips[(int)PlayerSound.Sprint];
            // sprintAudio.loop = true;
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