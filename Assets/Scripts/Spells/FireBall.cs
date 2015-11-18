using UnityEngine;
using System.Collections;

public class FireBall : SpellController
{
    public float knockbackIntensity;
    public float knockbackDuration;
    public float fadeTime;

    void OnTriggerEnter2D(Collider2D collider)
    {
        Damageable dmg = collider.gameObject.GetComponent<Damageable>();
        Knockbackable kb = collider.gameObject.GetComponent<Knockbackable>();
        if (dmg != null)
        {
            dmg.doDamage(emitter, damage);

        }
        if (kb != null && rb.velocity != Vector2.zero)
        {
            Vector2 force = (collider.transform.position - transform.position).normalized * knockbackIntensity;
            kb.knockback(force, knockbackDuration);
        }
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Wall") && other.bounds.Contains(transform.position))
        {
            rb.velocity = Vector2.zero;
            StartCoroutine(destroyAfterSeconds(fadeTime));
        }
    }
}
