using UnityEngine;
using System.Collections;

public class HPBar : MonoBehaviour {

    public Vector2 pos;

    public Texture2D backgroundTexture;
    public Texture2D foregroundTexture;
    public Texture2D frame;

    private GameObject hero;
    private float ratio;
    private Vector2 healthSize = new Vector2(199, 30);
    private Vector2 frameSize = new Vector2(266, 65);

    void Start()
    {
        hero = GameObject.Find("Hero");
    }

    void OnGUI()
    {
        GUI.DrawTexture(new Rect(pos.x, pos.y, frameSize.x, frameSize.y), backgroundTexture, ScaleMode.ScaleToFit, true, 0);
        GUI.DrawTexture(new Rect(pos.x+60, pos.y+25, healthSize.x * ratio, healthSize.y), foregroundTexture, ScaleMode.ScaleAndCrop, true, 0);
        GUI.DrawTexture(new Rect(pos.x, pos.y, frameSize.x, frameSize.y), frame, ScaleMode.ScaleToFit, true, 0);
    }

    void Update()
    {
        if (hero)
            ratio = hero.GetComponent<Damageable>().getHPRatio();
        else
            ratio = 0;
    }
}
