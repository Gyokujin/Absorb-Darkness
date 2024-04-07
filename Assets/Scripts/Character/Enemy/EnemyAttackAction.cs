using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Enemy AI/Enemy Actions/Attack Action")]
public class EnemyAttackAction : EnemyAction
{
    public int attackScore = 3;
    public float recoveryTime = 2;
    public float attackAngleMax = 35;
    public float attackAngleMin = -35;
    public float attackDisMin = 0;
    public float attackDisMax = 3;
}