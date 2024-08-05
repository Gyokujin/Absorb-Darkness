using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossItemDrop : MonoBehaviour
{
    [SerializeField]
    private Item[] dropItems;

    public void ItemLoot()
    {
        foreach (Item item in dropItems)
        {
            switch (item.itemType)
            {
                case Item.ItemType.WeaponItem:
                    WeaponItem weaponItem = item as WeaponItem;
                    InventoryManager.instance.GetWeaponItem(weaponItem);
                    break;

                case Item.ItemType.UsingItem:
                    UsingItem usingItem = item as UsingItem;
                    InventoryManager.instance.GetUsingItem(usingItem);
                    break;

                case Item.ItemType.InteractItem:
                    InteractItem interactItem = item as InteractItem;
                    InventoryManager.instance.GetInteractItem(interactItem);
                    break;
            }

            UIManager.instance.OpenItemPopUpUI(item.itemName, item.itemIcon.texture);
        }
    }
}