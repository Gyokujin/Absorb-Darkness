using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponInventory : MonoBehaviour
{
    public List<WeaponItem> weaponItems;
    public List<ItemSlot> weaponSlots = new List<ItemSlot>();

    [SerializeField]
    private Transform slotTransform;

    void Awake()
    {
        WeaponItemUpdate();
    }

    public void WeaponItemUpdate()
    {
        for (int i = 0; i < weaponItems.Count; i++)
        {
            if (i >= weaponSlots.Count)
            {
                ItemSlot createSlot = Instantiate(InventoryManager.instance.slotObject, slotTransform).GetComponent<ItemSlot>();
                weaponSlots.Add(createSlot);
            }

            Item item = weaponItems[i];
            weaponSlots[i].ItemSlotUpdate(item.itemIcon, item.itemCount, item.itemName, item.itemInfo);
        }
    }
}