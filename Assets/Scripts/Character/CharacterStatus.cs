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
    public int currentHealth;

    [Header("Stamina")]
    public int staminaLevel = 10;
    public int staminaLevelAmount = 10;
    public int maxStamina;
    public int currentStamina;
}