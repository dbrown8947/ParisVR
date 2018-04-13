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


	void Start()
	{
		PlayerPrefs.SetInt("flag", 0);
	}

	//FUNCTION      : srtBtn()
	//DESCRIPTION   : This Method is responsible for launching the game from the menu
	//PARAMETERS    : Nothing
	//RETURNS		: Nothing
    public void srtBtn()
    {
		//Load the first level of the game
        SceneManager.LoadScene("prototype");
    }

	//FUNCTION      : infoBtn()
	//DESCRIPTION   : This Method is responsible for launching the info menu from the main menu
	//PARAMETERS    : Nothing
	//RETURNS		: Nothing
    public void infoBtn()
    {
		//Load the menu scene from the scene list
        SceneManager.LoadScene("Info");
    }

	//FUNCTION      : quitBtn()
	//DESCRIPTION   : This Method is responsible for Closing the application
	//PARAMETERS    : Nothing
	//RETURNS		: Nothing
    public void quitBtn()
    {
		//Close the application
        Application.Quit();
    }

	//FUNCTION      : MainMenu()
	//DESCRIPTION   : This Method is responsible for returning to the mainmenu
	//PARAMETERS    : Nothing
	//RETURNS		: Nothing
    public void MainMenu()
    {
		SceneManager.LoadScene("Menu");
    }


	//FUNCTION      : Load()
	//DESCRIPTION   : This Method is responsible for loading a previous save from the menu
	//PARAMETERS    : Nothing
	//RETURNS		: Nothing
	public void Load()
	{
			string[] hold;

			//Open the file using the standalonefilebrowser plugin
			hold = StandaloneFileBrowser.OpenFilePanel("Load Game World", PlayerPrefs.GetString ("save", Application.dataPath), "dat", false);

			//If the path length is zero thorw an exception
			if(hold.Length != 0)
			{
				PlayerPrefs.SetString("lLoc", hold[0]);
				PlayerPrefs.SetInt("flag", 1);

			   StartCoroutine (StartLoad ());
			}
	}
		

	IEnumerator StartLoad()
	{
		loadScreen.SetActive (true);

		yield return new WaitForFixedUpdate ();

		srtBtn ();

	}


	//FUNCTION      : Options()
	//DESCRIPTION   : This Method is responsible for opening thew options menu
	//PARAMETERS    : Nothing
	//RETURNS		: Nothing
	public void Options()
	{
		SceneManager.LoadScene ("Options");
	}
}
