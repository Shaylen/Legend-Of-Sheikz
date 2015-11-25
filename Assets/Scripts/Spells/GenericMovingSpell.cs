using UnityEngine;
using System.Collections;

public class GenericMovingSpell : MovingSpell
{
    void OnTriggerEnter2D(Collider2D other)
    {
        Damageable dmg = other.gameObject.GetComponent<Damageable>();
        if (dmg != null)
        {
            dmg.doDamage(emitter, damage);
        }
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Wall") && other.bounds.Contains(transform.position))
        {
            rb.velocity = Vector2.zero;
            StartCoroutine(destroyAfterSeconds(0.5f));
        }
    }
}
