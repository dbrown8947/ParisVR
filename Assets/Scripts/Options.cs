using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;

public class Options : MonoBehaviour 
{
	public Dropdown res;
	public Dropdown wind;
	public Dropdown dtls;
	public Dropdown scl;

	public Resolution[] resolutions;

	public InputField saveFolder;
	public InputField assetFolder;

	// Use this for initialization
	void Start ()
	{
		string resName = Screen.currentResolution.width + " x " + Screen.currentResolution.height;
		resolutions = Screen.resolutions;
		int counter = 0;
		int hold = 0;
		bool found = false;

		//  res.ClearOptions();

		foreach (Resolution rez in resolutions)
		{
			res.options.Add(new Dropdown.OptionData(rez.width + " x " + rez.height));
			counter++;
		}

		int temp = res.value;
		res.value = res.value + 1;


		for(int i=0; i < res.options.Count; i++)
		{
			if (res.options[i].text.CompareTo(resName) == 0)
			{
				hold = i;
				found = true;
				res.value = hold;
				break;
			}
		}

		if(!found)
		{
			res.options.Add(new Dropdown.OptionData(Screen.currentResolution.width + " x " + Screen.currentResolution.height));
			res.value = 1;
		}
	}

	public void OnResolutionChange(int index)
	{
		Resolution rez = resolutions[index];

		// if()

		//Screen.SetResolution(rez.width, rez.width,);
	}

	public void OnDetailsChange()
	{

	}

	public void OnScaleChange()
	{

	}
		
	public void BrowseSaveFolder()
	{
		SaveLoadFolderHandler ("Save Folder", false);
	}

	public void BrowseAssetFolder()
	{
		SaveLoadFolderHandler ("Asset Folder", true);
	}

	private void SaveLoadFolderHandler(string menu, bool type)
	{
		try
		{
			string path = "";

			if(!type)
			{
				path = EditorUtility.OpenFolderPanel("Save Folder", "", "");
			}
			else
			{
				path = EditorUtility.OpenFolderPanel("Asset Folder", "", "");
			}	

			if(path.Length != 0)
			{
				if(!type)
				{
					saveFolder.text = path;
				}
				else
				{
					assetFolder.text = path;
				}
			}
			else
			{
				throw new Exception("Folder Not Found");	
			}
		}
		catch(Exception e)
		{
			//Display the error to the user if one occurs
			EditorUtility.DisplayDialog(menu + " Menu Error", e.Message, "OK");
		}
			
	}
}
