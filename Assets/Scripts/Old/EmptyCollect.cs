using UnityEngine;
using System.Collections;

public class EmptyCollect : MonoBehaviour 
{
	private GameObject[] spawners;

	void Update () 
	{
		spawners = GameObject.FindGameObjectsWithTag ("UnSpawnable");
	}

	void OnTriggerStay2D(Collider2D empty)
	{
		if (empty.tag == "AttachedTile") 
		{
			if (empty.GetComponent<WordCheck> ().completed || empty.gameObject.name == "")
			{
				foreach (GameObject s in spawners) 
				{
					s.GetComponent<SpawnRandom> ().SpawnEmpty ();
					Destroy (gameObject);
				}
			}
		}
	}
}
