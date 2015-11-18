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
        //Debug.Log("Getting destroyed: " + gameObject.name);
        // Loop for each lootable item and drop it according to the loot chance
        for (int i=0; i < items.Length; i++)
        {
            if (Random.Range(0f, 1f) <= lootChance[i])
            {
                if (GameManager.instance.isShuttingDown)
                    return;
                GameObject newItem = Instantiate(items[i], transform.position, Quaternion.identity) as GameObject;
                newItem.transform.SetParent(GameManager.instance.map.transform);
            }
        }
    }
}
