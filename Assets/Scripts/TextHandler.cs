/*
* FILE			: TextHandler.cs
* PROJECT		: Final Game Dev
* PROGRAMMERS	: Marco Fontana
* FIRST VERSION	: 12-26-2017
* DESCRIPTION   : This file contains the code and functionality required to modify a gameobject based on user input,
*                 Addtionally, this file contains the code used to make and save a tag
*/

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text;
using System.IO;

public class TextHandler : MonoBehaviour {

	//Public variables
	public InputField tagger;
	public GameObject plyr;
	public InputField posX;
	public InputField posY;
	public InputField posZ;
	public InputField rotX;
	public InputField rotY;
	public InputField rotZ;
	public InputField scleX;
	public InputField scleY;
	public InputField scleZ;

	//Private variables
	private GameObject obj;

	//FUNCTION      : PosXModifier()
	//DESCRIPTION   : This method parses user input to change the X value for Position
	//PARAMETERS    : Nothing
	//RETURNS		: Nothing
	public void PosXModifier()
	{
		float value = 0.0f;

		//Get the player script so we can access the selected building
		obj = plyr.GetComponent<PlayerController>().obj;

		//Attempt to parse the value from the inputbox
		if (float.TryParse (posX.text, out value)) 
		{
			//change the objects Position based on the inputted number
			obj.transform.position = new Vector3(value, obj.transform.position.y, obj.transform.position.z);
		}
	}

	//FUNCTION      : PosYModifier()
	//DESCRIPTION   : This method parses user input to change the Y value for Position
	//PARAMETERS    : Nothing
	//RETURNS		: Nothing
	public void PosYModifier()
	{
		float value = 0.0f;

		//Get the player script so we can access the selected building
		obj = plyr.GetComponent<PlayerController>().obj;

		//Attempt to parse the value from the inputbox
		if (float.TryParse (posY.text, out value)) 
		{
			//change the objects Position based on the inputted number
			obj.transform.position = new Vector3(obj.transform.position.x, value, obj.transform.position.z);
		}
	}

	//FUNCTION      : PosZModifier()
	//DESCRIPTION   : This method parses user input to change the Z value for Position
	//PARAMETERS    : Nothing
	//RETURNS		: Nothing
	public void PosZModifier()
	{
		float value = 0.0f;

		//Get the player script so we can access the selected building
		obj = plyr.GetComponent<PlayerController>().obj;

		//Attempt to parse the value from the inputbox
		if (float.TryParse (posZ.text, out value)) 
		{
			//change the objects Position based on the inputted number
			obj.transform.position = new Vector3(obj.transform.position.x, obj.transform.position.y, value);
		}
	}

	//FUNCTION      : RotXModifier()
	//DESCRIPTION   : This method parses user input to change the X value for Rotation
	//PARAMETERS    : Nothing
	//RETURNS		: Nothing
	public void RotXModifier()
	{
		float value = 0.0f;

		//Get the player script so we can access the selected building
		obj = plyr.GetComponent<PlayerController>().obj;

		//Attempt to parse the value from the inputbox
		if (float.TryParse (rotX.text, out value)) 
		{
			//change the objects Rotation based on the inputted number
			obj.transform.localEulerAngles = new Vector3(value, obj.transform.localEulerAngles.y, obj.transform.localEulerAngles.z);
		}
	}

	//FUNCTION      : RotYModifier()
	//DESCRIPTION   : This method parses user input to change the Y value for Rotation
	//PARAMETERS    : Nothing
	//RETURNS		: Nothing
	public void RotYModifier()
	{
		float value = 0.0f;

		//Get the player script so we can access the selected building
		obj = plyr.GetComponent<PlayerController>().obj;

		//Attempt to parse the value from the inputbox
		if (float.TryParse (rotY.text, out value)) 
		{
			//change the objects Rotation based on the inputted number
			obj.transform.localEulerAngles = new Vector3(obj.transform.localEulerAngles.x, value, obj.transform.localEulerAngles.z);
		}
	}

	//FUNCTION      : RotYModifier()
	//DESCRIPTION   : This method parses user input to change the Y value for Rotation
	//PARAMETERS    : Nothing
	//RETURNS		: Nothing
	public void RotZModifier()
	{
		float value = 0.0f;

		//Get the player script so we can access the selected building
		obj = plyr.GetComponent<PlayerController>().obj;

		//Attempt to parse the value from the inputbox
		if (float.TryParse (rotZ.text, out value)) 
		{
			//change the objects Rotation based on the inputted number
			obj.transform.localEulerAngles = new Vector3(obj.transform.localEulerAngles.x, obj.transform.localEulerAngles.y, value);
		}
	}

	//FUNCTION      : ScaleXModifier()
	//DESCRIPTION   : This method parses user input to change the X value for Scale
	//PARAMETERS    : Nothing
	//RETURNS		: Nothing
	public void ScaleXModifier()
	{
		float value = 0.0f;

		//Get the player script so we can access the selected building
		obj = plyr.GetComponent<PlayerController>().obj;

		//Attempt to parse the value from the inputbox
		if (float.TryParse (scleX.text, out value)) 
		{
			//change the objects Scale based on the inputted number
			obj.transform.localScale = new Vector3(value, obj.transform.localScale.y, obj.transform.localScale.z);
		}
	}

