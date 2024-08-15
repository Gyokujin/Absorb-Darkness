using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;
    public FieldInfo curField;

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

    void Start()
    {
        ReadFieldInfo(curField);
    }

    public void ReadFieldInfo(FieldInfo fieldInfo)
    {
        curField = fieldInfo;
        UIManager.instance.OpenStageUI(curField);
    }

    public void EntranceBossRoom()
    {
        AudioManager.instance.PlayBGM(AudioManager.instance.bgmClips[(int)AudioManager.GameBGM.Stage0Boss]);
    }

    public void LockCamera(bool onLock)
    {
        if (onLock)
        {
            Cursor.visible = false; // Ŀ���� ������ �ʰ� �Ѵ�.
            Cursor.lockState = CursorLockMode.Locked; // Ŀ���� �߾����� ����
        }
        else
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
    }
}