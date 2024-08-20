using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HudUI : MonoBehaviour
{
    [Header("UI")]
    [SerializeField]
    private Slider healthBar;
    [SerializeField]
    private Slider staminaBar;

    public void SetMaxHealth(int maxHealth)
    {
        healthBar.maxValue = maxHealth;
        healthBar.value = maxHealth;
    }

    public void SetCurrentHealth(int curHealth)
    {
        healthBar.value = curHealth;
    }

    public void SetMaxStamina(float maxStamina)
    {
        staminaBar.maxValue = maxStamina;
        staminaBar.value = maxStamina;
    }

    public void SetCurrentStamina(float curStamina)
    {
        staminaBar.value = curStamina;
    }
}
