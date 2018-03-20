/*
* FILE			: LoadingScreen.cs
* PROJECT		: ParisVR
* PROGRAMMERS	: Marco Fontana
* FIRST VERSION	: 3-17-2018
* DESCRIPTION   : This file contains the code and functionality required operate a load screen. Based on the example shown here:
*                 https://www.youtube.com/watch?v=rXnZE8MwK-E
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoadingScreen : MonoBehaviour 
{
	//Public variables
	public GameObject screen;
	public Slider loadBar;
	public Text loadText;

	//Private Variables
	private AsyncOperation load;

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
		//Start the routine for the scene loading process
		StartCoroutine (Loading ());
	}

	/*
	*  METHOD	    : Loading()
    *  DESCRIPTION  : This Method is used to asyncrously load the next level and provide the user
    *                 with information as to how far along it is in the process. based on the exmaple
    *                 found here https://www.youtube.com/watch?v=rXnZE8MwK-E
	*  PARAMETERS	: Nothing
    *  RETURNS  	: IEnumerator
    * 
	*/
	IEnumerator Loading()
	{
		//Start the async task of loading the next scene
		load = SceneManager.LoadSceneAsync ("prototype");
		load.allowSceneActivation = false;

		//While the load is still in process
		while (load.isDone == false) 
		{
			//If loading has finished
			if (load.progress == 0.9f) 
			{
				//Increase the value to 1f and allow the scene to activate
				loadBar.value = 1f;
				load.allowSceneActivation = true;
			}
			else
			{
				//Otherwise set the load bar to the value of the progress
				loadBar.value = load.progress;
			}

			//Show the user how far along we are in the loading process
			loadText.text = "Loading: " + (loadBar.value * 100).ToString () + "%";

			//Give up processing time for one frame
			yield return null;
		}
	}
		
}
