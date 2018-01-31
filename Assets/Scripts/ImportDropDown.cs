using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class ImportDropDown : MonoBehaviour {


    public Dropdown dropdown;
    public string filepath;
    private List<string> objObjects = new List<string>();

    // Use this for initialization
    void Start()
    {
        string[] tempPath = Directory.GetFiles(@filepath, "*.obj");

        for (int i = 0; i < tempPath.Length; i++)
        {
            objObjects.Add(tempPath[i]);
        }

        dropdown.AddOptions(objObjects);	
	}


	void Update()
	{
		
	}



}
