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
	public GameObject Parent;
	public GameObject Menu;
	public GameObject Camera;

    private GameObject building;
    // Use this for initialization

    void Start()
    {
        btn.onClick.AddListener(onClick);
    }

    void onClick()
    {
		if (plyr.GetComponent<PlayerController>().gridSelected) 
		{
			Asset asset = new Asset ();
			building = OBJLoader.LoadOBJFile (dropdown.options [dropdown.value].text);

			obj = plyr.GetComponent<PlayerController>().obj;
			Parent = plyr.GetComponent<PlayerController>().Parent;
			ApplySettings (asset);
			CaptureAsset (asset);
		}

    }

	void ApplySettings(Asset asset)
    {
		Quaternion rotation = new Quaternion(0,0,0,0);

		Destroy(building);
		//building.transform.position = obj.transform.position;
		building.transform.position = new Vector3 (0, 0, 0);
		building.transform.rotation = rotation;

		Vector3 center = Vector3.zero;
		Vector3 Test = Vector3.zero;

		foreach (Transform child in building.transform)
		{
			center += child.gameObject.GetComponent<Renderer>().bounds.center;
		}
		center /= building.transform.childCount; //center is average center of children

		center = (center * 0.3048f) / 12;//Real unity center for object

		building.tag = "Asset";
		building.transform.localScale = new Vector3(0.0257f, 0.0257f, 0.0257f);

		GameObject newBuilding= Instantiate(building,Parent.transform);
		float x = center.x;
		float y = center.y;
		float z = center.z;


		//newBuilding.transform.position = new Vector3 (newBuilding.transform.position.x - (x), newBuilding.transform.position.y - y, newBuilding.transform.position.z - z);
		//newBuilding.transform.Translate(new Vector3 ((Parent.transform.localPosition.x), (Parent.transform.position.y), (Parent.transform.position.z))* Time.deltaTime);
		//newBuilding.transform.position = new Vector3 (newBuilding.transform.position.x - (x), newBuilding.transform.position.y - (y), newBuilding.transform.position.z - (z));
		newBuilding.transform.localPosition = new Vector3 (newBuilding.transform.localPosition.x - (x), newBuilding.transform.localPosition.y  - (y), newBuilding.transform.localPosition.z  - (z));

		//newBuilding.transform.localPosition = new Vector3 (x, y, z);

		//newBuilding.transform.Translate(new Vector3 (newBuilding.transform.position.x - (x), (y), (y))* Time.deltaTime);

		//Populate
		asset.ParentInfo.Name = newBuilding.name;
		asset.ParentInfo.FileName = dropdown.options [dropdown.value].text;
		asset.ParentInfo.Area = newBuilding.transform.parent.parent.parent.parent.name;
		asset.ParentInfo.Area = newBuilding.transform.parent.name;
			
		obj.SetActive(false);
		Menu.SetActive (false);
		plyr.GetComponent<PlayerController> ().gridSelected = false;
		plyr.GetComponent<PlayerController> ().selected = false;
    }

	public void ApplySettings(TempVector pos, TempVector rot, TempVector scale, string area, string tile, string fileName)
    {
        GameObject bigParent = GameObject.Find(area);
        building = OBJLoader.LoadOBJFile(fileName);
       
		Parent = FindTileParent (area);
        Quaternion rotation = new Quaternion(0, 0, 0, 0);

        Debug.Log(tile);
        Debug.Log(pos.X.ToString());
        Debug.Log(pos.Y.ToString());
        Debug.Log(pos.Z.ToString());


        // building = GameObject.Find(parentName);
        Destroy(building);
        //building.transform.position = obj.transform.position;
        building.transform.position = new Vector3(0, 0, 0);
        building.transform.rotation = rotation;


        Vector3 center = Vector3.zero;
        Vector3 Test = Vector3.zero;

        foreach (Transform child in building.transform)
        {
            center += child.gameObject.GetComponent<Renderer>().bounds.center;
        }
        center /= building.transform.childCount; //center is average center of children

        center = (center * 0.3048f) / 12;//Real unity center for object

        building.tag = "Asset";
        building.transform.localScale = new Vector3(0.0257f, 0.0257f, 0.0257f);

        GameObject newBuilding = Instantiate(building, Parent.transform);
        float x = center.x;
        float y = center.y;
        float z = center.z;


        //newBuilding.transform.position = new Vector3 (newBuilding.transform.position.x - (x), newBuilding.transform.position.y - y, newBuilding.transform.position.z - z);
        //newBuilding.transform.Translate(new Vector3 ((Parent.transform.localPosition.x), (Parent.transform.position.y), (Parent.transform.position.z))* Time.deltaTime);
        //newBuilding.transform.position = new Vector3 (newBuilding.transform.position.x - (x), newBuilding.transform.position.y - (y), newBuilding.transform.position.z - (z));
        newBuilding.transform.position = new Vector3(pos.X, pos.Y, pos.Z);
        newBuilding.transform.eulerAngles = new Vector3(rot.X, rot.Y, rot.Z);
        ///newBuilding.transform.localScale = new Vector3(scale.X, scale.Y, scale.Z);

        //newBuilding.transform.localPosition = new Vector3 (x, y, z);

        //newBuilding.transform.Translate(new Vector3 (newBuilding.transform.position.x - (x), (y), (y))* Time.deltaTime);

        //obj.SetActive(false);
        //Menu.SetActive(false);
        //plyr.GetComponent<PlayerController>().gridSelected = false;
        //plyr.GetComponent<PlayerController>().selected = false;
    }

	void CaptureAsset(Asset asset)
	{
		GameObject.Find("PauseMenu").GetComponent<SLMenuHandler>().AddToAssetList(asset);
	}

	GameObject FindTileParent(string name)
	{
		GameObject par = null;
		GameObject[] tiles = GameObject.FindGameObjectsWithTag("GridTile");

		foreach (GameObject obj in tiles) 
		{
			if (obj.transform.parent.parent.parent.parent.name.CompareTo (name)) 
			{
				par = obj;
				break;
			}
		}

		return par;
	}
}
