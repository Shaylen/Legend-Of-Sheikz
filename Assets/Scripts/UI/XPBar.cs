using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class XPBar : MonoBehaviour
{
    private Image XPBarImage;
    private ExperienceReceiver heroXP;
    private float ratio;

    void Start()
    {
        heroXP = GameObject.Find("Hero").GetComponent<ExperienceReceiver>();
        if (!heroXP)
            Debug.Log("HeroXP not found");
        XPBarImage = GetComponent<Image>();
    }

    void OnGUI()
    {
        XPBarImage.fillAmount = ratio;
    }

    void Update()
    {
        if (heroXP)
            ratio = heroXP.getXPRatio();
        else
            ratio = 0;
    }
}

