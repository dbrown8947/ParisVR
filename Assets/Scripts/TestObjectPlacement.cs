	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;
	using System.IO;
	using System.Text.RegularExpressions;
	using System;
using GrahamScan;

//essentially a container for the gameobject
class OSMMap
{
	public double minLat;
	public double minLon;
	public double maxLat;
	public double maxLon;
	public List<OSMPositionNode> posNodes = new List<OSMPositionNode>();
	public List<OSMWay> ways = new List<OSMWay>();

}
	
//tags, match key with value
class OSMTag
{
	public string key;
	public string value;
}

//OSMWay definition, essentially an object, has a list of node ids + tags describing the object
class OSMWay
{
	public long id;
	public List<long> nodeReferences = new List<long>();
	public List<OSMTag> tags = new List<OSMTag>();
}



	public class TestObjectPlacement : MonoBehaviour {

	private int buildingNumber = 0;
	private int roadNumber = 0;
	List<List<OSMPositionNode>> ListOfBuildings = new List<List<OSMPositionNode>>();

	public GameObject Tile;

	public GameObject Road;


	OSMPositionNode findNode(long id, List<OSMPositionNode> n)
	{
		//function that fetches the node when supplied it's id
		OSMPositionNode ret = new OSMPositionNode();
		foreach (OSMPositionNode i in n)
		{
			if(i.id == id)
			{
				ret = i;
				break;
			}
		}
		return ret;
	}

		// Use this for initialization
	// Use this for initialization
	public List<List<OSMPositionNode>> ImportNodes () {

		//read osm 
		StreamReader sr = new StreamReader (@"C:\Users\Capstone\Documents\OSM\map.osm.xml");

		string line = sr.ReadLine ();
		OSMMap map = new OSMMap ();

		bool openElement = false;
		OSMWay currentWay = new OSMWay ();


		//parsing
		while (line != null) {
			Match m;

			if (line.Contains ("<node ") && line.Contains (("/>"))) {
				OSMPositionNode pn = new OSMPositionNode ();
				m = Regex.Match (line, @"(?<=id=.)[\w]+");
				pn.id = Int64.Parse (m.Value);
				m = Regex.Match (line, @"(?<=lat=.)[\w.-]+");
				pn.latidude = float.Parse (m.Value);
				m = Regex.Match (line, @"(?<=lon=.)[\w.-]+");
				pn.longitude = float.Parse (m.Value);
				map.posNodes.Add (pn);
			} else if (line.Contains ("<bounds")) {
				m = Regex.Match (line, @"(?<=minlat=.)[\w.-]+");
				map.minLat = float.Parse (m.Value);
				m = Regex.Match (line, @"(?<=maxlat=.)[\w.-]+");
				map.maxLat = float.Parse (m.Value);
				m = Regex.Match (line, @"(?<=minlon=.)[\w.-]+");
				map.minLon = float.Parse (m.Value);
				m = Regex.Match (line, @"(?<=maxlon=.)[\w.-]+");
				map.maxLon = float.Parse (m.Value);

			} else {
				if (line.Contains ("<way ")) {
					openElement = true;

					m = Regex.Match (line, @"(?<=id=.)[\w]+");
					currentWay.id = Int64.Parse (m.Value);

				} else if (line.Contains ("/way>")) {
					openElement = false;
					map.ways.Add (currentWay);
					currentWay = new OSMWay ();

				} else if (line.Contains ("<nd ") && openElement == true) {
					m = Regex.Match (line, @"(?<=ref=.)[\w]+");
					currentWay.nodeReferences.Add (Int64.Parse (m.Value));
				} else if (line.Contains ("<tag ") && openElement == true) {
					OSMTag tag = new OSMTag ();
					m = Regex.Match (line, @"(?<=k=.)[\w]+");
					tag.key = m.Value;
					m = Regex.Match (line, @"(?<=v=.)[\w]+");
					tag.value = m.Value;
					currentWay.tags.Add (tag);
				}
			}

			line = sr.ReadLine ();
		}


		//centre the map
		float latOffset = (float)(map.maxLat + map.minLat) / 2f;
		float lonOffset = (float)(map.maxLon + map.minLon) / 2f;

		//convert lat and lon to metres
		map.maxLat = (map.maxLat - latOffset) * 111000f;
		map.maxLon = (map.maxLon - lonOffset) * 111000f;
		map.minLon = (map.minLon - lonOffset) * 111000f;
		map.minLat = (map.minLat - latOffset) * 111000f;

		//apply offset to each node and convert to metres
		foreach (OSMPositionNode o in map.posNodes) {
			o.longitude = (o.longitude - lonOffset) * 111000f;
			o.latidude = (o.latidude - latOffset) * 111000f;
		}

		//TerrainData _terrainData = new TerrainData ();

		//set terrain width, height, length
		//_terrainData.size = new Vector3 (100000f, 0.1f, 100000f);

		//GameObject _terrain = Terrain.CreateTerrainGameObject (_terrainData);
		//_terrain.transform.position = new Vector3(-50000f, 0, -50000f);



		//reading ways
		foreach (OSMWay way in map.ways) {
			bool building = false;
			//check if the way has any relevant tags
			foreach (OSMTag t in way.tags) {
				if (t.key == "building" && t.value == "yes") {
					building = true;
					buildingNumber++;
					break;
				} 
			}

			//if way was a building
			if (building) {
				double? north = null;
				double? south = null;
				double? west = null; 
				double? east = null;

				List<OSMPositionNode> listOfNodes = new List<OSMPositionNode>();
				//find the most northern, southern, eastern, western nodes to draw the box
				//(replace with better algo later)
				//DO CALCULATIONS HERE

				int numberofPoints = 0;
				//Collect all the nodes
				foreach (long p in way.nodeReferences) {
					OSMPositionNode node = findNode (p, map.posNodes);
					listOfNodes.Add (node);
					numberofPoints++;

				}

				listOfNodes.RemoveAt (numberofPoints -1);
				ListOfBuildings.Add (listOfNodes);
				//listOfNodes.Clear ();
				//Gets the convex points
				//List<OSMPositionNode> convexPoints = calculator.ConvexHaul(listOfNodes);
			}
		}
		return ListOfBuildings;
	}


	#if false
	void OSMParser()
	{


	Console.WriteLine("Bounds:\nminLat:" + map.minLat + " maxLat:" + map.maxLat + " minLon:" + map.minLon + " maxLon:" + map.maxLon);
	foreach (OSMPositionNode o in map.posNodes)
	{
	Console.WriteLine("Node id:" + o.id + ", Long:" + o.longitude + ", Lat:" + o.latidude + "\n");
	}

	foreach (OSMWay w in map.ways)
	{
	Console.Write("\nWay id:" + w.id + "\nNodes:");
	foreach (long l in w.nodeReferences)
	{
	Console.Write("[" + l + "] ");
	}
	Console.Write("\nTags:");
	foreach (OSMTag t in w.tags)
	{
	Console.Write("[key: " + t.key + ", value: " + t.value + "] ");
	}
	}
	#endif
}
	