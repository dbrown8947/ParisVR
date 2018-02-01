/*
* FILE			: PlayerController.cs
* PROJECT		: Final Game Dev
* PROGRAMMERS	: Marco Fontana
* FIRST VERSION	: 12-26-2017
* DESCRIPTION   : This file contains the code and functionality required to handle the player's functions 
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour {

	//Public Variables
	public float speed;
	public GameObject screen;
	public GameObject gridScreen;
	public bool selected = false;
	public Text objName;
	public int escCount;
	public GameObject obj;
	public InputField posX;
	public InputField posY;
	public InputField posZ;
	public InputField rotX;
	public InputField rotY;
	public InputField rotZ;
	public InputField scleX;
	public InputField scleY;
	public InputField scleZ;
	public InputField tagger;
	public bool gridSelected = false;

	//Private Variables
	private int count;
	private Vector3 direction;

	//FUNCTION      : Start()
	//DESCRIPTION   : This Method is launched when the level is loaded and is used to gather
	//                information or initalize other variables
	//PARAMETERS    : Nothing
	//RETURNS		: Nothing
	void Start()
	{
		escCount = 0;
	}

	//FUNCTION      : Update()
	//DESCRIPTION   : This Method is responsible for starting the methods that handler player movement
	//                Targetting objects and executing hotkeys Update is called once per frame.
	//PARAMETERS    : Nothing
	//RETURNS		: Nothing
	void Update()
	{
        //Enter the movement handler
        Movement();

        //Enter the Selection Handler

        FindObject();
        

        //Enter the Hotkeys Handler
        HotKeys();
        
	}
		
	//FUNCTION      : Movement()
	//DESCRIPTION   : This Method is responsible handling player movement, based on the 
	//                inputs made by the user based on code from the unity documentation
	//                located at https://docs.unity3d.com/ScriptReference/CharacterController.Move.html
	//PARAMETERS    : Nothing
	//RETURNS		: Nothing
	void Movement()
	{
		//create the movement vector
		Vector3 moving;

		//Get the character controller
		CharacterController player = GetComponent<CharacterController>();

		//If the game is not paused
		if (Time.timeScale == 1.0f)
		{
			//Use Standard Movement for the movement vector
			moving = new Vector3 (Input.GetAxis ("Horizontal"), 0.0f, Input.GetAxis ("Vertical"));
		}
		else 
		{
			//otherwise dont allow the user to move
			moving = new Vector3 ( 0.0f, 0.0f,0.0f);
		}

		//Move the player based on the generated vector
		moving = transform.TransformDirection (moving);
		moving *= speed;
		player.Move (moving);
	}


	//FUNCTION      : FindObject()
	//DESCRIPTION   : This Method is responsible selecting a game object in the game world
	//                based on the idea in this post https://answers.unity.com/questions/411793/selecting-a-game-object-with-a-mouse-click-on-it.html
	//PARAMETERS    : Nothing
	//RETURNS		: Nothing
	void FindObject()
	{
		//If the player hasnt already selected an item and we are not paused
		if (!selected && Time.timeScale == 1.0f)
		{
			//if we are not paused and the left mouse button is pressed
			if (Input.GetMouseButtonDown (0) && Time.timeScale == 1.0f)
			{
				//Create a Raycast
				RaycastHit hit = new RaycastHit ();

				//Create a new raycast starting from the location where the mouse is (in this case the center of the screen)
				if (Physics.Raycast (Camera.main.ScreenPointToRay (Input.mousePosition), out hit))
				{
					//If the raycase made contact with an object with the Asset tag									
					if (hit.transform.gameObject.tag == "Asset") {	
						//Reset the esc counter
						escCount = 0;

						//Activate the modifcation menu
						screen.SetActive (true);

						//Toggle the selected flag and access the object so we can modify it.
						selected = true;
						obj = hit.transform.gameObject;

						//If the object has a text field
						if (obj.GetComponent<UnityEngine.UI.Text> () != null) {			
							//Update the tagger with the text located on the object.
							tagger.text = obj.GetComponent<UnityEngine.UI.Text> ().text;
						}

						//Update the rest of the menu text
						UpdateTextFields ();			
					} else if (hit.transform.gameObject.tag == "GridTile") {

						escCount = 0;

						//Activate the modifcation menu
						gridScreen.SetActive (true);
						selected = true;
						gridSelected = true;

						obj = hit.transform.gameObject;


					}
				}
			}
		}

		//If the user pressed esacape 
		if (Input.GetKeyDown (KeyCode.Escape)) 
		{
			//If the use has not selected an object
			if (!selected) 
			{
				//Increase the esc counter more
				escCount++;
			}

			//Deactivate the modifaction menu and increase the esc counter
			screen.SetActive(false);
			gridScreen.SetActive(false);
			selected = false;
			gridSelected = false;
			escCount++;
		}
	}

	//FUNCTION      : UpdateTextFields()
	//DESCRIPTION   : This Method is responsible for updating the menu text when the user
	//                selects a game object in the world
	//PARAMETERS    : Nothing
	//RETURNS		: Nothing
	void UpdateTextFields()
	{
		//Change the position text fields based on location of the targeted object
		posX.text = obj.transform.position.x.ToString ();
		posY.text = obj.transform.position.y.ToString ();
		posZ.text = obj.transform.position.z.ToString ();

		//Change the Rotation text fields based on location of the targeted object
		rotX.text = obj.transform.localEulerAngles.x.ToString ();
		rotY.text = obj.transform.localEulerAngles.y.ToString ();
		rotZ.text = obj.transform.localEulerAngles.z.ToString ();

		//Change the scale text fields based on location of the targeted object
		scleX.text = obj.transform.localScale.x.ToString ();
		scleY.text = obj.transform.localScale.y.ToString ();
		scleZ.text = obj.transform.localScale.z.ToString ();

		//Update the targetted text with the name of the object the user has selected
		objName.text = "Object Name: " + obj.transform.gameObject.name;

	}

	//FUNCTION      : HotKeys()
	//DESCRIPTION   : This Method is responsible for activating hotkeys based on user input
	//PARAMETERS    : Nothing
	//RETURNS		: Nothing
	void HotKeys()
	{
		//Only activate if the player has selected a game object
		if (selected && screen.activeSelf)
		{
			//If the user has pressed the left control button
			if (Input.GetKey (KeyCode.LeftControl)) 
			{
				//Set the object back to the origin point and reset its rotation
				obj.transform.rotation = Quaternion.identity;
				obj.transform.position = Vector3.zero;

				//Update the text fields with the new location
				UpdateTextFields ();
			}				
		}	
	}


}

