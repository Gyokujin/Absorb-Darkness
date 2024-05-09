using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuickSlotsUI : MonoBehaviour
{
    [Header("Item Info")]
    [SerializeField]
    private Image leftWeaponIcon;
    [SerializeField]
    private Image rightWeaponIcon;
    [SerializeField]
    private Image usingItemIcon;

    public void UpdateWeaponQuickSlotsUI(bool isLeft, WeaponItem weapon)
    {
        if (isLeft)
        {
            if (weapon.itemIcon != null)
            {
                leftWeaponIcon.sprite = weapon.itemIcon;
                leftWeaponIcon.enabled = true;
            }
            else
            {
                leftWeaponIcon.sprite = null;
                leftWeaponIcon.enabled = false;
            }
        }
        else
        {
            if (weapon.itemIcon != null)
            {
                rightWeaponIcon.sprite = weapon.itemIcon;
                rightWeaponIcon.enabled = true;
            }
            else
            {
                rightWeaponIcon.sprite = null;
                rightWeaponIcon.enabled = false;
            }
        }
    }

    public void UpdateUsingItemUI(UsingItem item)
    {
        if (item.itemIcon != null)
        {
            usingItemIcon.sprite = item.itemIcon;
            usingItemIcon.enabled = true;
        }
    }
}