using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using container;

public class TextureDropDown : MonoBehaviour {



	public Dropdown dd;
	public Container container;

	private List<string> materialNames = new List<string>();

	// Use this for initialization
	void Start () 
	{
		
		refreshList ();	
	}
	
	// Update is called once per frame
	void OnPointerClick () 
	{
		refreshList ();
	}

	void refreshList()
	{
		container.ContainerLoad ();
		foreach (string s in container.getMaterialNames()) {
			materialNames.Add (s);
		}
		dd.AddOptions (materialNames);
	}
}
