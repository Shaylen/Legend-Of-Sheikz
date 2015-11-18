using UnityEngine;
using System.Collections;

public class GreenFireBall : SpellController
{
    public float rotationSpeed;
    public float duration;
    public float fadeTime;

    void Start()
    {
        StartCoroutine(destroyAfterSeconds(duration));
    }

    void FixedUpdate()
    {
        transform.Rotate(0, 0, rotationSpeed);
    }

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
