using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public Camera mainCamera;
    public float moveSpeed = 5f;

    private PlayerInput playerInput; // �÷��̾� �Է��� �˷��ִ� ������Ʈ
    private Rigidbody playerRigidbody; // �÷��̾� ĳ������ ������ٵ�
    private Animator playerAnimator; // �÷��̾� ĳ������ �ִϸ�����
    Vector3 movement;
    int layerMask;

    private void Awake()
    {
        // ����� ������Ʈ���� ������ ��������
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

    // �Է°��� ���� ĳ���͸� �յڷ� ������
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
