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
	private Data data; //The class that handles saving and loading
	private ErrorHandler errorHandler;

	//Public Variables
	public Button btn;

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
		data = new Data (btn);
		data.Info = new List<Asset> ();
		errorHandler = GameObject.FindWithTag ("Menu").GetComponent<ErrorHandler> ();
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
			string path = StandaloneFileBrowser.OpenFilePanel("Load Game World", Application.dataPath + @"\Saves", "dat", false)[0];

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
			errorHandler.Error("Load Error", e.Message);           
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
			string path = StandaloneFileBrowser.SaveFilePanel("Save Game World", Application.dataPath + @"\Saves", "newSave.dat", "dat");

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
			errorHandler.Error("Save Error", e.Message);
        }
    }

	/*
	*  METHOD	    : AddToAssetList()
    *  DESCRIPTION  : This method is responsible for adding assets to the asset list
	*  PARAMETERS	: Asset asset : the asset you are trying to add to the data class
    *  RETURNS  	: Nothing
	*/
	public void AddToAssetList(Asset asset)
	{
		data.Info.Add(asset);
	}
}
