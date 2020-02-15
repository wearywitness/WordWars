using UnityEngine;
using System.Collections;

public class ActivateHighest : MonoBehaviour 
{
	public GameObject target;

	void OnTriggerEnter2D(Collider2D touch)
	{
		if (touch.tag == "AttachedTile") 
		{
			target.SetActive (true);
		}
	}
}
