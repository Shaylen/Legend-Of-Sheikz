using UnityEngine;
using System.Collections;
using System;

public abstract class MovingCharacter : MonoBehaviour
{
    public float speed;

    protected Vector2 movement;     // Direction in which the character moves
    protected Vector2 direction;    // Direction in which he is facing
    protected Rigidbody2D rb;
    protected Animator anim;
    protected CircleCollider2D circleCollider;
    protected SpriteRenderer spriteRenderer;
    

    // Use this for initialization
    protected void Start ()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        circleCollider = GetComponent<CircleCollider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    protected void updateAnimations()
    {
        if (anim)
        {
            if (movement != Vector2.zero)
                anim.SetBool("Moving", true);
            else
                anim.SetBool("Moving", false);

            anim.SetFloat("DirectionX", direction.x);
            anim.SetFloat("DirectionY", direction.y);
        }
    }

    protected void castSpell(GameObject spell, Vector3 position, Vector3 target)
    {
        GameObject newSpell = Instantiate(spell) as GameObject;
        newSpell.GetComponent<SpellController>().initialize(gameObject, position, target);
    }


    public abstract void die();

    public abstract void receivesDamage();
}
