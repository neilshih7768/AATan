using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;


public class TestBall : MonoBehaviour {

	public GameObject ball;
	public GameObject bar;
	
	public GUISkin customSkin;
	int iScore;
	int iRounds;
	GameObject goFirstBall;
	Ball bFirstBall;

	// Use this for initialization
	void Start () {
		SetAll();
		iScore = 0;
		iRounds = 1;
	}
	
	
	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButtonDown(1))
		{
			Application.LoadLevel(0);
		}
	}

	
	void SetAll() {
		SetBall ();
		SetBar	();
		
		NewBall().SetBallID(2);
	}

	
	public Ball NewBall() {
		//Vector2 vPointBall = new Vector2(0, -3F); // Set position
		Vector2 vPointBall = new Vector2(1, -3F); // Set position
		GameObject goBall = (GameObject)Instantiate(ball, vPointBall, Quaternion.identity);
		Ball b = goBall.GetComponent<Ball>();
		goBall.GetComponent<Renderer>().material.color = Color.blue;
		b.transform.parent = bFirstBall.transform;
		//Physics.IgnoreCollision(goBall.GetComponent<CircleCollider2D>(), goFirstBall.GetComponent<CircleCollider2D>());
		Physics2D.IgnoreCollision(b.transform.GetComponent<Collider2D>(), bFirstBall.transform.GetComponent<Collider2D>());
		//foreach (Transform child in bFirstBall)
		//	Physics.IgnoreCollision(b.transform.GetComponent<Collider>(), child.GetComponent<Collider>());
		return b;
	}

	
	void SetBall() {
		Vector2 vPointBall = new Vector2(0, -3F); // Set position
		goFirstBall = (GameObject)Instantiate(ball, vPointBall, Quaternion.identity);
		bFirstBall = goFirstBall.GetComponent<Ball>();
		bFirstBall.SetBallID(1);
	}

	
	void SetBar() {
		// Up Bar
		Vector2 vPointTopBar = new Vector2(0F, 4.0F); // Set position
		GameObject goTopBar = (GameObject)Instantiate(bar, vPointTopBar, Quaternion.identity);
		
		// Button Bar
		Vector2 vPointButtonBar = new Vector2(0F, -4.0F); // Set position
		GameObject goButtonBar = (GameObject)Instantiate(bar, vPointButtonBar, Quaternion.identity);
	}


	void OnGUI(){
		GUI.skin = customSkin;
		GUI.Label(new Rect(Screen.width/2, 20, 200, 60 ), "Scores:" + iScore.ToString() );
		GUI.Label(new Rect(20, 20, 250, 60 ), "Rounds:" + iRounds.ToString() );
	}

	
	public void AddScore() 
	{
		iScore++;
	}


	public void AddRounds()
	{
		iRounds++;
	}

}
