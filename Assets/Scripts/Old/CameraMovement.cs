using UnityEngine;
using System.Collections;

public class CameraMovement : MonoBehaviour 
{
	public GameObject target;
	public float offsetY;
	public float speed;

	void Update () 
	{
		if (GameObject.Find ("HighestBlock") != null) 
		{
			GameObject highBlock = GameObject.Find ("HighestBlock");
			target = highBlock.GetComponent<CalcHighestBlock> ().highestBlock;
		}
	}
}
