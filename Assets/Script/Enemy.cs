using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Pool;

public class Enemy : LivingEntity
{
    public LayerMask whatIsTarget;
    private LivingEntity targetEntity; // ������ ���

    private NavMeshAgent pathFinder; // ��ΰ�� AI ������Ʈ

    public ParticleSystem hitEffect; // �ǰݽ� ����� ��ƼŬ ȿ��
    public AudioClip deathSound; // ����� ����� �Ҹ�
    public AudioClip hitSound; // �ǰݽ� ����� �Ҹ�
    public Gun gun;

    private Animator enemyAnimator; // �ִϸ����� ������Ʈ
    private AudioSource enemyAudioPlayer; // ����� �ҽ� ������Ʈ
    private Renderer enemyRenderer; // ������ ������Ʈ

    public float damage = 20f; // ���ݷ�
    public float timeBetAttack = 0.5f; // ���� ����
    public float lastAttackTime; // ������ ���� ����

    public IObjectPool<Enemy> pool;
    // ������ ����� �����ϴ��� �˷��ִ� ������Ƽ
    private bool hasTarget
    {
        get
        {
            // ������ ����� �����ϰ�, ����� ������� �ʾҴٸ� true
            if (targetEntity != null && !targetEntity.dead)
            {
                return true;
            }

            // �׷��� �ʴٸ� false
            return false;
        }
    }
    private void Awake()
    {
        pathFinder = GetComponent<NavMeshAgent>();
        enemyAnimator = GetComponent<Animator>();
        enemyAudioPlayer = GetComponent<AudioSource>();
        enemyRenderer = GetComponentInChildren<SkinnedMeshRenderer>();
        GameObject gunGameObject = GameObject.FindWithTag("Gun");
        gun = gunGameObject.GetComponent<Gun>();
    }

    //public void Setup(ZombieData data)
    //{
    //    Setup(data.health, data.damage, data.speed);
    //}
    //// �� AI�� �ʱ� ������ �����ϴ� �¾� �޼���
    public void Reset()
    {
        dead = false;
        var rid = GetComponent<Rigidbody>();
        rid.isKinematic = true;
        health = startingHealth;
        pathFinder.enabled = true;
        pathFinder.isStopped = false;
        var cols = GetComponentsInChildren<Collider>();
        CancelInvoke("Disable");
        foreach (Collider col in cols)
        {
            col.enabled = true;
        }
        StartCoroutine(UpdatePath());
    }
    public void Setup(float newHealth, float newDamage, float newSpeed)
    {
        startingHealth = newHealth;
        damage = newDamage;
        pathFinder.speed = newSpeed;
    }

    private void Start()
    {
       StartCoroutine(UpdatePath());
    }
    private void Update()
    {
       enemyAnimator.SetBool("HasTarget", hasTarget);
    }

    private IEnumerator UpdatePath()
    {
        // ����ִ� ���� ���� ����
        while (!dead)
        {
            if (hasTarget)
            {
                pathFinder.SetDestination(targetEntity.gameObject.transform.position);
            }
            else
            {
                pathFinder.ResetPath();
                Collider[] cols = Physics.OverlapSphere(transform.position, 50f, whatIsTarget);
                foreach (Collider col in cols)
                {
                    LivingEntity livingEntity = col.GetComponent<LivingEntity>();
                    if (livingEntity != null)
                    {
                        targetEntity = livingEntity;
                        break;
                    }
                }
            }
            yield return new WaitForSeconds(0.25f);

        }
    }

    public override void OnDamage(float damage, Vector3 hitPoint, Vector3 hitNormal)
    {
        hitEffect.transform.position = hitPoint;
        hitEffect.transform.rotation = Quaternion.LookRotation(hitNormal);
        hitEffect.Play();

        enemyAudioPlayer.PlayOneShot(hitSound);

        // LivingEntity�� OnDamage()�� �����Ͽ� ������ ����
        base.OnDamage(damage, hitPoint, hitNormal);
    }

    public override void Die()
    {
        // LivingEntity�� Die()�� �����Ͽ� �⺻ ��� ó�� ����
        base.Die();

        var cols = GetComponentsInChildren<Collider>();
        foreach (Collider col in cols)
        {
            col.enabled = false;
        }

        pathFinder.isStopped = true;
        pathFinder.enabled = false;

        enemyAnimator.SetTrigger("Die");
        enemyAudioPlayer.PlayOneShot(deathSound);

        UIManager.instance.UpdateScoreText(100);
        gun.upgradePoint += 100;

    }

    private void OnTriggerStay(Collider other)
    {
        if (!dead && Time.time >= lastAttackTime + timeBetAttack)
        {
            var entity = other.GetComponent<LivingEntity>();
            if (entity != null && entity == targetEntity)
            {
                pathFinder.isStopped = true;

                var pos = transform.position;
                pos.y = 1f;
                var hitPoint = other.ClosestPoint(pos);
                var hitNormal = transform.position - other.transform.position;

                targetEntity.OnDamage(damage, hitPoint, hitNormal.normalized);
                lastAttackTime = Time.time;
            }

        }

    }
    private void OnTriggerExit(Collider other)
    {
        var entity = other.GetComponent<LivingEntity>();
        if (entity != null && entity == targetEntity)
        {
            pathFinder.isStopped = false;
        }
        }


    public void StartSinking()
    {
        var rid = GetComponent<Rigidbody>();
        rid.isKinematic = false;

        Invoke("Disable", 2f);
    }

    private void Disable()
    {
        pool.Release(this);
    }

}
