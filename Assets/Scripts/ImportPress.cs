/*
* FILE			: ImportPress.cs
* PROJECT		: ParisVR
* PROGRAMMERS	: Marco Fontana, Anthony Bastos, Dustin Brown
* FIRST VERSION	: 1-18-2018
* DESCRIPTION   : This file contains the code and functionality required access import functionalites for standard import
*                 as well as save and load
*/
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Threading;
using SFB;

public class ImportPress : MonoBehaviour
{
	//Public variables
	public Button btn;
	public Dropdown dropdown;
	public GameObject plyr;
	public GameObject obj;
	public GameObject Parent;
	public GameObject Menu;
	public GameObject Camera;
	public GameObject loadNotifcation;
	public Material defaultMat;

	//Private Variables
	private GameObject building;
	// Use this for initialization


	/*
    * FUNCTION : Start
    *
    * DESCRIPTION : start click listener
    */
	void Start ()
	{
		btn.onClick.AddListener (onClick);
	}

	/*
    * FUNCTION : onClick
    *
    * DESCRIPTION : When Import button is clicked
    */
	void onClick ()
	{
		if (plyr.GetComponent<PlayerController> ().gridSelected) {
			StartCoroutine (Importation ());
		}
	}

	/*
	*  METHOD	    : Importation()
    *  DESCRIPTION  : This Method is used to start the improtation process as well as notify the user that an import is currently
    *                 occuring
	*  PARAMETERS	: Nothing
    *  RETURNS  	: IEnumerator
    * 
	*/
	IEnumerator Importation ()
	{
		//Method varaibles
		string path = "";
		Asset asset = new Asset ();
		string[] hold;

		//Trigger the notification screen
		loadNotifcation.SetActive (true);

		//Wait for the fixed update so we can show the notification
		yield return new WaitForFixedUpdate ();

		//Attempt to get a file path for the asset we want to import
		hold = StandaloneFileBrowser.OpenFilePanel ("Open Object", PlayerPrefs.GetString ("asset", Application.dataPath), "obj", false);

		//If the user has selected only one file
		if (hold.Length == 1) {
			//Start the importation process
			path = hold [0];
			building = OBJLoader.LoadOBJFile (path, defaultMat);
			obj = plyr.GetComponent<PlayerController> ().obj;
			Parent = plyr.GetComponent<PlayerController> ().Parent;
			ApplySettings (asset, path);
		} 	
		//Disable the notification screen
		loadNotifcation.SetActive (false);
	}


	/*
    * FUNCTION : ApplySettings
    * PARAMETERS: Asset asset: the asset save information, String path : the location of the .obj file
    * DESCRIPTION : Applies properties to an import object
    */
	public void ApplySettings (Asset asset, string path)
	{
		//Destroy the duplicate building and set the base postion
		Destroy (building);
		building.transform.position = new Vector3 (0, 0, 0);

		//Find the center of the object

		Vector3 center = Vector3.zero;
		foreach (Transform child in building.transform) {
			center += child.gameObject.GetComponent<Renderer> ().bounds.center;
		}
		center /= building.transform.childCount; //center is average center of children

		center = (center * 0.3048f) / 12;//Real unity center for object

		//Apply the building with the asset tag and set its scale into unity units
		building.tag = "Asset";
		building.transform.localScale = new Vector3 (0.0257f, 0.0257f, 0.0257f);

		//Create the new building using the infromation of the loading building at the location of the parent object
		GameObject newBuilding = Instantiate (building, Parent.transform);
		float x = center.x;
		float y = center.y;
		float z = center.z;

		//Apply the base transformations to the object so we can center it inside the parent object
		newBuilding.transform.localPosition = new Vector3 (newBuilding.transform.localPosition.x - (x), newBuilding.transform.localPosition.y - (y), newBuilding.transform.localPosition.z - (z));
		newBuilding.AddComponent<Text> ();

		//Populate save information
		asset.ParentInfo.Name = newBuilding.name;
		asset.ParentInfo.Area = newBuilding.transform.parent.parent.name;
		asset.ParentInfo.Tile = newBuilding.transform.parent.name;

		//Find the file name of the asset imported
		string[] splits = path.Split ('\\', '/');
		asset.ParentInfo.FileName = splits [splits.Length - 1]; 

		//Find the file name and apply it to each saved asset
		splits = PlayerPrefs.GetString ("xml").Split ('\\', '/');
		asset.MapName = splits [splits.Length - 1];

		//Reapply the base shader on a lot so that the lot can be used later
		GameObject baseObj = obj.transform.GetChild (0).gameObject;
		baseObj.GetComponent<Renderer> ().material = plyr.GetComponent<PlayerController> ().baseShader;

		//Disable menus and lots 
		obj.SetActive (false);
		Menu.SetActive (false);
		GameObject lot = Parent.transform.GetChild (0).gameObject; 
		lot.SetActive (false);

		//Attempt to center a highlight around the imported asset
		CenterHighlight (newBuilding);

		//Reset flags, add information to the save list and apply final settings
		plyr.GetComponent<PlayerController> ().gridSelected = false;
		plyr.GetComponent<PlayerController> ().selected = false;
		GameObject.Find ("PauseMenu").GetComponent<SLMenuHandler> ().AddToAssetList (asset);
		Parent.transform.position += new Vector3 (0, 6, 0);
	}

