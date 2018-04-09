/*
* FILE			: MenuHandler.cs
* PROJECT		: Final Game Dev
* PROGRAMMERS	: Marco Fontana
* FIRST VERSION	: 9-29-2017
* DESCRIPTION   : This file contains the code and functionality required to handle the button click events on the main menu. Repurposed from my Game Dev assignment 1
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuHandler : MonoBehaviour
{
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
