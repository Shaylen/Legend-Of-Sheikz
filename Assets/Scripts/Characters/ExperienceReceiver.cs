using UnityEngine;
using System.Collections;

public class ExperienceReceiver : MonoBehaviour
{
	public float levelMultiplier;
	public int XPForFirstLevel;

	private int level;
	private int currentXP;
	private int totalXP;
	private int XPToNextLevel;
	
	// Use this for initialization
	void Start ()
	{
		level = 1;  // Level start at 1
		totalXP = 0;
		currentXP = 0;
		XPToNextLevel = XPForFirstLevel;
	}

	public void addXP(int xp)
	{
        Debug.Log("Received " + xp + " xp");
		currentXP += xp;
		if (currentXP >= XPToNextLevel)
		{
            levelUp();
		}
	}

    public void levelUp()
    {
        level++;
        Debug.Log("Level up! now level: " + level);
        XPToNextLevel = Mathf.RoundToInt(XPToNextLevel * levelMultiplier);
        currentXP = 0;
    }

	public float getXPRatio()
	{
		return (float)currentXP / (float)XPToNextLevel;
	}

    public int getLevel()
    {
        return level;
    }
}
