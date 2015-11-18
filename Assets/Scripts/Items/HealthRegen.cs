using UnityEngine;
using System.Collections;

public class HealthRegen : MonoBehaviour
{
    public int life;
    public float duration;

    void OnTriggerEnter2D(Collider2D other)
    {
        Damageable dmg = other.GetComponent<Damageable>();
        if (dmg && dmg.isHealable)
        {
            dmg.healOverTime(life, duration);
            Destroy(gameObject);
        }
    }
}
