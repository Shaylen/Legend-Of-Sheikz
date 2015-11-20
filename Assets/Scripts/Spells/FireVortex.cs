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

    void OnTriggerStay2D(Collider2D other)
    {
        if (!isDoingDamage)
            return;

        Damageable dmg = other.GetComponent<Damageable>();
        if (dmg)
        {
            dmg.doDamage(emitter, damage);
        }
    }

    protected IEnumerator doDamageAfterSeconds(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        isDoingDamage = true;
    }
}
