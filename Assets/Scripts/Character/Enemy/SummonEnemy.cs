using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SummonEnemy : MonoBehaviour
{
    public ISummoner summoner;

    [SerializeField]
    private float revertDelay = 2;
    private WaitForSeconds revertWait;

    void OnEnable()
    {
        if (GetComponentInChildren<CharacterDissolve>() == null)
            GetComponentInChildren<Renderer>().gameObject.AddComponent<CharacterDissolve>();

        if (revertWait == null)
            revertWait = new WaitForSeconds(revertDelay);
    }

    public IEnumerator RevertEnemy()
    {
        yield return revertWait;
        summoner.SummonEnemies.Remove(this);
    }
}