using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Importer;
using GrahamScan;
using UnityEditor;
using System.Linq;

public class LoadNatural : MonoBehaviour {


	public GameObject waterStarter;
	public GameObject waterMesh;
	public GameObject parentStart;
    public static TestObjectPlacement objectloader = new TestObjectPlacement();
    public List<List<OSMPositionNode>> ListOfWaterWays = new List<List<OSMPositionNode>>();
    public List<GameObject> listOfTmp = new List<GameObject>();
    public List<Vector3> vertices = new List<Vector3>();




    //    public List<Vector3> listOfVectors = new List<Vector3>();
    public IEnumerator<Vector3> nodes;
    public int betweenNodeCount = 10;
    public bool loop = false;
    //Has to be at least 4 points
    public Transform[] controlPointsList;
    //Are we making a line or a loop?
    public bool isLooping = false;
    public Vector3 center = new Vector3();

    //public Terrain terrain;
    private void Awake()
    {

        ListOfWaterWays = objectloader.ImportWaterways();
    }
    // Use this for initialization

    void Start()
    {
        List<OSMPositionNode> tempList = new List<OSMPositionNode>();
        List<OSMPositionNode> newList = new List<OSMPositionNode>();





      int OrganizerNumber = 1;
		int roadCount = 0;
		//List<List<OSMPositionNode>> ListOfRoads = objectloader.ImportRoads();
		//List<OSMPositionNode> listOfNodes = objectloader.ImportNodes();
		foreach (List<OSMPositionNode> listOfNodes in ListOfWaterWays)
		{

			GameObject Organizer = Instantiate(waterStarter,parentStart.transform);
			Organizer.name = "Base Water " + OrganizerNumber;
			listOfTmp.Add(Organizer);

			OrganizerNumber++;
			roadCount++;
			int count = 0;


			List<Vector3> listOfPoints = ConvertToVector3List (listOfNodes);

			CreateWater script = (CreateWater)parentStart.GetComponent(typeof(CreateWater));

			Vector3[] ListOfTangentControlPoints = script.GetTangentPoints(listOfPoints);

			Vector3 vertex1 = ListOfTangentControlPoints [0];
			Vector3 vertex2 = ListOfTangentControlPoints [1];
			Vector3 vertex3 = ListOfTangentControlPoints [2];
			Vector3 vertex4 = ListOfTangentControlPoints [3];

			//Keeps track of what vertice we are on
			int j = 1;

			int i = 3;
			while (i < (ListOfTangentControlPoints.Length)) {


				GameObject tmp = Instantiate(waterMesh, Organizer.transform);
				tmp.name = "Water Base " + j;
				j++;

				MeshFilter mesh = tmp.gameObject.GetComponent<MeshFilter> ();


				Vector3[] verticies = new Vector3[4];

				verticies [0] = vertex1;
				verticies [1] = vertex2;
				verticies [2] = vertex3;
				verticies [3] = vertex4;

				Mesh NewMesh = script.UpdateMesh (verticies);

				mesh.mesh = NewMesh;

				if ((i + 1) == (ListOfTangentControlPoints.Length)) {
					break;
				}

				//Set them to the previous 2
				vertex1 = vertex3;
				vertex2 = vertex4;

				i++;
				//Get the next 2
				vertex3 = ListOfTangentControlPoints[i];
				i++;
				vertex4 = ListOfTangentControlPoints[i];

				Material mat = Resources.Load("RiverWater") as Material;
				MeshRenderer meshRenderer = tmp.gameObject.GetComponent<MeshRenderer>();

				meshRenderer.material = mat;

			}


		}
    }
    public List<Vector3> ConvertToVector3List(List<OSMPositionNode> listOfNodes)
	{
		List<Vector3> NewListOfPoints = new List<Vector3>(); 

		foreach(OSMPositionNode node in listOfNodes)
		{
			Vector3 newNode = new Vector3 (node.longitude,0, node.latidude);
			NewListOfPoints.Add (newNode);
		}

		return NewListOfPoints;

	}
	public Vector3[] ConvertVector3ListToArray(List<Vector3> listOfVectors)
	{
		Vector3[] NewListOfPoints = new Vector3[listOfVectors.Count]; 
		int i = 0;
		foreach(Vector3 node in listOfVectors)
		{
			Vector3 newNode = new Vector3 (node.x,0, node.z);
			NewListOfPoints[i] = (newNode);
			i++;
		}

		return NewListOfPoints;

	}
 }

