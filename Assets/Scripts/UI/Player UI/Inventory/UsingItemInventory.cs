using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UsingItemInventory : MonoBehaviour
{
    public List<UsingItem> usingItems;
    public List<ItemSlot> usingItemSlots = new List<ItemSlot>();

    [SerializeField]
    private Transform slotTransform;

    void Awake()
    {
        UsingItemUpdate();
    }

    public void UsingItemUpdate()
    {
        for (int i = 0; i < usingItems.Count; i++)
        {
            if (i >= usingItemSlots.Count)
            {
                ItemSlot createSlot = Instantiate(InventoryManager.instance.slotObject.gameObject, slotTransform).GetComponent<ItemSlot>();
                usingItemSlots.Add(createSlot);
            }

            Item item = usingItems[i];
            usingItemSlots[i].ItemSlotUpdate(item.itemIcon, item.itemCount, item.itemName, item.itemInfo);
        }
    }
}