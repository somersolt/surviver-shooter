using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;


public class ObjectPool : MonoBehaviour
{
    IObjectPool<Enemy> enemyPool;
    public Enemy bunny;
    public Enemy bear;
    public Enemy hellipent;
    public Transform[] Point;
    private float respawnInterval = 1f;
    private float lastRespawnTime;

    private void Start()
    {
        enemyPool = new ObjectPool<Enemy>(CreatePooledItem, OnTakeFromPool, OnReturnToPool, OnDestroyPoolObject, true, 10, 1000);
    }

    private void Update()
    {
        if (Time.time > lastRespawnTime + respawnInterval && !GameMgr.instance.onboss) 
        {
            CreateEnemy();
            lastRespawnTime = Time.time;
        }
    }

    private void CreateEnemy()
    {
        var newEnemy = enemyPool.Get();
        int i = Random.Range(0, 3);
        newEnemy.transform.position = Point[i].position;
        newEnemy.gameObject.SetActive(true);
        newEnemy.Reset();
    }
    private Enemy CreatePooledItem()
    {
        int a = Random.Range(0, 100);
        switch (a)
        {
            case var _ when a >= 0 && a < 40:
                var enemy = Instantiate(bunny);
                enemy.pool = enemyPool;
                return enemy;
            case var _ when a >= 40 && a < 80:
                enemy = Instantiate(bear);
                enemy.pool = enemyPool;
                return enemy;
            case var _ when a >= 80 && a <= 100:
                enemy = Instantiate(hellipent);
                enemy.pool = enemyPool;
                return enemy;
            default:
                Debug.Log("a is out of range");
                return null;
        }
    }

    private void OnTakeFromPool(Enemy enemy)
    {
        enemy.gameObject.SetActive(true);
    }

    private void OnReturnToPool(Enemy enemy)
    {
        enemy.gameObject.SetActive(false);
    }

    private void OnDestroyPoolObject(Enemy enemy)
    {
        Destroy(enemy.gameObject);
    }
}