	/*
	*  METHOD	    : ApplySettings()
    *  DESCRIPTION  : This Method is used to start the improtation process when a file is loaded. It will apply the saved transformations to 
    *                 the imported objects automatically.
	*  PARAMETERS	: TempVector pos    : The saved position of the asset
	* 				  TempVector rot	: The saved rotation of the asset
	* 			      TempVector scale	: The saved scale of the asset
	*                 string area		: The name of the area where the asset was placed
	*                 string tile		: The name of the tile where the asset was placed
	*                 string fileName	: The file name used to import the object
	*                 string tagged		: The tag string attached to the object
    *  RETURNS  	: Nothing
    * 
	*/
	public void ApplySettings (TempVector pos, TempVector rot, TempVector scale, string area, string tile, string fileName, string tagged)
	{
		try {
			//Load the building from the obj file
			building = OBJLoader.LoadOBJFile (PlayerPrefs.GetString ("asset", Application.dataPath) + @"\" + fileName, defaultMat);

			//Find the parent object and set a base rotation
			Parent = FindTileParent (area, tile);
			Quaternion rotation = new Quaternion (0, 0, 0, 0);

			//Destroy the duplciated building
			Destroy (building);

			//Set inital coordinates for the building
			building.transform.position = new Vector3 (0, 0, 0);
			building.transform.rotation = rotation;

			//Find the center of the object
			Vector3 center = Vector3.zero;
			foreach (Transform child in building.transform) {
				center += child.gameObject.GetComponent<Renderer> ().bounds.center;
			}
			center /= building.transform.childCount; //center is average center of children

			center = (center * 0.3048f) / 12;//Real unity center for object

			//Apply the building with the asset tag and set its scale into unity units
			building.tag = "Asset";
			building.transform.localScale = new Vector3 (0.0257f, 0.0257f, 0.0257f);

			//Create the new building using the infromation of the loading building at the location of the parent object
			GameObject newBuilding = Instantiate (building, Parent.transform);
			float x = center.x;
			float y = center.y;
			float z = center.z;

			//Apply the base transformations to the object so we can center it inside the parent object
			newBuilding.transform.localPosition = new Vector3 (newBuilding.transform.localPosition.x - (x), newBuilding.transform.localPosition.y - (y), newBuilding.transform.localPosition.z - (z));

			//Add the text component to the asset and apply its tag
			newBuilding.AddComponent<Text> ();
			newBuilding.GetComponent<UnityEngine.UI.Text> ().text = tagged;

			//Center the highlight for the object around the building
			CenterHighlight (newBuilding);

			//Apply all the saved transformations to the parent
			Parent.transform.localPosition = new Vector3 (pos.X, pos.Y, pos.Z);
			Parent.transform.localEulerAngles = new Vector3 (rot.X, rot.Y, rot.Z);
			Debug.Log (Parent.transform.localEulerAngles);
			Parent.transform.localScale = new Vector3 (scale.X, scale.Y, scale.Z);

			//Disable the gound and hit box of the lot so the building will be unobstructed
			GameObject lot = Parent.transform.GetChild (0).gameObject; 
			GameObject hitBox = Parent.transform.GetChild (1).gameObject; 
			hitBox.SetActive (false);
			lot.SetActive (false);
		} catch (Exception) {
			throw new Exception ("Error in Loading Process. Asset Not Found or Missing Lot");
		}
	}

	/*
	*  METHOD	    : FindTileParent()
    *  DESCRIPTION  : This Method is used to find the correct tile to place the saved asset onto
	*  PARAMETERS	: string name : the name of the grandparent
	*                 string tile : the parent name
    *  RETURNS  	: GameObject par: the parent object
	*/
	GameObject FindTileParent (string name, string tile)
	{
		//Find all the gridtile tagged objects in the game world
		GameObject par = null;
		GameObject[] tiles = GameObject.FindGameObjectsWithTag ("GridTile");

		foreach (GameObject obj in tiles) {
			//Keep searching all gridtile objects until we find the one that we need
			if (obj.transform.parent.name.CompareTo (name) == 0 && obj.transform.name.CompareTo (tile) == 0) {
				//Save the obj when found and break the loop
				par = obj;
				break;
			}
		}
		return par;
	}

	/*
	*  METHOD	    : CenterHighlight()
    *  DESCRIPTION  : This Method is used to scale a highlight object around an imported asset
	*  PARAMETERS	: GameObject newBuilding : the building object we want to wrap the highlighter around
    *  RETURNS  	: Nothing
	*/
	void CenterHighlight (GameObject newBuilding)
	{
		//Center the highlight for the object around the building
		Vector3 assetSize = (building.GetComponent<BoxCollider> ().size * 0.0254f);
		Vector3 buildingCenter = (building.GetComponent<BoxCollider> ().center * 0.0254f);

		//Access the highlight object inside the parent object
		GameObject HighlightBox = Parent.transform.GetChild (2).gameObject;

		//Scale the highlighter around the building
		HighlightBox.transform.localScale = assetSize;
		HighlightBox.transform.position = newBuilding.transform.position + buildingCenter;
	}
}
