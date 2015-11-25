using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class LevelUpManager : MonoBehaviour {

    public GameObject[] possibleSkillList;  // All the possible skills

    private GameObject hero;
    private Image[] skillIcons;
    private GameObject[] skillChoices;     // The 3 skills that presented to the player at levelup
    private GameObject levelUpText;

    void Start()
    {
        hero = GameObject.Find("Hero");
        setupSkillIcons();
        skillChoices = new GameObject[3];
    }

    public void setupSkillIcons()
    {
        levelUpText = transform.Find("LevelUpText").gameObject;

        skillIcons = new Image[3];
        skillIcons[0] = transform.Find("Skill1").GetComponent<Image>();
        skillIcons[1] = transform.Find("Skill2").GetComponent<Image>();
        skillIcons[2] = transform.Find("Skill3").GetComponent<Image>();

        setIconsActive(false);
    }

    public void setIconsActive(bool a)
    {
        for (int i=0; i < skillIcons.Length; i ++)
        {
            skillIcons[i].gameObject.SetActive(a);
        }
        levelUpText.SetActive(a);
    }

    /// <summary>
    ///  Coming from the hero where there is a levelup. Need to display the interface
    /// </summary>
    public void levelUp()
    {
        setIconsActive(true);
        for (int i = 0; i < 3; i++)
        {
            skillChoices[i] = Utils.pickRandom(possibleSkillList);
            skillIcons[i].sprite = skillChoices[i].GetComponent<SpriteRenderer>().sprite;
        }
    }

    public void chooseSkill(int skillNumber)
    {
        skillChoices[skillNumber].GetComponent<Skill>().applySkill(hero);
        setIconsActive(false);
    }
}
