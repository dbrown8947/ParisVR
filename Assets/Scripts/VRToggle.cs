using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VR;

public class VRToggle : MonoBehaviour
{



	public GameObject CameraNormal;
	public GameObject CameraVR;
	public GameObject UINormal;
	public GameObject UIVR;
	public GameObject CameraRig;


	private bool VREnabled;

	void Start ()
	{
		VREnabled = false;
		if (PlayerPrefs.GetInt ("VR", 0) == 1) {
			UINormal.SetActive (true);

			setVRMode (true);
			UINormal.SetActive (false);
		
		}

	}


	public bool isVREnabled ()
	{
		return VREnabled;
	}

	public void setVRMode (bool onoff)
	{
		if (onoff) {
            
			UnityEngine.XR.XRSettings.LoadDeviceByName ("OpenVR");
			UnityEngine.XR.XRSettings.enabled = true;
			CameraNormal.SetActive (false);
			UINormal.SetActive (false);
			UIVR.SetActive (true);
			GetComponent<PlayerController> ().enabled = false; //.SetActive (false);
			VRPlayerController vr = GetComponent<VRPlayerController> ();
			//vr.onEnable ();
			vr.enabled = true;

			CameraVR.SetActive (true);	

		} else {	

			//FIXME: Switching from non-vr to vr to non-vr to vr will fail
			// the properties of the controller components of the camera rig need to be reset when reenabled

			UnityEngine.XR.XRSettings.enabled = false;  
			UnityEngine.XR.XRSettings.LoadDeviceByName ("");
			CameraNormal.SetActive (true);
			CameraVR.SetActive (false);
			//CameraVR.gameObject = CameraRig;
			UINormal.SetActive (true);
			UIVR.SetActive (false);
			this.GetComponent<PlayerController> ().enabled = true; //  .SetActive (true);
			this.GetComponent<VRPlayerController> ().enabled = false;//.gameObject.SetActive (false);
			//UnityEngine.XR.XRSettings.enabled = true;

		}

		VREnabled = onoff;
	}

}
