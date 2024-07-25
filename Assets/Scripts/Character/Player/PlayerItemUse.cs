using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerItemUse : MonoBehaviour
{
    private PlayerManager player;



    void Awake()
    {
        player = GetComponent<PlayerManager>();
    }

    
}