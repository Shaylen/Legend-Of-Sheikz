using UnityEngine;

public abstract class StaticSpell : SpellController
{
    public override void initialize(GameObject emitter, Vector3 position, Vector3 target)
    {
        transform.position = target;
        this.emitter = emitter;
    }

   

}
