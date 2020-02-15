using UnityEngine;
using System.Collections;

public class Destroy : MonoBehaviour 
{

	void OnTriggerEnter2D(Collider2D destruct)
	{
		if (destruct.tag == "StaticTile" || destruct.tag == "AttachedTile" || destruct.tag == "MovingTile") 
		{
			Destroy (destruct.gameObject);
		}
	}
}
