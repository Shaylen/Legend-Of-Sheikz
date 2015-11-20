using UnityEngine;
using System.Collections;
using System;

public abstract class MovingCharacter : MonoBehaviour
{
    public float speed;

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

    protected void updateAnimations(Vector2 movement)
    {
        anim.SetBool("Moving", true);
        anim.SetFloat("SpeedX", movement.x);
        anim.SetFloat("SpeedY", movement.y);

        if (movement != Vector2.zero)
        {
            anim.SetFloat("LastSpeedX", movement.x);
            anim.SetFloat("LastSpeedY", movement.y);
        }
        else
            anim.SetBool("Moving", false);
    }

    protected void castSpell(GameObject spell, Vector3 position, Vector3 target)
    {
        GameObject newSpell = Instantiate(spell) as GameObject;
        newSpell.GetComponent<SpellController>().initialize(gameObject, position, target);
    }

    protected void castSpell(GameObject spell)
    {
        GameObject newSpell = Instantiate(spell, transform.position, Quaternion.identity) as GameObject;
        //newSpell.GetComponent<SpellController>().initialize(gameObject);
        newSpell.transform.SetParent(gameObject.transform);
    }

    public abstract void die();

    public abstract void receivesDamage();
}
