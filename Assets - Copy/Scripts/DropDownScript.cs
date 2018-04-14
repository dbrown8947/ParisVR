/*  
*  FILE          : VRDropDown.cs 
*  PROJECT       : ParisVR
*  PROGRAMMER    : Dustin Brown 
*  FIRST VERSION : 2018-03-01 
*  DESCRIPTION   : 
*   Custom DropDown for virtual reality in unity
*/
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DropDownScript : MonoBehaviour
{

	//closed and open dropdown gameObjects
	public GameObject closedUI;
	public GameObject openUI;

	//internal variables
	private List<string> elements = new List<string> ();
	private int selectedIndex = 0;
	private int scrollPosition = 0;
	private bool dropDownOpen;
	private Text selectedText;
	//Individual element Text components
	private Text[] texts = new Text[10];


	/* 
    *  FUNCTION      : Awake 
    * 
    *  DESCRIPTION   : Called before the component starts, gets all required gameobjects
    */
	private void Awake ()
	{
		//get all the text components
		for (int i = 0; i < 10; i++) {
			texts [i] = openUI.transform.GetChild (i).gameObject.transform.GetChild (0).gameObject.transform.GetChild (0).gameObject.GetComponent<Text> ();
		}
		//get the closed dropdown text component
		selectedText = closedUI.transform.GetChild (0).gameObject.transform.GetChild (0).gameObject.GetComponent<Text> ();
	}


	/* 
    *  FUNCTION      : Start 
    * 
    *  DESCRIPTION   : Called when component starts. updates the user interface
    */
	void Start ()
	{

		//update the ui
		UpdateSelectedText ();
		UpdateOpenList ();

	}


	/* 
    *  FUNCTION      : addElement 
    * 
    *  DESCRIPTION   : Adds an element to the dropdown
    * 
    *  PARAMETERS    : string element : element to add
    */
	public void addElement (string element)
	{
		//add element to list
		elements.Add (element);
		//update ui
		UpdateSelectedText ();
		UpdateOpenList ();
	}


	/* 
    *  FUNCTION      : getElement 
    * 
    *  DESCRIPTION   : Returns the element at a specified index
    * 
    *  PARAMETERS    : int index : index of the element to return
    * 
    *  RETURNS       : string : the text of the element at the specified index
    */
	public string getElement (int index)
	{

		//if element exists, return it, else exception
		if (index > 0 && index < elements.Count) {
			return elements [index];
		} else {
			throw new System.Exception ("Element at index does not exist.");
		}
	}


	/* 
    *  FUNCTION      : getSelectedElement
    * 
    *  DESCRIPTION   : returns the selected index of the dropdown
    * 
    *  RETURNS       : string : the text of the current selected index
    */
	public string getSelectedElement ()
	{
		return elements [selectedIndex];
	}

	/* 
    *  FUNCTION      : isDropdownOpen
    * 
    *  DESCRIPTION   : returns whether the dropdown is open
    * 
    *  RETURNS       : bool : whether the dropdown is open
    */
	public bool isDropDownOpen ()
	{
		return dropDownOpen;
	}

	/* 
    *  FUNCTION      : openList
    * 
    *  DESCRIPTION   : sets the list to open
    */
	public void openList ()
	{
		//flag as opened
		dropDownOpen = true;
		//hide closed gameobject, show open
		closedUI.SetActive (false);
		openUI.SetActive (true);
		//update ui
		UpdateOpenList ();
	}

	/* 
    *  FUNCTION      : closeList
    * 
    *  DESCRIPTION   : closes the dropdown
    */
	public void closeList ()
	{
		//flag as closed
		dropDownOpen = false;
		//hide open gameobject, show closed
		closedUI.SetActive (true);
		openUI.SetActive (false);
		//update ui
		UpdateSelectedText ();
	}

	/* 
    *  FUNCTION      : scrollUp
    * 
    *  DESCRIPTION   : scrolls up in the dropdown if possible
    */
	public void scrollUp ()
	{
		//if room to scroll up, scroll up
		if (scrollPosition > 0) {
			scrollPosition -= 10;
		}
		//update list
		UpdateOpenList ();
	}

	/* 
    *  FUNCTION      : scrollDown
    * 
    *  DESCRIPTION   : scrolls down in the dropdown if possible
    */
	public void scrollDown ()
	{
		//if room to scroll down, scroll down
		if (scrollPosition < elements.Count) {
			scrollPosition += 10;
		}
		//update ui
		UpdateOpenList ();
	}

	/* 
    *  FUNCTION      : UpdateOpenList
    * 
    *  DESCRIPTION   : updates the ui in the openlist gameobject
    */
	public void UpdateOpenList ()
	{
		//iterate through elements, assigning appropriate value based on scroll position
		for (int i = scrollPosition; i < scrollPosition + 10; i++) {
			if (elements.Count > i) {
				texts [i - scrollPosition].text = elements [i];

			} else {
				texts [i - scrollPosition].text = " ";
			}
		}

	}

	/* 
    *  FUNCTION      : UpdateSelectedText
    * 
    *  DESCRIPTION   : updates the ui in the closed dropdown gameobject
    */
	public void UpdateSelectedText ()
	{
		//assign selected text value to text component
		if (elements.Count > 0) {
			selectedText.text = elements [selectedIndex];
		} else {
			selectedText.text = "List Empty";
		}

	}

	/* 
    *  FUNCTION      : selectElement 
    * 
    *  DESCRIPTION   : Sets the selected element to the element at the specified index
    * 
    *  PARAMETERS    : int openListindex : the index of the element
    */
	public void selectElement (int openListIndex)
	{
		//update selected value
		selectedIndex = scrollPosition + openListIndex;
		closeList ();
		UpdateSelectedText ();
	}
}
