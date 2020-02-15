using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System;
using System.Linq;
using System.Collections;

public class WordDictionary : MonoBehaviour
{
    public TextAsset dict;
    public Dictionary<string,string> words = new Dictionary<string, string>();

    public static WordDictionary data;

    void Start()
	{
        data = this;
        /* string[] separators = new[] { Environment.NewLine, "\t" }; */
        string[] separators = new[] { "\r\n", "\t" };
        string[] wordArray = dict.text.Split(separators, StringSplitOptions.RemoveEmptyEntries);
        int i = 0;
        foreach (string s in wordArray)
		{
            words.Add(i.ToString(), s);
            i++;
        }
    }

    public bool hasWord(string word)
	{
		word = word.ToLower();

        if (words.ContainsValue(word))
            return true;
        else
            return false;
    }
}