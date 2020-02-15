using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attach_Revised : MonoBehaviour
{
    public GameObject touchingObject;
    public GameObject attachedObject;
    public string[] touchableTags;
    public bool isHorizontal;
    public bool isVertical;
    public WordCheck_Revised parentWordCheck;

    void Start()
    {
        parentWordCheck = GetComponentInParent<WordCheck_Revised>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        CheckTouch(other, other.gameObject);
    }

    void OnTriggerExit2D(Collider2D other)
    {
        CheckTouch(other, null);

        if (other.tag == "AttachedTile")
        {
            if (isHorizontal)
                parentWordCheck.RemoveHorizontalWord();
            
            if (isVertical)
                parentWordCheck.RemoveVerticalWord();
        }
    }

    void CheckTouch(Collider2D otherCollider, GameObject otherObject)
    {
        foreach (string touchTag in touchableTags)
        {
            if (otherCollider.tag == touchTag)
                touchingObject = otherObject;
        }
    }
}
