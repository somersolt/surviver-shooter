using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    public static readonly string MoveZAxisName = "Vertical";
    public static readonly string MoveXAxisName = "Horizontal";
    public static readonly string fireButtonName = "Fire1";
    public static readonly string reloadButtonName = "Reload";

    // ���� ������Ƽ
    public float moveX { get; private set; } // ������ ������ �Է°�
    public float moveZ { get; private set; } // ������ ������ �Է°�
    public bool fire { get; private set; } // ������ �߻� �Է°�
    public bool reload { get; private set; } // ������ ������ �Է°�

    // �������� ����� �Է��� ����
    private void Update()
    {
        //// ���ӿ��� ���¿����� ����� �Է��� �������� �ʴ´�
        //if (GameManager.instance != null && GameManager.instance.isGameover)
        //{
        //    move = 0;
        //    rotate = 0;
        //    fire = false;
        //    reload = false;
        //    return;
        //}

        // move�� ���� �Է� ����
        moveX = Input.GetAxis(MoveXAxisName);
        // rotate�� ���� �Է� ����
        moveZ = Input.GetAxis(MoveZAxisName);
        //fire�� ���� �Է� ����
        fire = Input.GetButton(fireButtonName);
        // reload�� ���� �Է� ����
        //reload = Input.GetButtonDown(reloadButtonName);
    }
}
