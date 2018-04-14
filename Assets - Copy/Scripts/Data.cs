/*
* FILE			: Data.cs
* PROJECT		: ParisVR
* PROGRAMMERS	: Marco Fontana
* FIRST VERSION	: 1-18-2018
* DESCRIPTION   : This file contains the code and functionality required to save and load data about game objects based on the unity tutorial for data persistance
*                 located here: https://unity3d.com/learn/tutorials/topics/scripting/persistence-saving-and-loading-data
*/
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

/*
* Class Name	: Data 
* DESCRIPTION   : This class is responsible for gathering the information contained in gameobjects like transformation information and
*                 then save that information into a file for later use. Addtionally, this class will load the previously mentioned save file
*                 in order to restore a level to its saved state
*/
public class Data : MonoBehaviour
{
	//Private class variable(s)
	private List<Asset> info;
	private Button btn;

	//Info property. Used to acces a list of asset objects
	public List<Asset> Info
	{
		/*
		 *  Name		: get (info)
		 *	Description : Accessor for the info list
		 *	Parameters	: Nothing
		 *  Returns		: List<Asset> info, the list of assets in the gameworld.
		*/
		get 
		{
			return info;
		}
		/*
		 *  Name		: set (info)
		 *	Description : Mutator for the info list
		 *	Parameters	: List<Asset> info, the list of assets in the gameworld.
		 *  Returns		: Nothing
		*/
		set
		{
			info = value;
		}
	}

	/*
	*  METHOD	    : Data()
    *  DESCRIPTION  : This Method is a constructor for the Data class
	*  PARAMETERS	: Button bbtn: the button containing a script we want to access
    *  RETURNS  	: Nothing
    * 
	*/
	public Data(Button bbtn)
	{
		btn = bbtn;
	}

	/*
	*  METHOD	    : Data()
    *  DESCRIPTION  : This Method is a constructor for the Data class
	*  PARAMETERS	: Nothing
    *  RETURNS  	: Nothing
    * 
	*/
	public Data()
	{
		btn = null;
	}

	/*
	*  METHOD	    : Start()
    *  DESCRIPTION  : This Method is launched when the level is loaded and is used to gather
	*                 information or initalize other variable
	*  PARAMETERS	: Nothing
    *  RETURNS  	: Nothing
    * 
	*/
	void Start()
	{
		//Create the info list to hold all the game object information
		info = new List<Asset> ();
	}
		
	/*
	*  METHOD	    : Save()
    *  DESCRIPTION  : This method is responsible for finding all gameobjects in the game world that
    *                 contain information that needs to be persistant, and saves it all into a file
    *                 that can be accessed later
	*  PARAMETERS	: string saveName : The name of the save file the user is trying to save
    *  RETURNS  	: Nothing
	*/
	public void Save(string saveName)
	{
		try
		{
			//Create the binary formatter and save file so we can save the object info
			BinaryFormatter formatter = new BinaryFormatter ();
			FileStream file = File.Open (saveName, FileMode.OpenOrCreate);

            //Grab every asset in the game world that uses the Assets tag
            GameObject[] assets = GameObject.FindGameObjectsWithTag ("Asset");

			//For evert gameobject with the Asset Tag
			foreach (GameObject asset in assets) 
			{
				for(int i=0; i < info.Count; i++)
				{
					Debug.Log(asset.transform.parent.name);
					Debug.Log(asset.transform.parent.parent.name);
					//Make sure to capture the appropriate information for the assets we are trying to save
					if(info[i].ParentInfo.Tile.CompareTo(asset.transform.parent.name) == 0 && info[i].ParentInfo.Area.CompareTo(asset.transform.parent.parent.name) == 0)
					{
						//Save all the important information about the game object into the ObjectInfo containers
						info[i].ParentInfo.Position = new TempVector(asset.transform.parent.localPosition.x, asset.transform.parent.localPosition.y, asset.transform.parent.localPosition.z); //asset.transform.position;
						info[i].ParentInfo.Rotation = new TempVector(asset.transform.parent.localEulerAngles.x, asset.transform.parent.localEulerAngles.y, asset.transform.parent.transform.localEulerAngles.z); //asset.transform.localEulerAngles;
						info[i].ParentInfo.Scale = new TempVector(asset.transform.parent.localScale.x, asset.transform.parent.localScale.y, asset.transform.parent.localScale.z); //asset.transform.localScale;
						info[i].ParentInfo.Tag = asset.GetComponent<UnityEngine.UI.Text> ().text;
//						Debug.Log(info[i].ParentInfo.Tag);
						break;
					}					
				}
			}

            //Write the information inside the list into the save file in a binary format
            formatter.Serialize (file, info);
			file.Close();
		}
		catch(Exception) // For testing purposes, more generics to be used later
		{
			//Show the error message in the log
			throw new Exception("Error Attempting to save asset information to file");
		}
	}

	/*
	*  METHOD	    : Load()
    *  DESCRIPTION  : This method is responsible for reading a save file and deserialize the contents
    *                 so we can get the contained objectInfo list. With this list the level can be restored
    *                 to its saved state.
	*  PARAMETERS	: string saveName : The name of the save file the user is trying to load
    *  RETURNS  	: Nothing
	*/
	public void Load(string saveName)
	{
		try
		{
			ImportPress import = btn.GetComponent<ImportPress>(); 

            //Only try and open the file if it actually exists
            if (File.Exists (saveName))
			{
				//Create the info list to hold all the game object information
				info = new List<Asset> ();

				//Create the binary formatter and save file so we can load the object info
				BinaryFormatter formatter = new BinaryFormatter ();
				FileStream file = File.Open (saveName, FileMode.Open);

				//Read the list of object info from the file and then close the file
				info = (List<Asset>)formatter.Deserialize (file);
				file.Close();

				//Get the file name to see if this is the correct file to load on
				string[] splits = PlayerPrefs.GetString ("xml").Split ('\\', '/');
				string fileName = splits[splits.Length - 1];

				//For every asset found in the list reimport the objects and apply thier preivous states
				for(int i =0; i < info.Count; i++)
				{
					//As long as the map matches load all the saved assets
					if(info[i].MapName.CompareTo(fileName) == 0)
					{
						import.ApplySettings(info[i].ParentInfo.Position, info[i].ParentInfo.Rotation, info[i].ParentInfo.Scale, info[i].ParentInfo.Area, info[i].ParentInfo.Tile, info[i].ParentInfo.FileName, info[i].ParentInfo.Tag);
					}
					else
					{
						throw new Exception("Map Does Not Match Current xml file. Please find  " + info[i].MapName);
					}
				}
											
			}
			else 
			{
				//Otherwise throw an error
				throw new Exception("File Not Found");
			}
		}
		catch(Exception e) // For testing purposes, more generics to be used later
		{
			//Show the error message in the log
			throw e;
		}
			
	}
		
}
