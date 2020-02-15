using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TotalScore : MonoBehaviour 
{
	public int totalScore;
	public GameObject scoreTxt;
	
	// Update is called once per frame
	public void UpdateTotalScore () 
	{
		totalScore = 0;
		GameObject[] attachedTiles = GameObject.FindGameObjectsWithTag("AttachedTile");

		foreach (GameObject a in attachedTiles) 
		{
			if (a.GetComponent<WordCheck> ().completed) 
			{
				totalScore += a.GetComponent<WordCheck> ().score;
				scoreTxt.GetComponent<TextMesh>().text = "S C O R E   " + totalScore.ToString (); 
			}
		}
	}
}
