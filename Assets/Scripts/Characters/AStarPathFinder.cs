using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;


public class AStarPathFinder : MonoBehaviour
{
	private LevelMap map;

	public void initialize(LevelMap map)
	{
		this.map = map;
	}

	public Stack<Vector2i> getPath(Vector2i start, Vector2i goal)
	{
		HashSet<Vector2i> closedSet = new HashSet<Vector2i>();
		HashSet<Vector2i> openSet = new HashSet<Vector2i>();
		openSet.Add(start);
		int rows = map.getRows();
		int columns = map.getColumns();
		int maxLengthAllowed = 200;

		// Creation and initialization of both scores to Infinity
		float[,] gScore = new float[columns, rows];
		float[,] fScore = new float[columns, rows];
		Dictionary<Vector2i, Vector2i> cameFrom = new Dictionary<Vector2i, Vector2i>();

		for (int x = 0; x < rows; x++)
		{
			for (int y = 0; y < columns; y++)
			{
				gScore[y, x] = Mathf.Infinity;
				fScore[y, x] = Mathf.Infinity;
			}
		}
		gScore[start.y, start.x] = 0;
		fScore[start.y, start.x] = start.sqrDistanceTo(goal);

		while (openSet.Count > 0)
		{
			Vector2i current = getLowest(openSet, fScore);
			if (current.Equals(goal))
			{
				return reconstructPath(cameFrom, goal);
			}

			openSet.Remove(current);
			closedSet.Add(current);
			List<Vector2i> neighbors = map.getNeighbors(current);
            if (neighbors.Count == 0)
                Debug.Log("Neighbor count: " + neighbors.Count);
			foreach (Vector2i neighbor in neighbors)
			{
				if (closedSet.Contains(neighbor))
					continue;       // Ignore the neighbor that is already evaluated

				float tentativeGScore = gScore[current.y, current.x] + 1;   // Using 1 to estimate the distance between tiles. May use a cost value instead
                if (tentativeGScore >= maxLengthAllowed)
                {
                    return null;
                }

				if (!openSet.Contains(neighbor))
					openSet.Add(neighbor);
				else if (tentativeGScore >= gScore[neighbor.y, neighbor.x])
					continue;

				cameFrom[neighbor] = current;
				gScore[neighbor.y, neighbor.x] = tentativeGScore;
				fScore[neighbor.y, neighbor.x] = tentativeGScore + neighbor.sqrDistanceTo(goal);
			}
		}
        Debug.Log("openSet: " + openSet.Count);
		Debug.Log("End of algorithm");
		return null;

	}

	private Vector2i getLowest(HashSet<Vector2i> set, float[,] score)
	{
		Vector2i result = new Vector2i(0, 0);
		float lowestScore = Mathf.Infinity;
		foreach (Vector2i v in set)
		{
			if (score[v.y, v.x] < lowestScore)
			{
				lowestScore = score[v.y, v.x];
				result = v;
			}
		}
		return result;
	}

	private Stack<Vector2i> reconstructPath(Dictionary<Vector2i, Vector2i> cameFrom, Vector2i current)
	{
		Stack<Vector2i> path = new Stack<Vector2i>();
		path.Push(current);
		while (cameFrom.ContainsKey(current))
		{
			current = cameFrom[current];
			path.Push(current);
		}
		return path;
	}
}


