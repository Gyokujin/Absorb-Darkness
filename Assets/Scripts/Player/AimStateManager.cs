using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class AimStateManager : MonoBehaviour
{
    public Cinemachine.AxisState xAxis, yAxis;

    [SerializeField]
    private Transform camFollowPos;

    void Update()
    {
        xAxis.Update(Time.deltaTime);
        yAxis.Update(Time.deltaTime);
    }

    void LateUpdate()
    {
        camFollowPos.localEulerAngles = new Vector3(yAxis.Value, camFollowPos.localEulerAngles.y, camFollowPos.localEulerAngles.z);
        transform.eulerAngles = new Vector3(transform.eulerAngles.x, xAxis.Value, transform.eulerAngles.z);
    }
}