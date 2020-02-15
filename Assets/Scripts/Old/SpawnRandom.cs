using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class SpawnRandom : MonoBehaviour 
{
	public GameObject spawner; //All the alphabet probabilities
	public GameObject clone;
	public GameObject emptyClone;
	public int[] all;
	public int index;

	// Use this for initialization
	void Start () 
	{
		Spawn ();
	}

	void Update()
	{
		if (Input.GetKeyDown(KeyCode.A))
			SpawnEmpty ();

		if (Input.GetMouseButtonDown (0)) 
		{
			RaycastHit2D hit;
			if (hit = Physics2D.Raycast (Camera.main.ScreenToWorldPoint (Input.mousePosition), Vector2.zero)) 
			{
				if (hit.collider.gameObject == emptyClone)
					clone.SetActive (true);
			}
		}

		if (clone != null) 
		{
			if (clone.GetComponent<WordCheck> ().completed) 
			{
				clone = null;
			}
		} 

		else 
		{
			Spawn ();
		}
	}

	public void Spawn()
	{
		index = Random.Range (0, all.Length);
		
		clone = (GameObject)Instantiate (spawner, transform.position, transform.rotation); //Getting current instantiated object
		clone.transform.parent = GameObject.Find("MainCamera").transform;
		clone.tag = "SpawnedTile";
		clone.layer = 11;

		if (all [index] == 0) 
		{
			clone.GetComponentInChildren<TextMesh> ().text = "A";	
			clone.GetComponent<WordCheck> ().score = 1;
		}

		if (all [index] == 1) 
		{
			clone.GetComponentInChildren<TextMesh> ().text = "B";
			clone.GetComponent<WordCheck> ().score = 3;
		}

		if (all [index] == 2) 
		{
			clone.GetComponentInChildren<TextMesh> ().text = "C";
			clone.GetComponent<WordCheck> ().score = 3;
		}

		if (all [index] == 3) 
		{
			clone.GetComponentInChildren<TextMesh> ().text = "D";	
			clone.GetComponent<WordCheck> ().score = 2;
		}

		if (all [index] == 4) 
		{
			clone.GetComponentInChildren<TextMesh> ().text = "E";	
			clone.GetComponent<WordCheck> ().score = 1;
		}

		if (all [index] == 5) 
		{
			clone.GetComponentInChildren<TextMesh> ().text = "F";	
			clone.GetComponent<WordCheck> ().score = 4;
		}

		if (all [index] == 6) 
		{
			clone.GetComponentInChildren<TextMesh> ().text = "G";	
			clone.GetComponent<WordCheck> ().score = 2;
		}

		if (all [index] == 7) 
		{
			clone.GetComponentInChildren<TextMesh> ().text = "H";	
			clone.GetComponent<WordCheck> ().score = 4;
		}

		if (all [index] == 8) 
		{
			clone.GetComponentInChildren<TextMesh> ().text = "I";	
			clone.GetComponent<WordCheck> ().score = 1;
		}

		if (all [index] == 9) 
		{
			clone.GetComponentInChildren<TextMesh> ().text = "J";	
			clone.GetComponent<WordCheck> ().score = 8;
		}

		if (all [index] == 10) 
		{
			clone.GetComponentInChildren<TextMesh> ().text = "K";
			clone.GetComponent<WordCheck> ().score = 5;
		}

		if (all [index] == 11) 
		{
			clone.GetComponentInChildren<TextMesh> ().text = "L";	
			clone.GetComponent<WordCheck> ().score = 1;
		}

		if (all [index] == 12) 
		{
			clone.GetComponentInChildren<TextMesh> ().text = "M";	
			clone.GetComponent<WordCheck> ().score = 3;
		}

		if (all [index] == 13) 
		{
			clone.GetComponentInChildren<TextMesh> ().text = "N";	
			clone.GetComponent<WordCheck> ().score = 1;
		}

		if (all [index] == 14) 
		{
			clone.GetComponentInChildren<TextMesh> ().text = "O";	
			clone.GetComponent<WordCheck> ().score = 1;
		}

		if (all [index] == 15) 
		{
			clone.GetComponentInChildren<TextMesh> ().text = "P";	
			clone.GetComponent<WordCheck> ().score = 3;
		}

		if (all [index] == 16) 
		{
			clone.GetComponentInChildren<TextMesh> ().text = "Q";	
			clone.GetComponent<WordCheck> ().score = 10;
		}

		if (all [index] == 17) 
		{
			clone.GetComponentInChildren<TextMesh> ().text = "R";	
			clone.GetComponent<WordCheck> ().score = 1;
		}

		if (all [index] == 18) 
		{
			clone.GetComponentInChildren<TextMesh> ().text = "S";	
			clone.GetComponent<WordCheck> ().score = 1;
		}

		if (all [index] == 19) 
		{
			clone.GetComponentInChildren<TextMesh> ().text = "T";	
			clone.GetComponent<WordCheck> ().score = 1;
		}

		if (all [index] == 20) 
		{
			clone.GetComponentInChildren<TextMesh> ().text = "U";	
			clone.GetComponent<WordCheck> ().score = 1;
		}

		if (all [index] == 21) 
		{
			clone.GetComponentInChildren<TextMesh> ().text = "V";	
			clone.GetComponent<WordCheck> ().score = 4;
		}

		if (all [index] == 22) 
		{
			clone.GetComponentInChildren<TextMesh> ().text = "W";	
			clone.GetComponent<WordCheck> ().score = 4;
		}

		if (all [index] == 23) 
		{
			clone.GetComponentInChildren<TextMesh> ().text = "X";	
			clone.GetComponent<WordCheck> ().score = 8;
		}

		if (all [index] == 24) 
		{
			clone.GetComponentInChildren<TextMesh> ().text = "Y";	
			clone.GetComponent<WordCheck> ().score = 4;
		}

		if (all [index] == 25) 
		{
			clone.GetComponentInChildren<TextMesh> ().text = "Z";	
			clone.GetComponent<WordCheck> ().score = 10;
		}
	}

	public void SpawnEmpty()
	{
		if (emptyClone == null) 
		{
			int random;
			random = Random.Range (1, 4);

			if (random == 1) 
			{
				emptyClone = (GameObject)Instantiate (spawner, transform.position, transform.rotation); //Getting current instantiated object
				emptyClone.transform.parent = GameObject.Find ("MainCamera").transform;
				emptyClone.GetComponentInChildren<TextMesh> ().text = "";	
				emptyClone.GetComponent<WordCheck> ().score = 0;
				emptyClone.GetComponent<SpriteRenderer> ().sortingOrder = 9;
				emptyClone.tag = "SpawnedTile";
				emptyClone.layer = 11;
				clone.SetActive (false);
			}
		}
	}

	void OnTriggerStay2D(Collider2D alphaCheck)
	{
		if ((alphaCheck.tag == "StaticTile" || alphaCheck.tag == "MovingTile" || alphaCheck.tag == "SpawnedTile") && alphaCheck.gameObject.name != "") 
		{
			//transform.tag = "UnSpawnable";

			GameObject alphaObject = alphaCheck.gameObject;
			if (alphaObject == clone) 
			{
				alphaObject.transform.position = transform.position;
				alphaObject.GetComponent<Rigidbody2D> ().isKinematic = true;
			}
		}
	}

	void OnTriggerExit2D(Collider2D alphaCheck)
	{
		/*if (alphaCheck.gameObject == clone) 
		{
			transform.tag = "Spawnable";
		}*/

		if (alphaCheck.gameObject == emptyClone) 
		{
			emptyClone = null;
		}
	}
}
