using UnityEngine;
using System.Collections;
using Random = UnityEngine.Random;
using System.Collections.Generic;
using System;

public class MonsterController : MovingCharacter
{
    public float goalDistance;
    public float visionDistance;
    public Count preferedDistance;
    public LayerMask visionLayer;
    public LayerMask aimingLayer;
    public GameObject primarySpell;

    private Vector3 target;     // short-term target
    private Vector3 goal;       // long-term target
    private Stack<Vector2i> path;
    private LevelMap map;       // Reference to the map
    private EnemyState state;
    private GameObject hero;    // Reference to the hero
    private bool onCoolDown = false;
    private bool hasStrafeValueExpired = true;
    private float strafeValue = 1f;

    private enum EnemyState { DoNothing, Wander, Chase };

    new void Start()
    {
        base.Start();
        hero = GameObject.Find("Hero");
    }

    public void Initialize(LevelMap map)
    {
        this.map = map;

        setNewGoal();
        computePathToGoal();
        state = EnemyState.Wander;
    }

    // This update is called once every fixed framerate
    void FixedUpdate()
    {
        actAccordingToState();
        updateAnimations();
    }

    void Update()
    {
        Debug.DrawLine(transform.position, target, Color.green);
        Debug.DrawLine(transform.position, goal, Color.red);
    }

    void actAccordingToState()
    {
        switch (state)
        {
            case EnemyState.Wander:
                doWander();
                break;
            case EnemyState.Chase:
                doChase();
                break;
        }

    }

    void doWander()
    {
        if (hero && inLineOfSight(hero))
        {
            state = EnemyState.Chase;
            return;
        }
        if (hasReached(goal))
            setNewGoal();
        if (hasReached(target))
            setNewTarget();

        updateMovementToReachTarget();
        moveToTarget();
    }

    void doChase()
    {
        if (!hero || !inLineOfSight(hero))
        {
            state = EnemyState.Wander;
            computePathToGoal();
            return;
        }

        Vector3 lineToHero = hero.transform.position - transform.position;
        float distanceToHero = lineToHero.magnitude;

        if (!onCoolDown && hasClearTarget(hero, distanceToHero))
        {
            Vector3 spellTarget = hero.transform.position;
            castSpell(primarySpell, transform.position, spellTarget);
            StartCoroutine(startCoolDown(primarySpell.GetComponent<SpellController>().cooldown));
        }

        doStrafe(distanceToHero, lineToHero);
        goal = hero.transform.position;

        moveToTarget();
    }

    private void doStrafe(float distanceToHero, Vector3 lineToHero) // Strafe according to the distance to the hero
    {
        Vector3 directionToHero = lineToHero.normalized;
        if (distanceToHero < preferedDistance.minimum)  // Too close to hero, go back
        {
            target = transform.position + directionToHero * -1;
        }
        else if (distanceToHero > preferedDistance.minimum && distanceToHero < preferedDistance.maximum)    // In the prefered distance, do random strafe
        {
            updateRandomTemporaryStrafeValue();
            target = transform.position + (Vector3.Cross(directionToHero, Vector3.back)) * strafeValue;
        }
        else
            target = hero.transform.position;

        movement = (target - transform.position).normalized * speed;
        direction = lineToHero;   // Face the hero
    }

    private void updateRandomTemporaryStrafeValue()
    {
        if (hasStrafeValueExpired)
            StartCoroutine(resetStrafeValue(Random.Range(1f,3f)));
    }

    private IEnumerator resetStrafeValue(float duration)
    {
        strafeValue = Random.Range(0, 2) * 2 - 1; // Random value between -1 and +1
        hasStrafeValueExpired = false;
        yield return new WaitForSeconds(duration);
        hasStrafeValueExpired = true;
    }

    void updateMovementToReachTarget()
    {
        movement = (target - transform.position).normalized * speed;
        direction = movement;
    }

    void moveToTarget()
    {
        rb.MovePosition(Vector3.MoveTowards(rb.position, target, speed * Time.fixedDeltaTime));
    }

    protected IEnumerator startCoolDown(float cooldownTime)
    {
        onCoolDown = true;
        yield return new WaitForSeconds(cooldownTime);
        onCoolDown = false;
    }

    bool hasReached(Vector3 t)
    {
        float distanceToTarget = (t - transform.position).sqrMagnitude;
        if (distanceToTarget <= float.Epsilon)
            return true;

        return false;
    }

    bool inLineOfSight(GameObject hero)
    {
        if ((transform.position - hero.transform.position).sqrMagnitude > (visionDistance * visionDistance))
            return false;

        circleCollider.enabled = false;
        RaycastHit2D hit = Physics2D.Linecast(transform.position, hero.transform.position, visionLayer);
        circleCollider.enabled = true;
        if (hit.collider.gameObject == hero)
            return true;
        return false;
    }

    bool hasClearTarget(GameObject hero, float distanceToHero)
    {
        if (distanceToHero > visionDistance)
            return false;

        circleCollider.enabled = false;
        RaycastHit2D hit = Physics2D.Linecast(transform.position, hero.transform.position, aimingLayer);
        circleCollider.enabled = true;
        if (hit.collider.gameObject == hero)
            return true;
        return false;
    }

    void setNewTarget()
    {
        if (path == null || path.Count == 0)
        {
            setNewGoal();
            return;
        }
        target = path.Pop().toVector3();
    }

    void setNewGoal()
    {
        goal = findRandomTileWithinRadius(goalDistance, "Floor").transform.position;
        computePathToGoal();
    }

    private GameObject findRandomTileWithinRadius(float radius, string tag)
    {
        List<GameObject> result = new List<GameObject>();
        GameObject[] floorTiles = GameObject.FindGameObjectsWithTag(tag);
        foreach (GameObject tile in floorTiles)
        {
            if ((tile.transform.position - transform.position).sqrMagnitude <= (radius * radius))
            {
                result.Add(tile);
            }
        }
        return Utils.pickRandom(result);
    }

    void computePathToGoal()
    {
        path = map.getPath(new Vector2i(transform.position), new Vector2i(goal));
        if (path == null)
        {
            Debug.Log("no path found!");
            return;
        }
        setNewTarget();
    }

    public override void die()
    {
    }

    public override void receivesDamage()
    {
    }
}