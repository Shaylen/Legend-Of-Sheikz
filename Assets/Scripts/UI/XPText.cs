using UnityEngine;
using UnityEngine.UI;

public class XPText : MonoBehaviour
{
	private ExperienceReceiver heroXP;
	private Text text;
	private int level;

	// Use this for initialization
	void Start ()
	{
		text = GetComponent<Text>();
		heroXP = GameObject.Find("Hero").GetComponent<ExperienceReceiver>();
        if (!heroXP)
            Debug.Log("HeroXP not found");
	}

	void OnGUI()
	{
        text.text = "Lv" + level.ToString();
	}
	
	// Update is called once per frame
	void Update ()
	{
		level = heroXP.getLevel();
	}
}
