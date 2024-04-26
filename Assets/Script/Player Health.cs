using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : LivingEntity
{
    //public Slider healthSlider; // ü���� ǥ���� UI �����̴�

    public AudioClip deathClip; 
    public AudioClip hitClip; 

    private AudioSource playerAudioPlayer; // �÷��̾� �Ҹ� �����
    private Animator playerAnimator; // �÷��̾��� �ִϸ�����

    private PlayerMovement playerMovement; // �÷��̾� ������ ������Ʈ
    private PlayerShooter playerShooter; // �÷��̾� ���� ������Ʈ

    private void Awake()
    {
        // ����� ������Ʈ�� ��������
        playerAnimator = GetComponent<Animator>();
        playerMovement = GetComponent<PlayerMovement>();
        playerShooter = GetComponent<PlayerShooter>();
        playerAudioPlayer = GetComponent<AudioSource>();

    }

    protected override void OnEnable()
    {
        base.OnEnable();

        //healthSlider.gameObject.SetActive(true);
        //healthSlider.value = health / startingHealth;
        playerMovement.enabled = true;
        playerShooter.enabled = true;

        //UIManager.instance.SetActiveGameoverUI(false);
    }

    // ������ ó��
    public override void OnDamage(float damage, Vector3 hitPoint, Vector3 hitDirection)
    {
        if (dead)
        {
            return;
        }
        base.OnDamage(damage, hitPoint, hitDirection);
        //healthSlider.value = health / startingHealth;
        playerAudioPlayer.PlayOneShot(hitClip);
    }

    // ��� ó��
    public override void Die()
    {
        //base.Die();
        //healthSlider.gameObject.SetActive(false);
        //playerAudioPlayer.PlayOneShot(deathClip);
        //playerAnimator.SetTrigger("Die");

        //playerMovement.enabled = false;
        //playerShooter.enabled = false;

        ////UIManager.instance.SetActiveGameoverUI(true);
    }
}
