using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
	public static GameManager instance;

	public GameObject mapGenerator;
	public GameObject heroPrefab;
    public GameObject floatingText;

    [HideInInspector]
    public Image[] coolDownImages;
	[HideInInspector]
	public int levelNumber;
	[HideInInspector]
	public GameObject hero;
	[HideInInspector]
	public GameObject map;
	[HideInInspector]
	public bool isShuttingDown = false;
	[HideInInspector]
	public GameObject screenMask;
	[HideInInspector]
	public GameObject centerText;

	private MapGenerator mapGeneratorScript;
	private LevelMap mapComponent;

	void Awake ()
	{
		if (instance == null)
			instance = this;
		else if (instance != this)
			Destroy(gameObject);

		name = "GameManager";
		createMap();
		createHero();

		mapGeneratorScript = Instantiate(mapGenerator).GetComponent<MapGenerator>();
		mapGeneratorScript.generateMap(mapComponent);

		screenMask = GameObject.Find("ScreenMask");
        if (!screenMask)
        {
            Debug.LogError("Screen Mask not found!");
            Debug.Break();
        }
		centerText = GameObject.Find("CenterText");
        if (!centerText)
        {
            Debug.LogError("Center Text not found!");
            Debug.Break();
        }
        screenMask.SetActive(false);
        centerText.SetActive(false);

        coolDownImages = new Image[4];
        coolDownImages[2] = GameObject.Find("SpellIconDefensive").transform.Find("CooldownFill").GetComponent<Image>();
        if (!coolDownImages[2])
        {
            Debug.LogError("Defensive Spell cooldown image not found!");
            Debug.Break();
        }
        coolDownImages[0] = GameObject.Find("SpellIconPrimarySpell").transform.Find("CooldownFill").GetComponent<Image>();
        if (!coolDownImages[0])
        {
            Debug.LogError("Primary Spell cooldown image not found!");
            Debug.Break();
        }
        coolDownImages[1] = GameObject.Find("SpellIconSecondarySpell").transform.Find("CooldownFill").GetComponent<Image>();
        if (!coolDownImages[1])
        {
            Debug.LogError("Secondary Spell cooldown image not found!");
            Debug.Break();
        }
        coolDownImages[3] = GameObject.Find("SpellIconUltimate").transform.Find("CooldownFill").GetComponent<Image>();
        if (!coolDownImages[3])
        {
            Debug.LogError("Secondary Spell cooldown image not found!");
            Debug.Break();
        }

    }
	
	public void createHero()
	{
		hero = Instantiate(heroPrefab);
		hero.name = "Hero";
	}

	public void createMap()
	{
		map = new GameObject("Map");
		map.name = "Map";
		mapComponent = map.AddComponent<LevelMap>();
	}

	public void endLevel()
	{
		StartCoroutine(delayedEndLevel());
	}

	public IEnumerator delayedEndLevel()
	{
		Image screenImage = screenMask.GetComponent<Image>();
		Color originalColor = screenImage.color;
		screenImage.color = Color.black;

			centerText.SetActive(true);
			centerText.GetComponent<Text>().color = Color.white;
			centerText.GetComponent<Text>().text = "Loading...";

		levelNumber++;
		hero.SetActive(false);
		mapComponent.destroyMap();

		yield return null;      // This is necessary to let Unity destroy the objects at the next Update()

		mapGeneratorScript.generateMap(mapComponent);
		hero.SetActive(true);
		screenImage.color = originalColor;

		if (centerText)
		{
			centerText.SetActive(false);
		}

	}

	void OnApplicationQuit()
	{
		isShuttingDown = true;
	}
}
