using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAudio : CharacterAudio
{
    public enum EnemySound
    {
        Detect, Attack
    }

    public AudioClip[] enemyClips;
}