/*
* FILE			: ObjectInfo.cs
* PROJECT		: ParisVR
* PROGRAMMERS	: Marco Fontana
* FIRST VERSION	: 1-18-2018
* DESCRIPTION   : This file contains the code and functionality required to contain information about a gameobject 
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
* Class Name	: ObjectInfo 
* DESCRIPTION   : This is a serializable class that will be used to contain the information of any gameobject
*                 in the game world. This class will allow the information pertaining to the object, like transform
*                 values to be accessed later when loading.
*/
[System.Serializable]
public class ObjectInfo 
{
    //Private Class Variables 
    private string name;
    private string area;
    private string tile;
	private string fileName;
    private TempVector position;
    private TempVector rotation;
    private TempVector scale;
    private string tag;

	//Name property, used to access the name variable
	public string Name
	{
		/*
		 *  Name		: get (name)
		 *	Description : Accessor for the name string
		 *	Parameters	: Nothing
		 *  Returns		: string name, the name of the object in the game world.
		*/
		get
		{
			return name; 
		}

		/*
		 *  Name		: set (name)
		 *	Description : Mutator for the name string
		 *	Parameters	: string value : the name of the object in the game world. 
		 *  Returns		: Nothing
		*/
		set
		{
			name = value;
		}
	}

	//FileName property, used to access the fileName variable
	public string FileName
	{
		/*
		 *  Name		: get (fileName)
		 *	Description : Accessor for the fileName string
		 *	Parameters	: Nothing
		 *  Returns		: string fileName, the fileName of the object in the game world.
		*/
		get
		{
			return fileName; 
		}

		/*
		 *  Name		: set (fileName)
		 *	Description : Mutator for the fileName string
		 *	Parameters	: string value : the fileName of the object in the game world. 
		 *  Returns		: Nothing
		*/
		set
		{
			fileName = value;
		}
	}

    //Area property, used to access the area variable
    public string Area
    {
        /*
		 *  Name		: get (area)
		 *	Description : Accessor for the area string
		 *	Parameters	: Nothing
		 *  Returns		: string area, the name of the area in the game world.
		*/
        get
        {
            return area;
        }

        /*
		 *  Name		: set (area)
		 *	Description : Mutator for the area string
		 *	Parameters	: string value : the name of the area in the game world. 
		 *  Returns		: Nothing
		*/
        set
        {
            area = value;
        }
    }

    //Tile property, used to access the tile variable
    public string Tile
    {
        /*
		 *  Name		: get (tile)
		 *	Description : Accessor for the tile string
		 *	Parameters	: Nothing
		 *  Returns		: string tile, the name of the tile in the game world.
		*/
        get
        {
            return tile;
        }

        /*
		 *  Name		: set (tile)
		 *	Description : Mutator for the tile string
		 *	Parameters	: string value : the name of the tile in the game world. 
		 *  Returns		: Nothing
		*/
        set
        {
            tile = value;
        }
    }


    //Position property, used to access the position variable
    public TempVector Position
	{
		/*
		 *  Name		: get (position)
		 *	Description : Accessor for the position vector3 value
		 *	Parameters	: Nothing
		 *  Returns		: Vector3 position, the xyz position of the object in the game world.
		*/
		get
		{
			return position; 
		}

		/*
		 *  Name		: set (position)
		 *	Description : Mutator for the position vector3 value
		 *	Parameters	: Vector3 value : the xyz position of the object in the game world. 
		 *  Returns		: Nothing
		*/
		set
		{
			position = value;
		}
	}

    //Rotation property, used to access the rotation variable
    [SerializeField]
    public TempVector Rotation
	{
		/*
		 *  Name		: get (rotation)
		 *	Description : Accessor for the rotation vector3 value
		 *	Parameters	: Nothing
		 *  Returns		: Vector3 position, the xyz rotation of the object in the game world.
		*/
		get
		{
			return rotation; 
		}

		/*
		 *  Name		: set (rotation)
		 *	Description : Mutator for the rotation vector3 value
		 *	Parameters	: Vector3 value : the xyz rotation of the object in the game world. 
		 *  Returns		: Nothing
		*/
		set
		{
			rotation = value;
		}
	}

    //Scale property, used to access the scale variable
    public TempVector Scale
	{
		/*
		 *  Name		: get (scale)
		 *	Description : Accessor for the scale vector3 value
		 *	Parameters	: Nothing
		 *  Returns		: Vector3 scale, the xyz scale of the object in the game world.
		*/
		get
		{
			return scale; 
		}

		/*
		 *  Name		: set (scale)
		 *	Description : Mutator for the rotation vector3 value
		 *	Parameters	: Vector3 value : the xyz scale of the object in the game world. 
		 *  Returns		: Nothing
		*/
		set
		{
			scale = value;
		}
	}

	//Tag property, used to access the tag variable
	public string Tag
	{
		/*
		 *  Name		: get (tag)
		 *	Description : Accessor for the tag string
		 *	Parameters	: Nothing
		 *  Returns		: string tag, the information tag of the object in the game world.
		*/
		get
		{
			return tag; 
		}

		/*
		 *  Name		: set (tag)
		 *	Description : Mutator for the tag string
		 *	Parameters	: string value : the information tag of the object in the game world. 
		 *  Returns		: Nothing
		*/
		set
		{
			tag = value;
		}
	}
		
}


