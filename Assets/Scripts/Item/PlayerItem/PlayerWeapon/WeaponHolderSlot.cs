using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponHolderSlot : MonoBehaviour
{
    [SerializeField]
    private Transform parentTransform;
    public bool isLeftHandSlot;
    public bool isRightHandSlot;

    public GameObject currentWeaponModel;

    public void UnloadWeapon()
    {
        if (currentWeaponModel != null)
        {
            currentWeaponModel.SetActive(false);
        }
    }

    public void UnloadWeaponAndDestroy()
    {
        if (currentWeaponModel != null)
        {
            Destroy(currentWeaponModel); // 이후에 수정하자
        }
    }

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
            if (parentTransform != null)
            {
                model.transform.parent = parentTransform;
            }
            else
            {
                model.transform.parent = transform;
            }

            model.transform.localPosition = Vector3.zero;
            model.transform.localRotation = Quaternion.identity;
            model.transform.localScale = Vector3.one;
        }

        currentWeaponModel = model;
    }
}