using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponHolderSlot : MonoBehaviour
{
    [Header("Weapon")]
    public GameObject currentWeaponModel;

    [Header("Weapon Slot")]
    public Transform parentOverride;
    public WeaponItem currentWeapon;
    public bool isLeftHandSlot;
    public bool isRightHandSlot;
    public bool isBackSlot;

    public void LoadWeaponModel(WeaponItem weaponItem)
    {
        UnloadWeaponAndDestroy();

        if (weaponItem == null)
        {
            UnloadWeapon();
            return;
        }

        GameObject model = Instantiate(weaponItem.modelPrefab) as GameObject;
        
        if (model != null)
        {
            if (parentOverride != null)
                model.transform.parent = parentOverride;
            else
                model.transform.parent = transform;

            model.transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
            model.transform.localScale = Vector3.one;
        }

        currentWeaponModel = model;
    }

    public void UnloadWeapon()
    {
        if (currentWeaponModel != null)
            currentWeaponModel.SetActive(false);
    }

    public void UnloadWeaponAndDestroy()
    {
        if (currentWeaponModel != null)
            Destroy(currentWeaponModel);
    }
}