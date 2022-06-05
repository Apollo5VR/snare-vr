using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Snake : ForestEnemy
{
    protected override void Attack(float damage)
    {
        StartCoroutine(WaitToAttack());

        //TODO - launch projectile of ITSELF at the target destination (aka springing snake)

        base.Attack(damage);
    }

    private IEnumerator WaitToAttack()
    {
        while(true)
        {
            yield return new WaitForSeconds(5);
        }
    }
}
