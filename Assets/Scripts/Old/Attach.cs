using UnityEngine;
using System.Collections;

public class Attach : MonoBehaviour 
{
	public bool isHor;
	public bool isVer;
	public GameObject attachedObject;
	public string name;
	public GameObject highestBlock;

	private WordCheck parentWordCheck;
	private Children parentChildren;

	// Update is called once per frame
	void Update () 
	{
		highestBlock = GameObject.Find ("HighestBlock");

		parentWordCheck = GetComponentInParent<WordCheck> ();
		parentChildren = GetComponentInParent<Children> ();

		if (attachedObject != null) 
		{
			if ((attachedObject.tag != "AttachedTile" && attachedObject.tag != "Ground") || transform.parent.tag != "AttachedTile")
				attachedObject = null;	

			if (attachedObject.tag != "Ground") 
			{
				Children child = attachedObject.GetComponent<Children> ();
				GameObject parentObject = transform.parent.gameObject;

				if (name == "right")
					child.left.GetComponent<Attach> ().attachedObject = parentObject;

				if (name == "left")
					child.right.GetComponent<Attach> ().attachedObject = parentObject;

				if (name == "top")
					child.down.GetComponent<Attach> ().attachedObject = parentObject;

				if (name == "down")
					child.top.GetComponent<Attach> ().attachedObject = parentObject;
			}
		}

		if (transform.parent.tag == "AttachedTile" && transform.parent.GetComponent<FixedJoint2D> () == null) 
		{
			if (parentWordCheck.completed) 
			{
				parentWordCheck.broken = true;
				parentChildren.brokenSprite.SetActive (true);
				Destroy (transform.parent.gameObject, 2.0f);
			}
		}
	}

	void OnTriggerStay2D(Collider2D stick)
	{
		WordCheck stickWordCheck = stick.GetComponent<WordCheck> ();

		if (transform.parent.tag == "AttachedTile" && !parentWordCheck.broken) 
		{
			if (attachedObject == null) 
			{
				if ((stick.tag == "StaticTile" || stick.tag == "AttachedTile") && !stickWordCheck.broken && !stickWordCheck.completed) 
				{
					Transform stickTransform = stick.transform;
					stickTransform.position = transform.position;
					stickTransform.rotation = transform.rotation;

					stickTransform.tag = "AttachedTile";
					GetComponentInParent<Drag> ().Invoke ("CheckWord", 0.1f);

					if (highestBlock != null && highestBlock.activeSelf) 
					{
						CalcHighestBlock calcHiBlock = highestBlock.GetComponent<CalcHighestBlock> ();
						calcHiBlock.GetClosestCastleBlock ();
						highestBlock.transform.position = calcHiBlock.highestBlock.transform.position;
					}
				}

				if (stick.tag == "StaticTile" || stick.tag == "AttachedTile") 
				{
					attachedObject = stick.gameObject;
				}

				if (stick.tag == "AttachedTile")
				{
					GameObject parentObject = transform.parent.gameObject;
					parentObject.AddComponent<FixedJoint2D> ().connectedBody = stick.GetComponent<Rigidbody2D> ();
					parentObject.GetComponent<Rigidbody2D> ().isKinematic = false;

					foreach (FixedJoint2D f in transform.parent.gameObject.GetComponents<FixedJoint2D>()) 
					{
						FixedJointProps (f);
					}

					GameObject stickObject = stick.gameObject;
					stickObject.AddComponent<FixedJoint2D> ().connectedBody = parentObject.GetComponent<Rigidbody2D> ();
					stickObject.GetComponent<Rigidbody2D> ().isKinematic = false;

					foreach (FixedJoint2D f in stickObject.GetComponents<FixedJoint2D>()) 
					{
						FixedJointProps (f);
					}
				}
			}

			if (transform.parent.name != "" && stick.tag == "AttachedTile") 
			{
				if (stick.name != "") 
				{
					// Attaching sides
					if (!parentWordCheck.sides.Contains (stick.gameObject)) 
					{
						parentWordCheck.sides.Add (stick.gameObject);
						stickWordCheck.sides.Add (transform.parent.gameObject);		
					}
						
					// Attaching chain
					if (!parentWordCheck.chain.Contains (stick.gameObject) && stickWordCheck.completed)
						parentWordCheck.chain.Add (stick.gameObject);

					WordCheck stickParentWordCheck = stick.gameObject.GetComponentInParent<WordCheck> ();
						
					foreach (GameObject chainObject in stickParentWordCheck.chain) 
					{
						WordCheck chainWordCheck = chainObject.GetComponentInParent<WordCheck> ();

						if (chainWordCheck.completed) 
						{
							if (!parentWordCheck.chain.Contains (chainObject))
								parentWordCheck.chain.Add (chainObject);

							if (!stickWordCheck.chain.Contains (chainObject))
								stickWordCheck.chain.Add (chainObject);
						}
					}

					if (!stickWordCheck.chain.Contains (transform.parent.gameObject) && parentWordCheck.completed)
						stickWordCheck.chain.Add (transform.parent.gameObject);

					// Attach horizontal word
					if (isHor) 
					{
						if (!parentWordCheck.horizontalWord.Contains (stick.gameObject)) 
						{
							parentWordCheck.horizontalWord.Add (stick.gameObject);
							stickWordCheck.horizontalWord.Add (transform.parent.gameObject);
						}
					}

					// Attach Vertical word
					if (isVer) 
					{
						if (!parentWordCheck.verticalWord.Contains (stick.gameObject)) 
						{
							parentWordCheck.verticalWord.Add (stick.gameObject);
							stickWordCheck.verticalWord.Add (transform.parent.gameObject);
						}
					}
				}
			}
		}

		if (stick.tag == "Ground") 
		{
			attachedObject = stick.gameObject;
		}
	}

	void CheckCorrectJoints()
	{
		foreach (FixedJoint2D f in transform.parent.gameObject.GetComponents<FixedJoint2D>()) 
		{
			if (f != null) 
			{
				GameObject fObject = f.connectedBody.gameObject;
				Children thisChildren = GetComponent<Children> ();
				GameObject topObject = thisChildren.top.GetComponent<Attach> ().attachedObject;
				GameObject downObject = thisChildren.down.GetComponent<Attach> ().attachedObject;
				GameObject leftObject = thisChildren.left.GetComponent<Attach> ().attachedObject;
				GameObject rightObject = thisChildren.right.GetComponent<Attach> ().attachedObject;

				if (fObject != topObject && fObject != downObject && fObject != leftObject && fObject != rightObject)
					Destroy (f);
			}
		}
	}

	void FixedJointProps(FixedJoint2D newJoint)
	{
		newJoint.enableCollision = true;
		newJoint.dampingRatio = 1.0f;
		newJoint.frequency = 30;
		newJoint.breakForce = 800;
	}
}
