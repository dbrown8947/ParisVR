/*  
*  FILE          : NumericBox.cs
*  PROJECT       : ParisVR
*  PROGRAMMER    : Dustin Brown
*  FIRST VERSION : 2018-03-09
*  DESCRIPTION   : 
*    Script for the NumericBox Prefab 
*/

using UnityEngine;
using UnityEngine.UI;

public class NumericBox : MonoBehaviour
{

	public GameObject keyboard;

	private string value;
	// value in the numeric box
	private int cursorPos;
	//cursor position in the string
	private Text text;
	//text component of the GameObject
	private bool keyboardOpen = false;
	//whether the keyboard is active or not

	/* 
    *  FUNCTION      : Start 
    * 
    *  DESCRIPTION   : Initiates the variables, get the text component of the numericBox prefab
    */
	void Awake ()
	{
		// value = ""; //initialize vars
		cursorPos = 0;
		text = transform.GetChild (0).gameObject.transform.GetChild (0).gameObject.transform.GetChild (0).GetComponent<Text> (); //get text component of the numericBox
	}

	/* 
    *  FUNCTION      : myFirstFunction 
    * 
    *  DESCRIPTION   : Updates the text component of the NumericBox Prefab
    */
	void Update ()
	{

		if (keyboardOpen) {
			//show the cursor every other second
			if (Mathf.Round (Time.time) % 2 == 0) {
				text.text = value.Insert (cursorPos, "|");
			} else {
				text.text = value.Insert (cursorPos, " ");

			}

		} else {
			text.text = value;

		}
	}
	/* 
    *  FUNCTION      : getValue 
    * 
    *  DESCRIPTION   : returns the value of the NumericBox 
    * 
    *  RETURNS       : decimal : the value of the NumericBox
    */
	public float getValue ()
	{
		float ret = 0; 

		if (value.Length > 0 && value != ".") { //if string isn't null
			float.TryParse (value, out ret);
		}
		return ret;
	}

	/* 
    *  FUNCTION      : setValue 
    * 
    *  DESCRIPTION   : changes the value of the feild
    */
	public void setValue (float value)
	{
		this.value = value.ToString ();
	}

	/* 
    *  FUNCTION      : setValue 
    * 
    *  DESCRIPTION   : flips the sign of the current value
    */
	public void flipSign ()
	{
		this.value = (this.getValue () * -1).ToString ();
		if (getValue () > 0) {
			cursorLeft ();
		} else {
			cursorRight ();
		}
	}
	/* 
    *  FUNCTION      : clear 
    * 
    *  DESCRIPTION   : Clears the value of the NumbericBox
    */
	public void clear ()
	{
		//reset variables
		cursorPos = 0;
		value = "";
	}

	/* 
    *  FUNCTION      : backspace 
    * 
    *  DESCRIPTION   : Deletes the character behind the cursor in the value string
    */
	public void backspace ()
	{
		//delete character behind cursor
		value = value.Remove (cursorPos - 1, 1);
		cursorPos--;
	}

	/* 
    *  FUNCTION      : decimalPress 
    * 
    *  DESCRIPTION   : Places a decimal in the value where the cursor
    */
	public void decimalPress ()
	{
		//add decimal to value if one doesn't already exist
		if (!value.Contains (".")) {
			value = value.Insert (cursorPos, ".");
			cursorPos++;
		}
	}

	/* 
    *  FUNCTION      : numPress 
    * 
    *  DESCRIPTION   : Adds a digit to the value
    * 
    *  PARAMETERS    : int num : a digit to add
    */
	public void numPress (int num)
	{
		//accept numbers between 0 and 10
		if (num >= 0 && num < 10) {
			value = value.Insert (cursorPos, num.ToString ());
		}
		cursorPos++;
	}

	/* 
    *  FUNCTION      : cursorLeft 
    * 
    *  DESCRIPTION   : Moves the cursor left if not already all the way
    * 
    */
	public void cursorLeft ()
	{
		//move cursor left if not already all the way left
		if ((getValue () >= 0 && cursorPos > 0) || (getValue () < 0 && cursorPos > 1)) {
			cursorPos--;
		}
	}

	/* 
*  FUNCTION      : cursorRight 
* 
*  DESCRIPTION   : move the cursor right if not already all the way
*/
	public void cursorRight ()
	{
		//move cursor right if not already all the way right
		if (cursorPos < value.Length) {
			cursorPos++;
		}
	}

	/* 
    *  FUNCTION      : toggleKeyboard 
    * 
    *  DESCRIPTION   : toggles the dedicated keyboard on and off
    */
	public void toggleKeyboard ()
	{
		keyboardOpen = !keyboardOpen;

		if (keyboardOpen) {
			keyboard.SetActive (true);
		} else {
			keyboard.SetActive (false);
		}
	}
}
