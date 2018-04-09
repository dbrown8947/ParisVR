

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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


	public bool getGripButtonPressed ()
	{
		return GripButtonPressed;

	}

	public bool getGripButtonDown ()
	{
		return GripButtonDown;
	}

	public bool getGripButtonUp ()
	{
		return GripButtonUp;
	}

	public bool getTriggerButtonPressed ()
	{
		return TriggerButtonPressed;
	}

	public bool getTriggerButtonDown ()
	{
		return TriggerButtonDown;
	}

	public bool getTriggerButtonUp ()
	{
		return TriggerButtonUp;
	}

	public bool getMenuButtonPressed ()
	{
		return MenuButtonPressed;
	}

	public bool getMenuButtonDown ()
	{
		return MenuButtonDown;
	}

	public bool getMenuButtonUp ()
	{
		return MenuButtonUp;
	}

	public bool getTouchButtonPressed ()
	{
		return TouchButtonPressed;
	}

	public bool getTouchButtonDown ()
	{
		return TouchButtonDown;
	}

	public bool gettouchButtonUp ()
	{
		return TouchButtonUp;
	}

	public Vector2 getAxis ()
	{
		return Axis;
	}
	// Use this for initialization
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

	// Update is called once per frame
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
