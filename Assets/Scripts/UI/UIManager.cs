using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public GameObject selectWindow;
    
    public void ControlSelectWindow(bool onOpen)
    {
        selectWindow.SetActive(onOpen);
    }

    //public void OpenSelectWindow()
    //{
    //    selectWindow.SetActive(true);
    //}

    //public void CloseSelectWindow()
    //{
    //    selectWindow.SetActive(false);
    //}
}