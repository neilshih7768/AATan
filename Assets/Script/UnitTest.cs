using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UnitTest : MonoBehaviour {

	public GameObject cube;
	public GameObject ball;
	public GameObject wall;

	List<GameObject> ListCube = new List<GameObject>();

	// Use this for initialization
	void Start () {

		SetBall ();
		SetCubes ();
		SetWall ();
		
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButtonDown(1))
		{
			while (ListCube.Remove(null)) {};

			for(int i = 0; i < ListCube.Count; i++) {
				Cube cCube = ListCube[i].GetComponent<Cube>();
				cCube.DestroyWithouFX();
			}

			SetCubes();

			//Cube cCube = goCube.GetComponent<Cube>();
			//cCube.SetHP(3);
		}
	}


	void SetBall() {
		Vector2 vPointBall = new Vector2(0, -3F); // Set position
		GameObject goBall = (GameObject)Instantiate(ball, vPointBall, Quaternion.identity);
	}


	void SetCubes() {
		Vector2 vPointBoxQueue = new Vector2(-0.375F, 0); // Set position
		GameObject goCube = (GameObject)Instantiate(cube, vPointBoxQueue, Quaternion.identity);
		ListCube.Add(goCube);

		Cube cCube = goCube.GetComponent<Cube>();
		cCube.SetHP(99);
		//cCube.SetColor(Color.green);
		cCube.SetID(0);
		cCube.SetLocation(vPointBoxQueue);
		
		
		Vector2 vPointBoxQueue2 = new Vector2(0.375F, 0); // Set position
		goCube = (GameObject)Instantiate(cube, vPointBoxQueue2, Quaternion.identity);
		ListCube.Add(goCube);

		cCube = goCube.GetComponent<Cube>();
		cCube.SetHP(99);
		//cCube.SetColor(Color.green);
		cCube.SetID(1);
		cCube.SetLocation(vPointBoxQueue2);
	}


	void SetWall() {
		Vector2 vPointWall = new Vector2(3.335F, -0.0F); // Set position
		GameObject gowall = (GameObject)Instantiate(wall, vPointWall, Quaternion.identity);
	}

}
