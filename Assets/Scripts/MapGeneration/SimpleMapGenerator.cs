using UnityEngine;
using System.Collections;
using System;
using Random = UnityEngine.Random;
using System.Collections.Generic;

public class SimpleMapGenerator : MapGenerator
{
    [Range(0, 1000)]
    public int roomsNumber;
    public Count roomSize;
    [Range(0, 200)]
    public float timeAllowed;
    [Range(0, 10)]
    public int spaceBetweenRooms;
    [Range(0, 5)]
    public int spaceBetweenCorridors;
    [Range(1, 50)]
    public int corridorMaxLength;
    [Range(0, 10000)]
    public int monsterNumber;
    public int torchNumber;
    public float distanceBetweenTorchs;
    public GameObject[] floorTiles;
    public GameObject[] wallTiles;
    public GameObject frontierTile;
    public GameObject[] monsters;
    public GameObject exitTile;
    public GameObject torch;

    private List<Func<ObjectDirection, bool>> featureList;
    private int featuresCount = 0, roomsCount = 0, corridorsCount = 0;
    private GameObject hero;

    void Awake()
    {
        setupFeatures();
        hero = GameObject.Find("Hero");
    }

    public override void generateMap(LevelMap map)
    {
        featuresCount = 0;
        roomsCount = 0;
        corridorsCount = 0;

        levelMap = map;
        levelMap.initialize(rows, columns);
        setupWorld();
        putHero();
        createExit();
        createMonsters();
        map.initPathFinder();
        createTorchs();
    }


    private void putHero()
    {
        if (!hero)
        {
            Debug.LogError("Hero not found!");
            return;
        }
        // Choose a starting tile not close to a wall
        GameObject startingTile = Utils.pickRandom(levelMap.getFloorTilesWithSpace());
        hero.transform.position = startingTile.transform.position;
    }

    private void createExit()
    {
        GameObject exit = levelMap.getFurthestFloorTile(hero.transform);
        if (!exit)
        {
            Debug.LogError("Error while calculating Exit");
            return;
        }
        levelMap.createTile(new Vector2i(exit.transform.position), exitTile);
    }

    private void setupFeatures()
    {
        featureList = new List<Func<ObjectDirection, bool>>();
        //featureList.Add(createRoom);
        //featureList.Add(createCorridor);
        featureList.Add(createCorridorWithRoom);
    }
    
    private void setupWorld()
    {
        fillWithWalls();
        //fillWithFloors();
        // Dig a room in the center of the map
        fillRect(rows / 2, columns / 2, 5, 5, floorTiles);
        
        float startingTime = Time.realtimeSinceStartup;

        while (featuresCount < roomsNumber)
        {
            if (Time.realtimeSinceStartup - startingTime > timeAllowed)
            {
                Debug.Log("Allowed generation time is elapsed.");
                break;
            }
            ObjectDirection randomWall = levelMap.pickRandomWall();
            if (randomWall == null)
                return;

            if (featureList[Random.Range(0, featureList.Count)](randomWall)) // Pick a random feature to build
                featuresCount++;
        }
        Debug.Log(roomsCount+" rooms were generated.");
        Debug.Log(corridorsCount + " corridors were generated.");
    }


    private void createMonsters()
    {
        if (monsters.Length == 0)
        {
            Debug.LogError("No monsters defined!");
            return;
        }
        GameObject[] floorTiles = GameObject.FindGameObjectsWithTag("Floor");
        if (floorTiles.Length == 0)
        {
            Debug.LogError("Not a single floor tile to put monsters!");
            return;
        }

        // Only take the tiles that are far enough from the hero
        List<GameObject> floorTilesToPutMonster = new List<GameObject>();
        foreach (GameObject tile in floorTiles)
        {
            if ((tile.transform.position - hero.transform.position).magnitude > 10)      // Arbritrary safe distance value (greater than vision distance of mobs)
                floorTilesToPutMonster.Add(tile);
        }

        Transform monsterHolder = new GameObject("Monsters").transform;
        monsterHolder.SetParent(levelMap.transform);
        for (int i = 0; i < monsterNumber; i++)
        {
            Vector3 positionToPutMonster = Utils.pickRandom(floorTilesToPutMonster).transform.position;
            GameObject newMonster = Instantiate(Utils.pickRandom(monsters), positionToPutMonster, Quaternion.identity) as GameObject;
            newMonster.transform.SetParent(monsterHolder);
            newMonster.GetComponent<MonsterController>().Initialize(levelMap);
        }
    }

    private void createTorchs()
    {
        Transform torchHolder = new GameObject("Torches").transform;
        int torchPlaced = 0;
        float startingTime = Time.realtimeSinceStartup;
        while (torchPlaced < torchNumber)
        {
            if (Time.realtimeSinceStartup - startingTime > 1.0f) // Avoid infinite loops
            {
                Debug.Log("Max time elapsed for torch creation");
                break;
            }

            ObjectDirection wall = levelMap.pickRandomWall();
            if (levelMap.hasItemAround(wall.gameObject.transform.position, "Torch", distanceBetweenTorchs))
                continue;

            Vector3 torchPosition;
            if (wall.direction.y != 1)
                torchPosition = wall.gameObject.transform.position - wall.direction.toVector3() * 0.5f;
            else
                torchPosition = wall.gameObject.transform.position;

            GameObject newTorch = Instantiate(torch, torchPosition, Quaternion.identity) as GameObject;
            if (wall.direction.x == -1)
                newTorch.transform.localScale = new Vector3(-1, 1, 1);
            newTorch.transform.SetParent(torchHolder);

            torchPlaced++;
        }
    }

