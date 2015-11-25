using UnityEngine;
using System.Collections;
using Random = UnityEngine.Random;

public class ItemHolder : MonoBehaviour
{
	public GameObject[] items;
	[Range(0,1)]
	public float[] lootChance;

	void OnDestroy()
	{
		// Loop for each lootable item and drop it according to the loot chance
		for (int i=0; i < items.Length; i++)
		{
			if (Random.Range(0f, 1f) <= lootChance[i])
			{
				if (GameManager.instance.isShuttingDown)    // Avoid looting the items after the game is finished as they stay in the scene
					return;
				GameObject newItem = Instantiate(items[i], transform.position, Quaternion.identity) as GameObject;
				newItem.transform.SetParent(GameManager.instance.map.transform);
			}
		}
	}
}
