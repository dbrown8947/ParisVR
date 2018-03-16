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

public class NumericBox : MonoBehaviour {


    private string value; // value in the numeric box
    private int cursorPos; //cursor position in the string
    private Text text; //text component of the GameObject
    private bool keyboardOpen = false; //whether the keyboard is active or not

    /* 
    *  FUNCTION      : Start 
    * 
    *  DESCRIPTION   : Initiates the variables, get the text component of the numericBox prefab
    */
    void Start () {
        value = ""; //initialize vars
        cursorPos = 0;
        text = transform.GetChild(0).gameObject.transform.GetChild(0).gameObject.transform.GetChild(0).GetComponent<Text>(); //get text component of the numericBox
	}

    /* 
    *  FUNCTION      : myFirstFunction 
    * 
    *  DESCRIPTION   : Updates the text component of the NumericBox Prefab
    */
    void Update () {

        //show the cursor every other second
        if(Mathf.Round(Time.time) % 2 == 0)
        {
            text.text = value.Insert(cursorPos, "|");
        }
        else
        {
            text.text = value.Insert(cursorPos, " ");

        }
    }

    /* 
    *  FUNCTION      : getValue 
    * 
    *  DESCRIPTION   : returns the value of the NumericBox 
    * 
    *  RETURNS       : decimal : the value of the NumericBox
    */
    public decimal getValue()
    {
        decimal ret = 0; 

        if(value.Length > 0 && value != ".") //if string isn't null
        {
            decimal.TryParse(value, out ret);
        }
        return ret;
    }


    /* 
    *  FUNCTION      : clear 
    * 
    *  DESCRIPTION   : Clears the value of the NumbericBox
    */
    public void clear()
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
    public void backspace()
    {
        //delete character behind cursor
        value = value.Remove(cursorPos - 1, 1);
        cursorPos--;
    }

    /* 
    *  FUNCTION      : decimalPress 
    * 
    *  DESCRIPTION   : Places a decimal in the value where the cursor
    */
    public void decimalPress()
    {
        //add decimal to value if one doesn't already exist
        if(!value.Contains("."))
        {
            value = value.Insert(cursorPos, ".");
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
    public void numPress(int num)
    {
        //accept numbers between 0 and 10
        if(num >= 0 && num < 10)
        {
           value = value.Insert(cursorPos, num.ToString());
        }
        cursorPos++;
    }

    /* 
    *  FUNCTION      : cursorLeft 
    * 
    *  DESCRIPTION   : Moves the cursor left if not already all the way
    * 
    */
    public void cursorLeft()
    {
        //move cursor left if not already all the way left
        if(cursorPos > 0)
        {
            cursorPos--;
        }
    }

    /* 
*  FUNCTION      : cursorRight 
* 
*  DESCRIPTION   : move the cursor right if not already all the way
*/
    public void cursorRight()
    {
        //move cursor right if not already all the way right
        if(cursorPos < value.Length)
        {
            cursorPos++;
        }
    }
}
