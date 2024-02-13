using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    [SerializeField]
    private Transform target; // 카메라가 추적하는 대상 : 플레이어
    [SerializeField]
    private float xMoveSpeed = 500; // 카메라의 y축 회전 속도
    [SerializeField]
    private float yMoveSpeed = 250; // 카메라의 x축 회전 속도
    private float yMinLimit = 5; // 카메라 x축 회전 제한 최소 값
    private float yMaxLimit = 80; // 카메라 x축 회전 제한 최대 값
    private float moveX, moveY; // 마우스 이동 방향 값
    private float distance; // 카메라와 target의 거리

    private void Awake()
    {
        // 최초 카메라의 회전 값을 x, y 변수에 저장
        Vector3 angles = transform.eulerAngles;
        moveX = angles.y;
        moveY = angles.x;
    }

    private void Update()
    {
        // target이 존재하지 않으면 실행 하지 않는다
        if (target == null)
            return;

        // 마우스를 x, y축 움직임 방향 정보
        moveX += Input.GetAxis("Mouse X") * xMoveSpeed * Time.deltaTime;
        moveY -= Input.GetAxis("Mouse Y") * yMoveSpeed * Time.deltaTime;
        // 오브젝트의 위/아래 (x축) 한계 범위 설정
        moveY = ClampAngle(moveY, yMinLimit, yMaxLimit);
        // 카메라의 회전(Rotation) 정보 갱신
        transform.rotation = Quaternion.Euler(moveY, moveX, 0);
    }

    private void LateUpdate()
    {
        if (target == null)
            return;

        // 카메라의 위치(Position) 정보 갱신. target의 위치를 기준으로 distance만큼 떨어져서 쫓아간다.
        transform.position = transform.rotation * new Vector3(0, 0, -distance) + target.position;
    }

    private float ClampAngle(float angle, float min, float max)
    {
        if (angle < -360) angle += 360;
        if (angle > 360) angle -= 360;

        return Mathf.Clamp(angle, min, max);
    }
}