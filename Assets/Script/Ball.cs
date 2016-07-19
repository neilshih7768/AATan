//#define MOBILE

using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

enum BallStatus{
	Wait,
	Shoot,
	Assemble,
	End
}

public class Ball : MonoBehaviour {
	MainGame[] mg;

	GameConfig gc = new GameConfig();

	BallStatus bs;

	public Transform[] children;
	public Renderer[] renderers;

	float speed;

	private Rigidbody2D rbBall;
	private Vector2 vShoot;

	float fZRotDeg;
	float fZRotAng;

	Vector2 startP, EndP;

	float Slope;
	float fShootPower;

	int iBallID;	// first ball is 1

	float fLastTime;

	// Use this for initialization
	void Start () {
		GameObject go = GameObject.Find("Main Game");
		mg = go.GetComponents<MainGame>();

		vShoot = new Vector2(0, 30);
		rbBall = GetComponent<Rigidbody2D>();

		children = rbBall.GetComponentsInChildren<Transform>();
		renderers = rbBall.GetComponentsInChildren<Renderer>();
		renderers[1].enabled = false;

		speed = gc.GetSpeed();

		bs = BallStatus.Wait;

		fLastTime = Time.time;
	}


	// Update is called once per frame
	void Update () {
		if(iBallID != 0){
			fZRotDeg = rbBall.transform.eulerAngles.z;

			if (fZRotDeg > 180)
			{
				fZRotDeg = 360 - fZRotDeg;
				fZRotAng = fZRotDeg / 180 * 3.14159f;
			}
			else
			{
				fZRotDeg *= -1;
				fZRotAng = fZRotDeg / 180 * 3.14159f;
			}

			EndP = Input.mousePosition;


			if(mg[0].IsReady()){

				if (Input.GetMouseButtonDown(0))	// Click mouse left button 
				{
					mg[0].MouseDown();

					if(1 == iBallID)
						renderers[1].enabled = true;// Show shotting bar

					rbBall.MoveRotation(30);

					startP = Input.mousePosition;
				}

				if (Input.GetMouseButtonUp(0))		// Release mouse left button
				{
					mg[0].fTime = Time.time;
					mg[0].MouseUp();
					renderers[1].enabled = false;	// Hide shooting bar

					if(fShootPower > 10){
						if(BallStatus.Wait == bs){
							StartCoroutine(Shoot());
						}
					}
				}

				if(BallStatus.End == bs)
				{
					bs = BallStatus.Wait;
				}
			}

			if(mg[0].IsMouseDown())		// Draw shoot bar
			{
				const float fMinShootPower = 6;
				const float fMaxShootPower = 30;

				float fDistance = Vector2.Distance(startP, EndP);
				fShootPower = fDistance * 0.1F;

				if(fShootPower < fMinShootPower)
					children[1].localScale = new Vector3(10, fMinShootPower, 10);
				else if(fShootPower > fMaxShootPower)
					children[1].localScale = new Vector3(10, fMaxShootPower, 10);
				else
					children[1].localScale = new Vector3(10, fShootPower, 10);

				rbBall.MoveRotation((EndP.x - startP.x) * 0.3F);
			}

			if(BallStatus.Assemble == bs)
			{
				transform.position = Vector2.MoveTowards(transform.position, mg[0].GetAP(), Time.deltaTime * 10);
				if(mg[0].GetAP().Equals((Vector2)transform.position)){
					bs = BallStatus.End;
					mg[0].AddHoldBall();
				}
			}

			if(BallStatus.End == bs)
			{
				transform.position = mg[0].GetAP();
			}
		}
	}

	// ***************** Mouse Methods End *****************


	public void ShootButton()
	{
		vShoot = new Vector2(Mathf.Sin(fZRotAng) * 30, Mathf.Cos(fZRotAng) * 30);
		rbBall.AddForce(vShoot * speed);
	}

	public void RightButton()
	{
		rbBall.transform.Rotate(0, 0, -60 * Time.deltaTime);
	}

	public void LeftButton()
	{
		rbBall.transform.Rotate(0, 0, 60 * Time.deltaTime);
	}


	void FixedUpdate()
	{

	}

	void OnCollisionEnter2DChild(Collider2D other)
	{
		
	}


	void OnTriggerEnter2D(Collider2D other)
	{
		if (other.gameObject.CompareTag("Cube"))
		{
			Cube[] c = other.gameObject.GetComponentsInParent<Cube>();
			c[0].DecreadeHP();
		}

		if (other.gameObject.CompareTag("Botton"))
		{
			// Remove force
			rbBall.velocity = Vector2.zero;
			rbBall.angularVelocity = 0;

			if(false == mg[0].IsAssemble()) {
				Vector2 vAP = new Vector2(this.transform.position.x, -3.8F);
				mg[0].SetAP(vAP);	// Set assemble point
				mg[0].AddRounds();
				mg[0].SetAssemble();
			}

			if(BallStatus.Shoot == bs) {
				bs = BallStatus.Assemble;
			}
		}
	}

	public void SetSpeed()
	{
		speed = 0.5F;
	}

	public void SetBallID(int i)
	{
		iBallID = i;
	}

	public int GetBallID()
	{
		return iBallID;
	}

	IEnumerator Shoot() {
		yield return new WaitForSeconds((iBallID-1) * gc.GetShootWait());
		vShoot = new Vector2(Mathf.Sin(fZRotAng) * 30, Mathf.Cos(fZRotAng) * 30);
		rbBall.AddForce(vShoot * speed);
		bs = BallStatus.Shoot;

		mg[0].SubHoldBall();
		mg[0].fTime = Time.time;
	}

}
