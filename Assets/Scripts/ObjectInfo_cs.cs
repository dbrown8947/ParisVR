/*
* FILE			: ObjectInfo.cs
* PROJECT		: ParisVR Tool
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
	private Vector3 position;
	private Vector3 rotation;
	private Vector3 scale;
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

	//Position property, used to access the position variable
	public Vector3 Position
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
	public Vector3 Rotation
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
	public Vector3 Scale
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
