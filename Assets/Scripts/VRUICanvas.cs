/*  
*  FILE          : VRUICanvas.cs
*  PROJECT       : ParisVR
*  PROGRAMMER    : Dustin Brown
*  FIRST VERSION : 2018-03-01 
*  DESCRIPTION   : 
*    The functions in this file are used to ... 
*/
using UnityEngine;

public class VRUICanvas : MonoBehaviour
{

	public GameObject VRCanvis;

	public GameObject player;

	/* 
    *  FUNCTION      : Update 
    * 
    *  DESCRIPTION   : Adjusts the position of the VR canvas every frame
    */
	void LateUpdate ()
	{

		//if (Time.frameCount % 15 == 0) {
		Vector3 pos = player.transform.position; //get player condition
		pos.y += 5;
		pos.z += 25; //place canvas 30 units ahead of the player
		VRCanvis.transform.position = pos;
		//}
	}
}
