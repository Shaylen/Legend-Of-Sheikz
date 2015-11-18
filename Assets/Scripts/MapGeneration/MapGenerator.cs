using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MapGenerator : MonoBehaviour
{
    public int rows;
    public int columns;

    protected LevelMap levelMap;
   
    public abstract void generateMap(LevelMap map);

    /// <summary>
    /// Fill a rectangle with a randomized set of objects
    /// </summary>
    /// <param name="startX">starting X position</param>
    /// <param name="startY">starting Y position</param>
    /// <param name="width">width of the rectangle</param>
    /// <param name="height">height of the rectangle</param>
    /// <param name="objects">an array of objects to be picked from</param>
    protected bool fillRect(int startX, int startY, int width, int height, params GameObject[] objects)
    {
        if (!levelMap.checkValues(startX, startY, width, height))
            return false;

        for (int x = startX; x < startX + width; x++)
        {
            for (int y = startY; y < startY + height; y++)
            {
                levelMap.createTile(x, y, objects);
            }
        }
        return true;
    }

    

    /// <summary>
    /// Check if all the objects in a specified rectangle are of the specified tag
    /// </summary>
    /// <param name="startX"></param>
    /// <param name="startY"></param>
    /// <param name="width"></param>
    /// <param name="height"></param>
    /// <param name="tag"></param>
    /// <returns></returns>
    protected bool checkRect(int startX, int startY, int width, int height, string tag)
    {
        if (!levelMap.checkValues(startX, startY, width, height))
            return false;

        for (int x = startX; x < startX + width; x++)
        {
            for (int y = startY; y < startY + height; y++)
            {
                if (!levelMap.getTile(x, y).CompareTag(tag))
                {
                    return false;
                }
            }
        }
        return true;
    }
}
