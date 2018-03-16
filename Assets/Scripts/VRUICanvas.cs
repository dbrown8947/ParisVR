﻿/*  
*  FILE          : VRUICanvas.cs
*  PROJECT       : ParisVR
*  PROGRAMMER    : Dustin Brown
*  FIRST VERSION : 2018-03-01 
*  DESCRIPTION   : 
*    The functions in this file are used to ... 
*/
using UnityEngine;

public class VRUICanvas : MonoBehaviour {

    public GameObject VRCanvis;

    public SteamVR_Camera camera;

    /* 
    *  FUNCTION      : Update 
    * 
    *  DESCRIPTION   : Adjusts the position of the VR canvas every frame
    */
    void Update () {
        Vector3 pos = camera.transform.position; //get player condition
        pos.z += 30; //place canvas 30 units ahead of the player
        VRCanvis.transform.position = pos;
	}
}
