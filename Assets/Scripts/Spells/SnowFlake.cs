using UnityEngine;
using System.Collections;

public class SnowFlake : SpellController
{
    public float rotationSpeed;
    public float duration;

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

        if (other.gameObject.CompareTag("Wall"))
        {
            circleCollider.isTrigger = false;           // Deactivating the trigger so it will react to the collision
            other.sharedMaterial.bounciness = 1.0f;     // Change the bounciness of the wall so it will bounce
            other.enabled = false;                      // Weird fix because else it doesn't take effect
            other.enabled = true;
        }
    }

    void OnCollisionExit2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Wall"))
        {
            circleCollider.isTrigger = true;                // Revert back to trigger mode
            col.collider.sharedMaterial.bounciness = 0.0f;  // Revert back the bounciness
            col.collider.enabled = false;                   // Weird fix because else it doesn't take effect
            col.collider.enabled = true;
        }
    }
}
