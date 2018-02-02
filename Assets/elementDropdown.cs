using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor.EventSystems;


public class elementDropdown : MonoBehaviour{

	public GameObject player;
	public Dropdown elementList;

	private GameObject obj;
	private int elementCount;
	private List<string> elementOptions = new List<string>();
	// Use this for initialization
	void Start () {
		//	elementList.OnPointerClick.AddListener (OnPointerClick);
	}
	
	// Update is called once per frame
	void OnEnable () {

		string elementName;
	
		obj = player.GetComponent<PlayerController>().obj;
			elementCount = obj.GetComponent<MeshRenderer> ().materials.Length;
			elementOptions.Clear ();

		elementOptions.Add ("All");
			for (int i = 0; i < elementCount -1; i++) {
				elementName = "Element " + (i + 1);
				elementOptions.Add (elementName);
			}

			elementList.options.Clear ();
			elementList.AddOptions (elementOptions);
		}

}
