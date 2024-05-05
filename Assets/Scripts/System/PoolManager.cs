using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    public static PoolManager instance = null;

    public enum Enemy
    {
        Bat
    }

    public enum Effect
    {
        HitBlood
    }

    public enum EnemySpell
    {
        LightningImpact, ElectricShock, PosionMist, Summon, Meteor, MeteorExplosion
    }

    [Header("Enemy")]
    [SerializeField]
    private GameObject[] enemies;
    private List<GameObject>[] enemyPool;

    [Header("Effect")]
    [SerializeField]
    private GameObject[] effects;
    private List<GameObject>[] effectPool;

    [Header("Spell")]
    [SerializeField]
    private GameObject[] enemySpells;
    private List<GameObject>[] enemySpellPool;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }

        Init();
    }

    void Init()
    {
        enemyPool = new List<GameObject>[enemies.Length];
        effectPool = new List<GameObject>[effects.Length];
        enemySpellPool = new List<GameObject>[enemySpells.Length];

        for (int i = 0; i < enemyPool.Length; i++)
        {
            enemyPool[i] = new List<GameObject>();
        }

        for (int i = 0; i < effectPool.Length; i++)
        {
            effectPool[i] = new List<GameObject>();
        }

        for (int i = 0; i < enemySpellPool.Length; i++)
        {
            enemySpellPool[i] = new List<GameObject>();
        }
    }

    public GameObject GetEnemy(int index)
    {
        GameObject select = null;

        foreach (GameObject enemyObj in enemyPool[index])
        {
            if (!enemyObj.activeSelf)
            {
                select = enemyObj;
                select.SetActive(true);
                break;
            }
        }

        if (!select)
        {
            select = Instantiate(enemies[index]);
            enemyPool[index].Add(select);
        }

        return select;
    }

    public GameObject GetEffect(int index)
    {
        GameObject select = null;

        foreach (GameObject effectObj in effectPool[index])
        {
            if (!effectObj.activeSelf)
            {
                select = effectObj;
                select.SetActive(true);
                break;
            }
        }

        if (!select)
        {
            select = Instantiate(effects[index]);
            effectPool[index].Add(select);
        }

        return select;
    }

    public GameObject GetEnemySpell(int index)
    {
        GameObject select = null;

        foreach (GameObject spellObj in enemySpellPool[index])
        {
            if (!spellObj.activeSelf)
            {
                select = spellObj;
                select.SetActive(true);
                break;
            }
        }

        if (!select)
        {
            select = Instantiate(enemySpells[index]);
            enemySpellPool[index].Add(select);
        }

        select.transform.parent = null;
        return select;
    }

    public void Return(GameObject obj)
    {
        obj.transform.position = Vector3.zero;
        obj.transform.rotation = Quaternion.identity;
        obj.transform.parent = transform;
        obj.SetActive(false);
    }
}