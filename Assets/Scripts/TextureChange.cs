/*
*  FILE          : TextureChange.cs
*  PROJECT       : ParisVR
*  PROGRAMMER    : Dustin Brown
*  FIRST VERSION : 2018-02-02
*  DESCRIPTION   :
*    Changes the texture of the selected subAsset to the selected material in the drop down
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using container;

using UnityEditor;



public class TextureChange : MonoBehaviour {

	public GameObject player;
	public Dropdown textureList;
	public Dropdown ElementList;
	public Button btn;
	public Container container;

	private GameObject obj;
	private Material[] mats;

	/*
	* Method      : Start
	* Description : set listerner
	*/
	void Start()
	{
		btn.onClick.AddListener(onClick);
	}


	/*
	* Method      : onClick
	* Description : changes the texture of the selected sub asset to selected material
	*/
	void onClick()
	{
		int index = textureList.value;

		//get the players current selected object
		obj = player.GetComponent<PlayerController>().obj;

		//copy the current materials of the object
		mats = obj.GetComponent<MeshRenderer> ().materials;



		if (ElementList.value == 0) {
			//change them all
			for (int i = 0; i < mats.Length; i++) {
				//mats[i] = new Material (new Shader ());
				mats [i] = container.getMaterial (index);
		

			}
		} else {
			mats [ElementList.value] = container.getMaterial (index);
		}

		//update the object to our new mats
		obj.GetComponent<MeshRenderer>().materials = mats;



	}
}
