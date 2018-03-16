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

public class VRImportDropdown : MonoBehaviour {


    public GameObject dropdown; //to fill
    public string filepath; //to look int

    /* 
    *  METHOD      : Start 
    * 
    *  DESCRIPTION   : Filles the dropdown when Prefab starts
    */
    void Start () {

        string[] tempPath = Directory.GetFiles(@filepath, "*.obj"); //get a list of files

        for (int i = 0; i < tempPath.Length; i++) //add files to the dropdown
        {
             dropdown.GetComponent<DropDownScript>().addElement(tempPath[i]);
        }

    }
	
}
