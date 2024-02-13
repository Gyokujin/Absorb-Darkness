using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    [SerializeField]
    private Transform target; // ī�޶� �����ϴ� ��� : �÷��̾�
    [SerializeField]
    private float xMoveSpeed = 500; // ī�޶��� y�� ȸ�� �ӵ�
    [SerializeField]
    private float yMoveSpeed = 250; // ī�޶��� x�� ȸ�� �ӵ�
    private float yMinLimit = 5; // ī�޶� x�� ȸ�� ���� �ּ� ��
    private float yMaxLimit = 80; // ī�޶� x�� ȸ�� ���� �ִ� ��
    private float moveX, moveY; // ���콺 �̵� ���� ��
    private float distance; // ī�޶�� target�� �Ÿ�

    private void Awake()
    {
        // ���� ī�޶��� ȸ�� ���� x, y ������ ����
        Vector3 angles = transform.eulerAngles;
        moveX = angles.y;
        moveY = angles.x;
    }

    private void Update()
    {
        // target�� �������� ������ ���� ���� �ʴ´�
        if (target == null)
            return;

        // ���콺�� x, y�� ������ ���� ����
        moveX += Input.GetAxis("Mouse X") * xMoveSpeed * Time.deltaTime;
        moveY -= Input.GetAxis("Mouse Y") * yMoveSpeed * Time.deltaTime;
        // ������Ʈ�� ��/�Ʒ� (x��) �Ѱ� ���� ����
        moveY = ClampAngle(moveY, yMinLimit, yMaxLimit);
        // ī�޶��� ȸ��(Rotation) ���� ����
        transform.rotation = Quaternion.Euler(moveY, moveX, 0);
    }

    private void LateUpdate()
    {
        if (target == null)
            return;

        // ī�޶��� ��ġ(Position) ���� ����. target�� ��ġ�� �������� distance��ŭ �������� �Ѿư���.
        transform.position = transform.rotation * new Vector3(0, 0, -distance) + target.position;
    }

    private float ClampAngle(float angle, float min, float max)
    {
        if (angle < -360) angle += 360;
        if (angle > 360) angle -= 360;

        return Mathf.Clamp(angle, min, max);
    }
}