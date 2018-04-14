/*  
*  FILE          : VRTagViewer.cs
*  PROJECT       : ParisVR
*  PROGRAMMER    : Dustin Brown
*  FIRST VERSION : 2018-04-01
*  DESCRIPTION   : 
*    Toggles VR mode on and off
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*
* Class Name	: VRTagViewer 
* Description   : Controls the Tag Viewer
*/
public class VRTagViewer : MonoBehaviour
{

	public Text TagViewer;

	/*   
	* 	Name		: selectAsset
	*	Description : selects an asset for the TagViewer to read from
	*	Parameters	: GameObject selectedAsset
	*  	Returns		: Nothing
	*/
	public void selectAsset (GameObject selectedAsset)
	{
		//get text attached to asset and display
		TagViewer.text = selectedAsset.GetComponent<Text> ().text;
		this.transform.parent.gameObject.SetActive (true);


	}
	/*   
	* 	Name		: unselectAsset
	*	Description : unselects the asset and resets the tagViewer
	*	Parameters	: Nothing
	*  	Returns		: Nothing
	*/
	public void unselectAsset ()
	{
		TagViewer.text = "Empty";
		this.transform.parent.gameObject.SetActive (false);
	}

}
