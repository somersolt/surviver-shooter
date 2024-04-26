using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public Camera mainCamera;
    public float moveSpeed = 5f;

    private PlayerInput playerInput; // 플레이어 입력을 알려주는 컴포넌트
    private Rigidbody playerRigidbody; // 플레이어 캐릭터의 리지드바디
    private Animator playerAnimator; // 플레이어 캐릭터의 애니메이터
    Vector3 movement;
    int layerMask;

    private void Awake()
    {
        // 사용할 컴포넌트들의 참조를 가져오기
        playerInput = GetComponent<PlayerInput>();
        playerRigidbody = GetComponent<Rigidbody>();
        playerAnimator = GetComponent<Animator>();
        layerMask = LayerMask.GetMask("Floor");

    }

    private void Update()
    {
        playerAnimator.SetFloat("Move", movement.magnitude);
        Vector3 mousePosition = Input.mousePosition;

        Ray ray = mainCamera.ScreenPointToRay(mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, layerMask) )
        {
            Vector3 lookDiretion = hit.point - transform.position;
            lookDiretion.y = 0f;

            Quaternion targetRotation = Quaternion.LookRotation(lookDiretion);
            transform.rotation = targetRotation;
        }
    }


    private void FixedUpdate()
    {
        Move();
    }

    // 입력값에 따라 캐릭터를 앞뒤로 움직임
    private void Move()
    {
        movement = new Vector3(playerInput.moveX, 0f, playerInput.moveZ);
        if(movement.magnitude > 1f)
        movement.Normalize();
        var pos = playerRigidbody.position;
        pos += movement * moveSpeed * Time.deltaTime;
        playerRigidbody.MovePosition(pos);

    }
}
