using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class transformationMenu : MonoBehaviour {


    public NumericBox positionX;
    public NumericBox positionY;
    public NumericBox positionZ;

    public NumericBox rotationX;
    public NumericBox rotationY;
    public NumericBox rotationZ;

    public NumericBox scaleX;
    public NumericBox scaleY;
    public NumericBox scaleZ;

    private GameObject selectedAsset;
    private bool isAssetSelected = false;

    public void selectAsset(GameObject asset)
    {
        selectedAsset = asset;
        isAssetSelected = true;
        SetValues();
    }

    public void unselectAsset()
    {
        isAssetSelected = false;
    }

    public void SetValues()
    {
        positionX.setValue(selectedAsset.transform.position.x);
        positionY.setValue(selectedAsset.transform.position.y);
        positionZ.setValue(selectedAsset.transform.position.z);

        rotationX.setValue(selectedAsset.transform.rotation.x);
        rotationY.setValue(selectedAsset.transform.rotation.y);
        rotationZ.setValue(selectedAsset.transform.rotation.y);

        scaleX.setValue(selectedAsset.transform.localScale.x);
        scaleY.setValue(selectedAsset.transform.localScale.y);
        scaleZ.setValue(selectedAsset.transform.localScale.z);
    }
	
	// Update is called once per frame
	void Update () {

        if (isAssetSelected)
        {
            selectedAsset.transform.position = new Vector3(positionX.getValue(), positionY.getValue(), positionZ.getValue());
			selectedAsset.transform.rotation = Quaternion.Euler (rotationX.getValue (), rotationY.getValue (), rotationZ.getValue ());//Vector3 (rotationX.getValue (), rotationY.getValue (), rotationZ.getValue ());
			//selectedAsset.transform.Rotate(rotationX.getValue(), rotationY.getValue(), rotationZ.getValue());
            selectedAsset.transform.localScale = new Vector3(scaleX.getValue(), scaleY.getValue(), scaleZ.getValue());
        }
	}
}

