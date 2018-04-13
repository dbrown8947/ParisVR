
using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public class VRPlayerController : MonoBehaviour
{

	//Public Variables
	public float speed;
	public GameObject MainMenu;
	public GameObject ImportMenu;
	public SteamVR_TrackedObject leftHand;
	public SteamVR_TrackedObject rightHand;

	public bool gridSelected;
	public GameObject Parent;
	// public GameObject obj;
	public Material mat;

	public transformationMenu transformMenu;
	public GameObject AssetInteractionMenu;
	public GameObject tagViewer;
	public GameObject SLMENU;
	public GameObject OpenFileMenu;
	public GameObject SaveFileMenu;

	private WandInterface leftHandInterface;
	private WandInterface rightHandInterface;
	private PointerEventArgs laserPointerEvent;

	private GameObject importDropdown;
	private GameObject importButton;
	private OBJLoader objLoader = new OBJLoader ();

	//object selection
	private GameObject selectedAsset;
	private bool assetSelected = false;
	/* 
    *  FUNCTION      : onEnable 
    * 
    *  DESCRIPTION   : Gets necassary gameobjects for the player controller
    */
	public void OnEnable ()
	{
		leftHandInterface = leftHand.GetComponent<WandInterface> (); //get interface for controllers
		rightHandInterface = rightHand.GetComponent<WandInterface> ();

		//get the dropdown and button controls
		importDropdown = ImportMenu.transform.GetChild (0).gameObject;
		importButton = ImportMenu.transform.GetChild (1).gameObject;





	}

	/* 
    *  FUNCTION      : Update 
    * 
    *  DESCRIPTION   : Check for movement or VRControl input every frame
    */

	void Update ()
	{
		//Enter the movement handler
		Movement ();

		//Enter the VRControls Handler
		VRControls ();

	}

	/* 
    *  FUNCTION      : Movement 
    * 
    *  DESCRIPTION   : Checks for player movement every frame
    */
	void Movement ()
	{
		//create the movement vector
		Vector3 moving;

		//Get the character controller
		CharacterController player = GetComponent<CharacterController> ();

		//If the game is not paused
		if (Time.timeScale == 1.0f) {
			//Use Standard Movement for the movement vector
			moving = new Vector3 (Input.GetAxis ("Horizontal"), 0.0f, Input.GetAxis ("Vertical"));
		} else {
			//otherwise dont allow the user to move
			moving = new Vector3 (0.0f, 0.0f, 0.0f);
		}

		//Move the player based on the generated vector
		moving = transform.TransformDirection (moving);
		moving *= speed;
		player.Move (moving);
	}


	/* 
    *  FUNCTION      : VRControls 
    * 
    *  DESCRIPTION   : Check for VR controller input
    */
	void VRControls ()
	{
		//if either menu buttons are pressed down
		if (leftHandInterface.getMenuButtonDown () || rightHandInterface.getMenuButtonDown ()) { 
			MainMenu.SetActive (!MainMenu.activeSelf);   //toggle the menu 
		}

		//if right hand trigger down
		if (rightHandInterface.getTriggerButtonDown ()) {
			//raycast
			Ray raycast = new Ray (rightHand.GetComponent<SteamVR_LaserPointer> ().transform.position, rightHand.GetComponent<SteamVR_LaserPointer> ().transform.forward);
			RaycastHit hit = new RaycastHit ();

			//if the raycast hits
			if (Physics.Raycast (raycast, out hit)) {
				//FIXME: Should only require one Gameobject for returned raycast
				GameObject hitParentGameObject = hit.transform.parent.gameObject; // parent of hit gameobject
				GameObject hitGameObject = hit.transform.gameObject; //hit gameobject

				if (hitGameObject.tag == "GridTile") { //gridtile selected
					//FIXME: Why have GridTile, obj, and Parent. Only need 1.
					MainMenu.SetActive (false); //close main menu
					ImportMenu.SetActive (true); //open the import menu
					GameObject GridTile = hit.transform.parent.gameObject; //set variables
					gridSelected = true;
					Debug.Log ("Hit GridTile");
					// obj = hit.transform.gameObject;
					Parent = hit.transform.parent.gameObject;
				}

				if (hitGameObject.tag == "Asset") {
					selectedAsset = hitGameObject;
					assetSelected = true;
					AssetInteractionMenu.SetActive (true);


				}

				if (hitParentGameObject.tag == "NumericBox") {
					hitParentGameObject.GetComponent<NumericBox> ().toggleKeyboard ();
				}

				//if a closed dropdown list is selected
				if (hitParentGameObject.tag == "VRUIDropdownClosed") { 
					hitParentGameObject.transform.parent.gameObject.GetComponent<DropDownScript> ().openList (); //open the dropdown list
				}



				//if a dropdown list element is selected
				if (hitParentGameObject.tag == "VRUIDropdownElement") {
					string element = hit.transform.gameObject.name; //get element text
					int elementNumb = -1;
					element = element.Remove (0, "ElementCube".Length); //get the number of the elemnt
					int.TryParse (element, out elementNumb);
					elementNumb--;
					hitParentGameObject.transform.parent.gameObject.transform.parent.gameObject.GetComponent<DropDownScript> ().selectElement (elementNumb); //select the elemnt
				}
				//if we hit a VRUIScroll object
				if (hitGameObject.tag == "VRUIScroll") {
					if (hit.transform.gameObject.name == "ScrollUp") { //scrollup
						hitGameObject.transform.parent.gameObject.transform.parent.gameObject.GetComponent<DropDownScript> ().scrollUp ();
					} else { //scrolldown
						hitGameObject.transform.parent.gameObject.transform.parent.gameObject.GetComponent<DropDownScript> ().scrollDown ();
					}
				}

				////////////////////////////
				/// NUMERICBOX BUTTONS
				///////////////////////////
				if (hitGameObject.tag == "VRUINumericBoxKey") { //if we hit a key for a numericbox
					NumericBox hb = hitParentGameObject.gameObject.transform.parent.gameObject.GetComponent<NumericBox> (); //get the numericBox

					//if its a number key
					if (hitGameObject.name.Contains ("Key")) {
						if (hitGameObject.name.Length == 4) {
							int num;
							int.TryParse (hitGameObject.name.Remove (0, 3), out num);
							hb.numPress (num);
						}
						//other type of keys
						else if (hitGameObject.name == "KeyDelete") {
							hb.backspace ();
						} else if (hitGameObject.name == "KeyDecimal") {
							hb.decimalPress ();
						} else if (hitGameObject.name == "KeyClear") {
							hb.clear ();
						} else if (hitGameObject.name == "KeyLeft") {
							hb.cursorLeft ();
						} else if (hitGameObject.name == "KeyRight") {
							hb.cursorRight ();
						} else if (hitGameObject.name == "KeyConfirm") {
							hitParentGameObject.transform.parent.GetComponent<NumericBox> ().toggleKeyboard ();
						} else if (hitGameObject.name == "KeyFlipSign") {
							hb.flipSign ();
						}
					}
				}

				//if we find a button
				if (hitParentGameObject.tag == "Button" || hitGameObject.tag == "Button") {
					//if it's the import button
					//FIXME: change name of the import button to be more relevant
					if (hitGameObject.name == "ImportButton") {
						//create new gameobject for the building
						string path = @PlayerPrefs.GetString ("asset", Application.dataPath) + @"\" + importDropdown.GetComponent<DropDownScript> ().getSelectedElement ();
						GameObject building = OBJLoader.LoadOBJFile (path, mat);

						//destroy until we make transformations
						Destroy (building);

						Parent.transform.GetChild (0).gameObject.SetActive (false);
						Parent.transform.GetChild (1).gameObject.SetActive (false);

						building.transform.position = new Vector3 (0, 0, 0);

						Vector3 center = Vector3.zero;
						Vector3 Test = Vector3.zero;

						foreach (Transform child in building.transform) {
							center += child.gameObject.GetComponent<Renderer> ().bounds.center;
						}
						center /= building.transform.childCount; //center is average center of children

						center = (center * 0.3048f) / 12;//Real unity center for object

						building.tag = "Asset";
						building.transform.localScale = new Vector3 (0.0257f, 0.0257f, 0.0257f);
						building.AddComponent<UnityEngine.UI.Text> ();
						GameObject newBuilding = Instantiate (building, Parent.transform);
						float x = center.x;
						float y = center.y;
						float z = center.z;
						newBuilding.transform.localPosition = new Vector3 (newBuilding.transform.localPosition.x - (x), newBuilding.transform.localPosition.y - (y), newBuilding.transform.localPosition.z - (z));

						//Center the highlight for the object around the building
						Vector3 assetSize = (building.GetComponent<BoxCollider> ().size * 0.0254f);
						Vector3 buildingCenter = (building.GetComponent<BoxCollider> ().center * 0.0254f);

						//Access the highlight object inside the parent object
						GameObject HighlightBox = Parent.transform.GetChild (2).gameObject;

						//Scale the highlighter around the building
						HighlightBox.transform.localScale = assetSize;
						HighlightBox.transform.position = newBuilding.transform.position + buildingCenter;
					
						///marcos stuff for asset info
						Asset asset = new Asset ();
						//Populate save information

						asset.ParentInfo.Name = newBuilding.name;

						asset.ParentInfo.Area = newBuilding.transform.parent.parent.name;

						asset.ParentInfo.Tile = newBuilding.transform.parent.name;        //Find the file name of the asset imported

						string[] splits = path.Split ('\\', '/');

						asset.ParentInfo.FileName = splits [splits.Length - 1];         //Find the file name and apply it to each saved asset

						splits = PlayerPrefs.GetString ("xml").Split ('\\', '/');

						asset.MapName = splits [splits.Length - 1];
						SLMENU.GetComponent<SLMenuHandler> ().AddToAssetList (asset);

						//close all the menus
						ImportMenu.SetActive (false);
						//obj.SetActive (false);
						gridSelected = false;

					} else if (hitGameObject.name == "BtnQuitVR") {


						SLMENU.GetComponent<SLMenuHandler> ().VRQuit ();
					


					} else if (hitGameObject.name == "BtnResume") {



					} else if (hitGameObject.name == "BtnSave") {
						///////////////////////////////////////////this is the magic code//
						////////////////SLMENU.GetComponent<SLMenuHandler> ().VRSave ("save1");
						SaveFileMenu.SetActive (true);

					} else if (hitGameObject.name == "BtnLoad") {
						OpenFileMenu.SetActive (true);
						//SLMENU.GetComponent<SLMenuHandler> ().VRLoad ("C:\\Users\\Capstone\\Desktop\\ParisVR-FinalMerged\\Assets\\save1.dat");
					} else if (hitGameObject.name == "BtnRestart") {
						//Find out which level we are on
						string level = SceneManager.GetActiveScene ().name;    
						//Relsoad our current level
						SceneManager.LoadScene (level);
					} else if (hitGameObject.name == "BtnQuit") {

						Application.Quit ();

					} else if (hitGameObject.name == "TransformationMenuCloseButton") {
						hitParentGameObject.SetActive (false);
					} else if (hitGameObject.name == "VRUITagViewClose") {
						hitGameObject.transform.parent.gameObject.SetActive (false);
					}
					/////////////////////////
					/// SELECTED ASSET MENU
					/////////////////////////
					else if (hitGameObject.name == "TransformButton") {
						transformMenu.selectAsset (selectedAsset);
						transformMenu.transform.gameObject.SetActive (true);
						transformMenu.enabled = true;
						AssetInteractionMenu.SetActive (false);

					} else if (hitGameObject.name == "TagButton") {
						AssetInteractionMenu.SetActive (false);
						tagViewer.GetComponent<VRTagViewer> ().selectAsset (selectedAsset);

					} else if (hitGameObject.name == "DeleteAssetButton") {
						AssetInteractionMenu.SetActive (false);
						assetSelected = false;

					} else if (hitGameObject.name == "CloseButton") {

						CloseAllMenus ();
						//AssetInteractionMenu.SetActive (false);
						assetSelected = false;
					} else if (hitGameObject.name == "SaveFileButton") {

						DropDownScript dds = hitGameObject.transform.parent.transform.gameObject.transform.GetChild (0).GetComponent<DropDownScript> ();
						string file = dds.getSelectedElement ();
						//string path = @PlayerPrefs.GetString ("asset", Application.dataPath) + @"\";
						if (file == "New Save") {

							SLMENU.GetComponent<SLMenuHandler> ().VRSave ("VRSAVE-" + System.DateTime.Now.ToString ().Replace (@"/", "-").Replace (" ", "-").Replace (":", "-"));


						} else {
							SLMENU.GetComponent<SLMenuHandler> ().VRSave (file);
							hitGameObject.transform.parent.gameObject.SetActive (false);
						}
					} else if (hitGameObject.name == "OpenFileButton") {
						DropDownScript dds = hitGameObject.transform.parent.transform.gameObject.transform.GetChild (0).GetComponent<DropDownScript> ();
						string file = dds.getSelectedElement ();
						//string path = @PlayerPrefs.GetString ("asset", Application.dataPath) + @"\";
						SLMENU.GetComponent<SLMenuHandler> ().VRLoad (file);

					}



				}
			}
		}

		if (leftHandInterface.getGripButtonPressed ()) {
			//player y down
			Vector3 pos = transform.position;
			pos.y = pos.y + speed;
			transform.position = pos;
		} else if (rightHandInterface.getGripButtonPressed ()) {
			//player y down
			Vector3 pos = transform.position;
			pos.y = pos.y - speed;
			transform.position = pos;
		}


		if (rightHandInterface.getTouchButtonPressed ()) {
			this.transform.Rotate (0, rightHandInterface.getAxis ().x, 0);
		}




	}

	public void CloseAllMenus ()
	{

		MainMenu.SetActive (false);
		ImportMenu.SetActive (false);
		OpenFileMenu.SetActive (false);
		SaveFileMenu.SetActive (false);
		transformMenu.gameObject.SetActive (false);
		///blah
	}


}

