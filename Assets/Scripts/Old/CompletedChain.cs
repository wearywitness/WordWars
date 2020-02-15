using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CompletedChain : MonoBehaviour 
{
	public GameObject firstLetter;

	// Use this for initialization
	void Start () 
	{
		
	}
	
	// Update is called once per frame
	void Update () 
	{
		
	}

	public void FirstWord()
	{
		if (firstLetter == null)
		{
			foreach (GameObject tile in GameObject.FindGameObjectsWithTag ("AttachedTile")) 
			{
				if (tile.GetComponent<WordCheck> ().completed)
					firstLetter = tile;
			}
		}
	}
}
