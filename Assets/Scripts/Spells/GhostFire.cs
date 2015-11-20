using UnityEngine;
using System.Collections;

public class GhostFire : MovingSpell
{
    public float lifeTime;
    private float offsetMagnitude;

	// Use this for initialization
	void Start ()
    {

        float angle = Vector3.Angle(Vector3.down, direction);
        if (direction.x <= 0)
            angle *= -1;
        transform.Rotate(new Vector3(0, 0, angle));
        transform.position -= (Vector3)direction;
        StartCoroutine(destroyAfterSeconds(lifeTime));
        offsetMagnitude = circleCollider.offset.magnitude;
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
        Vector3 center = transform.position + (Vector3)direction * offsetMagnitude;
        if (other.gameObject.CompareTag("Wall") && other.bounds.Contains(center))
        {
            rb.velocity = Vector2.zero;
            StartCoroutine(destroyAfterSeconds(0.5f));
        }
    }
}
