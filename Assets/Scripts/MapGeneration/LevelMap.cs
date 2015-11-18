using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using Random = UnityEngine.Random;

public class LevelMap : MonoBehaviour
{
    private int rows = 0;
    private int columns = 0;
    private GameObject[,] grid;

    public Transform tilesHolder;
    private AStarPathFinder pathFinder;

    public void Awake()
    {
        pathFinder = gameObject.AddComponent<AStarPathFinder>();
        pathFinder.initialize(this);
    }

    public GameObject getTile(int x, int y)
    {
        if (!checkValues(x, y))
            return null;
        else
            return grid[y, x];
    }

    public void initialize(int rows, int columns)
    {
        createTileHolder();
        this.rows = rows;
        this.columns = columns;
        grid = new GameObject[columns, rows];
    }

    public void createTileHolder()
    {
        tilesHolder = new GameObject("Tiles").transform;
        tilesHolder.SetParent(gameObject.transform);
    }

    public void destroyMap()
    {
        Destroy(GameObject.Find("Tiles"));
        Destroy(GameObject.Find("Monsters"));
        Destroy(GameObject.Find("Torches"));
        GameObject[] spells = GameObject.FindGameObjectsWithTag("Spell");
        foreach (GameObject spell in spells)
            Destroy(spell);
    }

    public int getColumns()
    {
        return columns;
    }

    public int getRows()
    {
        return rows;
    }

    public void initPathFinder()
    {
        pathFinder.initialize(this);
    }

    /// <summary>
    /// Pick a wall and a direction to dig into
    /// </summary>
    /// <returns>An object and a direction</returns>
    public ObjectDirection pickRandomWall()
    {
        GameObject[] floorTiles = GameObject.FindGameObjectsWithTag("Floor");
        if (floorTiles.Length == 0)
        {
            Debug.LogError("Not a single floor tile to be found");
            return null;
        }
        List<ObjectDirection> result = new List<ObjectDirection>();
        Vector2i[] directions = new Vector2i[] { new Vector2i(1, 0), new Vector2i(-1, 0), new Vector2i(0, 1), new Vector2i(0, -1) };
        foreach (GameObject tile in floorTiles)
        {
            foreach (Vector2i direction in directions)
            {
                int x = Mathf.RoundToInt(tile.transform.position.x + direction.x);
                int y = Mathf.RoundToInt(tile.transform.position.y + direction.y);
                GameObject obj = getTile(x, y);
                if (obj != null && getTile(x, y).CompareTag("Wall"))
                {
                    result.Add(new ObjectDirection(getTile(x, y), direction));
                }
            }
        }
        if (result.Count == 0)
            return null;
        return result[Random.Range(0, result.Count)];
    }

    /// <summary>
    /// Instantiate or replace a tile taken randomly from the pool of objects
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="objects"></param>
    public bool createTile(int x, int y, params GameObject[] objects)
    {
        if (!checkValues(x, y))
            return false;
        if (grid[y, x] != null)
            Destroy(grid[y, x]);

        grid[y, x] = Instantiate(Utils.pickRandom(objects), new Vector3(x, y), Quaternion.identity) as GameObject;
        grid[y, x].transform.SetParent(tilesHolder);
        return true;
    }

    public bool createTile(Vector2i pos, params GameObject[] objects)
    {
        return createTile(pos.x, pos.y, objects);
    }


    public bool checkValues(int startX, int startY, int width, int height)
    {
        if (startX < 0 || startY < 0 || width < 0 || height < 0)
            return false;

        if (startX + width > rows || startY + height > columns)
            return false;

        return true;
    }

    /// <summary>
    /// check that the values are not out of the grid
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <returns></returns>
    public bool checkValues(int x, int y)
    {
        if (x < 0 || x > rows - 1 || y < 0 || y > columns - 1)
            return false;
        return true;
    }

    public List<Vector2i> getNeighbors(Vector2i current)
    {
        List<Vector2i> result = new List<Vector2i>();
        Vector2i[] directions = new Vector2i[] { new Vector2i(1, 0), new Vector2i(-1, 0), new Vector2i(0, 1), new Vector2i(0, -1) };
        foreach (Vector2i dir in directions)
        {
            Vector2i neighbor = current + dir;
            if (checkValues(neighbor.x, neighbor.y))
                if (grid[neighbor.y, neighbor.x].layer != LayerMask.NameToLayer("BlockingLayer"))
                    result.Add(neighbor);
        }
        return result;
    }

    public Stack<Vector2i> getPath(Vector2i start, Vector2i goal)
    {
        return pathFinder.getPath(start, goal);
    }

    public List<GameObject> getFloorTilesWithSpace()
    {
        GameObject[] floorTiles = GameObject.FindGameObjectsWithTag("Floor");

        if (floorTiles.Length == 0)
        {
            Debug.LogError("Not a single floor tile found!");
            return null;
        }

        List<GameObject> floorTilesNotNearWall = new List<GameObject>();
        foreach (GameObject tile in floorTiles)
        {
            if (getNeighbors(new Vector2i(tile.transform.position)).Count == 4)  // This tile is not near a wall
                floorTilesNotNearWall.Add(tile);

        }
        return floorTilesNotNearWall;
    }

    /// <summary>
    /// Verify if there are item closer than the distance specified of the specified tag
    /// </summary>
    /// <param name="position"></param>
    /// <param name="tag"></param>
    /// <param name="distance"></param>
    /// <returns></returns>
    public bool hasItemAround(Vector3 position, string tag, float distance)
    {
        GameObject[] items = GameObject.FindGameObjectsWithTag(tag);
        foreach (GameObject item in items)
        {
            if ((position - item.transform.position).magnitude < distance)
                return true;
        }
        return false;
    }

    public GameObject getFurthestFloorTile(Transform obj)
    {
        GameObject[] floorTiles = GameObject.FindGameObjectsWithTag("Floor");

        if (floorTiles.Length == 0)
        {
            Debug.LogError("Not a single floor tile found!");
            return null;
        }

        GameObject result = null;
        float maxDistance = 0;
        foreach (GameObject tile in floorTiles)
        {
            if (getNeighbors(new Vector2i(tile.transform.position)).Count == 4)  // This tile is not near a wall
                if ((tile.transform.position - obj.position).magnitude > maxDistance)
                {
                    result = tile;
                    maxDistance = (tile.transform.position - obj.position).magnitude;
                }
        }
        return result;
    }
}
