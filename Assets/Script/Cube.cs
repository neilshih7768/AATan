using UnityEngine;
using System.Collections;

public class Cube : MonoBehaviour {
    public GameObject dsetroyFX;
    GameObject cubeMeshText;
    int iCubeID;
    int iHP;
    Color cCubeColor = Color.blue;
    Color[] cColorArray = new Color[] { Color.red, Color.blue, Color.cyan, Color.green, Color.yellow, Color.white };
    Vector3 vLocation;

    // Initialization
    void Start () {

    }


    // Update methods
    void Update () {
        UpdateHP();
        UpdateColor();
        UpdateCube();
    }


    void UpdateHP()
    {
        cubeMeshText = this.transform.GetChild(0).gameObject;
        cubeMeshText.GetComponent<TextMesh>().text = iHP.ToString();
        //print("iHP = " + iHP);
    }


    void UpdateColor()
    {
        int i = iHP / 10;
        if (i < 0)
            i = 0;
        else if (i >= cColorArray.Length)
            i = cColorArray.Length - 1;

        cCubeColor = cColorArray[i];

        this.GetComponent<Renderer>().material.color = cCubeColor;  // Change cube color

        cubeMeshText = this.transform.GetChild(0).gameObject;       // Change mesh text color
        cubeMeshText.GetComponent<Renderer>().material.color = cCubeColor;
    }


    void UpdateCube()
    {
		if (0 == iHP) {
            Destroy();
		}
    }


    // Set methods
    public void SetID(int i){ iCubeID = i; }

	public int GetID(){ return iCubeID; }

	public void SetColor(Color c){ cCubeColor = c; }
	
	public void SetLocation(Vector3 v){ vLocation = v; }

    public void SetHP(int i)
    {
        if (0 > i)
            iHP = 0;
        else
            iHP = i;
    }


    // Other public methods
    public void DecreadeHP()
    {
        if (0 < iHP)
            iHP--;
    }


    // Other private methods
    void Destroy()
    {
        Object FX = Instantiate(dsetroyFX, vLocation, Quaternion.identity);

        Destroy(this.gameObject);
		Destroy(FX, 2);

		GameObject go = GameObject.Find("Main Game");
		MainGame[] mg = go.GetComponents<MainGame>();
		mg[0].AddScore();
    }

	public void DestroyWithouFX()
	{
		Destroy(this.gameObject);
	}
}
