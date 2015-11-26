using UnityEngine;
using System.Collections;

public class Explosion : MonoBehaviour
{
	public int damage = 10;
	public float delayBeforeExlosion;
	public float knockbackForce = 0;
	public float knockbackDuration = 0;

	private CircleCollider2D circleCollider;
	private GameObject emitter;

	// Use this for initialization
	void Start ()
	{
		circleCollider = GetComponent<CircleCollider2D>();
		if (delayBeforeExlosion > 0)
			StartCoroutine(enableAfterSeconds(delayBeforeExlosion));
	}

	public void initialize(GameObject emitter)
	{
		this.emitter = emitter;
	}

	void OnTriggerEnter2D(Collider2D collider)
	{
		GameObject other = collider.gameObject;
		Damageable dmg = other.gameObject.GetComponent<Damageable>();
		Knockbackable kb = other.gameObject.GetComponent<Knockbackable>();
		if (dmg)
		{
			dmg.doDamage(emitter, damage);

		}
		if (kb && knockbackForce != 0)
		{
			Vector2 force = (collider.transform.position - transform.position).normalized * knockbackForce;
			kb.knockback(force, knockbackDuration);
		}
	}

	private IEnumerator enableAfterSeconds(float delay)
	{
		circleCollider.enabled = false;
		yield return new WaitForSeconds(delay);
		circleCollider.enabled = true;
	}
}
