using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayerController : MovingCharacter
{
    public GameObject heroLight;
    public GameObject primarySpell;
    public GameObject secondarySpell;
    public GameObject defensiveSpell;

    new void Start()
    {
        base.Start();
        GameObject newLight = Instantiate(heroLight);
        newLight.transform.SetParent(transform);
        newLight.transform.localPosition = new Vector3(0, 0, -1);
    }

    
    void FixedUpdate()
    {
        float inputX = Input.GetAxisRaw("Horizontal");
        float inputY = Input.GetAxisRaw("Vertical");
        Vector2 movement = new Vector2(inputX, inputY).normalized * speed;

        updateAnimations(movement);
        rb.velocity = movement;

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            Vector3 target = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            target.z = 0;    // fix because camera see point at z = -5
            castSpell(primarySpell, transform.position, target);
        }
        else if (Input.GetButtonDown("Fire2"))
        {
            Vector3 target = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            target.z = 0;    // fix because camera see point at z = -5
            castSpell(secondarySpell, transform.position, target);
        }
        else if (Input.GetButtonDown("Jump"))
        {
            castSpell(defensiveSpell);
        }
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

