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

        b.createCirc(50, 200, 100, 100);
        b.createCirc(40, 100, 100, 150);
        b.createCirc(45, 150, 150, 100);
        b.createCirc(30, 80, 150, 150);
        
        b.createCubic(30, 20, 200, 50, 50);
        b.createCubic(20, 20, 200, 0, 50);
        b.createCubic(30, 40, 200, 50, 0);
        b.createCubic(30, 30, 200, 0, 0);
        b.createCubic(10, 30, 200, 100, 50);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
