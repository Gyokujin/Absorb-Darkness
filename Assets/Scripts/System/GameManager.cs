using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        Application.targetFrameRate = 60;
        LockCamera(true);
    }

    public void LockCamera(bool onLock)
    {
        if (onLock)
        {
            Cursor.visible = false; // 커서를 보이지 않게 한다.
            Cursor.lockState = CursorLockMode.Locked; // 커서를 중앙으로 고정
        }
        else
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
    }
}