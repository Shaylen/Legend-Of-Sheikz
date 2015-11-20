using UnityEngine;
using System.Collections;

public class ElectricBall : MovingSpell 
{	
	public float fadeTime;
	
	void Start()
	{
		
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
