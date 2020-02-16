using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HouseCreator : MonoBehaviour
{
    //currently a test script. will update later.
    public GameObject controller;
    private House h;

    // Start is called before the first frame update
    void Start()
    {
        h = controller.GetComponent<House>();
        h.createHouse("HOUSE_SMALL", 0, 0, 240);
        h.createHouse("HOUSE_SMALL", 20, 0, 45);
        h.createHouse("HOUSE_SMALL", 00, 20);
        h.createHouse("HOUSE_SMALL", 20, 20);

        h.createHouse("HOUSE_RICH", 60, 60);
        h.createHouse("HOUSE_RICH", 40, 60);
        h.createHouse("HOUSE_RICH", 60, 40);
        h.createHouse("HOUSE_RICH", 40, 40);

        h.createHouse("HOUSE_DENS", 100, 100);
        h.createHouse("HOUSE_DENS", 80, 100);
        h.createHouse("HOUSE_DENS", 100, 80);
        h.createHouse("HOUSE_DENS", 80, 80);

        h.createHouse("SCHOOL", 100, 100);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
