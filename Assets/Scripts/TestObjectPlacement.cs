/*  
*  FILE          : TestObjectPlacement.cs TODO: Rename to OSMParser 
*  PROJECT       : ParisVR
*  PROGRAMMER    : Dustin Brown, Ronnie Skowron, Anthony Bastos
*  FIRST VERSION : 2018-02-08
*  DESCRIPTION   : 
*    Parses map information from an exported OSM map file
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text.RegularExpressions;
using System;
using System.Linq;

namespace Importer
{
	/*
	*   NAME    : TestObjectPlacement TODO: Rename to OSMParser 
	*   PURPOSE : Reads and parsers data from exported OSM data
	*/
	public class TestObjectPlacement : MonoBehaviour
	{
		public string mapFilePath;
		//filepath of the osm file
		List<List<OSMPositionNode>> ListOfRoads = new List<List<OSMPositionNode>> ();
		//node lists
		List<List<OSMPositionNode>> ListOfBuildings = new List<List<OSMPositionNode>> ();
		List<List<OSMPositionNode>> ListOfWaterways = new List<List<OSMPositionNode>> ();
		//holds map data
		OSMMap map = new OSMMap ();
		//whether we have read the data
		bool fileRead = false;

		/* 
		*  METHOD        : ReadFile 
		* 
		*  DESCRIPTION   : Reads the OSM map data and stores it in map
		*/
		void ReadFile ()
		{
			//if the filename wasn't specified in the editor, default to this
			if (!fileRead) {

				if (mapFilePath == null) {
					mapFilePath = @"/Users/student/Documents/OSM/map7.osm.xml";
				}

				OSMWay currentWay = new OSMWay (); //holds a way until it's done reading
				OSMRelation currentRelation = new OSMRelation (); //holds a relation until it's done reading
				bool openWay = false; //whether parser is currently reading a way
				bool openRelation = false; //whether parser is currently reading a relation
				string line; //current line of the read
				StreamReader sr = new StreamReader (mapFilePath); //read file
				line = sr.ReadLine ();

				while (line != null) {
					Match m;

					//if line contains a single line node definition
					//TODO: Read nodes that are defined by multiple lines 
					if (line.Contains ("<node ")) { //if node
						OSMPositionNode pn = new OSMPositionNode ();
						m = Regex.Match (line, @"(?<=id=.)[\w]+"); //parse data
						pn.id = Int64.Parse (m.Value);
						m = Regex.Match (line, @"(?<=lat=.)[\w.-]+");
						pn.latidude = float.Parse (m.Value);
						m = Regex.Match (line, @"(?<=lon=.)[\w.-]+");
						pn.longitude = float.Parse (m.Value);
						map.posNodes.Add (pn);
					}
					//dimensions of the map
					else if (line.Contains ("<bounds")) { 
						m = Regex.Match (line, @"(?<=minlat=.)[\w.-]+"); //parse data
						map.minLat = float.Parse (m.Value);
						m = Regex.Match (line, @"(?<=maxlat=.)[\w.-]+");
						map.maxLat = float.Parse (m.Value);
						m = Regex.Match (line, @"(?<=minlon=.)[\w.-]+");
						map.minLon = float.Parse (m.Value);
						m = Regex.Match (line, @"(?<=maxlon=.)[\w.-]+");
						map.maxLon = float.Parse (m.Value);

					} 
					//opening a way element
					else if (line.Contains ("<way ")) {
						openWay = true; 
						m = Regex.Match (line, @"(?<=id=.)[\w]+");
						currentWay.id = Int64.Parse (m.Value);
					} 
					//closing and saving a way element
					else if (line.Contains ("/way>")) {
						openWay = false;
						map.ways.Add (currentWay);
						currentWay = new OSMWay ();
					}
					//opening a relation element
					else if (line.Contains ("<relation ")) {
						openRelation = true;
						m = Regex.Match (line, @"(?<=id=.)[\w]+");
						currentRelation.id = Int64.Parse (m.Value);
					}
					//closing a relation element
					else if (line.Contains ("/relation>")) {
						openRelation = false;
						map.relations.Add (currentRelation);
						currentRelation = new OSMRelation ();
					}
					//if we're inside a way element
					else if (openWay) {
						//add node reference
						if (line.Contains ("<nd ")) {
							m = Regex.Match (line, @"(?<=ref=.)[\w]+");
							currentWay.nodeReferences.Add (Int64.Parse (m.Value));
						}
						//add tag 
						else if (line.Contains ("<tag ")) {
							OSMTag tag = new OSMTag (); //parse tags
							m = Regex.Match (line, @"(?<=k=.)[\w]+");
							tag.key = m.Value;
							m = Regex.Match (line, @"(?<=v=.)[\w]+");
							tag.value = m.Value;
							currentWay.tags.Add (tag);
						}
					} 
					//if we're inside a relation element
					else if (openRelation) {
						//read tag
						if (line.Contains ("<tag ")) {
							OSMTag tag = new OSMTag ();
							m = Regex.Match (line, @"(?<=k=.)[\w]+");
							tag.key = m.Value;
							m = Regex.Match (line, @"(?<=v=.)[\w]+");
							tag.value = m.Value;
							currentRelation.tags.Add (tag);
						}
						//read member
						else if (line.Contains ("<member ")) {
							OSMMember member = new OSMMember ();
							m = Regex.Match (line, @"(?<=type=.)[\w]+");
							member.type = m.Value;
							m = Regex.Match (line, @"(?<=ref=.)[\w]+");
							member.reference = Int64.Parse (m.Value);
							m = Regex.Match (line, @"(?<=role=.)[\w]+");
							member.role = m.Value;
							currentRelation.members.Add (member);
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
			}
			fileRead = true;
		}

		/* 
		*  METHOD        : ImportRoads 
		* 
		*  DESCRIPTION   : Gathers the road infomration
		* 
		*  RETURNS       : List<List<OSMNode>> : A list of ways containing roads
		*/
		public List<List<OSMPositionNode>> ImportRoads ()
		{
			//load data if haven't already
			if (!fileRead) {
				ReadFile ();
			}

			//iterate through all the ways
			foreach (OSMWay way in map.ways) {
				bool road = false;
				//check if the way has tags identifying it as a road
				foreach (OSMTag t in way.tags) {
					if (t.key == "highway" && (t.value == "secondary" || t.value == "residential")) {
						road = true;
						break;
					} 
				}

				//if way is a road
				if (road) {
					string roadName = "";
					//get road name
					foreach (OSMTag t in way.tags) {
						if (t.key == "name" && (t.value != "")) {
							roadName = t.value;
							break;
						} 
					}

					List<OSMPositionNode> listOfNodes = new List<OSMPositionNode> ();
					foreach (long p in way.nodeReferences) {
						OSMPositionNode node = getNodeByID (p); //get all the nodes in the way
						node.name = roadName;
						if (node.latidude != 0 && node.longitude != 0) { //ignore the node if it is zeroed
							listOfNodes.Add (node);
						} else {

						}
					}
					ListOfRoads.Add (listOfNodes); //add the road

				}
			}
			return ListOfRoads;
		}



		/* 
		*  METHOD        : ImportNodes 		//TODO:Rename to something more accurate, perhaps ImmportBuildings
		* 
		*  DESCRIPTION   : This function calculates tax on a retail purchase in Ontario. 
		* 
		*  RETURNS       : List<List<OSMNode>> : A list of ways containning buildings
		*/
		public List<List<OSMPositionNode>> ImportNodes ()
		{
			//read file if haven't already
			if (!fileRead) {
				ReadFile ();
			}

			//iterate through all the relations
			foreach (OSMRelation relation in map.relations) {
				bool building = false;
				foreach (OSMTag t in relation.tags) { //if a relation is a building then flag it
					if (t.key == "building") {
						building = true;
						break;
					}

				}

				if (building) { //load the building
					OSMMember member = new OSMMember ();
					bool outer = false;
					foreach (OSMMember m in relation.members) {
						if (m.role == "outer") { //take the outer deminsions of the building 
							outer = true;
						}
						if (outer) {
							OSMWay way = getWayByID (m.reference);
							List<OSMPositionNode> listOfNodes = new List<OSMPositionNode> ();
							int numberOfPoints = 0;
							foreach (long p in way.nodeReferences) {
								OSMPositionNode node = getNodeByID (p);
								listOfNodes.Add (node);
								numberOfPoints++;
							}
							//TODO: Why do we remove the last node? Investigate.
							listOfNodes.RemoveAt (numberOfPoints - 1);
							ListOfBuildings.Add (listOfNodes);
						}
					}

				}
			}
			//iterate through ways for buildings
			foreach (OSMWay way in map.ways) { 
				bool building = false;
				foreach (OSMTag t in way.tags) {
					if (t.key == "building") { // if it's a building, flag it
						building = true;
						break;
					}
				}
				if (building) {
					List<OSMPositionNode> listOfNodes = new List<OSMPositionNode> ();
					int numberOfPoints = 0;
					foreach (long p in way.nodeReferences) {
						OSMPositionNode node = getNodeByID (p); //get the nodes of the way
						listOfNodes.Add (node);
						numberOfPoints++;
					}
					//TODO: Why do we remove the last node? Investigate.
					listOfNodes.RemoveAt (numberOfPoints - 1);
					ListOfBuildings.Add (listOfNodes); //add the way
				}

			}

			return ListOfBuildings;
		}

		/* 
		*  METHOD        : ImportWaterways 
		* 
		*  DESCRIPTION   : Imports riverbanks
		* 
		*  RETURNS       : List<List<OSMNode>> : A list of Ways containing riverbanks
		*/
		public List<List<OSMPositionNode>> ImportWaterways ()
		{
			//if file hasn't been read yet
			if (!fileRead) {
				ReadFile ();
			}

			GameObject parentTerrain = new GameObject("TerrainParent");
			Terrain terrain = new Terrain();
			TerrainData _terrainData = new TerrainData();
			_terrainData.size = new Vector3((map.maxLon - map.minLon), 0.0f, (map.maxLat - map.minLat));
			//_terrainData.size = new Vector3((1000000) * 10, 0.0f, (1000000) * 10);
			GameObject _Terrain = Terrain.CreateTerrainGameObject(_terrainData);
			_Terrain.transform.parent = parentTerrain.transform;
			parentTerrain.transform.localPosition = new Vector3(0.0f, -10.0f, 0.0f);
			parentTerrain.transform.position = new Vector3((map.maxLat * 10f)/2, -1.0f, (map.maxLon * 10f)/2f);
			Vector3 TS = _Terrain.GetComponent<Terrain>().terrainData.size;
			_Terrain.transform.position = new Vector3((-TS.x / 2), -0.01f, (-TS.z / 2));

			SplatPrototype[] terrainTexture = new SplatPrototype[1];
			terrainTexture[0] = new SplatPrototype();
			terrainTexture[0].texture = (Texture2D)Resources.Load("Hand_Painted_Grass");
			_terrainData.splatPrototypes = terrainTexture;

			foreach (OSMWay way in map.ways)
			{
				bool waterway = false;
				//check if the way has any relevant tags
				foreach (OSMTag t in way.tags)
				{
					if (t.key == "waterway" && (t.value == "river"))
					{
						waterway = true;
						break;
					}
				}
				//if way was a building
				if (waterway)
				{
					List<OSMPositionNode> listOfNodes = new List<OSMPositionNode>();
					//find the most northern, southern, eastern, western nodes to draw the box
					//(replace with better algo later)
					//DO CALCULATIONS HERE
					int numberofPoints = 0;
					//Collect all the nodes
					foreach (long p in way.nodeReferences)
					{
						OSMPositionNode node = getNodeByID(p);
						listOfNodes.Add(node);
						numberofPoints++;
					}
					//listOfNodes.RemoveAt (numberofPoints -1);
					ListOfWaterways.Add(listOfNodes);
					//listOfNodes.Clear ();
					//Gets the convex points
					//List<OSMPositionNode> convexPoints = calculator.ConvexHaul(listOfNodes);
				}
			}
			return ListOfWaterways;
		
		}


		/* 
		*  METHOD        : getNodeByID 
		* 
		*  DESCRIPTION   : Finds the node with a given ID 
		* 
		*  PARAMETERS    : long id : the id of the node to be returned 
		* 
		*  RETURNS       : OSMNode : The node with the provided ID
		*/
		OSMPositionNode getNodeByID (long id)
		{
			OSMPositionNode ret = new OSMPositionNode ();
			ret = map.posNodes.Where (OSMPositionNode => OSMPositionNode.id == id).First ();
			return ret;
		}

		/* 
		*  METHOD        : getWayByID 
		* 
		*  DESCRIPTION   : Finds the way with a given ID 
		* 
		*  PARAMETERS    : long id : the id of the way to be returned 
		* 
		*  RETURNS       : OSMNode : The way with the provided ID
		*/
		OSMWay getWayByID (long id)
		{
			OSMWay ret = new OSMWay ();
			ret = map.ways.Where (OSMWay => OSMWay.id == id).FirstOrDefault ();
			return ret;
		}
	}

	/*
	*   NAME    : OSMMap 
	*   PURPOSE : Holds all OSM map data required for ParisVR. 
	*/
	class OSMMap
	{
		public float minLat;
		public float minLon;
		public float maxLat;
		public float maxLon;
		public List<OSMPositionNode> posNodes = new List<OSMPositionNode> ();
		public List<OSMWay> ways = new List<OSMWay> ();
		public List<OSMRelation> relations = new List<OSMRelation> ();
	}

	/*
	*   NAME    : OSMTag 
	*   PURPOSE : Represents an OSM Tag element
	*/
	class OSMTag
	{
		public string key;
		public string value;
	}

	/*
	*   NAME    : OSMWay 
	*   PURPOSE : Represents an OSM way element
	*/
	class OSMWay
	{
		public long id;
		public List<long> nodeReferences = new List<long> ();
		public List<OSMTag> tags = new List<OSMTag> ();
	}

	/*
	*   NAME    : OSMRelation 
	*   PURPOSE : Represents an OSM relation element
	*/
	class OSMRelation
	{
		public long id;
		public List<OSMMember> members = new List<OSMMember> ();
		public List<OSMTag> tags = new List<OSMTag> ();
	}

	/*
	*   NAME    : OSMMember 
	*   PURPOSE : Represents an OSM member element
	*/
	class OSMMember
	{
		public string type;
		public long reference;
		public string role;
	}

	/*
	*   NAME    : OSMPositionNode TODO: Rename to OSMNode 
	*   PURPOSE : Represents an OSM node element
	*/
	public class OSMPositionNode
	{
		public long id;

		public float longitude { get; set; }

		public float latidude { get; set; }

		public string name { get; set; }
	}
}