/*
* FILE			: SLMenuHandler.cs
* PROJECT		: ParisVR
* PROGRAMMERS	: Marco Fontana
* FIRST VERSION	: 1-20-2018
* DESCRIPTION   : This file contains the code and functionality required handle the menus required to save and load game worlds
*/
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using SFB;


/*
* Class Name	: SLMenuHandler 
* DESCRIPTION   : This class is responsible for handling any menuing involving the save and load features of the application. This class
*                 will launch the save/load menu and pass errors to an error handler so that the user can see problems that occur and allows
*                 them to save/load game worlds.
*/
public class SLMenuHandler : MonoBehaviour
{
	//Private Variables
	private Data data;
	//The class that handles saving and loading
	private ErrorHandler errorHandler;

	//Public Variables
	public Button btn;
	public GameObject loadScreen;

	void Awake ()
	{
		//Initalize private variables
		data = new Data (btn);
		data.Info = new List<Asset> ();
	}

	/*
	*  METHOD	    : Start()
    *  DESCRIPTION  : This Method is launched when the level is loaded and is used to gather
	*                 information or initalize other variable
	*  PARAMETERS	: Nothing
    *  RETURNS  	: Nothing
    * 
	*/
	void Start ()
	{

		errorHandler = GameObject.FindWithTag ("Menu").GetComponent<ErrorHandler> ();
	}
		
	/*
	*  METHOD	    : OpenLoad()
    *  DESCRIPTION  : This method is responsible for changing the save/load menu into the load format and
    *                 activating it. This is an onclick handler.
	*  PARAMETERS	: Nothing
    *  RETURNS  	: Nothing
	*/
	public void OpenLoad ()
	{
		try {
			string[] hold;

			//Open the file using the standalonefilebrowser plugin
			hold = StandaloneFileBrowser.OpenFilePanel ("Load Game World", PlayerPrefs.GetString ("save", Application.dataPath), "dat", false);

			//If the path length is zero thorw an exception
			if (hold.Length == 0) {
				//Do Nothing
			} else {
				PlayerPrefs.SetString ("lLoc", hold [0]);
				PlayerPrefs.SetInt ("flag", 1);

				StartCoroutine (StartLoad ());
			}
		} catch (Exception e) {
			//If we are loading make sure that the error displays as a load error
			errorHandler.Error ("Load Error", e.Message);           
		}
	}

	/*
	*  METHOD	    : OpenSave()
    *  DESCRIPTION  : This method is responsible for changing the save/load menu into the save format and
    *                 activating it. This is an onclick handler.
	*  PARAMETERS	: Nothing
    *  RETURNS  	: Nothing
	*/
	public void OpenSave ()
	{
		try {
			//Save the file using the standalonefilebrowser plugin
			string path = StandaloneFileBrowser.SaveFilePanel ("Save Game World", PlayerPrefs.GetString ("save", Application.dataPath), "newSave.dat", "dat");

			//If the path length is zero thorw an exception
			if (path.Length == 0) {
				//Do nothing
			} else {
				//Otherwise save the data through the data class
				data.Save (path);
			}
		} catch (Exception e) {
			//If we are Saving make sure that the error displays as a save error
			errorHandler.Error ("Save Error", e.Message);
		}
	}

	/*
	*  METHOD	    : AddToAssetList()
    *  DESCRIPTION  : This method is responsible for adding assets to the asset list
	*  PARAMETERS	: Asset asset : the asset you are trying to add to the data class
    *  RETURNS  	: Nothing
	*/
	public void AddToAssetList (Asset asset)
	{
		data.Info.Add (asset);
	}

	/*
	*  METHOD	    : RemoveAssetFromList()
    *  DESCRIPTION  : This method is responsible for adding assets to the asset list
	*  PARAMETERS	: GameObject obj  : the asset we want to remove from the list
    *  RETURNS  	: Nothing
	*/
	public void RemoveAssetFromList (GameObject obj)
	{
		//find the asset in the list and remove it so that we dont accidently try to save it later
		for (int i = 0; i < data.Info.Count; i++) {
			if (data.Info [i].ParentInfo.Area.CompareTo (obj.name) == 0) {
				data.Info.RemoveAt (i);
				break;
			}
		}
	}

	/*
	*  METHOD	    : Load()
    *  DESCRIPTION  : This Method is responsible for loading a save file
	*  PARAMETERS	: string path  : the location of the save file
    *  RETURNS  	: Nothing
	*/
	public void Load (string path)
	{
		try {
			//Load the file at the data string
			data.Load (path);
		} catch (Exception e) {
			//If there is an error display the error
			errorHandler.Error ("Load Error", e.Message); 
		}
	}

	/*
	*  METHOD	    : StartLoad()
    *  DESCRIPTION  : This Method is used to start the load process so that a load screen will appear and the
    * 				  user will be unable to see the world rendering. Addtionally, this is will make it so the game
    *                 doesnt look like it is locking up
	*  PARAMETERS	: Nothing
    *  RETURNS  	: IEnumerator
    * 
	*/
	IEnumerator StartLoad ()
	{
		//Activate the loadScreen
		loadScreen.SetActive (true);

		//Resume the game so the load process can continue
		GameObject.FindWithTag ("Menu").GetComponent<PauseScript> ().Resume ();

		yield return new WaitForFixedUpdate ();

		//Load the next scene
		SceneManager.LoadScene ("prototype");
	}


	/*
	*  METHOD	    : ToggleVR()
    *  DESCRIPTION  : This method is responsible for toggle VR on
	*  PARAMETERS	: GameObject player  : The player object 
	*/
	public void ToggleVR (GameObject player)
	{
		player.GetComponent<VRToggle> ().setVRMode (true);
		GetComponent<PauseScript> ().Resume ();
	}


	public void VRSave (string filename)
	{
		string file = PlayerPrefs.GetString ("save", Application.dataPath) + "/" + filename + ".dat";
		data.Save (file);
		Debug.Log (file);
	}

	public void VRLoad (string filename)
	{		

		string file = PlayerPrefs.GetString ("save", Application.dataPath) + "/" + filename;
		PlayerPrefs.SetString ("lLoc", file);
		PlayerPrefs.SetInt ("flag", 1);
		PlayerPrefs.SetInt ("VR", 1);
		SceneManager.LoadScene ("prototype");

		//data.Load (file);
	}
}
