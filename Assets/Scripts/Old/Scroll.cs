using UnityEngine;
using System.Collections;

public class Scroll : MonoBehaviour 
{
	public bool clicked;
	public static float scrollTime;

	// Use this for initialization
	void Start () 
	{
		Camera.main.transform.position = new Vector3 (0.0f, 0.0f, -10.0f);
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (clicked) 
		{
			if (name == "UpArrow") 
			{
				scrollTime += Time.deltaTime;
				Camera.main.transform.position = new Vector3 (0.0f, scrollTime, -10.0f);
			}

			if (name == "DownArrow") 
			{
				scrollTime -= Time.deltaTime;
				Camera.main.transform.position = new Vector3 (0.0f, scrollTime, -10.0f);
			}
		}
	}

	void OnMouseDown() 
	{
		clicked = true;
	}

	void OnMouseUp() 
	{
		clicked = false;
	}

	void OnMouseExit() 
	{
		clicked = false;
	}
}
