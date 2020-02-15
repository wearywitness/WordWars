using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Drag : MonoBehaviour 
{
	public bool completed;
	public bool dragging;
	public GameObject[] attachedTiles;
	GameObject[] spawners;
	GameObject[] unspawners;
	public bool triggeredMass;

	private WordCheck thisWordCheck;
	private Rigidbody2D thisBody;

	void Update () 
	{
		completed = GetComponent<WordCheck> ().completed;

		attachedTiles = GameObject.FindGameObjectsWithTag ("AttachedTile");
		spawners = GameObject.FindGameObjectsWithTag ("Spawnable");
		unspawners = GameObject.FindGameObjectsWithTag ("UnSpawnable");

		gameObject.name = GetComponentInChildren<TextMesh> ().text;

		thisWordCheck = GetComponent<WordCheck> ();
		thisBody = GetComponent<Rigidbody2D> ();

		if (dragging) 
		{
			Vector3 cursorPos = Camera.main.ScreenToWorldPoint (Input.mousePosition);
			transform.position = new Vector2 (cursorPos.x, cursorPos.y); //Offset for dragging
			gameObject.tag = "MovingTile";
			thisBody.isKinematic = true;

			foreach (BoxCollider2D b in GetComponentsInChildren<BoxCollider2D>())
			{
				b.enabled = false;
			}	
		}

		//To check whether any fixed joint's gameobject object is disconnected
		if (GetComponents<FixedJoint2D>() != null)
		{
			foreach (FixedJoint2D f in GetComponents<FixedJoint2D>()) 
			{
				if (f == null || f.connectedBody.gameObject == null)
					Destroy (f);

				if (f.connectedBody.gameObject != null) 
				{
					GameObject connectedObject = f.connectedBody.gameObject;
					WordCheck connectedWordCheck = f.connectedBody.gameObject.GetComponent<WordCheck> ();

					if (connectedObject.name != "") 
					{
						if ((connectedObject.tag != "AttachedTile") || (!connectedWordCheck.completed && connectedWordCheck.strongParent) || (connectedWordCheck.broken))
							Destroy (f);
					}
				}

				if (!completed) 
				{
					if (GetComponent<Children> ().down.GetComponent<Attach> ().attachedObject == null) 
					{
						Invoke ("CheckStrongParent", 4.0f);
					}
				}
			}
		}

		if (tag == "AttachedTile") 
		{
			if (!triggeredMass) 
			{
				if (!thisWordCheck.completed) 
				{
					if (gameObject.name != "")
						thisBody.mass = 0.01f;	
				}

				else if (thisWordCheck.completed)
					thisBody.mass = 1.0f;
			}
		}
	}

	void OnMouseDown()
	{
		gameObject.layer = 0;
		transform.parent = null;

		if (tag == "AttachedTile" && completed)
			return;

		if (!completed) 
		{
			dragging = true;

			//Destroy all fixedjoints when detached
			foreach (FixedJoint2D f in GetComponents<FixedJoint2D>()) 
			{
				Destroy (f);
			}
		}
	}

	void OnMouseUp()
	{
		if (!completed) 
		{
			dragging = false;
			gameObject.tag = "AttachedTile";
		}

		ClearWord ();

		Invoke ("CheckWord", 1.0f);

		foreach (BoxCollider2D b in GetComponentsInChildren<BoxCollider2D>())
		{
			b.enabled = true;
		}

		Invoke ("RewardEmpty", 1.0f);

		FindObjectOfType<CompletedChain> ().FirstWord ();
	}

	public void ClearWord()
	{
		foreach (GameObject tile in attachedTiles) 
		{
			WordCheck tileWordCheck = tile.GetComponent<WordCheck> ();
			tileWordCheck.sides.Clear ();
			tileWordCheck.horizontalWord.Clear ();
			tileWordCheck.verticalWord.Clear ();
		}
	}

	public void CheckWord()
	{
		foreach (GameObject tile in attachedTiles) 
		{
			WordCheck tileWordCheck = tile.GetComponent<WordCheck> ();
			tileWordCheck.horWord = null;
			tileWordCheck.verWord = null;
			tileWordCheck.CheckWord ();
		}
	}

	void OnTriggerStay2D(Collider2D parentMass)
	{
		if (parentMass.tag == "ParentMass") 
		{
			triggeredMass = true;

			if (!thisWordCheck.completed) 
				thisBody.mass = 0.01f;	

			if (thisWordCheck.completed) 
				thisBody.mass = parentMass.GetComponent<Mass>().mass;	
		}
	}

	void OnTriggerExit2D(Collider2D parentMass)
	{
		if (parentMass.tag == "ParentMass") 
		{
			triggeredMass = false;
		}
	}

	void RewardEmpty()
	{
		if (completed) 
		{
			if (thisWordCheck.horizontalWord.Count > 5 || thisWordCheck.verticalWord.Count > 5) 
			{
				foreach (GameObject s in unspawners) 
				{
					s.GetComponent<SpawnRandom> ().SpawnEmpty ();
				}
			}
		}
	}

	void CheckStrongParent()
	{
		if (thisWordCheck.strongParent && !completed) 
		{
			foreach (FixedJoint2D f in GetComponents<FixedJoint2D>()) 
			{
				if (f.connectedBody.gameObject != null) 
					Destroy (f.gameObject);
			}
		}
	}
}
		