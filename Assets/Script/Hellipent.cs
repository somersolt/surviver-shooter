using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HellipentAttack : MonoBehaviour
{
    public Enemy hellipent;

    private void OnTriggerEnter(Collider other)
    {
        if (!hellipent.dead && Time.time >= hellipent.lastAttackTime + hellipent.timeBetAttack)
        {
            var entity = other.GetComponent<LivingEntity>();
            if (entity != null && entity == hellipent.TargetEntity)
            {
                hellipent.PathFinder.isStopped = true;
                
                var pos = hellipent.transform.position;
                pos.y = 1f;
                var hitPoint = other.ClosestPoint(pos);
                var hitNormal = hellipent.transform.position - other.transform.position;

                hellipent.TargetEntity.OnDamage(hellipent.damage, hitPoint, hitNormal.normalized);
                hellipent.lastAttackTime = Time.time;
            }

        }
    }

}
