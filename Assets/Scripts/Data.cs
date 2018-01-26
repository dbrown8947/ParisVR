/*
* FILE			: Data.cs
* PROJECT		: ParisVR Tool
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
	private List<ObjectInfo> info;

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
			FileStream file = File.Open (saveName, FileMode.Open);

			//Create the info list to hold all the game object information
			info = new List<ObjectInfo> ();

			//Grab every asset in the game world that uses the Assets tag
			GameObject[] assets = GameObject.FindGameObjectsWithTag ("Asset");

			//For evert gameobject with the Asset Tag
			foreach (GameObject asset in assets) 
			{
				ObjectInfo obj = new ObjectInfo ();

				//Save all the important information about the game object into the ObjectInfo container
				obj.Position = asset.transform.position;
				obj.Rotation = asset.transform.localEulerAngles;
				obj.Scale = asset.transform.localScale;
				obj.Name = asset.gameObject.name;
				obj.Tag = asset.GetComponent<UnityEngine.UI.Text> ().text;

				//Add the object into the info list
				info.Add (obj);
			}

			//Write the information inside the list into the save file in a binary format
			formatter.Serialize (file, info);
			file.Close();
		}
		catch(Exception e) // For testing purposes, more generics to be used later
		{
			//Show the error message in the log
			Debug.Log (e.Message);
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
			//Only try and open the file if it actually exists
			if (File.Exists (saveName))
			{
				//Create the info list to hold all the game object information
				info = new List<ObjectInfo> ();

				//Create the binary formatter and save file so we can load the object info
				BinaryFormatter formatter = new BinaryFormatter ();
				FileStream file = File.Open (saveName, FileMode.Open);

				//Read the list of object info from the file and then close the file
				info = (List<ObjectInfo>)formatter.Deserialize (file);
				file.Close();	

				//*********************************************************************
				//THIS IS WHERE YOU WOULD IMPORT ASSETS AND PLACE THEM BACK IN POSITION
				//*********************************************************************
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
			Debug.Log (e.Message);
		}
			
	}
}
