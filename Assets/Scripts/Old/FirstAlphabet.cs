using UnityEngine;
using System.Collections;

public class FirstAlphabet : MonoBehaviour 
{
	public GameObject firstAlphabet;

	void OnCollisionStay2D(Collision2D ground)
	{
		ground.gameObject.tag = "AttachedTile";

		if (firstAlphabet == null) 
		{
			firstAlphabet = ground.gameObject;
			firstAlphabet.tag = "AttachedTile";
		}
	}
}
