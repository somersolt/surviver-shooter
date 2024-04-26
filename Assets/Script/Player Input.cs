using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    public static readonly string MoveZAxisName = "Vertical";
    public static readonly string MoveXAxisName = "Horizontal";
    public static readonly string fireButtonName = "Fire1";
    public static readonly string reloadButtonName = "Reload";

    // 오토 프로퍼티
    public float moveX { get; private set; } // 감지된 움직임 입력값
    public float moveZ { get; private set; } // 감지된 움직임 입력값
    public bool fire { get; private set; } // 감지된 발사 입력값
    public bool reload { get; private set; } // 감지된 재장전 입력값

    // 매프레임 사용자 입력을 감지
    private void Update()
    {
        //// 게임오버 상태에서는 사용자 입력을 감지하지 않는다
        //if (GameManager.instance != null && GameManager.instance.isGameover)
        //{
        //    move = 0;
        //    rotate = 0;
        //    fire = false;
        //    reload = false;
        //    return;
        //}

        // move에 관한 입력 감지
        moveX = Input.GetAxis(MoveXAxisName);
        // rotate에 관한 입력 감지
        moveZ = Input.GetAxis(MoveZAxisName);
        //fire에 관한 입력 감지
        fire = Input.GetButton(fireButtonName);
        // reload에 관한 입력 감지
        //reload = Input.GetButtonDown(reloadButtonName);
    }
}
