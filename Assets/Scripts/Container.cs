/*
*  FILE          : Container.cs
*  PROJECT       : ParisVR
*  PROGRAMMER    : Dustin Brown
*  FIRST VERSION : 2018-02-02
*  DESCRIPTION   :
*    File for the Container class which manages assets
*/

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace container {
/*
 * Class : Container
 * Description : Manages assets
 */
public class Container : MonoBehaviour {

	//container assets
	private List<Material> matList = new List<Material>();

	/*
	* Method      : ContainerLoad
	* Description : reads all resources from disk into the container
	*/
	public void ContainerLoad()
	{
			matList.Clear ();
		//load Materials
		Material[] mats = Resources.LoadAll("Materials", typeof(Material)).Cast<Material>().ToArray();
		foreach (Material m in mats) {
			addMaterial (m);
		}
	}

	/*
	* Method      : addMaterial
	* Description : adds a material to the container
	* Parameters  : Material m    : The Material to Add
	*/
	public void addMaterial(Material m)
	{
		bool matAlreadyExists = false;
		//check if Material already exists
		for (int i = 0; i < matList.Count; i++) {
			if (m.name == matList [i].name) {
				matAlreadyExists = true;
				break;
			}
		
		}

		//if it doesn't already exist, add it
		if (!matAlreadyExists) {
			
			matList.Add (m);
		}
	}

	/*
	* Method      : getMaterialNames
	* Description : gets all the names of the materials from the container
	* Returns     : String[]   : list of names
	*/
	public string[] getMaterialNames()
	{
		int matCount = matList.Count;
		string[] ret = new string[matCount];
		for (int i = 0; i < matCount; i++) {
			ret [i] = matList [i].name;
		}
		return ret;
	}

	/*
	* Method      : getMaterial
	* Description : gets the material from the container at the given index
	* Returns     : Material   : the requested Material
	*/
	public Material getMaterial(int index)
	{
		Material ret;
		if (index >= 0 && index <= matList.Count) { //if the material exists
			ret = matList [index];
		} else {
			ret = null;
		}
		return  ret; 
	}

}
}