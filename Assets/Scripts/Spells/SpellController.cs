using UnityEngine;
using System.Collections;

public abstract class SpellController : MonoBehaviour
{
    public float cooldown = 0.2f;
	public Sprite icon;

    protected Animator anim;
	protected Rigidbody2D rb;
	protected CircleCollider2D circleCollider;
    [HideInInspector]
	public GameObject emitter;   // Reference to the caster of the spell

    protected void Awake()
	{
		anim = GetComponent<Animator>();
		rb = GetComponent<Rigidbody2D>();
		circleCollider = GetComponent<CircleCollider2D>();
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

    public abstract void initialize(GameObject emitter, Vector3 position, Vector3 target);
}
