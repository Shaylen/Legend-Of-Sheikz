using UnityEngine;
using System.Collections;

public class Exit : MonoBehaviour
{

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Hero"))
            if (other.bounds.Contains(transform.position))
                GameManager.instance.endLevel();
    }
}
