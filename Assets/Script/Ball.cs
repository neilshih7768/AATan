//#define MOBILE

using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

enum BallStatus{
	Wait,
	Shoot,
	Assemble
}

public class Ball : MonoBehaviour {
	MainGame[] mg;

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
	bool bMouseDown = false;
	float fShootPower;

	int iBallID;	// first ball is 1

	// Use this for initialization
	void Start () {
		GameObject go = GameObject.Find("Main Game");
		mg = go.GetComponents<MainGame>();

		vShoot = new Vector2(0, 30);
		rbBall = GetComponent<Rigidbody2D>();

		children = rbBall.GetComponentsInChildren<Transform>();
		renderers = rbBall.GetComponentsInChildren<Renderer>();
		renderers[1].enabled = false;

		speed = 15;

		bs = BallStatus.Wait;
	}


	// Update is called once per frame
	void Update () {
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

		#if MOBILE
		// **************** TouchPad methods ****************
		if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
		{
			bMouseDown = true;
			renderers[1].enabled = true;

			rbBall.MoveRotation(30);		// Set direct up

			startP = Input.mousePosition;

			Debugtext.text = "Touch";
		}

		if(Input.GetTouch(0).phase == TouchPhase.Canceled)
		{
			bMouseDown = false;
			renderers[1].enabled = false;

			vShoot = new Vector2(Mathf.Sin(fZRotAng) * 30, Mathf.Cos(fZRotAng) * 30);
			rbBall.AddForce(vShoot * speed);

			Debugtext.text = "Release";
		}
		// **************** TouchPad methods End ****************
		#endif

		// ***************** Mouse Methods *****************


		if (Input.GetMouseButtonDown(1))
		{
			
		}

		if(mg[0].IsReady()){

			if (Input.GetMouseButtonDown(0))	// Click mouse left button 
			{
				bMouseDown = true;

				if(1 == iBallID)
					renderers[1].enabled = true;// Show shotting bar

				rbBall.MoveRotation(30);

				startP = Input.mousePosition;
			}

			if (Input.GetMouseButtonUp(0))		// Release mouse left button
			{
				bMouseDown = false;
				renderers[1].enabled = false;	// Hide shooting bar

				if(fShootPower > 10){
					if(BallStatus.Wait == bs){
						StartCoroutine(Shoot());
					}
				}
			}
		}

		if(bMouseDown)		// Draw shoot bar
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
				bs = BallStatus.Wait;
				mg[0].AddHoldBall();
			}
		}

		if(BallStatus.Wait == bs)
		{
			transform.position = mg[0].GetAP();
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
				mg[0].SetAP(this.transform.position);	// Set assemble point
				mg[0].AddRounds();
				//mg[0].NewBall().SetBallID( mg[0].GetRounds() );
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
		yield return new WaitForSeconds((iBallID-1)*0.05F);
		vShoot = new Vector2(Mathf.Sin(fZRotAng) * 30, Mathf.Cos(fZRotAng) * 30);
		rbBall.AddForce(vShoot * speed);
		bs = BallStatus.Shoot;
		mg[0].SetNotAssemble();
		mg[0].SubHoldBall();
	}

}
