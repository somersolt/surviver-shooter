using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public GunData gunData;
    public Transform fireTransform; // 총알이 발사될 위치

    public ParticleSystem muzzleFlashEffect; // 총구 화염 효과
    private LineRenderer bulletLineRenderer; // 총알 궤적을 그리기 위한 렌더러
    private AudioSource gunAudioPlayer; // 총 소리 재생기
    int layerMask;
    private float fireDistance = 50f; // 사정거리
    private float lastFireTime; // 총을 마지막으로 발사한 시점

    private void Awake()
    {
        bulletLineRenderer = GetComponent<LineRenderer>();
        gunAudioPlayer = GetComponent<AudioSource>();
        bulletLineRenderer.enabled = false;
        bulletLineRenderer.positionCount = 2;
        layerMask = ~LayerMask.GetMask("Ignore Raycast");
    }
    public void Fire()
    {
        if (Time.time > lastFireTime + gunData.timeBetFire)
        {
            lastFireTime = Time.time;
            Shot();
        }
    }


    private void Shot()
    {
        var hitPoint = Vector3.zero;
        var ray = new Ray(fireTransform.position, fireTransform.forward);
        if (Physics.Raycast(ray, out RaycastHit hitInfo, fireDistance, layerMask))
        {
            hitPoint = hitInfo.point;
            //Damage
            var damagable = hitInfo.collider.GetComponent<IDamageable>();
            if (damagable != null)
            {
                damagable.OnDamage(gunData.damage, hitPoint, hitInfo.normal);
            }
        }
        else
        {
            hitPoint = fireTransform.position + fireTransform.forward * fireDistance;
        }
        StartCoroutine(ShotEffect(hitPoint));
    }

    private IEnumerator ShotEffect(Vector3 hitPosition)
    {
        bulletLineRenderer.SetPosition(0, fireTransform.position);
        bulletLineRenderer.SetPosition(1, hitPosition);
        bulletLineRenderer.enabled = true;

        gunAudioPlayer.PlayOneShot(gunData.shotClip);
        muzzleFlashEffect.Play();

        yield return new WaitForSeconds(0.03f);
        bulletLineRenderer.enabled = false;

    }
}

