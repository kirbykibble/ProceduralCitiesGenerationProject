using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingCreator : MonoBehaviour
{
    //test function for building.cs
    public GameObject controller;

    private Building b;
    // Start is called before the first frame update
    void Start()
    {
        b = controller.GetComponent<Building>();

        b.createBuilding(20, 20, 100, 50, 50);
        b.createBuilding(30, 20, 120, 0, 50);
        b.createBuilding(20, 30, 140, 50, 0);
        b.createBuilding(40, 40, 150, 0, 0);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
