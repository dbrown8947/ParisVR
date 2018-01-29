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
    // Use this for initialization

    void Start()
    {
        btn.onClick.AddListener(onClick);
        gameObjectCount = 0;
    }

    void onClick()
    {

        if (dropdown.options[dropdown.value].text == "All")
        {
            for (int i = 1; i < dropdown.options.Count; i++)
            {
                go = OBJLoader.LoadOBJFile(dropdown.options[i].text);
               
                ApplySettings();

            }
        }
        else
        {
            go = OBJLoader.LoadOBJFile(dropdown.options[dropdown.value].text);
            ApplySettings();
        }


    }

    void ApplySettings()
    {
        go.transform.position = new Vector3(-15 * gameObjectCount - 10, 0, -15 * gameObjectCount);
        go.transform.localScale = new Vector3(0.02f, 0.02f, 0.02f);
        go.tag = "Asset";
        Destroy(go);
        Instantiate(go);

        gameObjectCount++;
    }

}
