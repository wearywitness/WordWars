using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WordCheck_Revised : MonoBehaviour
{
    public bool completed;
    public int score;

    [Space(15)]
    public List<GameObject> horizontalWord;
    public List<GameObject> leftSideHorizontalWord;
    public List<GameObject> rightSideHorizontalWord;

    [Space(15)]
    public List<GameObject> verticalWord;
    public List<GameObject> topSideVerticalWord;
    public List<GameObject> downSideVerticalWord;

    [Space(15)]
    public List<GameObject>[] allHorizontalLists;
    public List<GameObject>[] allVerticalLists;

    [Space(15)]
    public string horWord;
    public string verWord;

    [Space(15)]
    public Drag_Revised drag;

    void Start()
    {
        drag = GetComponent<Drag_Revised>();

        allHorizontalLists = new List<GameObject>[] { leftSideHorizontalWord, rightSideHorizontalWord, horizontalWord };
        allVerticalLists = new List<GameObject>[] { topSideVerticalWord, downSideVerticalWord, verticalWord };

        AddItself();
    }

    public void AddItself()
    {
        AddIfNot(verticalWord, gameObject);
        AddIfNot(horizontalWord, gameObject);
    }

    void AddIfNot(List<GameObject> thisList, GameObject thisObject)
    {
        if (!thisList.Contains(thisObject))
            thisList.Add(thisObject);
    }

    public void AddHorizontalWord()
    {
        AddItself();

        foreach (GameObject child in drag.children)
        {
            Attach_Revised childAttach = child.GetComponent<Attach_Revised>();

            if (childAttach.attachedObject != null && childAttach.isHorizontal)
            {
                AddTwoLists(gameObject, childAttach.attachedObject, true);

                foreach (GameObject letter in childAttach.attachedObject.GetComponent<WordCheck_Revised>().horizontalWord)
                {
                    if (letter != gameObject && letter != childAttach.attachedObject)
                        AddTwoLists(gameObject, letter, true);
                }
            }
        }

        CheckWordString();
    }

    public void AddVerticalWord()
    {
        AddItself();

        foreach (GameObject child in drag.children)
        {
            Attach_Revised childAttach = child.GetComponent<Attach_Revised>();

            if (childAttach.attachedObject != null && childAttach.isVertical)
            {
                AddTwoLists(gameObject, childAttach.attachedObject, false);

                foreach (GameObject letter in childAttach.attachedObject.GetComponent<WordCheck_Revised>().verticalWord)
                {
                    if (letter != gameObject && letter != childAttach.attachedObject)
                        AddTwoLists(gameObject, letter, false);
                }
            }
        }

        CheckWordString();
    }

    void AddTwoLists(GameObject thisObject, GameObject oppObject, bool checkIfHor)
    {
        AddToLists(thisObject, oppObject, checkIfHor);
        AddToLists(oppObject, thisObject, checkIfHor);
    }

    void AddToLists(GameObject thisObject, GameObject otherObject, bool checkIfHor)
    {
        WordCheck_Revised thisWordCheck = thisObject.GetComponent<WordCheck_Revised>();

        if (checkIfHor)
        {
            AddIfNot(thisWordCheck.horizontalWord, otherObject);
            CheckHorSide(thisObject, otherObject, thisWordCheck.leftSideHorizontalWord, thisWordCheck.rightSideHorizontalWord);
        }

        else
        {
            AddIfNot(thisWordCheck.verticalWord, otherObject);
            CheckVerSide(thisObject, otherObject, thisWordCheck.topSideVerticalWord, thisWordCheck.downSideVerticalWord);
        }

        thisWordCheck.CheckWordString();
    }

    void CheckHorSide(GameObject thisObject, GameObject otherObject, List<GameObject> thisLeftSideWord, List<GameObject> thisRightSideWord)
    {
        if (otherObject.transform.position.x < thisObject.transform.position.x)
            AddIfNot(thisLeftSideWord, otherObject);

        if (otherObject.transform.position.x > thisObject.transform.position.x)
            AddIfNot(thisRightSideWord, otherObject);
    }

    void CheckVerSide(GameObject thisObject, GameObject otherObject, List<GameObject> thisTopSideVerWord, List<GameObject> thisDownSideVerWord)
    {
        if (otherObject.transform.position.y < thisObject.transform.position.y)
            AddIfNot(thisDownSideVerWord, otherObject);

        if (otherObject.transform.position.y > thisObject.transform.position.y)
            AddIfNot(thisTopSideVerWord, otherObject);
    }

    public void RemoveHorizontalWord()
    {
        foreach (GameObject letter in horizontalWord)
        {
            WordCheck_Revised letterWordCheck = letter.GetComponent<WordCheck_Revised>();

            if (transform.position.x < letter.transform.position.x)
                RemoveWithRef(leftSideHorizontalWord, "left");

            if (transform.position.x > letter.transform.position.x)
                RemoveWithRef(rightSideHorizontalWord, "right");

            RemoveLetters(letter, new List<GameObject>[] { letterWordCheck.rightSideHorizontalWord, letterWordCheck.leftSideHorizontalWord });

            if (letter != gameObject)
                letterWordCheck.horizontalWord.Remove(gameObject);
        }

        ClearLists(allHorizontalLists);
        AddItself();
        CheckWordString();
    }

    public void RemoveVerticalWord()
    {
        foreach (GameObject letter in verticalWord)
        {
            WordCheck_Revised letterWordCheck = letter.GetComponent<WordCheck_Revised>();

            if (transform.position.y > letter.transform.position.y)
                RemoveWithRef(topSideVerticalWord, "top");

            if (transform.position.y < letter.transform.position.y)
                RemoveWithRef(downSideVerticalWord, "down");

            RemoveLetters(letter, new List<GameObject>[] { letterWordCheck.downSideVerticalWord, letterWordCheck.topSideVerticalWord });

            if (letter != gameObject)
                letterWordCheck.verticalWord.Remove(gameObject);
        }

        ClearLists(allVerticalLists);
        AddItself();
        CheckWordString();
    }

    void RemoveWithRef(List<GameObject> theWordList, string theOrientation)
    {
        foreach (GameObject theLetter in theWordList)
        {
            WordCheck_Revised theLetterWordCheck = theLetter.GetComponent<WordCheck_Revised>();

            if (theOrientation == "left")
                RemoveLetters(theLetter, new List<GameObject>[] { theLetterWordCheck.rightSideHorizontalWord, theLetterWordCheck.horizontalWord });

            if (theOrientation == "right")
                RemoveLetters(theLetter, new List<GameObject>[] { theLetterWordCheck.leftSideHorizontalWord, theLetterWordCheck.horizontalWord });

            if (theOrientation == "down")
                RemoveLetters(theLetter, new List<GameObject>[] { theLetterWordCheck.topSideVerticalWord, theLetterWordCheck.verticalWord });

            if (theOrientation == "top")
                RemoveLetters(theLetter, new List<GameObject>[] { theLetterWordCheck.downSideVerticalWord, theLetterWordCheck.verticalWord });
        }
    }

    void RemoveLetters(GameObject theLetter, List<GameObject>[] theseLists)
    {
        WordCheck_Revised theLetterWordCheck = theLetter.GetComponent<WordCheck_Revised>();

        foreach (List<GameObject> thisList in theseLists)
        {
            if (thisList.Contains(theLetter))
                thisList.Remove(theLetter);
        }

        theLetterWordCheck.CheckWordString();
    }

    public void ClearLists(List<GameObject>[] theseLists)
    {
        foreach (List<GameObject> thisList in theseLists)
            thisList.Clear();
    }

    public void CheckWordString()
    {
        if (completed)
            return;

        if (!completed)
        {
            if (WordDictionary.data != null)
            {
                horWord = verWord = "";

                CheckWords(true, horizontalWord, verticalWord, horWord);
                CheckWords(false, verticalWord, horizontalWord, verWord);

                if (horizontalWord.Count > 1 && verticalWord.Count > 1)
                {
                    if (WordDictionary.data.hasWord(horWord) && WordDictionary.data.hasWord(verWord))
                        Completion();

                    else
                        completed = false;
                }
            }
        }
    }

    void CheckWords(bool checkIfHor, List<GameObject> theWordList, List<GameObject> theOtherWordList, string theWord)
    {
        if (checkIfHor)
            theWordList = theWordList.OrderBy(platform => platform.transform.position.x).ToList();

        else
            theWordList = theWordList.OrderBy(platform => -(platform.transform.position.y)).ToList();

        for (int i = 0; i < theWordList.Count; i++)
            theWord += theWordList[i].transform.name;

        if (theWordList.Count > 1 && theOtherWordList.Count <= 1)
        {
            if (WordDictionary.data.hasWord(theWord))
                Completion();

            else
                completed = false;
        }
    }

    void Completion()
    {
        completed = true;
        GetComponentInChildren<TextMesh>().color = new Color(0.18f, 0.56f, 0.694f, 1.00f);
    }
}
