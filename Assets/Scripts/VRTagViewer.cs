using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class VRTagViewer : MonoBehaviour {

	public Text TagViewer;

	public void selectAsset(GameObject selectedAsset)
	{
		TagViewer.text = selectedAsset.GetComponent<Text> ().text;
		this.transform.parent.gameObject.SetActive (true);


	}

	public void unselectAsset()
	{
		TagViewer.text = "Empty";
		this.transform.parent.gameObject.SetActive (false);
	}

}
