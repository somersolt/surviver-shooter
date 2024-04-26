using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public GunData gunData;
    public Transform fireTransform; // �Ѿ��� �߻�� ��ġ

    public ParticleSystem muzzleFlashEffect; // �ѱ� ȭ�� ȿ��
    private LineRenderer bulletLineRenderer; // �Ѿ� ������ �׸��� ���� ������
    private AudioSource gunAudioPlayer; // �� �Ҹ� �����
    int layerMask;
    private float fireDistance = 50f; // �����Ÿ�
    private float lastFireTime; // ���� ���������� �߻��� ����

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

