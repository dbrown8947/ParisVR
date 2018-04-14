/*  
*  FILE          : WandInterface.cs
*  PROJECT       : ParisVR
*  PROGRAMMER    : Dustin Brown
*  FIRST VERSION : 2018-03-01
*  DESCRIPTION   : 
*    Wand interface to abstract Controller interaction
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/*
* Class Name	: WandInterface 
* Description   :An Abstract interface for HTC Vive Controllers
*/
public class WandInterface : MonoBehaviour
{
	private SteamVR_Controller.Device controller { get { return SteamVR_Controller.Input ((int)trackedObj.index); } }

	private SteamVR_TrackedObject trackedObj;


	//buttons
	private Valve.VR.EVRButtonId gripButton = Valve.VR.EVRButtonId.k_EButton_Grip;
	private Valve.VR.EVRButtonId trigger = Valve.VR.EVRButtonId.k_EButton_SteamVR_Trigger;
	private Valve.VR.EVRButtonId menu = Valve.VR.EVRButtonId.k_EButton_ApplicationMenu;
	private Valve.VR.EVRButtonId touch = Valve.VR.EVRButtonId.k_EButton_SteamVR_Touchpad;

	//button inputs
	//grip
	// private bool gripButtonPressed = false;
	private bool GripButtonPressed;
	private bool GripButtonDown;
	private bool GripButtonUp;
	//trigger
	bool TriggerButtonPressed;
	bool TriggerButtonDown;
	bool TriggerButtonUp;
	//menu
	bool MenuButtonPressed;
	bool MenuButtonDown;
	private bool MenuButtonUp;
	//touch
	bool TouchButtonPressed;
	bool TouchButtonDown;
	bool TouchButtonUp;

	private Vector2 Axis;

	/*   
	*  Name		: getGripButtonPressed
	*	Description : Gets the state
	*	Parameters	: Nothing
	*  Returns		: The state
	*/
	public bool getGripButtonPressed ()
	{
		return GripButtonPressed;

	}

	/*   
	*  Name		: getGripButtonDown
	*	Description : Gets the state
	*	Parameters	: Nothing
	*  Returns		: The state
	*/
	public bool getGripButtonDown ()
	{
		return GripButtonDown;
	}
	/*   
	*  Name		: getGripButtonUp
	*	Description : Gets the state
	*	Parameters	: Nothing
	*  Returns		: The state
	*/
	public bool getGripButtonUp ()
	{
		return GripButtonUp;
	}
	/*   
	*  Name		: getTriggerButtonPressed
	*	Description : Gets the state
	*	Parameters	: Nothing
	*  Returns		: The state
	*/
	public bool getTriggerButtonPressed ()
	{
		return TriggerButtonPressed;
	}
	/*   
	*  Name		: getTriggerButtonDown
	*	Description : Gets the state
	*	Parameters	: Nothing
	*  Returns		: The state
	*/
	public bool getTriggerButtonDown ()
	{
		return TriggerButtonDown;
	}
	/*   
	*  Name		: getTriggerButtonUp
	*	Description : Gets the state
	*	Parameters	: Nothing
	*  Returns		: The state
	*/
	public bool getTriggerButtonUp ()
	{
		return TriggerButtonUp;
	}
	/*   
	*  Name		: getMenuButtonPressed
	*	Description : Gets the state
	*	Parameters	: Nothing
	*  Returns		: The state
	*/
	public bool getMenuButtonPressed ()
	{
		return MenuButtonPressed;
	}
	/*   
	*  Name		: getMenuButtonDown
	*	Description : Gets the state
	*	Parameters	: Nothing
	*  Returns		: The state
	*/
	public bool getMenuButtonDown ()
	{
		return MenuButtonDown;
	}
	/*   
	*  Name		: getMenuButtonUp
	*	Description : Gets the state
	*	Parameters	: Nothing
	*  Returns		: The state
	*/
	public bool getMenuButtonUp ()
	{
		return MenuButtonUp;
	}
	/*   
	*  Name		: getTouchButtonPressed
	*	Description : Gets the state
	*	Parameters	: Nothing
	*  Returns		: The state
	*/
	public bool getTouchButtonPressed ()
	{
		return TouchButtonPressed;
	}
	/*   
	*  Name		: getTouchButtonDown
	*	Description : Gets the state
	*	Parameters	: Nothing
	*  Returns		: The state
	*/
	public bool getTouchButtonDown ()
	{
		return TouchButtonDown;
	}
	/*   
	*  Name		: gettouchButtonUp
	*	Description : Gets the state
	*	Parameters	: Nothing
	*  Returns		: The state
	*/
	public bool gettouchButtonUp ()
	{
		return TouchButtonUp;
	}
	/*   
	*  Name		: getAxis
	*	Description : Gets the state
	*	Parameters	: Nothing
	*  Returns		: The Axis
	*/
	public Vector2 getAxis ()
	{
		return Axis;
	}

	/*   
	*  Name		: Start
	*	Description : Gets the tracked object component and instantiates the button states
	*	Parameters	: Nothing
	*  Returns		: Nothing
	*/
	void Start ()
	{
		trackedObj = GetComponent<SteamVR_TrackedObject> ();
		GripButtonPressed = false;
		GripButtonDown = false;
		GripButtonUp = false;
		TriggerButtonPressed = false;
		TriggerButtonDown = false;
		TriggerButtonUp = false;
		MenuButtonPressed = false;
		MenuButtonDown = false;
		MenuButtonUp = false;
	}

	/*   
	*  Name		: Update
	*	Description : Updates the Button states
	*	Parameters	: Nothing
	*  Returns		: Nothing
	*/
	void Update ()
	{

		GripButtonDown = controller.GetPressDown (gripButton);
		GripButtonUp = controller.GetPressUp (gripButton);
		GripButtonPressed = controller.GetPress (gripButton);

		TriggerButtonDown = controller.GetPressDown (trigger);
		TriggerButtonUp = controller.GetPressUp (trigger);
		TriggerButtonPressed = controller.GetPress (trigger);

		MenuButtonPressed = controller.GetPress (menu);
		MenuButtonDown = controller.GetPressDown (menu);
		MenuButtonUp = controller.GetPressUp (menu);

		TouchButtonPressed = controller.GetTouch (touch);
		TouchButtonDown = controller.GetTouchDown (touch);
		TouchButtonUp = controller.GetTouchUp (touch);

		if (TouchButtonPressed) {
			Axis = controller.GetAxis ();
		} else {
			Axis = Vector2.zero;
		}


	}


}
