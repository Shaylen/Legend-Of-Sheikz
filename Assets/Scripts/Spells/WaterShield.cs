using UnityEngine;
using System.Collections;

public class WaterShield : DefensiveSpell 
{
	public float duration;

	// Use this for initialization
	void Start () 
	{
		StartCoroutine (destroyAfterSeconds (duration));
	}

	void OnTriggerEnter2D(Collider2D other)
	{
		SpellController otherSpell = other.GetComponent<SpellController>();
		if (otherSpell) // It's a spell
		{
			if (otherSpell.emitter == emitter)  // Dont reflect our own spells!
				return;
			Rigidbody2D otherRB = other.GetComponent<Rigidbody2D>();
			if (otherRB)
			{
				otherRB.velocity *= -1;
				other.transform.Rotate(0, 0, 180);
				otherSpell.emitter = emitter;
			}
		}
		
	}
}
