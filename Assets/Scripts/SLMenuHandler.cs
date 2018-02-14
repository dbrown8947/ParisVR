/*
* FILE			: SLMenuHandler.cs
* PROJECT		: ParisVR Tool
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
using UnityEditor;
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

	//The class that handles saving and loading
	private Data data; 

	//Error Handler Variables
	public Transform errorMenu;
	public Text eLabel;
	public Text eText;


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
		//Initalize private variables
		data = new Data ();
	}


	/*
	*  METHOD	    : ErrorHandler()
    *  DESCRIPTION  : This method is responsible for displaying an error and the text involved with 
    *                 that error to the user.
	*  PARAMETERS	: string errorType : The error type i.e save error
	* 				  string error     : The contents of the error i.e what went wrong
    *  RETURNS  	: Nothing
	*/
	void ErrorHandler(string errorType, string error)
	{
		//Display the error to the user with the proper information
		eLabel.text = errorType + " Error";
		eText.text = error;	
		errorMenu.gameObject.SetActive (true);
	}

	/*
	*  METHOD	    : CloseErrorHandler()
    *  DESCRIPTION  : This method is responsible for closing the error handler after the user is done with it.
    *                 This is an onclick handler.
	*  PARAMETERS	: Nothing
    *  RETURNS  	: Nothing
	*/
	public void CloseErrorHandler()
	{
		//Close the error handler and enable the save/load buttons again
		errorMenu.gameObject.SetActive (false);
	}

	/*
	*  METHOD	    : OpenLoad()
    *  DESCRIPTION  : This method is responsible for changing the save/load menu into the load format and
    *                 activating it. This is an onclick handler.
	*  PARAMETERS	: Nothing
    *  RETURNS  	: Nothing
	*/
	public void OpenLoad()
	{
        try
        {
			//Open the file using the standalonefilebrowser plugin
			string path = StandaloneFileBrowser.OpenFilePanel("Load Game World", "", "dat", false)[0];

			//If the path length is zero thorw an exception
            if(path.Length == 0)
            {
                throw new Exception("No File Selected, Or The File Was Not Found");
            }
            else
            {
				//Load the file through the data class.
                data.Load(path);
            }
        }
        catch (Exception e)
        {
            //If we are Saving make sure that the error displays as a save error
			ErrorHandler ("Load", e.Message);           
        }
    }

	/*
	*  METHOD	    : OpenSave()
    *  DESCRIPTION  : This method is responsible for changing the save/load menu into the save format and
    *                 activating it. This is an onclick handler.
	*  PARAMETERS	: Nothing
    *  RETURNS  	: Nothing
	*/
	public void OpenSave()
	{
        try
        {
			//Save the file using the standalonefilebrowser plugin
			string path = StandaloneFileBrowser.SaveFilePanel("Save Game World", "", "newSave.dat", "dat");

			//If the path length is zero thorw an exception
           if(path.Length == 0)
           {
                throw new Exception("No File Selected, Or The File Was Not Found");
            }
           else
           {
				//Otherwise save the data through the data class
                data.Save(path);
           }
        }
        catch(Exception e)
        {
            //If we are Saving make sure that the error displays as a save error
			ErrorHandler ("Save", e.Message);
        }
    }


	public void AddToAssetList(Asset asset)
	{
		data.Info.Add(asset);
	}
}
