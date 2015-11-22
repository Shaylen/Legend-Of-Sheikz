using UnityEngine;
using System.Collections;
using System;

public class Damageable : MonoBehaviour
{
    public int maxHP;
    public float invincibilityTime;
    public bool isHealable;
    public GameObject deathAnimation;
    public GameObject healAnimation;
    public Material flashingMaterial;

    private GameObject floatingText;
    private int HP;
    private bool isInvincible;
    private GameObject healingAnimation;
    private SpriteRenderer spriteRenderer;
    private MovingCharacter movingChar;
    private int healEffects = 0;
    private Material originalMaterial;

    void Start ()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        HP = maxHP;
        movingChar = GetComponent<MovingCharacter>();
        floatingText = GameManager.instance.floatingText;
        originalMaterial = spriteRenderer.material;
    }
    
    public int getHP()
    {
        return HP;
    }

    public void doDamage(GameObject emitter, int damage)
    {
        if (!isDamageable(emitter))  // Am I damaging myself?
            return;

        if (isInvincible)               // Invincible?
            return;

        GameObject dmgText = Instantiate(floatingText) as GameObject;
        dmgText.GetComponent<FloatingText>().initialize(gameObject, damage);

        if (movingChar)
            movingChar.receivesDamage();
       
        HP -= damage;
        if (HP <= 0)
        {
            if (movingChar)
                movingChar.die();
            Instantiate(deathAnimation, transform.position, Quaternion.identity);
            Destroy(gameObject);
            return;
        }

        StartCoroutine(makeInvincible(invincibilityTime)); // Make invincible
    }
    
    /// <summary>
    /// Heal for a certain amount, and create a green floating text
    /// </summary>
    /// <param name="life"></param>
    public void heal(int life)
    {
        HP += life;
        if (HP >= maxHP)
            HP = maxHP;

        GameObject healText = Instantiate(floatingText) as GameObject;
        healText.GetComponent<FloatingText>().initialize(gameObject, life);
        healText.GetComponent<FloatingText>().setColor(Color.green);
    }

    public float getHPRatio()
    {
        return (float)HP / (float)maxHP;
    }

    /// <summary>
    ///  Can this object damage me?
    /// </summary>
    /// <param name="emitterTag"></param>
    /// <returns></returns>
    private bool isDamageable(GameObject emitter)
    {
        if (emitter == null)    // If the emitter no longer exists, the damage can surely happen
            return true;
        if (emitter == gameObject)  // If the emitter is the same as the receiver, there is no damage
            return false;
        return true;
    }

    /// <summary>
    /// Make the character invincible and flash for the duration
    /// </summary>
    /// <param name="duration"></param>
    /// <returns></returns>
    IEnumerator makeInvincible(float duration)
    {
        float startingTime = Time.time;
        isInvincible = true;
        while (Time.time - startingTime < duration)
        {
            if (spriteRenderer.material == originalMaterial)
                spriteRenderer.material = flashingMaterial;
            else
                spriteRenderer.material = originalMaterial;

            // Flickering frequence
            yield return new WaitForSeconds(0.05f);

        }
        isInvincible = false;
        spriteRenderer.material = originalMaterial;
    }

    public void healOverTime(int life, float duration)
    {
        StartCoroutine(healOverTimeRoutine(life, duration));
    }

    private IEnumerator healOverTimeRoutine(int life, float duration)
    {
        if (healAnimation)
        {
            healEffects++;
            if (!healingAnimation)
            {
                healingAnimation = Instantiate(healAnimation, transform.position, Quaternion.identity) as GameObject;
                healingAnimation.transform.SetParent(transform);
            }
        }

        for (int i = 0; i < 10; i++)
        {
            heal(Mathf.RoundToInt(life / duration));
            yield return new WaitForSeconds(duration / 10f);
        }

        if (healAnimation)
        {
            healEffects--;
            if (healEffects <= 0)
            {
                healEffects = 0;
                Destroy(healingAnimation);
                healingAnimation = null;
            }
        }
    }

}
