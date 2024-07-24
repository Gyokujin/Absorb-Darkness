using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAudio : CharacterAudio
{
    public enum PlayerSound
    {
        FootStep, Rolling, Backstep, Attack1, Attack2, Drink, Recovery
    }

    public AudioClip[] playerClips;
}