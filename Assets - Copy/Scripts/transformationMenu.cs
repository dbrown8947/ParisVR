/*  
*  FILE          : transformationMenu.cs
*  PROJECT       : ParisVR
*  PROGRAMMER    : Dustin Brown
*  FIRST VERSION : 2018-03-20 
*  DESCRIPTION   : 
*    This script provides NumericBoxes with an assets transformation values to allow the user to change them in vr mode
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/*
* Class Name	: transformationMenu 
* Description   : provides NumericBoxes with an assets transformation values to allow the user to change them in vr mode
*/
public class transformationMenu : MonoBehaviour
{

	//numericboxes for each transformation value
	public NumericBox positionX;
	public NumericBox positionY;
	public NumericBox positionZ;

	public NumericBox rotationX;
	public NumericBox rotationY;
	public NumericBox rotationZ;

	public NumericBox scaleX;
	public NumericBox scaleY;
	public NumericBox scaleZ;

	private GameObject selectedAsset;
	private bool isAssetSelected = false;
	/*   
	* 	Name		: selectAsset
	*	Description : select an asset to attach to the menu
	*	Parameters	: GameObject asset : the asset
	*  	Returns		: Nothing
	*/
	public void selectAsset (GameObject asset)
	{
		selectedAsset = asset;
		isAssetSelected = true;
		SetValues ();
	}
	/*   
	* 	Name		: unselectAsset
	*	Description : unselects the asset
	*	Parameters	: Nothing
	*  	Returns		: Nothing
	*/
	public void unselectAsset ()
	{
		isAssetSelected = false;
	}

	/*   
	* 	Name		: SetValues
	*	Description : Sets the values of the numericBoxes to the current transform values of the selected asset 
	*	Parameters	: Nothing
	*  	Returns		: Nothing
	*/
	public void SetValues ()
	{
		positionX.setValue (selectedAsset.transform.position.x);
		positionY.setValue (selectedAsset.transform.position.y);
		positionZ.setValue (selectedAsset.transform.position.z);

		rotationX.setValue (selectedAsset.transform.rotation.x);
		rotationY.setValue (selectedAsset.transform.rotation.y);
		rotationZ.setValue (selectedAsset.transform.rotation.y);

		scaleX.setValue (selectedAsset.transform.localScale.x);
		scaleY.setValue (selectedAsset.transform.localScale.y);
		scaleZ.setValue (selectedAsset.transform.localScale.z);
	}
	
	/*   
	* 	Name		: Update
	*	Description : Applies changed to the transform of the selected asset 
	*	Parameters	: Nothing
	*  	Returns		: Nothing
	*/
	void Update ()
	{

		if (isAssetSelected) {
			selectedAsset.transform.position = new Vector3 (positionX.getValue (), positionY.getValue (), positionZ.getValue ());
			selectedAsset.transform.rotation = Quaternion.Euler (rotationX.getValue (), rotationY.getValue (), rotationZ.getValue ());//Vector3 (rotationX.getValue (), rotationY.getValue (), rotationZ.getValue ());
			//selectedAsset.transform.Rotate(rotationX.getValue(), rotationY.getValue(), rotationZ.getValue());
			selectedAsset.transform.localScale = new Vector3 (scaleX.getValue (), scaleY.getValue (), scaleZ.getValue ());
		}
	}
}

