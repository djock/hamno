using UnityEngine;
using System.Collections;

public class StartGame : MonoBehaviour {

	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.UpArrow))
		{
			Application.LoadLevel("Game");
		}
	}

	void OnGUI() {

	}
}
