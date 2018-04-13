/*  
*  FILE          : VRImportDropdown.cs
*  PROJECT       : PROG1345 - Assignment #1 
*  PROGRAMMER    : Joe Student 
*  FIRST VERSION : 2012-05-01 
*  DESCRIPTION   : 
*    The functions in this file are used to ... 
*/
using System.IO;
using UnityEngine;

public class VRImportDropdown : MonoBehaviour
{


	public GameObject dropdown;
	//to fill
	public string playerPrefFilepath;
	public string fileExtension;
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
			//tempPath[i].TrimStart(@PlayerPrefs.GetString (playerPrefFilepath, Application.dataPath));
			tempPath [i] = tempPath [i].Substring (@PlayerPrefs.GetString (playerPrefFilepath, Application.dataPath).Length + 1, tempPath [i].Length - @PlayerPrefs.GetString (playerPrefFilepath, Application.dataPath).Length - 1);
			dropdown.GetComponent<DropDownScript> ().addElement (tempPath [i]);
		}

	}
	
}
