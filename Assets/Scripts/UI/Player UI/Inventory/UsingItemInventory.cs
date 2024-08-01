using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UsingItemInventory : MonoBehaviour
{
    public List<UsingItem> usingItems;

    [SerializeField]
    private Transform slotTransform;
    [SerializeField]
    private GameObject slotObject;

    void OnEnable()
    {
        UsingItemUpdate();
    }

    void UsingItemUpdate()
    {
        for (int i = 0; i < usingItems.Count; i++)
        {
            Debug.Log(usingItems[i].name);
        }
    }
}