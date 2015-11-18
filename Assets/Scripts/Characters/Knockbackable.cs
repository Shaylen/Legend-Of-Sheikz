using UnityEngine;
using System.Collections;

public class Knockbackable : MonoBehaviour {

    public float knockbackImmuneTime;

    private bool isImmuneKnockback;
    private Rigidbody2D rb;

    void Start ()
    {
        isImmuneKnockback = false;
        rb = GetComponent<Rigidbody2D>();
    }

    public void knockback(Vector2 direction, float duration)
    {
        StartCoroutine(knockbackRoutine(direction, duration));
    }

    public IEnumerator knockbackRoutine(Vector2 direction, float duration)
    {
        if (isImmuneKnockback)
            yield break;
        StartCoroutine(immunizeKnockback(knockbackImmuneTime));

        float startingTime = Time.time;
        while ((Time.time - startingTime) < duration)
        {
            if (rb == null)
                break;
            rb.AddForce(direction * Mathf.Lerp(1, 0, (Time.time - startingTime) / duration));
            yield return null;
        }
    }

    public IEnumerator immunizeKnockback(float time)
    {
        isImmuneKnockback = true;
        yield return new WaitForSeconds(time);
        isImmuneKnockback = false;
    }

}
