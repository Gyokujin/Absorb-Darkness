using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossStatus : EnemyStatus
{
    public override void TakeDamage(int damage, CharacterStatus player)
    {
        if (enemyManager.onDie)
            return;

        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            // DieProcess();
        }
        else
        {
            if (enemyManager.enemyType != EnemyType.Boss)
            {
                StopCoroutine("DamageProcess");
                enemyManager.onDamage = true;
                damageAmount += damage;

                if (damageAmount >= (float)maxHealth * 0.6f) // 한번에 최대 체력의 절반 이상의 피해가 들어오면 스매시를 실행
                {
                    StartCoroutine("KnockbackProcess", player);
                }
                else
                {
                    StartCoroutine("DamageProcess", player);
                }
            }
        }
    }
}