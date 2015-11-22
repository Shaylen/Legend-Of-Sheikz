using System.Collections.Generic;
using UnityEngine;

public abstract class StaticSpell : SpellController
{
    public int damage;

    protected List<GameObject> affectedObjects;

    public override void initialize(GameObject emitter, Vector3 position, Vector3 target)
    {
        transform.position = target;
        this.emitter = emitter;
        affectedObjects = new List<GameObject>();
    }

    // Weird fix because OnTriggerStay2D randomly doesn't work. Need to keep a list of objects triggering
    void OnTriggerEnter2D(Collider2D other)
    {
        GameObject obj = other.gameObject;
        if (obj)
        {
            affectedObjects.Add(obj);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        GameObject obj = other.gameObject;
        if (obj)
        {
            affectedObjects.Remove(obj);
        }
    }
}
