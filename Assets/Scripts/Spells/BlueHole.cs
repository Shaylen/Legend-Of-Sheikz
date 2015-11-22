using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BlueHole : StaticSpell
{
    public float duration;
    [Tooltip("The delay after which damage applies")]
    public float damageDelay;
    public float pullStrength;

    private bool isEnabled = false;

    void Start()
    {
        StartCoroutine(destroyAfterSeconds(duration));
        StartCoroutine(doDamageAfterSeconds(damageDelay));
    } 

    void Update()
    {
        if (!isEnabled)
            return;

        foreach (GameObject obj in affectedObjects)
        {
            if (!obj)
                continue;

            Damageable dmg = obj.GetComponent<Damageable>();
            if (dmg)
            {
                dmg.doDamage(emitter, damage);
                Rigidbody2D otherRB = obj.GetComponent<Rigidbody2D>();
                if (otherRB)
                    otherRB.AddForce((transform.position - obj.transform.position) * pullStrength);
            }
        }
    }

    /*
    void OnTriggerStay2D(Collider2D other)
    {
        Debug.Log("Triggered with "+other.gameObject.name);
        /*if (!isEnabled)
            return;*/
        /*
        Damageable dmg = other.GetComponent<Damageable>();
        if (dmg)
        {
            dmg.doDamage(emitter, damage);
            //Rigidbody2D otherRB = other.GetComponent<Rigidbody2D>();
            //otherRB.AddForce((transform.position - other.transform.position) * pullStrength);
        }
    }*/

    protected IEnumerator doDamageAfterSeconds(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        isEnabled = true;
    }
}
