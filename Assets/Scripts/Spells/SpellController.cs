using UnityEngine;
using System.Collections;

public class SpellController : MonoBehaviour
{
	public float speed;
	public int damage;
	new public GameObject light;

	protected Animator anim;
	protected Rigidbody2D rb;
	protected CircleCollider2D circleCollider;
	protected GameObject emitter;   // Reference to the caster of the spell

	protected void Awake()
	{
		anim = GetComponent<Animator>();
		rb = GetComponent<Rigidbody2D>();
		circleCollider = GetComponent<CircleCollider2D>();
		if (light != null) // If a light is defined for this object
		{
			GameObject newLight = Instantiate(light);
			newLight.transform.SetParent(gameObject.transform);
			newLight.transform.localPosition = new Vector3(0, 0, -1);
		}
	}

	void FixedUpdate()
	{
		if (anim != null)
		{
			if (rb.velocity != Vector2.zero)
			{
				anim.SetFloat("directionX", rb.velocity.x);
				anim.SetFloat("directionY", rb.velocity.y);
			}
		}
	}
	
	protected IEnumerator destroyAfterSeconds(float seconds)
	{
		yield return new WaitForSeconds(seconds);
		Destroy(gameObject);
	}

	/// <summary>
	/// Initiliaze with a direction and the emitter
	/// </summary>
	/// <param name="emitter Tag"></param>
	/// <param name="direction"></param>
	public void initialize(GameObject emitter, Vector2 dir)
	{
        Debug.Log("Initializing dir: "+dir);
        dir.Normalize();
		rb.velocity = dir * speed;
		this.emitter = emitter;
	}

    public void initialize(GameObject emitter)
    {
        this.emitter = emitter;
        transform.SetParent(emitter.transform);
    }
}
