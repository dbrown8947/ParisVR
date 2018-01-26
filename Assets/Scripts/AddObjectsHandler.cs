/*
* FILE			: AddObjectsHandler.cs
* PROJECT		: Final Game Dev
* PROGRAMMERS	: Anthony Bastos
* FIRST VERSION	: 01-11-2017
* DESCRIPTION   : This file contains the code and functionality required to handle the player adding a building
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;


public class AddObjectsHandler : MonoBehaviour {

    public GameObject[] buildings;
    public Dropdown listOfBuilding;
    //private string[] NamesOfBuildings;
    Dropdown.OptionData m_NewData;
    List<Dropdown.OptionData> m_Messages = new List<Dropdown.OptionData>();
    public GameObject screen;
    public GameObject OtherScreen;
    public GameObject plyr;


    public InputField posX;
    public InputField posY;
    public InputField posZ;

    private GameObject newBuilding;

    //FUNCTION      : Start()
    //DESCRIPTION   : This Method is used to handle the start up of the object. In this case,
    //                it is used to fill a combobox with information
    //PARAMETERS    : Nothing
    //RETURNS		: Nothing
    void Start () {
        //posX.text = "0";
        //posY.text = "0";
        //posZ.text = "0";

        //NamesOfBuildings = new string[buildings.Length];
        //listOfBuilding.ClearOptions();
        //for (int i = 0; i < buildings.Length; i++)
        //{
        //    m_NewData = new Dropdown.OptionData();
        //    NamesOfBuildings[i] = buildings[i].name;
        //    m_NewData.text = buildings[i].name;
        //    m_Messages.Add(m_NewData);
        //}

        //listOfBuilding.AddOptions(m_Messages);
    }

    //FUNCTION      : Update()
    //DESCRIPTION   : This Method is used to run methods on every update
    //PARAMETERS    : Nothing
    //RETURNS		: Nothing
    void Update () {
        HotKeys();
    }


    //FUNCTION      : AddBuilding()
    //DESCRIPTION   : This Method is used to create a building at teh given postion.
    //PARAMETERS    : Nothing
    //RETURNS		: Nothing
    public void AddBuilding()
    {


        float xValue = (float)Convert.ToDouble(posX.text);
        float yValue = (float)Convert.ToDouble(posY.text);
        float zValue = (float)Convert.ToDouble(posZ.text);
        
        int selectedItem = listOfBuilding.value;

        newBuilding = Instantiate(buildings[selectedItem]);

        newBuilding.transform.position = new Vector3(xValue, yValue, zValue);
        screen.SetActive(false);
        

    }

    //FUNCTION      : HotKeys()
    //DESCRIPTION   : This Method is responsible for activating hotkeys based on user input
    //PARAMETERS    : Nothing
    //RETURNS		: Nothing
    void HotKeys()
    {
        //Make sure other screen is not active
        if (!OtherScreen.activeSelf)
        {
            //If the user has pressed the Tab control button
            if (Input.GetKey(KeyCode.Tab))
            {
                screen.SetActive(true);
                plyr.GetComponent<PlayerController>().escCount = 0;
            }
            if (Input.GetKey(KeyCode.Escape))
            {
                screen.SetActive(false);
            }
        }
        
    }
}
