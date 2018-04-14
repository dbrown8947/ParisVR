/*  
*  FILE          : LoadRoads.cs
*  PROJECT       : ParisVR
*  PROGRAMMER    : Anthony Bastos, Ronnie Skowron
*  FIRST VERSION : 2018-02-08
*  DESCRIPTION   : 
*    This file is used get the required data from an OSM xml file, and preform a catmull rom spline
*    caluclations on the given points in order to draw roads.
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Importer;
using GrahamScan;


/*
*   NAME    : LoadRoads 
*   PURPOSE : Prepares data to be drawn in the game world
*/
public class LoadRoads : MonoBehaviour
{
	public GameObject roadStarter;
	public GameObject roadMesh;
	public GameObject parentStart;
	public TestObjectPlacement objectloader;
	public List<List<OSMPositionNode>> ListOfRoads = new List<List<OSMPositionNode>> ();
	public List<GameObject> listOfTmp = new List<GameObject> ();

	//    public List<Vector3> listOfVectors = new List<Vector3>();
	public IEnumerator<Vector3> nodes;
	public int betweenNodeCount = 10;
	public bool loop = false;
	//Has to be at least 4 points
	public Transform[] controlPointsList;
	//Are we making a line or a loop?
	public bool isLooping = false;

	private void Awake ()
	{
		objectloader = new TestObjectPlacement ();
		ListOfRoads = objectloader.ImportRoads ();
	}



	//Display a spline between 2 points derived with the Catmull-Rom spline algorithm
	void DisplayCatmullRomSpline (int pos)
	{
		//The 4 points we need to form a spline between p1 and p2
		Vector3 p0 = controlPointsList [ClampListPos (pos - 1)].position;
		Vector3 p1 = controlPointsList [pos].position;
		Vector3 p2 = controlPointsList [ClampListPos (pos + 1)].position;
		Vector3 p3 = controlPointsList [ClampListPos (pos + 2)].position;

		//The start position of the line
		Vector3 lastPos = p1;

		//The spline's resolution
		//Make sure it's is adding up to 1, so 0.3 will give a gap, but 0.2 will work
		float resolution = 0.1f;

		//How many times should we loop?
		int loops = Mathf.FloorToInt (1f / resolution);

		for (int i = 1; i <= loops; i++) {
			//Which t position are we at?
			float t = i * resolution;

			//Find the coordinate between the end points with a Catmull-Rom spline
			Vector3 newPos = GetCatmullRomPosition (t, p0, p1, p2, p3);

			//Draw this line segment
			Gizmos.DrawLine (lastPos, newPos);

			//Save this pos so we can draw the next line segment
			lastPos = newPos;
		}
	}

	//Clamp the list positions to allow looping
	int ClampListPos (int pos)
	{
		if (pos < 0) {
			pos = controlPointsList.Length - 1;
		}

		if (pos > controlPointsList.Length) {
			pos = 1;
		} else if (pos > controlPointsList.Length - 1) {
			pos = 0;
		}

		return pos;
	}

	//Returns a position between 4 Vector3 with Catmull-Rom spline algorithm
	//http://www.iquilezles.org/www/articles/minispline/minispline.htm
	Vector3 GetCatmullRomPosition (float t, Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3)
	{
		//The coefficients of the cubic polynomial (except the 0.5f * which I added later for performance)
		Vector3 a = 2f * p1;
		Vector3 b = p2 - p0;
		Vector3 c = 2f * p0 - 5f * p1 + 4f * p2 - p3;
		Vector3 d = -p0 + 3f * p1 - 3f * p2 + p3;

		//The cubic polynomial: a + b * t + c * t^2 + d * t^3
		Vector3 pos = 0.5f * (a + (b * t) + (c * t * t) + (d * t * t * t));

		return pos;
	}



	/* 
    *  METHOD        : Awake 
    * 
    *  DESCRIPTION   : Get the location data for roads from a given OSM xml file
    * 
    */
	void Start ()
	{
		int OrganizerNumber = 1;
		int roadCount = 0;
		//List<List<OSMPositionNode>> ListOfRoads = objectloader.ImportRoads();
		//List<OSMPositionNode> listOfNodes = objectloader.ImportNodes();
		foreach (List<OSMPositionNode> listOfNodes in ListOfRoads) {

			GameObject Organizer = Instantiate (roadStarter, parentStart.transform);
			Organizer.name = "Base Road " + OrganizerNumber;
			listOfTmp.Add (Organizer);
			Organizer.transform.localPosition = new Vector3 (0f, 0.01f, 0f);
			OrganizerNumber++;
			roadCount++;
			int count = 0;


			//For Visuals
			foreach (OSMPositionNode o in listOfNodes) {
				count++;
			}

			List<Vector3> listOfPoints = ConvertToVector3List (listOfNodes);

			CreateRoad script = (CreateRoad)Organizer.GetComponent (typeof(CreateRoad));

			Vector3[] ListOfTangentControlPoints = script.GetTangentPoints (listOfPoints);

			Vector3 vertex1 = ListOfTangentControlPoints [0];
			Vector3 vertex2 = ListOfTangentControlPoints [1];
			Vector3 vertex3 = ListOfTangentControlPoints [2];
			Vector3 vertex4 = ListOfTangentControlPoints [3];

			//Keeps track of what vertice we are on
			int j = 1;

			int i = 3;
			while (i < (ListOfTangentControlPoints.Length)) {


				GameObject tmp = Instantiate (roadMesh, Organizer.transform);
				tmp.name = "Road Base " + j;
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
				vertex3 = ListOfTangentControlPoints [i];
				i++;
				vertex4 = ListOfTangentControlPoints [i];

			}


		}
	}


	public List<Vector3> ConvertToVector3List (List<OSMPositionNode> listOfNodes)
	{
		List<Vector3> NewListOfPoints = new List<Vector3> (); 

		foreach (OSMPositionNode node in listOfNodes) {
			Vector3 newNode = new Vector3 (node.longitude, 0, node.latidude);
			NewListOfPoints.Add (newNode);
		}

		return NewListOfPoints;

	}

}