using UnityEngine;
using System.Collections;

[RequireComponent(typeof(SpriteRenderer))]
public abstract class Skill : MonoBehaviour
{
    public abstract void applySkill(GameObject hero);
}
