using UnityEngine;
using System.Collections;
using System;

public class Generate : MonoBehaviour
{
	public Generate SP;
	public Transform rocks;
	public int score;
	private GUIStyle largeFont;
	public ArrayList arrayKnives;
	
	// Use this for initialization
	void Start()
	{
		arrayKnives = new ArrayList();
		largeFont = new GUIStyle();
		largeFont.fontSize = 30;
		InvokeRepeating("CreateObstacle", 0.1f, 1f);
	}
	
	
	// Update is called once per frame
	void OnGUI () 
	{
		GUI.color = Color.black;
		GUILayout.Label(" Score: " + score.ToString(),largeFont);
	}
	
	void CreateObstacle()
	{	
;
		Transform knive = Instantiate(rocks) as Transform;
		arrayKnives.Add(knive);
		if (arrayKnives.Count > 6)
		{
			Destroy(((Transform)arrayKnives[arrayKnives.Count-7]).gameObject);
		}
		
	}
}
