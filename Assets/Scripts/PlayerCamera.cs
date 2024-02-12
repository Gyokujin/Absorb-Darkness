using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    [SerializeField]
    private Transform target; // ī�޶� �����ϴ� ��� : �÷��̾�
    [SerializeField]
    private float minDistance = 3;
    [SerializeField]
    private float maxDistance = 10;
    [SerializeField]
    private float wheelSpeed = 500;
    [SerializeField]
    private float xMoveSpeed = 500; // ī�޶��� y�� ȸ�� �ӵ�
    [SerializeField]
    private float yMoveSpeed = 250; // ī�޶��� x�� ȸ�� �ӵ�
    private float yMinLimit = 5; // ī�޶� x�� ȸ�� ���� �ּ� ��
    private float yMaxLimit = 80; // ī�޶� x�� ȸ�� ���� �ִ� ��
    private float moveX, moveY; // ���콺 �̵� ���� ��
    private float distance;

    private void Awake()
    {
        // ���� ������ target�� ī�޶��� ��ġ�� �������� distance �� �ʱ�ȭ
        distance = Vector3.Distance(transform.position, target.position);
        // ���� ī�޶��� ȸ�� ���� x, y ������ ����
        Vector3 angles = transform.eulerAngles;
        moveX = angles.y;
        moveY = angles.x;
    }
}