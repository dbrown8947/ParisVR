﻿/*
* FILE			: Options.cs
* PROJECT		: ParisVR Tool
* PROGRAMMERS	: Marco Fontana
* FIRST VERSION	: 1-23-2018
* DESCRIPTION   : This file contains the code and functionality required handle the options menu
*/
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SFB;

public class Options : MonoBehaviour 
{
	//Public Variables
	//Drop Down Menus
	public Dropdown res;
	public Dropdown wind;
	public Dropdown dtls;
	public Dropdown scl;

	//Resoultion List
	public Resolution[] resolutions;

	//InputFields
	public InputField saveFolder;
	public InputField assetFolder;
	public InputField osmFile;

	//Private Variables
	private ErrorHandler errorHandler;

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
		//Method Variables
		string resName = Screen.currentResolution.width + " x " + Screen.currentResolution.height;
		resolutions = Screen.resolutions;
		int counter = 0;
		int hold = 0;
		bool found = false;

		errorHandler = GameObject.FindWithTag ("Menu").GetComponent<ErrorHandler> ();

		//set the details value
		dtls.value = PlayerPrefs.GetInt ("dtl", 1);

		//Find the appropriate information relating to the locations of the save, asset and xml folders/files
		saveFolder.text = PlayerPrefs.GetString ("save", Application.dataPath);
		assetFolder.text = PlayerPrefs.GetString ("asset", Application.dataPath);
		osmFile.text = PlayerPrefs.GetString ("xml", Application.dataPath);

		//For each support resolution found
		foreach (Resolution rez in resolutions)
		{
			//Add it to the dropdown menu and increase the counter
			res.options.Add(new Dropdown.OptionData(rez.width + " x " + rez.height));
			counter++;
		}

		//Set the selected index to the first value in the list
		int temp = res.value;
		res.value = res.value + 1;

		//Search the list to find if the resolution we are on is already included in the list
		for(int i=0; i < res.options.Count; i++)
		{
			//If we find the resolution that we are on in the list
			if (res.options[i].text.CompareTo(resName) == 0)
			{
				//Set the sleected value in the list to the found resoltion
				hold = i;
				found = true;
				res.value = hold;
				break;
			}
		}

