using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnRandom_Revised : MonoBehaviour
{
    [Space(15)]
    // Objects that shall be spawned
    public GameObject baseObject; // Base of all the alphabet probabilities
    public GameObject clone; // Clone of Object spawned

    [Space(15)]
    // Alphabet measurements
    public int[] all;
    public int index;

    [Space(15)]
    // Check interactions
    public bool cloneIsTouching;
    public bool movingTileIsTouching; // To check if clone is moving
    public string[] touchableObjectTags;

    void Start()
    {
        Spawn();
    }

    void Update()
    {
        if (clone != null)
        {
            if (clone.GetComponent<WordCheck_Revised>().completed)
                clone = null;
        }

        else
            Spawn();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject == clone)
            cloneIsTouching = true;

        if (!cloneIsTouching)
            CheckTouches(other, true);
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject == clone)
            cloneIsTouching = false;


        if (!cloneIsTouching)
            CheckTouches(other, false);
    }

    public void Spawn()
    {
        clone = Instantiate(baseObject, transform.position, transform.rotation);
        clone.transform.parent = GameObject.Find("MainCamera").transform;
        clone.tag = "SpawnedTile";
        clone.layer = 11;

        LettersWithScores();
    }

    void LettersWithScores()
    {
        index = Random.Range(0, all.Length);
        
        Category(0, "A", 1);
        Category(1, "B", 3);
        Category(2, "C", 3);
        Category(3, "D", 2);
        Category(4, "E", 1);
        Category(5, "F", 4);
        Category(6, "G", 2);
        Category(7, "H", 4);
        Category(8, "I", 1);
        Category(9, "J", 8);
        Category(10, "K", 5);
        Category(11, "L", 1);
        Category(12, "M", 3);
        Category(13, "N", 1);
        Category(14, "O", 1);
        Category(15, "P", 3);
        Category(16, "Q", 10);
        Category(17, "R", 1);
        Category(18, "S", 1);
        Category(19, "T", 1);
        Category(20, "U", 1);
        Category(21, "V", 4);
        Category(22, "W", 4);
        Category(23, "X", 8);
        Category(24, "Y", 4);
        Category(25, "Z", 10);
    }

    void Category(int theIndexNumber, string theAlphabet, int theScore)
    {
        if (all[index] == theIndexNumber)
            InstantiateClone(theAlphabet, theScore);
    }

    void InstantiateClone(string thisLetter, int thisScore)
    {
        clone.GetComponentInChildren<TextMesh>().text = thisLetter;
        clone.GetComponent<WordCheck_Revised>().score = thisScore;
        clone.name = clone.GetComponentInChildren<TextMesh>().text;
    }

    void CheckTouches(Collider2D otherCollider, bool touch)
    {
        foreach (string touchableObjectTag in touchableObjectTags)
        {
            if (otherCollider.tag == touchableObjectTag)
                movingTileIsTouching = touch;
        }
    }
}
