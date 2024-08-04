using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponInventory : MonoBehaviour
{
    public List<ItemSlot> weaponSlots = new List<ItemSlot>();
    [SerializeField]
    private Transform slotTransform;

    void Awake()
    {
        WeaponItemUpdate();
    }

    public void WeaponItemUpdate()
    {
        for (int i = 0; i < InventoryManager.instance.weaponItems.Count; i++)
        {
            if (i >= weaponSlots.Count)
            {
                ItemSlot createSlot = Instantiate(InventoryManager.instance.slotObject, slotTransform).GetComponent<ItemSlot>();
                weaponSlots.Add(createSlot);
            }

            Item item = InventoryManager.instance.weaponItems[i];
            weaponSlots[i].ItemSlotUpdate(true, item.itemIcon, item.itemCount, item.itemName, item.itemInfo);
        }
    }
}