		//If we didnt find the resoltuion in the list 
		if(!found)
		{
			//Add the resolution to the list
			res.options.Add(new Dropdown.OptionData(Screen.currentResolution.width + " x " + Screen.currentResolution.height));
			res.value = 1;
		}
	}


	/*
	*  METHOD	    : WindowToggle()
    *  DESCRIPTION  : This Method is called when the user tries to switch between fullscreen and windowed mode.
    *                 This is an OnClick handler
	*  PARAMETERS	: Nothing
    *  RETURNS  	: Nothing
    * 
	*/
	public void WindowToggle()
	{
		//Depending on the selected index change the fullscreen bool
		if (wind.value == 0)
		{
			Screen.SetResolution(Screen.currentResolution.width , Screen.currentResolution.height,true);
		} 
		else
		{
			Screen.SetResolution(Screen.currentResolution.width , Screen.currentResolution.height,false);
		}			
	}

	/*
	*  METHOD	    : OnResolutionChange()
    *  DESCRIPTION  : This Method is called when the user tries to switch between different resolutions.
    *                 This is an OnClick handler
	*  PARAMETERS	: int index : The selected value in the dropdown menu
    *  RETURNS  	: Nothing
    * 
	*/
	public void OnResolutionChange(int index)
	{
		//Find the selected resolution from the drop
		Resolution rez = resolutions[index];

		//Depending on the selected screen type change the resoltion accordingly
		if (wind.value == 0)
		{
			Screen.SetResolution(rez.width, rez.height,true);
		} 
		else
		{
			Screen.SetResolution(rez.width, rez.height,false);
		}			
	}


	/*
	*  METHOD	    : OnDetailsChange()
    *  DESCRIPTION  : This Method is called when the user tries to switch between different graphics levels.
    *                 This is an OnClick handler
	*  PARAMETERS	: int index : The selected value in the dropdown menu
    *  RETURNS  	: Nothing
    * 
	*/
	public void OnDetailsChange(int index)
	{
		QualitySettings.SetQualityLevel (index);
		PlayerPrefs.SetInt ("dtl", index);
	}
		
	/*
	*  METHOD	    : BrowseSaveFolder()
    *  DESCRIPTION  : This Method is called when the user tries to find a save folder.
    *                 This is an OnClick handler
	*  PARAMETERS	: Nothing
    *  RETURNS  	: Nothing
    * 
	*/
	public void BrowseSaveFolder()
	{
		FileFolderHandler ("Save Folder", 0);
	}

	/*
	*  METHOD	    : BrowseAssetFolder()
    *  DESCRIPTION  : This Method is called when the user tries to find a asset folder.
    *                 This is an OnClick handler
	*  PARAMETERS	: Nothing
    *  RETURNS  	: Nothing
    * 
	*/
	public void BrowseAssetFolder()
	{
		FileFolderHandler ("Asset Folder", 1);
	}


	/*
	*  METHOD	    : BrowseOSMFile()
    *  DESCRIPTION  : This Method is called when the user tries to find an osm File.
    *                 This is an OnClick handler
	*  PARAMETERS	: Nothing
    *  RETURNS  	: Nothing
    * 
	*/
	public void BrowseOSMFile()
	{
		FileFolderHandler ("OSM XML File", 3);
	}


	/*
	*  METHOD	    : FileFolderHandler()
    *  DESCRIPTION  : This Method is called when the user tries to pick a folder/file location.
    *                 This is an OnClick handler
	*  PARAMETERS	: string menu : The Name name of the menu
	* 				  int type    : What kind of menu to open
    *  RETURNS  	: Nothing
    * 
	*/
	private void FileFolderHandler(string menu, int type)
	{
		try
		{
			string path = "";

			//Depending on the type launch the appropriate menu
			if(type == 3)
			{
				//Open the file menu to select an osm file
				path = MenuHandler(false, menu);
			}
			else
			{
				path = MenuHandler(true, menu);
			}	

			//If the path is found
			if(path.Length != 0)
			{
				//Depending on the type set the appropriate text
				if(type == 0)
				{
					saveFolder.text = path;
					PlayerPrefs.SetString ("save", path);
				}
				else if (type == 1)
				{
					assetFolder.text = path;
					PlayerPrefs.SetString ("asset", path);
				}
				else
				{
					osmFile.text = path;
					PlayerPrefs.SetString ("xml", path);
				}
			}
			else //Otherwise throw an exception
			{
				throw new Exception("Folder Not Found");	
			}
		}
		catch(Exception e)
		{
			errorHandler.Error ("Menu Error", e.Message);
		}			
	}

	/*
	*  METHOD	    : MenuHandler()
    *  DESCRIPTION  : This Method is called when the user tries to pick a folder/file location.
	*  PARAMETERS	: bool isFolder : Is the menu looking for a folder or no
	* 				  string title  : The title of the menu
    *  RETURNS  	: string path   : the file or folder location
    * 
	*/
	private string MenuHandler(bool isFolder, string title)
	{
		string[] hold;
		string path = "";

		try
		{
			//if the item we are lookinf for is a folder
			if(isFolder)
			{
				//Open the File menu
				hold = StandaloneFileBrowser.OpenFolderPanel(title, Application.dataPath,false);
			}
			else
			{
				//Otherwise open the file menu
				hold = StandaloneFileBrowser.OpenFilePanel(title, Application.dataPath, "xml", false);
			}	

			//If they didnt select anything change the path
			if(hold.Length == 0)
			{
				path = Application.dataPath;
			}
			else
			{
				path = hold[0]; //otherwise use the selected path
			}
		}
		catch(Exception e)
		{
			throw e;
		}

		return path;
	}
}
