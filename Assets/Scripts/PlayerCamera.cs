using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    [SerializeField]
    private Transform target; // 카메라가 추적하는 대상 : 플레이어
    [SerializeField]
    private float minDistance = 3;
    [SerializeField]
    private float maxDistance = 10;
    [SerializeField]
    private float wheelSpeed = 500;
    [SerializeField]
    private float xMoveSpeed = 500; // 카메라의 y축 회전 속도
    [SerializeField]
    private float yMoveSpeed = 250; // 카메라의 x축 회전 속도
    private float yMinLimit = 5; // 카메라 x축 회전 제한 최소 값
    private float yMaxLimit = 80; // 카메라 x축 회전 제한 최대 값
    private float moveX, moveY; // 마우스 이동 방향 값
    private float distance;

    private void Awake()
    {
        // 최초 설정된 target과 카메라의 위치를 바탕으로 distance 값 초기화
        distance = Vector3.Distance(transform.position, target.position);
        // 최초 카메라의 회전 값을 x, y 변수에 저장
        Vector3 angles = transform.eulerAngles;
        moveX = angles.y;
        moveY = angles.x;
    }
}