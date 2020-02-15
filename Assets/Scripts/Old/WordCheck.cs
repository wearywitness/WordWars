using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class WordCheck : MonoBehaviour 
{
	public bool completed;
	public List<GameObject> sides;
	public List<GameObject> chain;
	public List<GameObject> horizontalWord;
	public List<GameObject> verticalWord;
	public string horWord;
	public string verWord;
	public bool strongParent;
	public bool broken;
	public int score;

	GameObject[] spawners;

	// Update is called once per frame
	void Update () 
	{
		spawners = GameObject.FindGameObjectsWithTag ("Spawnable");

		RemoveNull ();

		if (tag == "AttachedTile") 
		{
			CheckHorWord ();
			CheckVerWord ();
		}

		else
		{
			ClearAll ();
		}

		CheckStrongParent ();

		if (completed) 
		{
			ChangeColor ();

			if (chain != null) 
			{
				GameObject firstLetter = FindObjectOfType<CompletedChain> ().firstLetter;
				if (!chain.Contains (firstLetter) && firstLetter != null)
					Destroy (gameObject);
			}
		}
	}

	void RemoveNull()
	{
		if (sides.Contains(null)) 
			sides.Remove(null);

		if (horizontalWord.Contains(null)) 
			horizontalWord.Remove(null);

		if (verticalWord.Contains(null)) 
			verticalWord.Remove(null);
	}

	void CheckHorVerWord(List<GameObject> horVer, List<GameObject> letterHorVer)
	{
		foreach (GameObject letter in horVer) 
		{
			if (letter != null) 
			{
				WordCheck letterCheck = letter.GetComponent<WordCheck> ();
				foreach (GameObject tile in letterHorVer) 
				{
					if (tile != null) 
					{
						if (!horVer.Contains (tile)) 
						{
							horVer.Add (tile);
						}

						if (horVer.Contains (tile)) 
						{
							if (tile.tag != "AttachedTile")
								horVer.Remove (tile);
						}
					}
				}
			}
		}
	}

	void CheckHorWord()
	{
		foreach (GameObject letter in horizontalWord) 
		{
			WordCheck letterCheck = letter.GetComponent<WordCheck> ();
			CheckHorVerWord (horizontalWord, letterCheck.horizontalWord);
		}
	}

	void CheckVerWord()
	{
		foreach (GameObject letter in verticalWord) 
		{
			WordCheck letterCheck = letter.GetComponent<WordCheck> ();
			CheckHorVerWord (verticalWord, letterCheck.verticalWord);
		}
	}

	void CheckStrongParent()
	{
		GameObject attachment = GetComponent<Children> ().down.GetComponent<Attach> ().attachedObject;

		if (horizontalWord.Count > 0 && verticalWord.Count > 0) 
		{
			if (attachment == null)
				strongParent = true;
		}

		else if (horizontalWord.Count == 0 || verticalWord.Count == 0) 
		{
			if (attachment != null)
				strongParent = false;
		}
	}

	void ClearAll()
	{
		sides.Clear ();
		chain.Clear ();
		horizontalWord.Clear ();
		verticalWord.Clear ();
		horWord = null;
		verWord = null;
	}

	void ChangeColor()
	{
		GetComponentInChildren<TextMesh> ().color = new Color (0.18f, 0.56f, 0.694f, 1.00f);
	}

	public void CheckWord()
	{
		if (completed)
			return;

		if (!completed) 
		{
			if (WordDictionary.data != null) 
			{
				horizontalWord = horizontalWord.OrderBy (platform => platform.transform.position.x).ToList ();

				for (int i = 0; i < horizontalWord.Count; i++) 
					horWord += horizontalWord [i].transform.name;

				if (string.IsNullOrEmpty (horWord) && string.IsNullOrEmpty (verWord))
					completed = false;

				if (!string.IsNullOrEmpty (horWord) && string.IsNullOrEmpty (verWord)) 
				{
					if (WordDictionary.data.hasWord (horWord)) 
						completed = true;	

					if (!WordDictionary.data.hasWord (horWord)) 
						completed = false;	
				}

				verticalWord = verticalWord.OrderBy (platform => -(platform.transform.position.y)).ToList ();

				for (int i = 0; i < verticalWord.Count; i++) 
					verWord += verticalWord[i].transform.name;

				if (!string.IsNullOrEmpty (verWord) && string.IsNullOrEmpty (horWord)) 
				{
					if (WordDictionary.data.hasWord (verWord)) 
						completed = true;	

					if (!WordDictionary.data.hasWord (verWord)) 
						completed = false;	
				}

				if (!string.IsNullOrEmpty (horWord) && !string.IsNullOrEmpty (verWord)) 
				{
					if (WordDictionary.data.hasWord (horWord) && WordDictionary.data.hasWord (verWord)) 
						completed = true;	

					if (!WordDictionary.data.hasWord (horWord) && WordDictionary.data.hasWord (verWord)) 
						completed = false;	

					if (WordDictionary.data.hasWord (horWord) && !WordDictionary.data.hasWord (verWord)) 
						completed = false;	

					if (!WordDictionary.data.hasWord (horWord) && !WordDictionary.data.hasWord (verWord)) 
						completed = false;	
				}

				GameObject.Find ("TotalScore").GetComponent<TotalScore> ().UpdateTotalScore ();
			}
		}
	}
}
