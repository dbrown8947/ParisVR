/*
* FILE			: ErrorHandler.cs
* PROJECT		: ParisVR
* PROGRAMMERS	: Marco Fontana
* FIRST VERSION	: 3-20-2018
* DESCRIPTION   : This file contains the code and functionality required to operate the error handler functionality
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ErrorHandler : MonoBehaviour {

	//Public Variables
	public GameObject errorMenu;
	public Text errorText;
	public Text errorTitle;

	/*
	*  METHOD	    : Error()
    *  DESCRIPTION  : This method is responsible for displaying an error and the text involved with 
    *                 that error to the user.
	*  PARAMETERS	: string errorTi : The error type i.e save error
	* 				  string errorTex: The contents of the error i.e what went wrong
    *  RETURNS  	: Nothing
	*/
	public void Error(string errorTi, string errorTex)
	{
		//Change the text to represent the error and then display it
		errorText.text = errorTex;
		errorTitle.text = errorTi;
		errorMenu.SetActive (true);

		//Pause the game
		//Restart the time scale
		Time.timeScale = 0f;
	}

	/*
	*  METHOD	    : DisableError()
    *  DESCRIPTION  : This method is responsible for closing the error handler after the user is done with it.
    *                 This is an onclick handler.
	*  PARAMETERS	: Nothing
    *  RETURNS  	: Nothing
	*/
	public void DisableError()
	{
		//Disable the notification and resume gameplay
		errorMenu.SetActive (false);
		Time.timeScale = 1.0f;
	}
}