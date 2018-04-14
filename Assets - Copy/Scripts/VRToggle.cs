/*  
*  FILE          : VRToggle.cs
*  PROJECT       : ParisVR
*  PROGRAMMER    : Dustin Brown
*  FIRST VERSION : 2018-03-20
*  DESCRIPTION   : 
*    Toggles VR mode on and off
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VR;

/*
* Class Name	: VRToggle 
* Description   : Toggles VR mode on and off
*/
public class VRToggle : MonoBehaviour
{
	public GameObject CameraNormal;
	public GameObject CameraVR;
	public GameObject UINormal;
	public GameObject UIVR;
	public GameObject CameraRig;


	private bool VREnabled;
	/*   
	*  Name		: Start
	*	Description : Sets the scene to VR mode if flagged
	*	Parameters	: Nothing
	*  Returns		: Nothing
	*/
	void Start ()
	{
		VREnabled = false;
		if (PlayerPrefs.GetInt ("VR", 0) == 1) {
			UINormal.SetActive (true);
			setVRMode (true);		
		}

	}

	/*   
	* 	Name		: isVREnabled
	*	Description : whether or not VR is enabled
	*	Parameters	: Nothing
	*  	Returns		: bool : whether VR is enabled
	*/
	public bool isVREnabled ()
	{
		return VREnabled;
	}

	/*   
	*  Name		: setVRMode
	*	Description : set scene to either VR or Desktop Mode
	*	Parameters	: Bool : enablevr?
	*  Returns		: Nothing
	*/
	public void setVRMode (bool onoff)
	{
		if (onoff) {
			//turn on OpenVR, change menus and cameras
			UnityEngine.XR.XRSettings.LoadDeviceByName ("OpenVR");
			UnityEngine.XR.XRSettings.enabled = true;
			CameraNormal.SetActive (false);
			UINormal.SetActive (false);
			UIVR.SetActive (true);
			GetComponent<PlayerController> ().enabled = false; //.SetActive (false);
			VRPlayerController vr = GetComponent<VRPlayerController> ();
			vr.enabled = true;

			CameraVR.SetActive (true);	

		} else {	

			//FIXME: Switching from non-vr to vr to non-vr to vr will fail
			// the properties of the controller components of the camera rig need to be reset when reenabled


			//the code below is never called. We reload the scene instead of deactivating. This is a bandaid solution
			UnityEngine.XR.XRSettings.enabled = false;  
			UnityEngine.XR.XRSettings.LoadDeviceByName ("");
			CameraNormal.SetActive (true);
			CameraVR.SetActive (false);
			UINormal.SetActive (true);
			UIVR.SetActive (false);
			this.GetComponent<PlayerController> ().enabled = true;
			this.GetComponent<VRPlayerController> ().enabled = false;

		}

		VREnabled = onoff;
	}

}
