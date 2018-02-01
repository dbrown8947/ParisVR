using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImportPress : MonoBehaviour
{
    public Button btn;
    public Dropdown dropdown;
	public GameObject plyr;
	public GameObject obj;

    private GameObject building;
    private List<GameObject> buildings = new List<GameObject>();
    private int gameObjectCount = 0;
    // Use this for initialization

    void Start()
    {
        btn.onClick.AddListener(onClick);
        gameObjectCount = 0;
    }

    void onClick()
    {

		if (plyr.GetComponent<PlayerController>().gridSelected) {
        
			building = OBJLoader.LoadOBJFile (dropdown.options [dropdown.value].text);

			obj = plyr.GetComponent<PlayerController>().obj;
			ApplySettings ();
        
		}

    }

    void ApplySettings()
    {
		//building.transform.position = obj.transform.position;
		building.transform.position = new Vector3 (obj.transform.position.x/2 , obj.transform.position.y/2, obj.transform.position.z/2);
		building.transform.localScale = new Vector3(0.02f, 0.02f, 0.02f);
		building.tag = "Asset";
		Destroy(building);
		Instantiate(building);
		obj.SetActive(false);
        gameObjectCount++;
    }

}
