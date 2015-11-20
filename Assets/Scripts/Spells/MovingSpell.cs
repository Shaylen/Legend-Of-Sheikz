using UnityEngine;
using System.Collections;

public abstract class MovingSpell : SpellController
{
    public float speed;

    protected Vector2 direction;
    protected Vector3 initialPosition;
    protected Vector3 target;


    /// <summary>
	/// Initiliaze with a direction and the emitter
	/// </summary>
	/// <param name="emitter Tag"></param>
	/// <param name="direction"></param>
	public override void initialize(GameObject emitter, Vector3 position, Vector3 target)
    {
        transform.position = position;
        initialPosition = position;
        this.target = target;
        this.emitter = emitter;

        direction = target - initialPosition;
        direction.Normalize();
        rb.velocity = direction * speed;
    }
}