/*
* Class Name	: SubObjectInfo 
* DESCRIPTION   : This is a serializable class that will be used to contain the information of any gameobject
*                 that is a child of anthoer object in the game world. This class will allow the information pertaining to the object, like transform
*                 values to be accessed later when loading.
*/
[System.Serializable]
public class SubObjectInfo : ObjectInfo
{
	//Private Variables
	private List<string> materials;


	public List<string> Materials
	{
		/*
		 *  Name		: get (materials)
		 *	Description : Accessor for the material list
		 *	Parameters	: Nothing
		 *  Returns		: List<string> materials, the materials list for a gameobject
		*/
		get
		{
			return materials; 
		}

		/*
		 *  Name		: set (materials)
		 *	Description : Mutator for the  material list
		 *	Parameters	: List<string> materials, the materials list for a gameobject
		 *  Returns		: Nothing
		*/
		set
		{
			materials = value;
		}
	}
}


/*
* Class Name	: Asset 
* DESCRIPTION   : This is a serializable class that will be used to contain the information of any gameobject
*                 in the game world. This class will allow the information pertaining to the object, like transform
*                 values to be accessed later when loading.
*/
[System.Serializable]
public class Asset
{
	//Private Variables
	private ObjectInfo parentInfo;
	private List<SubObjectInfo> childInfo;
	private string mapName;

	/*
	*   Name		: Asset()
    *	Description : Constructor for the asset class
    *	Parameters	: Nothing
    *   Returns		: Nothing
	*/
	public Asset()
	{
		parentInfo = new ObjectInfo ();
		childInfo = new List<SubObjectInfo> ();
	}
		
    //MapName Property, used to modify the mapName varaiable
	public string MapName
	{
		/*
		 *  Name		: get (mapInfo)
		 *	Description : Accessor for the mapName string
		 *	Parameters	: Nothing
		 *  Returns		: string mapName, the name of the map for the asset
		 */
		get 
		{
			return mapName;
		}
		/*
		 *  Name		: set (mapInfo)
		 *	Description : Mutator for the mapName string
		 *	Parameters	: value, the new string for mapName
		 *  Returns		: Nothing
		 */
		set
		{
			mapName = value;
		}
	}

	//ChildInfo Property, used to modify the Parentinfo list
	public ObjectInfo ParentInfo
	{
		/*
		 *  Name		: get (parentInfo)
		 *	Description : Accessor for the parentInfo Object
		 *	Parameters	: Nothing
		 *  Returns		: ObjectInfo parentInfo, the information about the parent object
		*/
		get
		{
			return parentInfo; 
		}

		/*
		 *  Name		: set (parentInfo)
		 *	Description : Mutator for the parentInfo Object
		 *	Parameters	: ObjectInfo parentInfo, the information about the parent object
		 *  Returns		: Nothing
		*/
		set
		{
			parentInfo = value;
		}
	}

	//ChildInfo Property, used to modify the childinfo list
	public List<SubObjectInfo> ChildInfo
	{
		/*
		 *  Name		: get (childInfo)
		 *	Description : Accessor for the childInfo Object
		 *	Parameters	: Nothing
		 *  Returns		: SubObjectInfo childInfo, the information about the child object(s)
		*/
		get
		{
			return childInfo; 
		}

		/*
		 *  Name		: set (childInfo)
		 *	Description : Mutator for the childInfo Object
		 *	Parameters	: SubObjectInfo childInfo, the information about the child object(s)
		 *  Returns		: Nothing
		*/
		set
		{
			childInfo = value;
		}
	}
}

/*
* Class Name	: TempVector
* DESCRIPTION   : This is a serializable class that will be used model a unity vector3 object but 
*                 with the ability to be serialized so that we can save this information into a file
*/
[System.Serializable]
public class TempVector
{
	//Private Variables for coordinates
    private float x;
    private float y;
    private float z;

	//X property, allows for the modification of the x value
    public float X
    {
		/*
		 *  Name		: get (x)
		 *	Description : Accessor for the x value
		 *	Parameters	: Nothing
		 *  Returns		: float x, the floating point value of x
		*/
        get
        {
            return x;
        }
		/*
		 *  Name		: set (x)
		 *	Description : Mutator for the x value
		 *	Parameters	: value, the new value of x
		 *  Returns		: Nothing
		*/
        set
        {
            x = value;
        }
    }

	//Y property, allows for the modification of the y value
    public float Y
    {
		/*
		 *  Name		: get (y)
		 *	Description : Accessor for the y value
		 *	Parameters	: Nothing
		 *  Returns		: float y, the floating point value of y
		*/
        get
        {
            return y;
        }
		/*
		 *  Name		: set (y)
		 *	Description : Mutator for the y value
		 *	Parameters	: value, the new value of y
		 *  Returns		: Nothing
		*/
        set
        {
            y = value;
        }
    }

	//Z property, allows for the modification of the z value
    public float Z
    {
		/*
		 *  Name		: get (x)
		 *	Description : Accessor for the x value
		 *	Parameters	: Nothing
		 *  Returns		: float x, the floating point value of x
		*/
        get
        {
            return z;
        }
		/*
		 *  Name		: set (z)
		 *	Description : Mutator for the z value
		 *	Parameters	: value, the new value of z
		 *  Returns		: Nothing
		*/
        set
        {
            z = value;
        }
    }

	/*
	*  Name		: TempVector()
	*	Description : Constructor for the tempvector class
	*	Parameters	: float nX,nY,nZ : new 3d coordinate values
    *  Returns		: Nothing
    */
    public TempVector(float nX, float nY, float nZ)
    {
		//initalize the class
        x = nX;
        y = nY;
        z = nZ;
    }
}