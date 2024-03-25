using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Weapon", menuName = "Items/Weapon Item")]
public class WeaponItem : Item
{
    public GameObject modelPrefab;
    [SerializeField]
    private bool isUnarmed;

    [Header("Idle Animations")]
    public string right_Hand_Idle;
    public string left_Hand_Idle;

    [Header("Attack Animations")]
    public string OneHand_LightAttack1;
    public string OneHand_LightAttack2;
    public string OneHand_HeavyAttack1;

    [Header("Stamina Costs")]
    public int baseStamina;
    public float lightAttackMultiplier;
    public float heavyAttackMultiplier;
}