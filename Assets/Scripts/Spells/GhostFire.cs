using UnityEngine;
using System.Collections;

public class GhostFire : SpellController
{

	// Use this for initialization
	void Start ()
    {
        Vector3 direction = rb.velocity.normalized;
        Debug.Log("Direction: " + direction.ToString());
        Debug.Log("Angle: "+ Vector3.Angle(Vector3.down, direction).ToString());
        //circleCollider.offset = direction.normalized;
        float angle = Vector3.Angle(Vector3.down, direction);
        if (direction.x <= 0)
            angle *= -1;
        transform.Rotate(new Vector3(0, 0, angle));
        transform.position -= direction;
    }
}
