using UnityEngine;
using System.Collections;
using System;
using Random = UnityEngine.Random;
using System.Collections.Generic;

public static class Utils
{
	/// <summary>
	/// Pick a random GameObject from an array of objects
	/// </summary>
	/// <param name="objects">objects array</param>
	/// <returns>a random object</returns>
	public static GameObject pickRandom(params GameObject[] objects)
	{
        if (objects.Length == 0)
            return null;

		return objects[Random.Range(0, objects.Length)];
	}

    public static T pickRandom<T>(List<T> list)
    {
        if (list.Count == 0)
            return default(T);
        return list[Random.Range(0, list.Count)];
    }
}

public enum Direction { UP, LEFT, DOWN, RIGHT, UNKNOWN};

[System.Serializable]
public class Count
{
	public int minimum;
	public int maximum;
	public Count(int min, int max)
	{
		minimum = min;
		maximum = max;
	}
}

[System.Serializable]
public class Vector2i
{
	public int x;
	public int y;
	public Vector2i(int x, int y)
	{
		this.x = x;
		this.y = y;
	}

    public Vector2i(Vector3 v)
    {
        x = Mathf.RoundToInt(v.x);
        y = Mathf.RoundToInt(v.y);
    }

    public override bool Equals(object obj)
    {
        return obj is Vector2i && Equals((Vector2i)obj);
    }

    public bool Equals(Vector2i v)
    {
        return (x == v.x) && (y == v.y);
    }

    public override int GetHashCode()
    {
        return x * 17 + y * 31;
    }

    public Vector3 toVector3()
    {
        return new Vector3(x, y);
    }

    public float sqrDistanceTo(Vector2i goal)
    {
        return Mathf.Pow(goal.x - x, 2) + Mathf.Pow(goal.x - x, 2);
    }

    public static Vector2i operator+ (Vector2i a, Vector2i b)
    {
        return new Vector2i(a.x + b.x, a.y + b.y);
    }

    public override string ToString()
    {
        return "[" + x + "," + y + "]";
    }
}

/// <summary>
/// A class used to encapsulates an object and a direction
/// </summary>
[System.Serializable]
public class ObjectDirection
{
	public GameObject gameObject;
	public Vector2i direction;
	public int x = 0;
	public int y = 0;

	public ObjectDirection(GameObject o, Vector2i d)
	{
		gameObject = o;
		direction = d;
		x = Mathf.RoundToInt(gameObject.transform.position.x);
		y = Mathf.RoundToInt(gameObject.transform.position.y);
	}

	public Vector2i getStartingPosition()
	{
		int x = Mathf.RoundToInt(gameObject.transform.position.x + direction.x);
		int y = Mathf.RoundToInt(gameObject.transform.position.y + direction.y);
		return new Vector2i(x, y);
	}

	public Direction getDirection()
	{
		if (direction.x == 0 && direction.y == 1)
			return Direction.UP;
		else if (direction.x == 0 && direction.y == -1)
			return Direction.DOWN;
		else if(direction.x == 1 && direction.y == 0)
			return Direction.RIGHT;
		else if(direction.x == -1 && direction.y == 0)
			return Direction.LEFT;
		return Direction.UNKNOWN;
	}
}


