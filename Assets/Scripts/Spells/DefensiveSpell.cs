using UnityEngine;
using System.Collections;
using System;

public class DefensiveSpell : SpellController
{
    public override void initialize(GameObject emitter, Vector3 position, Vector3 target)
    {
        transform.position = position;
        this.emitter = emitter;
        transform.SetParent(emitter.transform);
    }
}
