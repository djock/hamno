using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Player : MonoBehaviour
{
	// The force which is added when the player jumps
	// This can be changed in the Inspector window
	public Vector2 jumpForce = new Vector2(0, 200);
	bool lockInput = false;
	public Generate m_Generate;
	private int getScore;
	string playerName = "";
	string code = "";
	GUIStyle smallFont;
	GUIStyle largeFont;


	dreamloLeaderBoard dl;
	dreamloPromoCode pc;

	enum gameState {
		running,
		enterscore,
		leaderboard
	};

	gameState gs;

	void Start()
	{
		smallFont = new GUIStyle ();
		largeFont = new GUIStyle ();

		smallFont.fontSize = 18;
		largeFont.fontSize = 20;

		lockInput = false;
		this.gs = gameState.running;

		// get the reference here...
			this.dl = dreamloLeaderBoard.GetSceneDreamloLeaderboard();
		
		// get the other reference here
		this.pc = dreamloPromoCode.GetSceneDreamloPromoCode();
		}
	// Update is called once per frame
	void Update ()
	{
		if (lockInput)
						return;

		// Jump
		if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.UpArrow))
		{
			GetComponent<Rigidbody2D>().velocity = Vector2.zero;
			GetComponent<Rigidbody2D>().AddForce(jumpForce);
		}
		
		// Die by being off screen
		Vector2 screenPosition = Camera.main.WorldToScreenPoint(transform.position);
		if (screenPosition.y > Screen.height || screenPosition.y < 0)
		{
			OffScreen();
		}
	}
	
	// Die by collision
	void OnCollisionEnter2D(Collision2D other)
	{ 
		StartCoroutine(Die());
	}

	void OnTriggerEnter2D(Collider2D other)
	{	m_Generate.score++;
		getScore = m_Generate.score;
		}

	IEnumerator Die()
	{ 
		lockInput = true;
		yield return new WaitForSeconds (0.7f);
//		Application.LoadLevel(Application.loadedLevel);
		this.gs = gameState.enterscore;
	}
	void OffScreen()
	{
		this.gs = gameState.enterscore;
//		Application.LoadLevel(Application.loadedLevel);
	}

	void OnGUI()
	{  	
		if (this.gs != gameState.running) {
			GUILayoutOption[] width200 = new GUILayoutOption[] {GUILayout.Width (200)};
		
			float width = 400;  // Make this wider to add more columns
			float height = 300;

			Rect r = new Rect ((Screen.width / 2) - (width / 2), (Screen.height / 1.5f) - (height), width, height);
			GUILayout.BeginArea (r, new GUIStyle ("box"));
		
			GUILayout.BeginVertical ();
			if (this.gs == gameState.enterscore) {
				GUILayout.Label ("Total Score: " + this.getScore.ToString (),largeFont);
				GUILayout.BeginHorizontal ();
				GUILayout.Label ("Your Name: ");
				this.playerName = GUILayout.TextField (this.playerName, width200);
			
				if (GUILayout.Button ("Save Score")) {
					// add the score...
					if (dl.publicCode == "")
					Debug.LogError ("You forgot to set the publicCode variable");
					if (dl.privateCode == "")
						Debug.LogError ("You forgot to set the privateCode variable");
				
						dl.AddScore (this.playerName, getScore);
				
						this.gs = gameState.leaderboard;
				}
				GUILayout.EndHorizontal ();
				}
		
				if (this.gs == gameState.leaderboard) {
					GUILayout.Label ("High Scores:",smallFont);
					List<dreamloLeaderBoard.Score> scoreList = dl.ToListHighToLow ();
			
					if (scoreList == null) {
						GUILayout.Label ("(loading...)");
					} else {
						int maxToDisplay = 20;
						int count = 0;
						foreach (dreamloLeaderBoard.Score currentScore in scoreList) {
							count++;
							GUILayout.BeginHorizontal ();
							GUILayout.Label (currentScore.playerName, width200);
							GUILayout.Label (currentScore.score.ToString (), width200);
							GUILayout.EndHorizontal ();
					
							if (count >= maxToDisplay)
								break;
						}
					}
				}
				GUILayout.EndArea ();
		
				r.y = r.y + r.height + 20;
				GUILayout.EndVertical ();
				if(GUI.Button(new Rect(440,410,80,30), "Restart")) {
					Application.LoadLevel("StartGame");
				}

			}
	}
}
