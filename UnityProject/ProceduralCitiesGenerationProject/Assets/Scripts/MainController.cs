using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainController : MonoBehaviour
{
    public bool generateBuildings;
    public bool generateHouses;
    public bool generatePathing;
    public bool connect;
    public bool test;

    public int citySize;
    public int cityBlockSize;

    public float heatmapRandomness;
    public float heatmapRandAmount;

    public float cityNodeRemovalRand;
    public float cityNodeDiagRand;
    public float cityNodeNewRand;
    private float cityNodeNumConnectors = 1;

    public int surbanSize;
    public int surbanBlockSize;
    public float CDSRand;

    public int maxSize;
    public int maxHeight;
    public int variance;
    public int buildingGap;
    public float buildingVariation;

    //public int smallSize;
    //public int richSize;
    //public int highDensSize;
    //public int utilSize;
    //public int maxHouse;
    public int maxHouseSize;

    public float smallPerc;
    public float richPerc;
    public float highDensPerc;
    public float utilPerc;
    public float schoolRatio;

    public GameObject controller;
    private CityNodeGenerator cng;
    private HeatmapGen hmg;
    private BuildingCreator bc;
    private SurbanNodeGenerator sng;
    private houseMap hm;
    private HouseCreator hc;
    private RoadUseMap rum;
    private csConnector csc;
    private audioController ac;
    private FloorResizer fs;
    private UnitTest ut;

    // Start is called before the first frame update
    void Start()
    {
        bc = controller.GetComponent<BuildingCreator>();
        cng = controller.GetComponent<CityNodeGenerator>();
        hmg = controller.GetComponent<HeatmapGen>();
        sng = controller.GetComponent<SurbanNodeGenerator>();
        hm = controller.GetComponent<houseMap>();
        hc = controller.GetComponent<HouseCreator>();
        rum = controller.GetComponent<RoadUseMap>();
        csc = controller.GetComponent<csConnector>();
        ac = controller.GetComponent<audioController>();
        fs = controller.GetComponent<FloorResizer>();
        ut = controller.GetComponent<UnitTest>();

        citySize = citySize - (citySize % cityBlockSize); //makes sure the block size is a divisor of the actual size. 
        citySize++;

        surbanSize = surbanSize - (surbanSize % surbanBlockSize);
        surbanSize++;
        
        //ensures that the variance isn't greater than the max width or height.
        //Variance should never get this high anyways.
        variance = variance > maxSize ? maxSize - 1 : variance;
        string output;

        if(generatePathing)
        {
            output = hmg.generateHeatmap(citySize, heatmapRandomness, heatmapRandAmount) ? "Heatmap successfully generated" : "Heatmap generation failed";
            Debug.Log(output);
            output = cng.generateNode(citySize, cityBlockSize, cityNodeRemovalRand, cityNodeDiagRand, cityNodeNumConnectors, cityNodeNewRand) ? "City Node Successfully Generated" : "City node generation failed";
            Debug.Log(output);
            rum.createUseMap();
        }
        if (generateBuildings)
        {
            output = bc.createBuildings(maxSize, maxHeight, buildingGap, variance, buildingVariation) ? "Buildings successfully generated. " : "Building generation failed at some point";
            Debug.Log(output);
            output = ac.placeCityAudio(citySize, maxSize) ? "City Audio placed" : "City audio failed to place";
            Debug.Log(output);
        }
        if (generateHouses)
        {
            output = sng.createSurbanNodemap(surbanSize, surbanBlockSize, CDSRand, cityNodeNumConnectors) ? "Suburban nodemap successfully generated" : "Suburban Node failed to generate";
            Debug.Log(output);
            output = hm.createHouseMap(smallPerc, richPerc, highDensPerc, utilPerc, schoolRatio) ? "Housemap successfully generated" : "Housemap failed to generate";
            Debug.Log(output);
            output = hc.createHouses(maxHouseSize) ? "Houses successfully created": "Houses failed to generate properly";
            Debug.Log(output);
            output = ac.placeSurbanAudio(surbanSize, maxHouseSize) ? "Audio placed" : "Audio failed to place";
            Debug.Log(output);
        }
        if(connect)
        {
            if(generateBuildings && generateHouses)
            {
                output = csc.connect(citySize, surbanSize, maxSize, maxHouseSize) ? "Connection successfully created" : "Connection failed to be created";
                Debug.Log(output);
                fs.resizeFloor(citySize, surbanSize, maxSize, maxHouseSize);
            }
            else
            {
                Debug.Log("ERROR: Cannot connect city and suburban when one isn't generated!!");
            }
        }
        if(test)
        {
            ut.test();
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
