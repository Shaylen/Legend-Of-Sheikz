using UnityEngine;
using System.Collections;

public class WaterShield : MonoBehaviour 
{
	public float duration;

	// Use this for initialization
	void Start () 
	{
		StartCoroutine (destroyAfterSeconds (duration));
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}

	void OnTriggerEnter2D(Collider2D other)
	{
		Rigidbody2D otherRB = other.GetComponent<Rigidbody2D> ();
		if (otherRB)
			otherRB.velocity *= -1;
	}

	private IEnumerator destroyAfterSeconds(float seconds)
	{
		yield return new WaitForSeconds(seconds);
		Destroy(gameObject);
	}
}
