using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class FloatingText : MonoBehaviour
{
    public float speed;
    [Range(0, 10)]
    public float duration;

    private Text text;
    private RectTransform rectTransform;
    private Vector3 speedOffset;
    private GameObject parent;
    private Vector3 parentPosition;
    private float parentRadius;
    private float randomHorizontalOffset;

    void Awake()
    {
        text = GetComponent<Text>();
        transform.SetParent(GameObject.Find("Canvas").transform);
        rectTransform = GetComponent<RectTransform>();
        speedOffset = Vector3.zero;
        randomHorizontalOffset = Random.Range(-0.7f, 0.7f);
        StartCoroutine(fadeAfterSeconds(duration));
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (parent)
            parentPosition = parent.transform.position;

        speedOffset += Vector3.up * speed;
        rectTransform.position = Camera.main.WorldToScreenPoint
            (parentPosition
            + Vector3.up * parentRadius
            + Vector3.right * parentRadius * randomHorizontalOffset
            ) 
            + speedOffset;
	}

    public void initialize(GameObject parent, string t) // Initialization attached to a parent
    {
        text.text = t;
        this.parent = parent;
        parentPosition = parent.transform.position;
        parentRadius = parent.GetComponent<CircleCollider2D>().radius;
    }

    IEnumerator fadeAfterSeconds(float duration)
    {
        float startingTime = Time.time;
        while (Time.time - startingTime < duration)
        {
            Color newColor = text.color;
            newColor.a = Mathf.Lerp(1, 0, (Time.time - startingTime) / duration);
            text.color = newColor;
            yield return null;
        }
        Destroy(gameObject);
    }

    public void setColor(Color color)
    {
        text.color = color;
    }
}