    private bool createRoom(ObjectDirection wall)
    {
        int roomWidth = Random.Range(roomSize.minimum, roomSize.maximum+1);
        int roomHeight = Random.Range(roomSize.minimum, roomSize.maximum+1);
        Vector2i roomOffset = new Vector2i(Random.Range(0, roomWidth), Random.Range(0, roomHeight));
        Vector2i roomOrigin = new Vector2i(0, 0);

        //Debug.Log("Chose the wall at " + wall.x + ","+wall.y);

        switch(wall.getDirection())
        {
            case Direction.UP:
                roomOrigin.x = wall.x - roomWidth + 1 + roomOffset.x;
                roomOrigin.y = wall.y + 1;
                break;
            case Direction.RIGHT:
                roomOrigin.x = wall.x + 1;
                roomOrigin.y = wall.y - roomOffset.y;
                break;
            case Direction.LEFT:
                roomOrigin.x = wall.x - roomWidth;
                roomOrigin.y = wall.y - roomOffset.y;
                break;
            case Direction.DOWN:
                roomOrigin.x = wall.x - roomOffset.x;
                roomOrigin.y = wall.y - roomHeight;
                break;
            default:
                Debug.LogError("Unknown Direction found: " + wall.getDirection().ToString());
                break;

        }
        // Check if the rectangle is available (filled with walls)
        if (!checkRect(roomOrigin.x-spaceBetweenRooms, roomOrigin.y-spaceBetweenRooms, roomWidth+ spaceBetweenRooms*2, roomHeight+ spaceBetweenRooms*2, "Wall"))
            return false;

        //Debug.Log("room Origin: " + roomOrigin.ToString());
        // If so, create a room
        fillRect(roomOrigin.x, roomOrigin.y, roomWidth, roomHeight, floorTiles);
        // Create the entrace
        fillRect(wall.x, wall.y, 1, 1, floorTiles);

        roomsCount++;
        return true;
    }

    /// <summary>
    ///  Create a corridor starting at the specified wall
    /// </summary>
    /// <param name="wall"></param>
    /// <returns></returns>
    private bool createCorridor(ObjectDirection wall)
    {
        int corridorLength = Random.Range(1, corridorMaxLength + 1);
        if (!checkCorridor(wall, corridorLength))
            return false;

        digCorridor(wall, corridorLength);
        return true;
    }

    private void digCorridor(ObjectDirection wall, int corridorLength)
    {
        int x = wall.x;
        int y = wall.y;
        for (int i = 0; i < corridorLength; i++)
        {
            levelMap.createTile(x, y, floorTiles);
            x += wall.direction.x;
            y += wall.direction.y;

        }

        corridorsCount++;
    }

    /// <summary>
    /// Check if the corridor is feasible
    /// </summary>
    /// <param name="wall"></param>
    /// <param name="length"></param>
    /// <returns></returns>
    protected bool checkCorridor(ObjectDirection wall, int length)
    {
        Count xRange = new Count(1, 1);
        Count yRange = new Count(1, 1);
        switch (wall.getDirection())
        {
            case Direction.UP:
            case Direction.DOWN:
                xRange = new Count(wall.x - spaceBetweenCorridors, wall.x + spaceBetweenCorridors);
                yRange = new Count(wall.y, wall.y);
                break;
            case Direction.LEFT:
            case Direction.RIGHT:
                xRange = new Count(wall.x, wall.x);
                yRange = new Count(wall.y - spaceBetweenCorridors, wall.y + spaceBetweenCorridors);
                break;
        }
        for (int startingX = xRange.minimum; startingX <= xRange.maximum; startingX++)
        {
            for (int startingY = yRange.minimum; startingY <= yRange.maximum; startingY++)
            {
                int x = startingX;
                int y = startingY;
                for (int i = 0; i < length+spaceBetweenCorridors; i++)
                {
                    GameObject obj = levelMap.getTile(x, y);
                    if (obj == null || !obj.CompareTag("Wall"))
                    {
                        return false;
                    }

                    x += wall.direction.x;
                    y += wall.direction.y;
                }
            }
        }
        return true;
    }

    private bool createCorridorWithRoom(ObjectDirection wall)
    {
        // First, check if a corridor can be put there
        int corridorLength = Random.Range(1, corridorMaxLength + 1);
        if (!checkCorridor(wall, corridorLength))
            return false;
        // If the corridor is ok, we check if we can put a room
        ObjectDirection roomEntrace = new ObjectDirection(levelMap.getTile(wall.x + wall.direction.x * (corridorLength-1), wall.y + wall.direction.y * (corridorLength-1)), wall.direction);
        if (!createRoom(roomEntrace))
            return false;

        digCorridor(wall, corridorLength);

        return true;
    }

    /// <summary>
    /// Fill the world with walls, and frontier tiles at the edge
    /// </summary>
    private void fillWithWalls()
    {
        // Fill the Edges
        fillRect(0, 0, rows, columns, frontierTile);
        // Fill the Walls
        fillRect(1, 1, rows-2, columns-2, wallTiles);
    }

    /// <summary>
    /// Fill the world with walls, and frontier tiles at the edge
    /// </summary>
    private void fillWithFloors()
    {
        // Fill the Edges
        fillRect(0, 0, rows, columns, frontierTile);
        // Fill the Floor
        fillRect(1, 1, rows - 2, columns - 2, floorTiles);
    }

}
