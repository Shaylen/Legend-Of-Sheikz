using UnityEngine;
using System.Collections;

public class ElectricBall : MovingSpell 
{	
	public float fadeTime;
	public float oscillatingAmplitude;
	public float oscillatingFrequency;

	private Vector2 lateralDirection;
    private float creationTime;

	void Start()
	{
		lateralDirection = Vector3.Cross(Vector3.back, direction);
        lateralDirection *= Random.Range(-1f, 1f);
        lateralDirection.Normalize();
        creationTime = Time.time;
	}

	public void FixedUpdate()
	{
        rb.AddForce(lateralDirection * Mathf.Cos((Time.time - creationTime) * oscillatingFrequency) * oscillatingAmplitude);
    }

    public void OnDrawGizmos()
    {
        Gizmos.DrawLine(transform.position, transform.position + (Vector3)lateralDirection);
    }
	
	void OnTriggerEnter2D(Collider2D collider)
	{
		Damageable dmg = collider.gameObject.GetComponent<Damageable>();
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
			StartCoroutine(destroyAfterSeconds(fadeTime));
		}
	}
}
