/*  
*  FILE          : RotatingLotIcon.cs
*  PROJECT       : ParisVR
*  PROGRAMMER    : Anthony Bastos
*  FIRST VERSION : 2018-02-08
*  DESCRIPTION   : 
*    This file is used to alter the shape of a quad object's mesh/meshfilter
*    to match 4 given verticies.
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
*   NAME    : RotatingLotIcon 
*   PURPOSE : Makes an object rotate
*/
public class RotatingLotIcon : MonoBehaviour
{

	// Update is called once per frame
	void Update ()
	{
		transform.Rotate (new Vector3 (15, 30, 45) * Time.deltaTime);
	}
}
