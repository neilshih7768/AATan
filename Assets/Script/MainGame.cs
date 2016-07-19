using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class MainGame : MonoBehaviour {

	public GameObject cube;
	public GameObject ball;
	public GameObject wall;
	public GameObject bar;

	public GUISkin customSkin;

	int iScore;
	int iRounds;
	int iHoldBalls;
	GameObject goFirstBall;
	Ball bFirstBall;
	List<Ball> bBallList = new List<Ball>();

	Vector2 vAssemblePoint;
	bool bAssemble;		// Is assembling ?

	public float fTime;

	// Use this for initialization
	void Start () {
		iHoldBalls = 0;
		iScore = 0;
		iRounds = 1;
		bAssemble = false;
		SetAll();
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
		SetCubes();
		SetWall ();
		SetBar	();

		for(int i = 2; i <= 15; i++)
			NewBall().SetBallID(i);
	}


	public Ball NewBall() {
		GameObject goBall = (GameObject)Instantiate(ball, vAssemblePoint, Quaternion.identity);
		Ball b = goBall.GetComponent<Ball>();
		bBallList.Add(b);
		iHoldBalls++;
		b.transform.parent = bFirstBall.transform;

		Physics2D.IgnoreCollision(b.transform.GetComponent<Collider2D>(), bFirstBall.transform.GetComponent<Collider2D>());
		foreach (Transform child in goFirstBall.transform)
			Physics2D.IgnoreCollision(b.transform.GetComponent<Collider2D>(), child.GetComponent<Collider2D>());
		return b;
	}


	void SetBall() {	// First ball
		vAssemblePoint = new Vector2(0, -3.5F); // Set position
		goFirstBall = (GameObject)Instantiate(ball, vAssemblePoint, Quaternion.identity);
		bFirstBall = goFirstBall.GetComponent<Ball>();
		bFirstBall.SetBallID(1);
		bBallList.Add(bFirstBall);
		iHoldBalls++;
	}


	void SetCubes() {

		for (int i = 0; i < 7; i++)
		{
			for (int j = 0; j < 7; j++)
			{
				Vector2 vPointBoxQueue = new Vector2(j * 0.8F - 2.4F, i * -0.8F + 3.5F); // Set position
				GameObject goCube = (GameObject)Instantiate(cube, vPointBoxQueue, Quaternion.identity);
				Cube cCube = goCube.GetComponent<Cube>();
				cCube.SetHP(3);
				cCube.SetID(j + i * 7);
				cCube.SetLocation(vPointBoxQueue);
			}
		}
	}


	void SetWall() {
		Vector2 vPointWall = new Vector2(3.335F, 0F); // Set position
		GameObject goWall = (GameObject)Instantiate(wall, vPointWall, Quaternion.identity);
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
		//GUI.Label(new Rect(vAssemblePoint.x+5, vAssemblePoint.y, 250, 60 ), "X" + iRounds.ToString() );
		GUI.Label(new Rect(20, Screen.height-80, 400, 100 ), "AP:" + vAssemblePoint.ToString() );
		GUI.Label(new Rect(Screen.width/2, Screen.height-60, 400, 100 ), "X " + iHoldBalls.ToString() );
	}


	public void AddScore(){ iScore++; }

	public void AddRounds(){ iRounds++;	}

	public int GetRounds(){	return iRounds; }

	public void SetAP(Vector2 v){ vAssemblePoint = v; }

	public Vector2 GetAP(){ return vAssemblePoint; }

	public bool IsAssemble() { return bAssemble; }

	public void SetAssemble() { bAssemble = true; }

	public void SetNotAssemble() { bAssemble = false; }

	public bool IsReady() { 
		if( bBallList.Count == iHoldBalls)
			return true; 
		else
			return false;
	}
		
	public void AddHoldBall() { iHoldBalls++; }

	public void SubHoldBall() { iHoldBalls--; }

}
