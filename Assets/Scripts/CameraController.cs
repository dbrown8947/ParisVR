/*
* FILE			: CameraController.cs
* PROJECT		: Final Game Dev
* PROGRAMMERS	: Marco Fontana
* FIRST VERSION	: 12-26-2017
* DESCRIPTION   : This file contains the code and functionality required to handle the camera movement based on Unity Lab Code and 
*                 the tutorial made by Holistic3d found here https://www.youtube.com/watch?v=blO039OzUZc
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

	//Public Variables
	public GameObject player;
	public float sens = 5f;
	public float smoothing = 2f;

	TextHandler Logger;

	//Private Variables
	private Vector3 offset;
	Vector2 mouseLook;
	Vector2 smoothV;
	bool locked;




	//FUNCTION      : Start()
	//DESCRIPTION   : This Method is launched when the level is loaded and is used to gather
	//                information or initalize other variables
	//PARAMETERS    : Nothing
	//RETURNS		: Nothing
	void Start ()
	{
		Logger = gameObject.AddComponent<TextHandler> () as TextHandler;
		//Lock the cursor into the center of the screen
		Cursor.lockState = CursorLockMode.Locked;
		Logger.WriteToLog ("The screen is locked");

		//Created the offset and toggle the lock flag
		offset = transform.position - player.transform.position;
		locked = true;

	}


	//FUNCTION      : Update()
	//DESCRIPTION   : This Method is responsible for starting the methods that handler player movement
	//                Targetting objects and executing hotkeys Update is called once per frame.
	//PARAMETERS    : Nothing
	//RETURNS		: Nothing
	void Update()
	{
		//if the cursor is locked and we are not paused
		if (locked && Time.timeScale == 1.0f) 
		{
			//This segment of code is from the tutorial from Holistic3d found here https://www.youtube.com/watch?v=blO039OzUZc

			//Find out where and how the user is moving the mouse
			var look = new Vector2 (Input.GetAxis ("Mouse X"), Input.GetAxis ("Mouse Y"));

			//scale the vector based on the sensitivity and smooth vectors
			look = Vector2.Scale (look, new Vector2 (sens * smoothing, sens * smoothing));

			//Get the x and y values for the final camera movement
			smoothV.x = Mathf.Lerp (smoothV.x, look.x, 1f / smoothing);
			smoothV.y = Mathf.Lerp (smoothV.y, look.y, 1f / smoothing);

			//add the smoothed vector to the previous location
			mouseLook += smoothV;

			//Change the camera and players location based on the math we just calculated
			transform.localRotation = Quaternion.AngleAxis (-mouseLook.y, Vector3.right);
			player.transform.localRotation = Quaternion.AngleAxis (mouseLook.x, player.transform.up);
		}

		//If the player has pressed the left alt button
		if (Input.GetKeyDown (KeyCode.LeftAlt)) 
		{
			
			//If the mouse is locked
			if (locked) 
			{
				Logger.WriteToLog ("The screen has been unlocked with alt");
				//Unlock the mouse and display the cursor
				Cursor.lockState = CursorLockMode.None;
				Cursor.visible = true;
				locked = false;
			} 
			else 
			{
				Logger.WriteToLog ("The screen has been locked with alt");
				//Otherwise lock the mouse and hide the cursor
				Cursor.lockState = CursorLockMode.Locked;
				Cursor.visible = false;
				locked = true;
			}
		}
	}
	
	//FUNCTION      : LateUpdate()
	//DESCRIPTION   : This Method is called at the end of a frame update, and moves the camera,
	//                based on player location and the offset
	//PARAMETERS    : Nothing
	//RETURNS		: Nothing
	void LateUpdate ()
	{
		//change the location of the camera
		transform.position = player.transform.position + offset;
		
	}
}
