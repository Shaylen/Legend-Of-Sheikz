using UnityEngine;
using System.Collections;
using Random = UnityEngine.Random;
using System.Collections.Generic;
using System;

public class MonsterController : MovingCharacter
{
    public float goalDistance;
    public float visionDistance;
    public LayerMask heroAndBlockingLayer;
    public GameObject primarySpell;

    private Vector2 movement;
    private Vector3 target;     // short-term target
    private Vector3 goal;       // long-term target
    private Stack<Vector2i> path;
    private LevelMap map;       // Reference to the map
    private EnemyState state;
    private GameObject hero;    // Reference to the hero
    private bool onCoolDown = false;

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
        updateMovementToReachTarget();
        updateAnimations(movement);
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
    void updateMovementToReachTarget()
    {
        movement = (target - transform.position).normalized * speed;
        rb.velocity = movement;
        rb.MovePosition(Vector3.MoveTowards(rb.position, target, speed * Time.fixedDeltaTime));
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
    }

    void doChase()
    {
        if (!hero || !inLineOfSight(hero))
        {
            state = EnemyState.Wander;
            computePathToGoal();
            return;
        }
        if (!onCoolDown && hasClearTarget(hero))
        {
            Vector2 direction = hero.transform.position - transform.position;
            castSpell(primarySpell, direction);
            StartCoroutine(startCoolDown());
        }
        goal = hero.transform.position;
        target = hero.transform.position;

    }

    private IEnumerator startCoolDown()
    {
        onCoolDown = true;
        yield return new WaitForSeconds(1);
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
        RaycastHit2D hit = Physics2D.Linecast(transform.position, hero.transform.position, heroAndBlockingLayer);
        circleCollider.enabled = true;
        if (hit.collider.gameObject == hero)
            return true;
        return false;
    }

    bool hasClearTarget(GameObject hero)
    {
        if ((transform.position - hero.transform.position).sqrMagnitude > (visionDistance * visionDistance))
            return false;

        circleCollider.enabled = false;
        RaycastHit2D hit = Physics2D.Linecast(transform.position, hero.transform.position);
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