using UnityEngine;
using System.Collections;
using System;

public class HPUpSkill : Skill
{
    public int additionalHP;

    public override void applySkill(GameObject hero)
    {
        if (!hero)
            return;
        hero.GetComponent<Damageable>().increaseMaxHP(additionalHP);
    }
}
