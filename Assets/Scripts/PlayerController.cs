/*
* FILE			: PlayerController.cs
* PROJECT		: ParisVR
* PROGRAMMERS	: Marco Fontana
* FIRST VERSION	: 12-26-2017
* DESCRIPTION   : This file contains the code and functionality required to handle the player's functions 
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GrahamScan;

public class PlayerController : MonoBehaviour
{

    //Public Variables
    public float speed;
    public GameObject screen;
    public GameObject importMenu;
    public Text TileName;
    public bool selected = false;
    public Text objName;
    public int escCount;
    public GameObject obj;
    public GameObject Parent;
	public GameObject lighter;
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
	public Material highlightShader;
	public Material baseShader;

    //Private Variables
    private TextHandler Logger;
    private int count;
    private Vector3 direction;
    private string assetType;
    private GameObject parentObj;
    private bool locked;
    private bool creative;
	private GameObject baseObj;
	private Transform safety;


	//Locked property. Used to access the locked flag
	public bool Locked
	{
		/*
		 *  Name		: get (info)
		 *	Description : Accessor for the info list
		 *	Parameters	: Nothing
		 *  Returns		: List<Asset> info, the list of assets in the gameworld.
		*/
		get
		{
			return locked;
		}
	}
		
    //FUNCTION      : Start()
    //DESCRIPTION   : This Method is launched when the level is loaded and is used to gather
    //                information or initalize other variables
    //PARAMETERS    : Nothing
    //RETURNS		: Nothing
    void Start()
    {
        Logger = gameObject.AddComponent<TextHandler>() as TextHandler;
		//Lock the cursor into the center of the screen
		Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = false;
		locked = true;
        escCount = 0;
        creative = false;
		safety = null;
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
            moving = new Vector3(Input.GetAxis("Horizontal"), 0.0f, Input.GetAxis("Vertical"));

            if (creative == true)
            {
                if (Input.GetKeyDown(KeyCode.Space))
                {
					player.transform.position = new Vector3 (player.transform.position.x, player.transform.position.y + .5f, player.transform.position.z);
                }
                else if (Input.GetKeyDown(KeyCode.Z)) //NEEDS TO CHANGE
                {
					if (player.transform.position.y >= 0)
					{
						player.transform.position = new Vector3 (player.transform.position.x, player.transform.position.y - .5f, player.transform.position.z);
					}
                }
            }
        }
        else
        {
            //otherwise dont allow the user to move
            moving = new Vector3(0.0f, 0.0f, 0.0f);
        }

        //Move the player based on the generated vector
        moving = transform.TransformDirection(moving);
        moving *= speed;
        player.Move(moving);
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
            //if we are not paused and the left or right mouse button is pressed
            if (Input.GetMouseButtonDown(0) && Time.timeScale == 1.0f)
            {
                //Create a Raycast
                RaycastHit hit = new RaycastHit();

                //Create a new raycast starting from the location where the mouse is (in this case the center of the screen)
                if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit))
                {
					GameObject GetHighLight;

                    if (hit.transform.gameObject.tag == "Asset")
                    {
                        //Reset the esc counter
                        escCount = 0;

                        //Activate the modifcation menu
                        screen.SetActive(true);

                        //Toggle the selected flag and access the object so we can modify it.
                        selected = true;

						//Start the highlighting an selection process
                        obj = hit.transform.parent.gameObject;
						GetHighLight = hit.transform.gameObject;
						HighLighter (GetHighLight,false);

                        Logger.WriteToLog("Object Tagged, Name: " + obj.gameObject.name + " At X =" + obj.gameObject.transform.position.x + " Y =" + obj.gameObject.transform.position.x + " Z =" + obj.gameObject.transform.position.z);

                        //Update the rest of the menu text
                        UpdateTextFields();
                    }
                    else if (hit.transform.gameObject.tag == "GridTile")
                    {
                        escCount = 0;
						safety = hit.transform;
                        //Activate the modifcation menu
                        importMenu.SetActive(true);
                        selected = true;
                        gridSelected = true;

						obj = hit.transform.gameObject;
                        
						HighLighter (obj,true);

                        TileName.text = "Selected Tile: " + hit.transform.parent.gameObject.name + " In " + hit.transform.parent.parent.parent.gameObject.name + " In " + hit.transform.parent.parent.parent.parent.gameObject.name;

                        Logger.WriteToLog("Grid Tile Tagged, Name: " + hit.transform.parent.gameObject.name + " At X=" + obj.gameObject.transform.position.x + " Y=" + obj.gameObject.transform.position.x + " Z=" + obj.gameObject.transform.position.z);

                        Parent = hit.transform.parent.gameObject;
                    }
                }
            }
        }
    }

    //FUNCTION      : UpdateTextFields()
    //DESCRIPTION   : This Method is responsible for updating the menu text when the user
    //                selects a game object in the world
    //PARAMETERS    : Nothing
    //RETURNS		: Nothing
    public void UpdateTextFields()
    {
        //Change the position text fields based on location of the targeted object
        posX.text = obj.transform.position.x.ToString();
        posY.text = obj.transform.position.y.ToString();
        posZ.text = obj.transform.position.z.ToString();

        //Change the Rotation text fields based on location of the targeted object
		rotX.text = obj.transform.rotation.eulerAngles.x.ToString();
		rotY.text = obj.transform.rotation.eulerAngles.y.ToString();
		rotZ.text = obj.transform.rotation.eulerAngles.z.ToString();

        //Change the scale text fields based on location of the targeted object
        scleX.text = obj.transform.localScale.x.ToString();
        scleY.text = obj.transform.localScale.y.ToString();
        scleZ.text = obj.transform.localScale.z.ToString();

		tagger.text = obj.transform.GetChild (2).gameObject.GetComponent<UnityEngine.UI.Text> ().text;

        //Update the targetted text with the name of the object the user has selected
        objName.text = "Object Name: " + obj.transform.gameObject.name;
    }

	//FUNCTION      : HotKeys()
	//DESCRIPTION   : This Method is responsible for activating hotkeys based on user input
	//PARAMETERS    : GameObject SetHighLighter : The object we want to highlight
	//              : Bool type				    : Is the object is a grid tile or not
	//RETURNS		: Nothing
	void HighLighter(GameObject SetHighLighter, bool isGrid)
	{
		float scaler = 0.0254f;
			
		if (isGrid)
		{
			baseObj = obj.transform.GetChild(0).gameObject;
			baseObj.GetComponent<Renderer> ().material = highlightShader;
		}
		else 
		{
			lighter.SetActive (true);

			Vector3 assetSize = (SetHighLighter.GetComponent<BoxCollider> ().size * scaler);
			Vector3 center = (SetHighLighter.GetComponent<BoxCollider> ().center * scaler);
			//assetSize = new Vector3 (assetSize.x * scaler, assetSize.y * scaler, assetSize.z * scaler);

			lighter.transform.position = obj.transform.position + new Vector3 (center.x * obj.transform.localScale.x, center.y * obj.transform.localScale.y, center.z * obj.transform.localScale.z);
			lighter.transform.localScale = new Vector3 (assetSize.x * obj.transform.localScale.x, assetSize.y * obj.transform.localScale.y, assetSize.z * obj.transform.localScale.z);
			lighter.transform.rotation = obj.transform.rotation;
		}
			
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
            if (Input.GetKey(KeyCode.LeftControl))
            {
                //Set the object back to the origin point and reset its rotation
                obj.transform.localEulerAngles = Vector3.zero;
                obj.transform.localPosition = Vector3.zero;

                Logger.WriteToLog("Game Object : " + obj.gameObject.name + " has been reset to its original position.");

                //Update the text fields with the new location
                UpdateTextFields();
            }

			//Depending on the use of the scroll wheel change the postion of the object +/- .5f
            if(Input.GetAxis("Mouse ScrollWheel") > 0f)
            {
				obj.transform.position = new Vector3 (obj.transform.position.x, obj.transform.position.y + .5f, obj.transform.position.z);
				UpdateTextFields ();
            }
            else if(Input.GetAxis("Mouse ScrollWheel") < 0f)
            {
				obj.transform.position = new Vector3 (obj.transform.position.x, obj.transform.position.y - .5f, obj.transform.position.z);
				UpdateTextFields ();
            }

			if (Input.GetKey (KeyCode.Delete)) 
			{
				RemoveAsset ();
				obj.transform.GetChild (0).gameObject.SetActive (true);
				obj.transform.GetChild (1).gameObject.SetActive (true);
				Destroy (obj.transform.GetChild(2).gameObject);
				ResetTile ();
				EscCommands ();
			}
        }

		//Activate Creative Mode
        if(Input.GetKeyDown(KeyCode.C))
        {
			//If creative mode is off
            if(creative == false)
            {
				//turn it on
                creative = true;
            }
            else
            {
				//Otherwise turn off creative mode
                creative = false;
            }
        }
			
        //If the user pressed esacape 
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            //If the use has not selected an object
            if (!selected)
            {
                //Increase the esc counter more
                escCount++;
            }
			else
			{
				if (safety != null) 
				{
					if (safety.gameObject.tag == "GridTile")
					{
						baseObj.GetComponent<Renderer> ().material = baseShader;
						safety = null;
					}
				}
			}

            //Deactivate the modifaction menu and increase the esc counter
			EscCommands();
        }

        //If the player has pressed the left alt button
        if (Input.GetKeyDown(KeyCode.LeftAlt))
        {
            //If the mouse is locked
            if (locked)
            {
                //Unlock the mouse and display the cursor
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                locked = false;
            }
            else
            {
                //Otherwise lock the mouse and hide the cursor
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
                locked = true;
            }
        }
    }

	void EscCommands()
	{
		//Deactivate the modifaction menu and increase the esc counter
		screen.SetActive(false);
		importMenu.SetActive(false);
		lighter.SetActive(false);
		selected = false;
		gridSelected = false;
		escCount++;
	}


	void RemoveAsset()
	{
		GameObject del = obj.transform.parent.gameObject;
		GameObject.Find ("PauseMenu").GetComponent<SLMenuHandler> ().RemoveAssetFromList (del);
	}

	void ResetTile()
	{
		List<ObjectInfo> tiles = GameObject.Find ("GeneratedWorld").GetComponent<GrahamScan.GrahamScan> ().objs;

		foreach (ObjectInfo obje in tiles) 
		{
			if (obje.Name.CompareTo (obj.transform.name) == 0) 
			{
				obj.transform.localPosition = new Vector3 (obje.Position.X, obje.Position.Y, obje.Position.Z);
				obj.transform.localEulerAngles = new Vector3 (obje.Rotation.X, obje.Rotation.Y, obje.Rotation.Z);
				obj.transform.localScale = new Vector3 (obje.Scale.X, obje.Scale.Y, obje.Scale.Z);
				break;
			}
		}
	}

}
