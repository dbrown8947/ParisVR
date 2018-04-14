/*
* FILE : ImprotDropDown.cs
* PROJECT : PARISVR
* PROGRAMMER : Dustin Brown
* FIRST VERSION : 2018-01-18
* DESCRIPTION :
* UI for selecting an obj file to import
*/
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;


/*
* NAME : ImportDropDown
* PURPOSE : Gets the filepath for OBJ files in a given directory
*/
public class ImportDropDown : MonoBehaviour {


    public Dropdown dropdown; //to fill
    public string filepath; //to look int

    private List<string> objObjects = new List<string>();

    void Start()
	{
		//read directory
		string[] tempPath = Directory.GetFiles (@filepath, "*.obj");

		//add to list
		for (int i = 0; i < tempPath.Length; i++) {
			objObjects.Add (tempPath [i]);
		}
		//fill drop down with list
		dropdown.AddOptions (objObjects);	
	
	}
}
