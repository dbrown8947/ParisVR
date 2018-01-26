/*
* FILE			: SLMenuHandler.cs
* PROJECT		: ParisVR Tool
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


/*
* Class Name	: SLMenuHandler 
* DESCRIPTION   : This class is responsible for handling any menuing involving the save and load features of the application. This class
*                 will launch the save/load menu and pass errors to an error handler so that the user can see problems that occur and allows
*                 them to save/load game worlds.
*/
public class SLMenuHandler : MonoBehaviour
{

	//Private Variables

	//The class that handles saving and loading
	private Data data; 


	/*
	*  METHOD	    : Start()
    *  DESCRIPTION  : This Method is launched when the level is loaded and is used to gather
	*                 information or initalize other variable
	*  PARAMETERS	: Nothing
    *  RETURNS  	: Nothing
	*/
	void Start () 
	{
		//Initalize private variables
		data = new Data ();
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
            string path = EditorUtility.OpenFilePanel("Load Game World", "", "dat");

            if(path.Length == 0)
            {
                throw new Exception("No File Selected, Or The File Was Not Found");
            }
            else
            {
                data.Load(path);
            }
        }
        catch (Exception e)
        {
            //If we are Saving make sure that the error displays as a save error
            EditorUtility.DisplayDialog("Load Error", e.Message, "OK");
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
           string path = EditorUtility.SaveFilePanel("Save Game World", "", "newSave.dat", "dat");

           if(path.Length == 0)
           {
                throw new Exception("No File Selected, Or The File Was Not Found");
            }
           else
           {
                data.Save(path);
           }
        }
        catch(Exception e)
        {
            //If we are Saving make sure that the error displays as a save error
            EditorUtility.DisplayDialog("Save Error", e.Message, "OK");
        }
    }
}
