using System.Collections;
using System.Collections.Generic;
using System.Security.Principal;
using UnityEngine;
using UnityEngine.UIElements;

public class Gun : MonoBehaviour
{
    public GunData gunData;
    public Transform fireTransform; // �Ѿ��� �߻�� ��ġ
    public GameObject bulletLine;
    public ParticleSystem muzzleFlashEffect; // �ѱ� ȭ�� ȿ��
    private LineRenderer bulletLineRenderer; // �Ѿ� ������ �׸��� ���� ������
    private AudioSource gunAudioPlayer; // �� �Ҹ� �����
    int layerMask;
    private float fireDistance = 50f; // �����Ÿ�
    private float lastFireTime; // ���� ���������� �߻��� ����

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

