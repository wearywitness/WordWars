using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drag_Revised : MonoBehaviour
{
    public bool dragging;
    [Space(15)]
    private Rigidbody2D thisBody;
    public CircleCollider2D[] circleColliderChildren;
    public GameObject[] children;

    [Space(10)]
    private WordCheck_Revised wordCheck;

    void Start()
    {
        thisBody = GetComponent<Rigidbody2D>();

        wordCheck = GetComponent<WordCheck_Revised>();
    }

    void Update()
    {
        if (!dragging)
            return;

        else
        {
            Vector3 cursorPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            transform.position = new Vector2(cursorPos.x, cursorPos.y); //Offset for dragging
            gameObject.tag = "MovingTile";
            thisBody.isKinematic = true;
        }
    }

    void OnMouseDown()
    {
        if (wordCheck.completed)
            return;

        else
        {
            dragging = true;

            DetectAttachedObject(false);

            wordCheck.ClearLists(wordCheck.allHorizontalLists);
            wordCheck.ClearLists(wordCheck.allVerticalLists);
            
            wordCheck.AddItself();
        }
    }

    void OnMouseUp()
    {
        if (wordCheck.completed)
            return;

        else
        {
            transform.parent = null;
            dragging = false;

            DetectAttachedObject(true);
        }
    }

    void DetectAttachedObject(bool detecting)
    {
        foreach (GameObject child in children)
        {
            Attach_Revised attachScript = child.GetComponent<Attach_Revised>();

            if (attachScript.touchingObject != null)
            {
                if (detecting)
                {
                    attachScript.attachedObject = attachScript.touchingObject;

                    TagsAndLayers(gameObject, "AttachedTile", 0);
                    TagsAndLayers(attachScript.attachedObject, "AttachedTile", 0);

                    TransferToAttached(attachScript, gameObject);

                    gameObject.AddComponent<FixedJoint2D>();
                    attachScript.attachedObject.AddComponent<FixedJoint2D>();

                    FixedJointProperties(GetComponents<FixedJoint2D>(), attachScript.attachedObject.GetComponent<Rigidbody2D>());
                    FixedJointProperties(attachScript.attachedObject.GetComponents<FixedJoint2D>(), thisBody);

                    if (attachScript.isHorizontal)
                        wordCheck.AddHorizontalWord();

                    if (attachScript.isVertical)
                        wordCheck.AddVerticalWord();
                }

                else
                {
                    if (GetComponent<FixedJoint2D>() != null)
                    {
                        foreach (FixedJoint2D f in attachScript.attachedObject.GetComponents<FixedJoint2D>())
                        {
                            if (f.connectedBody == thisBody)
                                Destroy(f);
                        }

                        foreach (FixedJoint2D f in GetComponents<FixedJoint2D>())
                            Destroy(f);
                    }

                    gameObject.tag = "MovingTile";
                    attachScript.attachedObject = null;
                    TransferToAttached(attachScript, null);
                }
            }
        }
    }

    void TagsAndLayers(GameObject thisObject, string objectTag, int objectLayer)
    {
        thisObject.tag = objectTag;
        thisObject.layer = objectLayer;
    }

    void TransferToAttached(Attach_Revised thisAttachRevised, GameObject thisObject)
    {
        GameObject[] attachedChildren = thisAttachRevised.touchingObject.GetComponent<Drag_Revised>().children;
        foreach (GameObject attachedChild in attachedChildren)
        {
            Attach_Revised attachRevised = attachedChild.GetComponent<Attach_Revised>();
            if (attachRevised.touchingObject != null)
            {
                if (attachRevised.touchingObject == this.gameObject)
                {
                    attachRevised.attachedObject = thisObject;
                    transform.position = attachedChild.transform.position;
                }
            }
        }
    }

    void FixedJointProperties(FixedJoint2D[] fixedJoints, Rigidbody2D otherBody)
    {
        foreach (FixedJoint2D f in fixedJoints)
        {
            if (f.connectedBody == null)
            {
                f.connectedBody = otherBody;
                f.enableCollision = true;
                otherBody.bodyType = RigidbodyType2D.Dynamic;
            }
        }
    }
}
