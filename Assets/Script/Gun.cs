using System.Collections;
using System.Collections.Generic;
using System.Security.Principal;
using UnityEngine;
using UnityEngine.UIElements;

public class Gun : MonoBehaviour
{
    public GunData gunData;
    public Transform fireTransform; // 총알이 발사될 위치
    public GameObject bulletLine;
    public ParticleSystem muzzleFlashEffect; // 총구 화염 효과
    private LineRenderer bulletLineRenderer; // 총알 궤적을 그리기 위한 렌더러
    private AudioSource gunAudioPlayer; // 총 소리 재생기
    int layerMask;
    private float fireDistance = 50f; // 사정거리
    private float lastFireTime; // 총을 마지막으로 발사한 시점

    public int upgradePoint = 0;
    Vector3 boxsize = new Vector3(0.1f, 0.1f, 0.1f);

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
        Vector3 hitPoint = Vector3.zero;
        if(Physics.BoxCast(fireTransform.position, boxsize, fireTransform.forward, out RaycastHit hit, Quaternion.identity, fireDistance, layerMask))
        {
            hitPoint = hit.point;
            //Damage
            var damagable = hit.collider.GetComponent<IDamageable>();
            if (damagable != null)
            {
                damagable.OnDamage(gunData.damage, hitPoint, hit.normal);
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
        gunAudioPlayer.PlayOneShot(gunData.shotClip);
        muzzleFlashEffect.Play();


        //bulletLineRenderer.SetPosition(0, fireTransform.position);
        //bulletLineRenderer.SetPosition(1, hitPosition);

        GameObject cylinder = Instantiate(bulletLine, fireTransform.position, Quaternion.identity);
        cylinder.transform.up = hitPosition - fireTransform.position;

        float distance = Vector3.Distance(fireTransform.position, hitPosition);

        cylinder.transform.localScale = new Vector3(1f, distance / 2f, 1f);
        cylinder.transform.position += fireTransform.forward * distance / 2f;

        yield return new WaitForSeconds(0.03f);

        Destroy(cylinder.gameObject);

    }
}

