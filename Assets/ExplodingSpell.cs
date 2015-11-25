using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ExplodingSpell : MovingSpell
{
    public GameObject explosion;

    private List<Collider2D> affectedObjects;

    public override void initialize(GameObject emitter, Vector3 position, Vector3 target)
    {
        base.initialize(emitter, position, target);
        affectedObjects = new List<Collider2D>();
    }

    void Update()
    {
        foreach (Collider2D collider in affectedObjects)
        {
            if (!collider)
                return;

            GameObject other = collider.gameObject;
            if (other == emitter)
                return;

            if (collider.bounds.Contains(transform.position))
                explode();
        }
    }

    public void explode()
    {
        GameObject newExplosion = Instantiate(explosion, transform.position, Quaternion.identity) as GameObject;
        newExplosion.GetComponent<Explosion>().initialize(emitter);
        Destroy(gameObject);
    }

    // Weird fix because OnTriggerStay2D randomly doesn't work. Need to keep a list of objects triggering
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other)
        {
            affectedObjects.Add(other);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other)
        {
            affectedObjects.Remove(other);
        }
    }
}
