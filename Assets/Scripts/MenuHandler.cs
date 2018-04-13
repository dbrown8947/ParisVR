/*
* FILE			: MenuHandler.cs
* PROJECT		: Final Game Dev
* PROGRAMMERS	: Marco Fontana
* FIRST VERSION	: 9-29-2017
* DESCRIPTION   : This file contains the code and functionality required to handle the button click events on the main menu. Repurposed from my Game Dev assignment 1
*/
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using SFB;

public class MenuHandler : MonoBehaviour
{
	public GameObject loadScreen;
	private ErrorHandler errorHandler;

	//FUNCTION      : Start()
	//DESCRIPTION   : This Method is used to initalize variables when the application launches
	//PARAMETERS    : Nothing
	//RETURNS		: Nothing
	void Start ()
	{
		//Reset the save flag incase there was an inproper shutdown during the load process
		PlayerPrefs.SetInt ("flag", 0);
		PlayerPrefs.SetInt ("VR", 0);

		//Only activate if this is the menu scene
		if (SceneManager.GetActiveScene ().name.CompareTo ("Menu") == 0) {
			//Get the error handler
			errorHandler = GameObject.FindWithTag ("Menu").GetComponent<ErrorHandler> ();
		}
	}

	//FUNCTION      : srtBtn()
	//DESCRIPTION   : This Method is responsible for launching the game from the menu
	//PARAMETERS    : Nothing
	//RETURNS		: Nothing
	public void srtBtn ()
	{
		//Find the path
		string path = PlayerPrefs.GetString ("xml", Application.dataPath);

		//check the file to make sure its valid for loading
		if (path.Contains (".osm") == true || path.Contains (".xml") == true) {
			StartCoroutine (StartLoad ());
		} else {
			errorHandler.Error ("Start Error", "Please select a .osm/.osm.xml file"); 
		}
	}

	//FUNCTION      : infoBtn()
	//DESCRIPTION   : This Method is responsible for launching the info menu from the main menu
	//PARAMETERS    : Nothing
	//RETURNS		: Nothing
	public void infoBtn ()
	{
		//Load the menu scene from the scene list
		SceneManager.LoadScene ("Info");
	}

	//FUNCTION      : quitBtn()
	//DESCRIPTION   : This Method is responsible for Closing the application
	//PARAMETERS    : Nothing
	//RETURNS		: Nothing
	public void quitBtn ()
	{
		//Close the application
		Application.Quit ();
	}

	//FUNCTION      : MainMenu()
	//DESCRIPTION   : This Method is responsible for returning to the mainmenu
	//PARAMETERS    : Nothing
	//RETURNS		: Nothing
	public void MainMenu ()
	{
		SceneManager.LoadScene ("Menu");
	}


	//FUNCTION      : Load()
	//DESCRIPTION   : This Method is responsible for loading a previous save from the menu
	//PARAMETERS    : Nothing
	//RETURNS		: Nothing
	public void Load ()
	{
		string[] hold;

		//Open the file using the standalonefilebrowser plugin
		hold = StandaloneFileBrowser.OpenFilePanel ("Load Game World", PlayerPrefs.GetString ("save", Application.dataPath), "dat", false);

		//If the path length is zero thorw an exception
		if (hold.Length != 0) {
			PlayerPrefs.SetString ("lLoc", hold [0]);
			PlayerPrefs.SetInt ("flag", 1);

			StartCoroutine (StartLoad ());
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
		//Activate the load screen
		loadScreen.SetActive (true);

		yield return new WaitForFixedUpdate ();

		//Load the first level of the game
		SceneManager.LoadScene ("prototype");
	}


	//FUNCTION      : Options()
	//DESCRIPTION   : This Method is responsible for opening thew options menu
	//PARAMETERS    : Nothing
	//RETURNS		: Nothing
	public void Options ()
	{
		SceneManager.LoadScene ("Options");
	}
}
