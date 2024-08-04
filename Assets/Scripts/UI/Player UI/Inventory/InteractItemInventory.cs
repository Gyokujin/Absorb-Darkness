using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractItemInventory : MonoBehaviour
{
    public List<ItemSlot> interactItemSlots = new List<ItemSlot>();
    [SerializeField]
    private Transform slotTransform;

    void Awake()
    {
        InteractItemUpdate();
    }

    public void InteractItemUpdate()
    {
        for (int i = 0; i < InventoryManager.instance.interactItems.Count; i++)
        {
            if (i >= interactItemSlots.Count)
            {
                ItemSlot createSlot = Instantiate(InventoryManager.instance.slotObject, slotTransform).GetComponent<ItemSlot>();
                interactItemSlots.Add(createSlot);
            }

            Item item = InventoryManager.instance.interactItems[i];
            interactItemSlots[i].ItemSlotUpdate(false, item.itemIcon, item.itemCount, item.itemName, item.itemInfo);
        }
    }
}