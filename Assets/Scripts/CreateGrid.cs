using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateGrid : MonoBehaviour {

    public GameObject tile;
    public int worldWidth = 100;
    public int worldHeight = 100;
    public int tileX = 20;
    public int tileZ = 10;

	private int tileNumber = 1;

	void Awake()
	{
		CreateWorld();

		if (PlayerPrefs.GetInt ("flag", 0) == 1) 
		{
			PlayerPrefs.SetInt ("flag", 0);
			Data data = new Data ();
			data.Load(PlayerPrefs.GetString(""));
		}
		GameObject.Find ("LoadingScreen").SetActive (false);
	}

	// Use this for initialization
	void Start () {
        
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    void CreateWorld()
    {
        for (int x = 0; x < worldWidth; x += tileX) {
            for (int z= 0; z < worldHeight; z += tileZ) {
                GameObject block = Instantiate(tile, this.transform);
				block.name = "Tile" + z + "x"+x;
				tileNumber++;
                block.transform.position = new Vector3(this.transform.position.x + x, this.transform.position.y, this.transform.position.z + z);
            }
        }
    }
}
