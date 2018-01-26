/*
* FILE			: PauseScript.cs
* PROJECT		: Final Game Dev
* PROGRAMMERS	: Marco Fontana
* FIRST VERSION	: 9-29-2017
* DESCRIPTION   : This file contains the code and functionality of the pause menu. The pause menu is used to stop the game temporaily, restart the level, or
*                 return to the main menu. Based on code writen by Kristiel from https://www.studica.com/blog/create-ui-unity-tutorial Repurposed from my first gamedev assignment
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseScript : MonoBehaviour {

	//The panel from the game we want to disable/enable
    public Transform pauseMenu;
	public GameObject plyr;
	public Transform cross;

	//A bool flag to tell us when we are paused
    private bool paused;


	//FUNCTION      : Start()
	//DESCRIPTION   : This Method is launched when the level is loaded and is used to gather
	//                information or initalize other variables
	//PARAMETERS    : Nothing
	//RETURNS		: Nothing
    void Start ()
    {
		//Disable the panel and make sure we are unpaused
		pauseMenu.gameObject.SetActive(false);
		cross.gameObject.SetActive (true);
        paused = false;
    }
	

	//FUNCTION      : Update()
	//DESCRIPTION   : This Method is responsible for pausing the game by halting the timescale, or resuming,
	//                a paused game to normal action. Based on code writen by Kristiel from https://www.studica.com/blog/create-ui-unity-tutorial/
	//                Update is called once per frame
	//PARAMETERS    : Nothing
	//RETURNS		: Nothing
	void Update ()
    {
		
		//If the use pressed the esc key and we are currently not paused AND the escCounter is 2
		if (Input.GetKeyDown(KeyCode.Escape) && paused == false && plyr.GetComponent<PlayerController> ().escCount >= 2)
        {
			//Enable the pause menu
            pauseMenu.gameObject.SetActive(true);

			//Disable the crosshair
			cross.gameObject.SetActive (false);

			//Toggle the paused flag
            paused = true;

			//Disable the timescale to halt gameplay
            Time.timeScale = 0.0f;
        }
		else if(Input.GetKeyDown(KeyCode.Escape) && paused == true)//If the use pressed the esc key and we are currently paused
        {
			//Run the resume method
            Resume();
        }
	}

	//FUNCTION      : Resume()
	//DESCRIPTION   : This Method is responsible for resuming the game by resetting the timescale.
	//                Based on code writen by Kristiel from https://www.studica.com/blog/create-ui-unity-tutorial/
	//PARAMETERS    : Nothing
	//RETURNS		: Nothing
    public void Resume()
    {
		//Toggle the paused flag
        paused = false;

		//Disable the pause menu
        pauseMenu.gameObject.SetActive(false);

		//Enable the crosshair
		cross.gameObject.SetActive (true);

		//Reset the Timescale
        Time.timeScale = 1.0f;
    }

	//FUNCTION      : Restart()
	//DESCRIPTION   : This Method is responsible for resetting the game level with the current game level
	//PARAMETERS    : Nothing
	//RETURNS		: Nothing
    public void Restart()
    {
		//Find out which level we are on
        string level = SceneManager.GetActiveScene().name;

		//Call resume for saftey
        Resume();     

		//Reload our current level
        SceneManager.LoadScene(level);
    }


	//FUNCTION      : Restart()
	//DESCRIPTION   : This Method is responsible for quitting the application in a deployed version
	//PARAMETERS    : Nothing
	//RETURNS		: Nothing
    public void Quit()
    {
		//Close the appication
        Application.Quit();
    }

	//FUNCTION      : MainMenu()
	//DESCRIPTION   : This Method is responsible for reloading the main menu scene
	//PARAMETERS    : Nothing
	//RETURNS		: Nothing
    public void MainMenu()
    {
		//Call resume for saftey
        Resume();

		//Reload the menu scene
        SceneManager.LoadScene("Menu");
    }
}


