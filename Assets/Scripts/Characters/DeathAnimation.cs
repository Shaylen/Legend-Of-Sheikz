using UnityEngine;
using System.Collections;

public class DeathAnimation : MonoBehaviour
{

    public float speed;
    [Range(0, 10)]
    public float duration;

    private GameObject parent;
    new private SpriteRenderer renderer;

    void Awake()
    {
        renderer = GetComponent<SpriteRenderer>();
        StartCoroutine(fadeAfterSeconds(duration));
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += Vector3.up * speed;
    }

    IEnumerator fadeAfterSeconds(float duration)
    {
        float startingTime = Time.time;
        while (Time.time - startingTime < duration)
        {
            Color newColor = renderer.color;
            newColor.a = Mathf.Lerp(1, 0, (Time.time - startingTime) / duration);
            renderer.color = newColor;
            yield return null;
        }
        Destroy(gameObject);
    }
}
