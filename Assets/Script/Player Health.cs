using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : LivingEntity
{
    //public Slider healthSlider; // 체력을 표시할 UI 슬라이더

    public AudioClip deathClip; 
    public AudioClip hitClip;

    public Slider HpBar;
    private AudioSource playerAudioPlayer; // 플레이어 소리 재생기
    private Animator playerAnimator; // 플레이어의 애니메이터

    private PlayerMovement playerMovement; // 플레이어 움직임 컴포넌트
    private PlayerShooter playerShooter; // 플레이어 슈터 컴포넌트

    private void Awake()
    {
        // 사용할 컴포넌트를 가져오기
        playerAnimator = GetComponent<Animator>();
        playerMovement = GetComponent<PlayerMovement>();
        playerShooter = GetComponent<PlayerShooter>();
        playerAudioPlayer = GetComponent<AudioSource>();

    }

    public void HpUpdate()
    {
        HpBar.GetComponentInChildren<Image>().fillAmount = health / startingHealth;
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        HpBar.gameObject.SetActive(true);
        HpUpdate();
        playerMovement.enabled = true;
        playerShooter.enabled = true;

        UIManager.instance.SetActiveGameoverUI(false);
    }

    // 데미지 처리
    public override void OnDamage(float damage, Vector3 hitPoint, Vector3 hitDirection)
    {
        if (dead)
        {
            return;
        }
        base.OnDamage(damage, hitPoint, hitDirection);
        playerAudioPlayer.PlayOneShot(hitClip);
        HpUpdate();
    }

    // 사망 처리
    public override void Die()
    {
        base.Die();
        HpBar.gameObject.SetActive(false);
        playerAudioPlayer.PlayOneShot(deathClip);
        playerAnimator.SetTrigger("Dead");

        playerMovement.enabled = false;
        playerShooter.enabled = false;

        UIManager.instance.SetActiveGameoverUI(true);
    }
}
