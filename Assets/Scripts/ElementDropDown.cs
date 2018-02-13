/*
*  FILE          : ElementDropDown.cs
*  PROJECT       : ParisVR
*  PROGRAMMER    : Dustin Brown
*  FIRST VERSION : 2018-02-05
*  DESCRIPTION   :
*    File for the Container class which manages assets
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using container;

public class ElementDropDown : MonoBehaviour {

	public GameObject player;
	public Dropdown elementList;
	public Container container;


	void Start () {
		List<string> l = new List<string> ();
		GameObject obj = player.GetComponent<PlayerController>().obj;
		string item;
			//get the names from the container
		for (int i = 0; i < obj.GetComponent<MeshRenderer>().materials.Length; i++){ 
			item = "Element " + i;
			l.Add (item);
		}
		//add names to the dropdowns
		elementList.AddOptions (l);
	}

	void onPointerClick(){
		List<string> l = new List<string> ();
		GameObject obj = player.GetComponent<PlayerController>().obj;
		string item;
		//get the names from the container
		for (int i = 0; i < obj.GetComponent<MeshRenderer>().materials.Length; i++){ 
			item = "Element " + i;
			l.Add (item);
		}
		//add names to the dropdowns
		elementList.AddOptions (l);
		
	}

}
