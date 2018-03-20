/*
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
using UnityEditor;
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

		//Find the appropriate information relating to the locations of the save, asset and xml folders/files
		saveFolder.text = PlayerPrefs.GetString ("save", Application.dataPath + @"/Saves");
		assetFolder.text = PlayerPrefs.GetString ("asset", Application.dataPath + @"/MyObjects");
		osmFile.text = PlayerPrefs.GetString ("xml", Application.dataPath + @"\map.osm-roads.xml");

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
			Screen.fullScreen = true;
		} 
		else
		{
			Screen.fullScreen = false;
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
			Screen.SetResolution(rez.width, rez.width,true);
		} 
		else
		{
			Screen.SetResolution(rez.width, rez.width,false);
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
	public void OnDetailsChange()
	{

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
			//Display the error to the user if one occurs
			//EditorUtility.DisplayDialog(menu + " Menu Error", e.Message, "OK");
		}			
	}

	private string MenuHandler(bool isFolder, string title)
	{
		string path = "";

		try
		{
			if(isFolder)
			{
				path = StandaloneFileBrowser.OpenFolderPanel(title, Application.dataPath,false)[0];
			}
			else
			{
				path = StandaloneFileBrowser.OpenFilePanel(title, Application.dataPath, "xml", false)[0];
			}				
		}
		catch(Exception e)
		{
			throw e;
		}

		return path;
	}
}
