using UnityEngine;
using System.Collections;
using System.Linq;

public class CalcHighestBlock : MonoBehaviour 
{
	public GameObject[] castleBlocks;
	public GameObject highestBlock;

	public void GetClosestCastleBlock() 
	{ 
		castleBlocks = GameObject.FindGameObjectsWithTag("AttachedTile");
		castleBlocks = castleBlocks.OrderBy (platform => -platform.transform.position.y).ToArray ();

		highestBlock = castleBlocks [0];
	}
}
