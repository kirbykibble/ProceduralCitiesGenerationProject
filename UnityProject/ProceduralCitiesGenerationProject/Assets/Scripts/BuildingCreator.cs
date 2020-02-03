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

        b.createCubic(40, 20, 200, 50, 50);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
