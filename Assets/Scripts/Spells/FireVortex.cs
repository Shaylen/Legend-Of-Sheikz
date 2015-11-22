using UnityEngine;
using System.Collections;
using System;

public class FireVortex : StaticSpell
{
    public float duration;
    [Tooltip("The delay after which damage applies")]
    public float damageDelay;

    private bool isDoingDamage = false;

    void Start()
    {
        StartCoroutine(destroyAfterSeconds(duration));
        StartCoroutine(doDamageAfterSeconds(damageDelay));
    }

    void Update()
    {
        if (!isDoingDamage)
            return;

        foreach (GameObject obj in affectedObjects)
        {
            if (!obj)
                continue;

            Damageable dmg = obj.GetComponent<Damageable>();
            if (dmg)
            {
                dmg.doDamage(emitter, damage);
            }
        }
    }

    protected IEnumerator doDamageAfterSeconds(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        isDoingDamage = true;
    }
}
