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
    public string th_idle;

    [Header("Attack Animations")]
    public string oneHand_LightAttack1;
    public string oneHand_LightAttack2;
    public string oneHand_HeavyAttack1;
    public string twoHand_LightAttack1;
    public string twoHand_LightAttack2;
    public string twoHand_HeavyAttack1;

    [Header("Stamina Costs")]
    public int baseStamina;
    public float lightAttackMultiplier;
    public float heavyAttackMultiplier;
}