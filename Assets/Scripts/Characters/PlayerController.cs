using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
using System.Collections.Generic;

public class PlayerController : MovingCharacter
{
    public GameObject heroLight;
    public GameObject[] spellList;

    private bool[] isOnCoolDown;

    new void Start()
    {
        base.Start();
        GameObject newLight = Instantiate(heroLight);
        newLight.transform.SetParent(transform);
        newLight.transform.localPosition = new Vector3(0, 0, -1);
        isOnCoolDown = new bool[spellList.Length];
        for (int i = 0; i < isOnCoolDown.Length; i++)
            isOnCoolDown[i] = false;
    }

    
    void FixedUpdate()
    {
        float inputX = Input.GetAxisRaw("Horizontal");
        float inputY = Input.GetAxisRaw("Vertical");
        movement = new Vector2(inputX, inputY).normalized * speed;
        if (movement != Vector2.zero)
            direction = movement;

        updateAnimations();
        rb.velocity = movement;
    }

    // Update is called once per frame
    void Update()
    {
        List<int> buttonPressed = new List<int>();
        Vector3 target = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        target.z = 0;    // fix because camera see point at z = -5

        if (Input.GetButton("Fire1"))
        {
            buttonPressed.Add(0);
        }
        if (Input.GetButton("Fire2"))
        {
            buttonPressed.Add(1);
        }
        if (Input.GetButton("Jump"))
        {
            buttonPressed.Add(2);
        }
        if (Input.GetButton("Fire3"))
        {
            buttonPressed.Add(3);
        }

        foreach (int button in buttonPressed)
        {
            if (!isOnCoolDown[button])
            {
                StartCoroutine(startCooldown(button));
                castSpell(spellList[button], transform.position, target);
            }
        }
    }

    private IEnumerator startCooldown(int spellIndex)
    {
        isOnCoolDown[spellIndex] = true;
        float startingTime = Time.time;
        float cooldown = spellList[spellIndex].GetComponent<SpellController>().cooldown;
        while (Time.time - startingTime < cooldown)
        {
            GameManager.instance.coolDownImages[spellIndex].fillAmount = Mathf.Lerp(1, 0, (Time.time - startingTime) / cooldown);
            yield return null;
        }
        GameManager.instance.coolDownImages[spellIndex].fillAmount = 0;
        isOnCoolDown[spellIndex] = false;
    }

    public override void receivesDamage()
    {
        GameObject screenMask = GameManager.instance.screenMask;
        if (screenMask)
            StartCoroutine(enableForOneFrame(screenMask));
    }

    private IEnumerator enableForOneFrame(GameObject screenMask)
    {
        screenMask.SetActive(true);
        yield return null;
        screenMask.SetActive(false);
    }

    public override void die()
    {
        Instantiate(heroLight, transform.position + new Vector3(0, 0, -1), Quaternion.identity);
        GameObject centerText = GameManager.instance.centerText;
        GameObject screenMask = GameManager.instance.screenMask;

        if (centerText)
        {
            centerText.SetActive(true);
            centerText.GetComponent<Text>().color = Color.white;
            centerText.GetComponent<Text>().text = "Game Over";
        }

        if (screenMask)
        {
            screenMask.SetActive(true);
            screenMask.GetComponent<Image>().CrossFadeAlpha(1.0f, 5, false);
        }
    }
}

