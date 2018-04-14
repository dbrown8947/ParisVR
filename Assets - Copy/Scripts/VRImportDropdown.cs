/*  
*  FILE          : VRImportDropdown.cs
*  PROJECT       : ParisVR
*  PROGRAMMER    : Dustin Brown
*  FIRST VERSION : 2018-03-01 
*  DESCRIPTION   : 
*    This script populates a VRDropdown gameobject with files from a specifed directory
*/
using System.IO;
using UnityEngine;

public class VRImportDropdown : MonoBehaviour
{

	//to fill
	public GameObject dropdown;
	//key of file path in registry
	public string playerPrefFilepath;
	//file type to look for
	public string fileExtension;
	//whether to include a "New Save" element before populating list
	public bool NewByDefault;

	/* 
    *  METHOD      : Start 
    * 
    *  DESCRIPTION   : Filles the dropdown when Prefab starts
    */
	void Start ()
	{

		string[] tempPath = Directory.GetFiles (@PlayerPrefs.GetString (playerPrefFilepath, Application.dataPath), "*." + fileExtension); //get a list of files
		if (NewByDefault) {
			dropdown.GetComponent<DropDownScript> ().addElement ("New Save");
		}

		for (int i = 0; i < tempPath.Length; i++) { //add files to the dropdown

			tempPath [i] = tempPath [i].Substring (@PlayerPrefs.GetString (playerPrefFilepath, Application.dataPath).Length + 1, tempPath [i].Length - @PlayerPrefs.GetString (playerPrefFilepath, Application.dataPath).Length - 1);
			dropdown.GetComponent<DropDownScript> ().addElement (tempPath [i]);
		}

	}

}
