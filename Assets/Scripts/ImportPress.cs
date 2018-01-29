using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImportPress : MonoBehaviour
{
    public Button btn;
    public Dropdown dropdown;

    private GameObject go;
    private int gameObjectCount = 0;


    /*
    * FUNCTION : Start
    *
    * DESCRIPTION : start click listener
    */
    void Start()
    {
        btn.onClick.AddListener(onClick);
        gameObjectCount = 0;
    }

    /*
    * FUNCTION : onClick
    *
    * DESCRIPTION : When Import button is clicked
    */
    void onClick()
    {
        //if all, import all objects in folder
        if (dropdown.options[dropdown.value].text == "All")
        {
            //iterate through directory
            for (int i = 1; i < dropdown.options.Count; i++)
            {
                //import asset
                go = OBJLoader.LoadOBJFile(dropdown.options[i].text);
               
                ApplySettings();

            }
        }
        else
        {
            //import the selected asset
            go = OBJLoader.LoadOBJFile(dropdown.options[dropdown.value].text);
            ApplySettings();
        }


    }

    /*
    * FUNCTION : ApplySettings
    *
    * DESCRIPTION : Applies properties to an import object
    */
    void ApplySettings()
    {
        //place in new postion
        go.transform.position = new Vector3(-15 * gameObjectCount - 10, 0, -15 * gameObjectCount);
        //reduce scale
        go.transform.localScale = new Vector3(0.02f, 0.02f, 0.02f);
        //tag the parent object as an asset
        go.tag = "Asset";
        //destroy the original
        Destroy(go);
        //create new version with transformations applied
        Instantiate(go);

        gameObjectCount++;
    }

}
