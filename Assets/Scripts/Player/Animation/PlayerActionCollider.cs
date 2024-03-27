using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerActionCollider : MonoBehaviour
{
    [SerializeField]
    private float jumpCenterY = 5;
    [SerializeField]
    private float landCenterY;

    private CapsuleCollider collider;

    void Awake()
    {
        collider = GetComponent<CapsuleCollider>();
    }

  
    public void LandColliderSetting()
    {

    }

    public void JumpColliderSetting() 
    {
        collider.center = new Vector3(0, jumpCenterY, 0);
    }
}