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
	private List<Asset> info;

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
		int counter = 0;

		try
		{
			//Create the binary formatter and save file so we can save the object info
			BinaryFormatter formatter = new BinaryFormatter ();
			FileStream file = File.Open (saveName, FileMode.OpenOrCreate);

			//Create the info list to hold all the game object information
			info = new List<Asset> ();

            //Grab every asset in the game world that uses the Assets tag
            GameObject[] assets = GameObject.FindGameObjectsWithTag ("Asset");

			//For evert gameobject with the Asset Tag
			foreach (GameObject asset in assets) 
			{
                Asset obj = new Asset ();

                obj.ParentInfo = new ObjectInfo();

                //Save all the important information about the game object into the ObjectInfo container s
                obj.ParentInfo.Position = new TempVector(asset.transform.position.x, asset.transform.position.y, asset.transform.position.z); //asset.transform.position;
				obj.ParentInfo.Rotation = new TempVector(asset.transform.localEulerAngles.x, asset.transform.localEulerAngles.y, asset.transform.localEulerAngles.z); //asset.transform.localEulerAngles;
                obj.ParentInfo.Scale = new TempVector(asset.transform.localScale.x, asset.transform.localScale.y, asset.transform.localScale.z); //asset.transform.localScale;
                obj.ParentInfo.Name = asset.gameObject.name;
               // obj.ParentInfo.Tag = asset.GetComponent<UnityEngine.UI.Text> ().text;
                obj.ParentInfo.Area = asset.transform.parent.parent.parent.parent.name;
                obj.ParentInfo.Tile = asset.transform.parent.name;
                Debug.Log(obj.ParentInfo.Area);

                //				GameObject[] subObj =  asset.GetComponentsInChildren<GameObject>();
                //
                //				foreach (GameObject ob in subObj)
                //				{
                //					SubObjectInfo nfo = new SubObjectInfo();
                //						
                //					//Save all the important information about the game object into the ObjectInfo container s
                //					nfo.Position = subObj.transform.position;
                //					nfo.Rotation = subObj.transform.localEulerAngles;
                //					nfo.Scale = subObj.transform.localScale;
                //					nfo.Name = subObj.gameObject.name;
                //					//nfo.Tag = asset.GetComponent<UnityEngine.UI.Text> ().text;	
                //
                //					obj.ChildInfo.Add(nfo);
                //				}
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
            ImportPress import = new ImportPress();
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

                import.ApplySettings(info[0].ParentInfo.Position, info[0].ParentInfo.Rotation, info[0].ParentInfo.Scale, info[0].ParentInfo.Area, info[0].ParentInfo.Tile);


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
