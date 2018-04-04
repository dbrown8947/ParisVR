using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace GrahamScan
{
	public class GrahamScan : MonoBehaviour
	{
	public GameObject World;
	public GameObject BaseLot;
	TestObjectPlacement objectloader = new TestObjectPlacement();

	void Start()
	{
		List<List<OSMPositionNode>> ListOfBuildings = objectloader.ImportNodes();
		int BuildingNum = 1;
		//List<OSMPositionNode> listOfNodes = objectloader.ImportNodes();
		foreach (List<OSMPositionNode> listOfNodes in ListOfBuildings) {


			
			int numberOfSortedPoints = 0;
			int numberOfPoints = 0;
			List<OSMPositionNode> listOfGrahamScanPoints = new List<OSMPositionNode> ();
			List<PolarAngle> listOfAngles = new List<PolarAngle> ();

			List<OSMPositionNode> tempList = new List<OSMPositionNode> ();

			/*List<OSMPositionNode> listOfNodes = new List<OSMPositionNode>()
            {
                new OSMPositionNode(){id = 0,  longitude = 0, latidude = 3 },
                new OSMPositionNode(){id = 1,  longitude = 1, latidude = 1 },
                new OSMPositionNode(){id = 2,  longitude = 2, latidude = 2 },
                new OSMPositionNode(){id = 3,  longitude = 4, latidude = 4 },
                new OSMPositionNode(){id = 4,  longitude = 0, latidude = 0 },
                new OSMPositionNode(){id = 5,  longitude = 1, latidude = 2 },
                new OSMPositionNode(){id = 6,  longitude = 3, latidude = 1 },
                new OSMPositionNode(){id = 7,  longitude = 3, latidude = 3 },
            };*/

			/*List<OSMPositionNode> listOfNodes = new List<OSMPositionNode>()
			{
				new OSMPositionNode(){id = 0,  longitude = 2f, latidude = 0f },
				new OSMPositionNode(){id = 1,  longitude = 4f, latidude = 6f },
				new OSMPositionNode(){id = 2,  longitude = 3f, latidude = 5f },
				new OSMPositionNode(){id = 3,  longitude = 0.1f, latidude = 4f },
				new OSMPositionNode(){id = 4,  longitude = 2.1f, latidude = 4f },
				new OSMPositionNode(){id = 5,  longitude = 6.1f, latidude = 4f },
				new OSMPositionNode(){id = 6,  longitude = 4.1f, latidude = 4f },
				new OSMPositionNode(){id = 7,  longitude = 1.3f, latidude = 3f },
				new OSMPositionNode(){id = 8,  longitude = 3.3f, latidude = 3f },
				new OSMPositionNode(){id = 9,  longitude = 5.3f, latidude = 3f },
				new OSMPositionNode(){id = 10, longitude = 0f, latidude = 2f },
				new OSMPositionNode(){id = 11, longitude = 2.1f, latidude = 2f },
				new OSMPositionNode(){id = 12, longitude = 4.1f, latidude = 2f },
				new OSMPositionNode(){id = 13, longitude = 6.1f, latidude = 2f },
				new OSMPositionNode(){id = 14, longitude = 3f, latidude = 1f },
				new OSMPositionNode(){id = 15, longitude = 2f, latidude = 6f },
				new OSMPositionNode(){id = 16, longitude = 4f, latidude = 0f }
			};*/

			/*List<OSMPositionNode> listOfNodes = new List<OSMPositionNode>()
			{
				new OSMPositionNode(){id = 0,  longitude = 0, latidude = 0 },
				new OSMPositionNode(){id = 1,  longitude = 0, latidude = 1 },
				new OSMPositionNode(){id = 2,  longitude = 1, latidude = 0 },
				new OSMPositionNode(){id = 3,  longitude = 1, latidude = 1 },
				new OSMPositionNode(){id = 4,  longitude = 1, latidude = 2 },
				new OSMPositionNode(){id = 5,  longitude = 2, latidude = 0 },
				new OSMPositionNode(){id = 6,  longitude = 2, latidude = 1 },
				new OSMPositionNode(){id = 7,  longitude = 2, latidude = 2 },
				new OSMPositionNode(){id = 8,  longitude = 3, latidude = 0 },
				new OSMPositionNode(){id = 9,  longitude = 3, latidude = 1 },
				new OSMPositionNode(){id = 10, longitude = 4, latidude = 0 },
				new OSMPositionNode(){id = 11, longitude = 4, latidude = 1 },
				new OSMPositionNode(){id = 12, longitude = 4, latidude = 2 },
				new OSMPositionNode(){id = 13, longitude = 5, latidude = 0 },
				new OSMPositionNode(){id = 14, longitude = 5, latidude = 1 }
			};*/
					
			OSMPositionNode anchorNode = new OSMPositionNode ();

			// Find the bottom most point
			double ymin = listOfNodes [0].latidude;
			int min = 0;
			for (int i = 1; i < listOfNodes.Count; i++) {
				double y = listOfNodes [i].latidude;

				// Pick the bottom-most or chose the left
				// most point in case of tie
				if ((y < ymin) || (ymin == y &&
					listOfNodes [i].longitude < listOfNodes [min].longitude)) {
					ymin = listOfNodes [i].latidude;
					min = i;
				}
			}
			Swap (listOfNodes, min);

			anchorNode = listOfNodes [0];

			// Create a list of nodes with their polar angle relative to the anchor node
			foreach (OSMPositionNode o in listOfNodes) {
				PolarAngle currentPolarAngle = new PolarAngle ();

				double angle = PolarAngle (o, anchorNode) * 180f / Math.PI;

				currentPolarAngle.angle = angle;
				currentPolarAngle.node = o;

				listOfAngles.Add (currentPolarAngle);                                       
			}

			// get the duplicate angles
			var duplicates = listOfAngles.GroupBy (i => new { i.angle })
				.Where (g => g.Count () > 1)
				.Select (g => g.Key);

			List<PolarAngle> tempAngleList = new List<PolarAngle> ();
			PolarAngle tempPolarAngle = new PolarAngle ();


			/*NOTE: I didn't have time to test it. But this might be used to skip the prev,cur,next method below
			because this returns the outer most nodes only! */
			// iterate through each duplicate angle

			foreach (var v in duplicates) {
				// add the duplicate angles with their nodes to a temporary list
				foreach (PolarAngle currentAngle in listOfAngles) {
					if (v.angle == currentAngle.angle) {
						if (currentAngle.node.id != anchorNode.id) {
							tempAngleList.Add (currentAngle);
						}
					}
				}

				// get the polar angle with the highest y coordinate
				tempPolarAngle = tempAngleList.OrderByDescending (node => node.node.latidude).First ();

				// iterate through the temporary angle list that hold the duplicate angles
				foreach (PolarAngle n in tempAngleList) {
					// check if it exists in the main node list
					bool node_exists = listOfNodes.Exists (s => s.id == n.node.id);

					// if it does exist
					if (node_exists) {
						//make sure it is not the polar angle with the highest y coordinate
						// if it is not then remove the lower y coordinate duplicate nodes from the main list
						if (n.node.id != tempPolarAngle.node.id) {
							listOfNodes.Remove (n.node);	
						}
					}
				}

				// clear the temp list if there are more duplicate angles to check.
				tempList.Clear ();
			}

			//order the nodes by the polar angle
			List<OSMPositionNode> newListOfNodes = listOfNodes.OrderBy (x => PolarAngle (x, anchorNode)).ToList ();

			// Here is where the perimeter is made by determining if the current angle being checked is counter clockwise 
			//https://www.geeksforgeeks.org/convex-hull-set-2-graham-scan/

			/*The output from this example is 
			(0, 3)
			(4, 4)
			(3, 1)
			(0, 0) */

			/* The output generated here is only (0,0) (3,1) and (4,4) */
			/*if you look at the geeksforgeek diagram where they show how the prev,cur,next checking is implemented
			I might be checking them wrong? not sure. I just add the anchor node to the beginning and the greatest y coord with the least x coord node to the end */
			OSMPositionNode prev = new OSMPositionNode ();
			OSMPositionNode cur = new OSMPositionNode ();
			OSMPositionNode next = new OSMPositionNode ();

			// add the anchor node to the list of needed cooridnates for graham scan
			// Add the first three points to the GrahamScan array
			listOfGrahamScanPoints.Add (newListOfNodes [0]);
			listOfGrahamScanPoints.Add (newListOfNodes [1]);
			listOfGrahamScanPoints.Add (newListOfNodes [2]);
			numberOfPoints = 2;

			// iterate through each node checking if the current node is counter clockwise
				for (int i = 3; i < newListOfNodes.Count; i++) {
				//Get the last item in the list
				cur = listOfGrahamScanPoints [numberOfPoints];
				//Get the next item in the list of stored points
				next = newListOfNodes [i];
				//Get the previous point in the list
				prev = listOfGrahamScanPoints [numberOfPoints - 1];

				// check if the current node being checked is counter clockwise
				// reject the node if it is clockwise.
				//Order is current, previous, next
				//A number less then 0 is a right turn, left is greater then 0
				while (direction (listOfGrahamScanPoints [numberOfPoints], listOfGrahamScanPoints [numberOfPoints - 1], newListOfNodes [i]) >= 0) {
					listOfGrahamScanPoints.RemoveAt (numberOfPoints);//remove top from list
					numberOfPoints--;//numberin list is less now
				}
				//Add the next point to list if right turn
				listOfGrahamScanPoints.Add (next);
				numberOfPoints++;//array is now bigger
			}

			//Draw the rectangle
			Vector3[] Verticies = new Vector3[4];

			if (listOfGrahamScanPoints.Count != 4) {
				List<OSMPositionNode> SortedPoints = GetProperVerticies (listOfGrahamScanPoints);
				Verticies = CreateVerticiesDiff (SortedPoints);
			} else {
				Verticies = CreateVerticies (listOfGrahamScanPoints);
			}

			Vector3 position = GetPosition (Verticies);

			Quaternion rot = new Quaternion (0, 0, 0, 0);

			GameObject building = Instantiate (BaseLot,position,rot,World.transform);

			building.name = "Lot " + BuildingNum;
			BuildingNum++;

			GameObject tile = building.transform.GetChild(0).gameObject;
			GameObject ground = tile.transform.GetChild(0).gameObject;
			GameObject Hitbox = tile.transform.GetChild(1).gameObject;




				ChangeShape script = (ChangeShape)ground.GetComponent(typeof(ChangeShape));

				script.UpdateMesh(Verticies);


				Vector3 size;
				//calculate the size
				size.z = (float)((Math.Abs ((decimal)(Verticies[3].z - Verticies[0].z )))/4);
				size.x = (float)((Math.Abs ((decimal)(Verticies[1].x  - Verticies[2].x )))/4);

				size.y = 10;
				Hitbox.GetComponent<BoxCollider>().size = size;

				//building.transform.localPosition = position;
				ground.transform.localPosition = -position;

				Hitbox.transform.localEulerAngles = GetRotation(Verticies [0], Verticies [1],position);

		}
		/*List<OSMPositionNode> CaliperPoints = caliper.FindCalipers (listOfGrahamScanPoints);

			foreach (OSMPositionNode o in CaliperPoints)
			{
				count++;
				GameObject tmp = Instantiate (PointPlaceholder4);
				tmp.name = "CaliperPoints " + count;
				tmp.gameObject.transform.localPosition = new Vector3 ((float)o.longitude, 1f, (float)o.latidude);
			}
			count = 0;*/


	}

		//This function should find the rotation for the object
		static public Vector3 GetRotation(Vector3 point1, Vector3 point2,Vector3 center)
		{
			Vector3 rotation = new Vector3 ();

			float Rad2Deg = (float)(180.0 / Math.PI);

			//float angle2 = (float)(Math.Atan2(point1.y - point2.y, point2.x - point1.x) * Rad2Deg);

			float v1x = point1.x - center.x;
			float v1y = point1.z - center.z;
			float v2x = point2.x - center.x;
			float v2y = point2.z - center.z;

			float angle = (float)(Math.Atan2(v1x, v1y) - Math.Atan2(v2x, v2y));

			angle = angle * Rad2Deg;

			//float xDiff = point2.x - point1.x;
			//float yDiff = point2.z - point1.z;
			//float rotate = (float)(Math.Atan2 (yDiff, xDiff) * 180.0f / Math.PI);

			rotation = new Vector3 (0,angle , 0);
			return rotation;
		}

		//This function should find the center to place a grid tile
		static public Vector3 GetPosition(Vector3[] points)
		{
			Vector3 center = new Vector3 ();

			center.x = (points[0].x + points[1].x + points[2].x + points[3].x) / 4;

			center.z = (points[0].z + points[1].z + points[2].z + points[3].z) / 4;

			center.y = (0);

			return center;
		}

	static public List<OSMPositionNode> GetProperVerticies(List<OSMPositionNode> sortedList)
	{

		var SortedList = sortedList.OrderBy(p => p.longitude)
			.ThenBy(p => p.latidude)
			.ToList();
		List<OSMPositionNode> myList = new List<OSMPositionNode> ();
		OSMPositionNode lowestx_greatesty = new OSMPositionNode();
		OSMPositionNode lowestX_lowestY = new OSMPositionNode();
		OSMPositionNode greatestX_lowestY = new OSMPositionNode();
		OSMPositionNode greatestX_greatestY = new OSMPositionNode();

		lowestx_greatesty = LowestXGreatestY(SortedList);

		lowestX_lowestY = LowestXLowestY(SortedList);

		greatestX_lowestY = GreatestXLowestY(SortedList);

		greatestX_greatestY = GreatestXGreatestY(SortedList);

		myList.Add (lowestx_greatesty);
		myList.Add (greatestX_greatestY);
		myList.Add (greatestX_lowestY);
		myList.Add (lowestX_lowestY);

		return myList;
	}

	static public OSMPositionNode LowestXGreatestY(List<OSMPositionNode> sortedList)
	{
		OSMPositionNode currentNode = new OSMPositionNode();
		OSMPositionNode nextNode = new OSMPositionNode();
		OSMPositionNode returnNode = new OSMPositionNode();

		// start from least to greatest

		for (int x = 1; x < sortedList.Count; x++)
		{
			currentNode = sortedList[x - 1];

			nextNode = sortedList[x];

			if (!currentNode.Equals(nextNode))
			{
				if (currentNode.longitude <= nextNode.longitude && currentNode.latidude >= nextNode.latidude)
				{
					returnNode = currentNode;
					break;
				}

			}
		}

		return returnNode;
	}

	static public OSMPositionNode LowestXLowestY(List<OSMPositionNode> sortedList)
	{
		OSMPositionNode currentNode = new OSMPositionNode();
		OSMPositionNode nextNode = new OSMPositionNode();
		OSMPositionNode returnNode = new OSMPositionNode();

		// start from least to greatest
		for (int x = 1; x < sortedList.Count; x++)
		{
			currentNode = sortedList[x - 1];

			nextNode = sortedList[x];

			if (!currentNode.Equals(nextNode))
			{
				if (currentNode.longitude <= nextNode.longitude && currentNode.latidude <= nextNode.latidude)
				{
					returnNode = currentNode;
					break;
				}

			}
		}



		return returnNode;
	}
	static public OSMPositionNode GreatestXLowestY(List<OSMPositionNode> sortedList)
	{
		OSMPositionNode currentNode = new OSMPositionNode();
		OSMPositionNode nextNode = new OSMPositionNode();
		OSMPositionNode returnNode = new OSMPositionNode();

		// Start greatest to least
		for (int b = sortedList.Count - 1; b >= 0; b--)
		{
			currentNode = sortedList[b];
			nextNode = sortedList[b - 1];

			if (!currentNode.Equals(nextNode))
			{
				// Greatest X Lowest Y
				if (currentNode.longitude > nextNode.longitude && currentNode.latidude < nextNode.latidude)
				{
					returnNode = currentNode;
					break;
				}
			}
		}
		return returnNode;
	}


	static public OSMPositionNode GreatestXGreatestY(List<OSMPositionNode> sortedList)
	{
		OSMPositionNode currentNode = new OSMPositionNode();
		OSMPositionNode nextNode = new OSMPositionNode();
		OSMPositionNode returnNode = new OSMPositionNode();

		// Start greatest to least
		for (int b = sortedList.Count - 1; b >= 0; b--)
		{
			currentNode = sortedList[b];

			nextNode = sortedList[b - 1];

			if (!currentNode.Equals(nextNode))
			{
				// Greatest X Greatest Y
				if (currentNode.longitude > nextNode.longitude && currentNode.latidude > nextNode.latidude)
				{
					returnNode = currentNode;
					break;
				}
			}
		}
		return returnNode;
	}





	//http://www2.lawrence.edu/fast/GREGGJ/CMSC210/convex/convex.html
	// Determine the turn direction around the corner
	// formed by the points a, b, and c. Return a 
	// positive number for a left turn and negative
	// for a right turn.
	static double direction(OSMPositionNode a, OSMPositionNode b, OSMPositionNode c)
	{
		return (b.longitude - a.longitude) * (c.latidude - a.latidude) - (c.longitude - a.longitude) * (b.latidude - a.latidude);
	}

	static Vector3[] CreateVerticies(List<OSMPositionNode> ListOfNodes)
	{
		Vector3[] vertices = new Vector3[4];

		vertices[0] = new Vector3(ListOfNodes[0].longitude, 0, ListOfNodes[0].latidude);
		vertices[1] = new Vector3(ListOfNodes[1].longitude, 0, ListOfNodes[1].latidude);
		vertices[2] = new Vector3(ListOfNodes[3].longitude, 0, ListOfNodes[3].latidude);
		vertices[3] = new Vector3(ListOfNodes[2].longitude, 0, ListOfNodes[2].latidude);

		return vertices;
	}

	static Vector3[] CreateVerticiesDiff(List<OSMPositionNode> ListOfNodes)
	{
		Vector3[] vertices = new Vector3[4];

		vertices[0] = new Vector3(ListOfNodes[3].longitude, 0, ListOfNodes[3].latidude);
		vertices[1] = new Vector3(ListOfNodes[2].longitude, 0, ListOfNodes[2].latidude);
		vertices[2] = new Vector3(ListOfNodes[0].longitude, 0, ListOfNodes[0].latidude);
		vertices[3] = new Vector3(ListOfNodes[1].longitude, 0, ListOfNodes[1].latidude);

		return vertices;
	}

	// Compute the polar angle in radians formed
	// by the line segment that runs from p0 to p
	static double PolarAngle(OSMPositionNode p, OSMPositionNode p0)
	{
		return Math.Atan2(p.latidude - p0.latidude, p.longitude - p0.longitude);
	}


	void Swap(List<OSMPositionNode> listOfNodes, int min)
	{
		OSMPositionNode tempNode = listOfNodes [0];

		listOfNodes [0] = listOfNodes[min];

		listOfNodes[min] = tempNode;

	}

	static bool Compare(OSMPositionNode previous, OSMPositionNode current, OSMPositionNode next)
	{

		bool counterClockwise = false;

		double angle1 = Math.Atan2(previous.latidude - current.latidude, previous.longitude - current.longitude);
		double angle2 = Math.Atan2(next.latidude - current.latidude, next.longitude - current.longitude);

		//For counter-clockwise, just reverse the signs of the return values
		if (angle1 < angle2)
		{
			counterClockwise = false;
		}
		else if (angle2 < angle1)
		{
			counterClockwise = true;

		}

		return counterClockwise;
	}
}

	public class OSMPositionNode
	{
		public long id;
		public float longitude { get; set; }
		public float latidude { get; set; }

	}

public class PolarAngle
{
	public double angle { get; set;}
	public OSMPositionNode node { get; set; }
}
}
