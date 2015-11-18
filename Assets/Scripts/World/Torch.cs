using UnityEngine;
using System.Collections;

public class Torch : MonoBehaviour
{
	public GameObject torchLight;

	// Use this for initialization
	void Start ()
	{
		GameObject newLight = Instantiate(torchLight, transform.position, Quaternion.identity) as GameObject;
		newLight.transform.SetParent(gameObject.transform);
		newLight.transform.localPosition = new Vector3(0, 0, -1);
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}
}
