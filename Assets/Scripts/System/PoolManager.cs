using OpenCover.Framework.Model;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    public static PoolManager instance = null;

    [Header("Spell")]
    [SerializeField]
    private GameObject[] spells;
    private List<GameObject>[] spellPool;

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

        spellPool = new List<GameObject>[spells.Length];

        for (int i = 0; i < spellPool.Length; i++)
        {
            spellPool[i] = new List<GameObject>();
        }
    }

    public GameObject GetSpell(int index)
    {
        GameObject select = null;

        foreach (GameObject spellObj in spellPool[index])
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
            select = Instantiate(spells[index], transform);
            spellPool[index].Add(select);
        }

        return select;
    }

    public void Return(GameObject obj)
    {
        obj.transform.position = Vector3.zero;
        obj.transform.rotation = Quaternion.identity;
        obj.SetActive(false);
    }
}