using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterManager : MonoBehaviour
{
    [HideInInspector]
    public bool onDamage;
    [HideInInspector]
    public bool onDie;

    [Header("LockOn")]
    public Transform lockOnTransform;
}