	//FUNCTION      : ScaleYModifier()
	//DESCRIPTION   : This method parses user input to change the Y value for Scale
	//PARAMETERS    : Nothing
	//RETURNS		: Nothing
	public void ScaleYModifier()
	{
		float value = 0.0f;

		//Get the player script so we can access the selected building
		obj = plyr.GetComponent<PlayerController>().obj;

		//Attempt to parse the value from the inputbox
		if (float.TryParse (scleY.text, out value)) 
		{
			//change the objects Scale based on the inputted number
			obj.transform.localScale = new Vector3(obj.transform.localScale.x, value, obj.transform.localScale.z);
		}
	}

	//FUNCTION      : ScaleZModifier()
	//DESCRIPTION   : This method parses user input to change the Z value for Scale
	//PARAMETERS    : Nothing
	//RETURNS		: Nothing
	public void ScaleZModifier()
	{
		float value = 0.0f;

		//Get the player script so we can access the selected building
		obj = plyr.GetComponent<PlayerController>().obj;

		//Attempt to parse the value from the inputbox
		if (float.TryParse (scleZ.text, out value)) 
		{
			//change the objects Scale based on the inputted number
			obj.transform.localScale = new Vector3(obj.transform.localScale.x, obj.transform.localScale.y, value);
		}
	}

	//FUNCTION      : Clear()
	//DESCRIPTION   : This method clears the tagger text
	//PARAMETERS    : Nothing
	//RETURNS		: Nothing
	public void Clear()
	{
		//reset the tagger text
		tagger.text = "";
		Time.timeScale = 1.0f;
	}

	//FUNCTION      : Inputs()
	//DESCRIPTION   : This method pauses time while the user inputs a tag
	//PARAMETERS    : Nothing
	//RETURNS		: Nothing
	public void Inputs()
	{
		Time.timeScale = 0.0f;
	}

	//FUNCTION      : Resume()
	//DESCRIPTION   : This method resets the time after the user has finish with input
	//PARAMETERS    : Nothing
	//RETURNS		: Nothing
	public void Resume()
	{
		Time.timeScale = 1.0f;
	}


	//FUNCTION      : WriteToLog()
	//DESCRIPTION   : This method is responsible for creating and writing to a tag file and
	//                saving the tagger text to the gameobject itself.
	//PARAMETERS    : Nothing
	//RETURNS		: Nothing
	public void WriteToLog()
	{
		//Method variables
		Stream logging;
		StreamWriter write;
		string fileName = "";
		DateTime today = new DateTime();

		//Restart the time scale
		Time.timeScale = 1.0f;

		//Get the player controller so we can access the selected building
		obj = plyr.GetComponent<PlayerController> ().obj;

		try
		{
			//Set the text in the object to the text in the tagger
		    obj.GetComponent<UnityEngine.UI.Text> ().text = tagger.text;


		    //get the current date
			today = DateTime.Now;
			
			//Create a daily unique log file name
			fileName = "Tags " + today.ToString("MM_dd_yyyy") + ".txt";

			//If the file doesnt exist
			if (!File.Exists(fileName))
			{
				//Make the file and open it for appending
				logging = new FileStream(fileName, FileMode.Append);
				write = new StreamWriter(logging);
			}
			else
			{
				//If the file does exist get ready to append to it
				write = File.AppendText(fileName);
			}

			//Get the current time of error
			DateTime errorTime = DateTime.Now;

			//Write the information to the tag file then close it
			write.WriteLine("Object Name: " + obj.transform.gameObject.name);
			write.WriteLine("Date Of Tag: " + errorTime );
			write.WriteLine("Tag Details: " + tagger.text);
			write.WriteLine(" ");
			write.Flush();
			write.Close();
		}
		catch (Exception)
		{
			//Do nothing
		}

	}

	//FUNCTION      : WriteToLog()
	//DESCRIPTION   : This method is responsible for creating and writing to a tag file and
	//                saving the tagger text to the gameobject itself.
	//PARAMETERS    : Nothing
	//RETURNS		: Nothing
	public void WriteToLog(string message)
	{
		//Method variables
		Stream logging;
		StreamWriter write;
		string fileName = "";
		DateTime today = new DateTime();

		//Restart the time scale
		Time.timeScale = 1.0f;

		try
		{
			//get the current date
			today = DateTime.Now;

			//Create a daily unique log file name
			fileName = "Logs " + today.ToString("MM_dd_yyyy") + ".txt";

			//If the file doesnt exist
			if (!File.Exists(fileName))
			{
				//Make the file and open it for appending
				logging = new FileStream(fileName, FileMode.Append);
				write = new StreamWriter(logging);
			}
			else
			{
				//If the file does exist get ready to append to it
				write = File.AppendText(fileName);
			}

			//Get the current time of error
			DateTime errorTime = DateTime.Now;

			//Write the information to the tag file then close it
			write.WriteLine(DateTime.Now + ": " + message);
			write.Flush();
			write.Close();
		}
		catch (Exception)
		{
			//Do nothing
		}

	}

}
