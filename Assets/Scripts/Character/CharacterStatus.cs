using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStatus : MonoBehaviour
{
    [Header("Speed")]
    public float walkSpeed = 3;
    public float runSpeed = 5;
    public float rotationSpeed = 10;

    [Header("Health")]
    public int healthLevel = 10;
    public int healthLevelAmount = 10;
    public int maxHealth;

    private int currentHealth;
    [HideInInspector]
    public int CurrentHealth
    {
        get
        {
            return currentHealth;
        }
        set
        {
            if (value > 0)
                currentHealth = value;
            else
                currentHealth = 0;
        }
    }

    [Header("Effect")]
    [SerializeField]
    protected Transform effectTransform;
}