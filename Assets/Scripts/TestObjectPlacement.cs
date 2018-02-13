	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;
	using System.IO;
	using System.Text.RegularExpressions;
	using System;

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

//definition of a node
//longitude and latitude are converted into metres later
class OSMPositionNode
{
	public long id;
	public double longitude;
	public double latidude;
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

	public GameObject Tile;


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
		void Start () {

		//read osm 
		StreamReader sr = new StreamReader (@"/Users/student/Documents/OSM/map.osm.xml");
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
				pn.latidude = double.Parse (m.Value);
				m = Regex.Match (line, @"(?<=lon=.)[\w.-]+");
				pn.longitude = double.Parse (m.Value);
				map.posNodes.Add (pn);
			} else if (line.Contains ("<bounds")) {
				m = Regex.Match (line, @"(?<=minlat=.)[\w.-]+");
				map.minLat = double.Parse (m.Value);
				m = Regex.Match (line, @"(?<=maxlat=.)[\w.-]+");
				map.maxLat = double.Parse (m.Value);
				m = Regex.Match (line, @"(?<=minlon=.)[\w.-]+");
				map.minLon = double.Parse (m.Value);
				m = Regex.Match (line, @"(?<=maxlon=.)[\w.-]+");
				map.maxLon = double.Parse (m.Value);



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
		double latOffset = (map.maxLat + map.minLat) / 2;
		double lonOffset = (map.maxLon + map.minLon) / 2;

		//convert lat and lon to metres
		map.maxLat = (map.maxLat - latOffset) * 111000;
		map.maxLon = (map.maxLon - lonOffset) * 111000;
		map.minLon = (map.minLon - lonOffset) * 111000;
		map.minLat = (map.minLat - latOffset) * 111000;

		//apply offset to each node and convert to metres
		foreach (OSMPositionNode o in map.posNodes) {
			o.longitude = (o.longitude - lonOffset) * 111000;
			o.latidude = (o.latidude - latOffset) * 111000;
		}

		//TerrainData _terrainData = new TerrainData ();

		//set terrain width, height, length
		//_terrainData.size = new Vector3 (100000f, 0.1f, 100000f);

		//GameObject _terrain = Terrain.CreateTerrainGameObject (_terrainData);
		//_terrain.transform.position = new Vector3(-50000f, 0, -50000f);

	
		//reading ways
		foreach (OSMWay way in map.ways) {
			bool building = false;
			bool highway = false;
			//check if the way has any relevant tags
			foreach (OSMTag t in way.tags) {
				if (t.key == "building" && t.value == "yes") {
					building = true;
					break;
				} else if (t.key == "highway" && (t.value == "secondary" || t.value == "residential")) {
					highway = true;
					break;
				}
			}

			//if way was a building
			if (building) {
				double? north = null;
				double? south = null;
				double? west = null; 
				double? east = null;

				//find the most northern, southern, eastern, western nodes to draw the box
				//(replace with better algo later)
				foreach (long p in way.nodeReferences) {
					OSMPositionNode node = findNode (p, map.posNodes);

					if (north == null || node.longitude > north) {
						north = node.longitude;
					}

					if (node.longitude < south || south == null) {
						south = node.longitude;
					}

					if (node.latidude > east || east == null) {
						east = node.latidude;
					}
					if (node.latidude < west || west == null) {
						west = node.latidude;
					}

				}



				//create cube to reperesent the lot
				GameObject lot = Instantiate(Tile,this.gameObject.transform);

				GameObject tile = lot.transform.GetChild(0).gameObject;
				GameObject Hitbox = tile.transform.GetChild(0).gameObject;
				GameObject ground = Hitbox.transform.GetChild(1).gameObject;

				Vector3 position;
				Vector3 size;
				//calculate the size
				size.x = (float)(Math.Abs ((decimal)(north - south)));
				size.y = 0.3f;
				size.z = (float)(Math.Abs ((decimal)(east - west)));
				//calculate the position
				position.x = (float)(south + north) / 2;
				position.y = 5;
				position.z = (float)(east + west) / 2;

				//apply transformations
				//lot.transform.localScale = size;
				lot.transform.position = position;

				//lot.GetComponent<Renderer> ().material.color = Color.red;	

				//Alter the hit box on the object
				size.y = 10;
				Hitbox.GetComponent<BoxCollider>().size = size;

				//Update the visual representation of the ground
				position.x = 0;
				position.y = -5;
				position.z = 0;
				size.y = 0.3f;
				ground.gameObject.transform.localScale = size;
				ground.gameObject.transform.localPosition = position;
			
				//if it's a highway AKA any sort of road
			} else if (highway) {
				Vector3 position;
				position.y = 0;
				float rotation = 0;
				float length = 0;

				//iterate through pairs of nodes
				for (int i = 0; i < way.nodeReferences.Count -1; i++) {
					OSMPositionNode nodeA = findNode(way.nodeReferences[i],map.posNodes);
					OSMPositionNode nodeB = findNode (way.nodeReferences [i + 1], map.posNodes);

					//if nodes arent dead
					if (nodeA.latidude != 0 && nodeB.longitude != 0) {
						//average longitude
						position.x = (float)((nodeA.longitude + nodeB.longitude) / 2);
						//average latitude
						position.z = (float)((nodeA.latidude + nodeB.latidude) / 2);
						//apply rotation
						rotation = (float)(Math.Atan((nodeB.longitude - nodeA.longitude) / (nodeB.latidude - nodeA.latidude)) * 180/ Math.PI + 90);
						//scale to correct length
						length = (float)(Math.Sqrt (Math.Pow ((nodeB.latidude - nodeA.latidude), 2) + Math.Pow ((nodeB.longitude - nodeA.longitude), 2)+10));

						//create cube for the road
						GameObject road = GameObject.CreatePrimitive (PrimitiveType.Cube);
						//apply transformations
						road.transform.position = position;
						road.transform.Rotate (new Vector3 (0.0f, rotation, 0.0f));
						road.transform.localScale = new Vector3 (length, 0.1f, 5f);
						road.GetComponent<Renderer> ().material.color = Color.blue;
					}

				}

			
			}

		
		
		
		}

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

	


