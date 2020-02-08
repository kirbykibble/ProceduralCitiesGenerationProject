using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainController : MonoBehaviour
{
    public int citySize;
    public int cityBlockSize;

    public float heatmapRandomness;
    public float heatmapRandAmount;

    public float cityNodeRemovalRand;
    public float cityNodeDiagRand;
    public float cityNodeNumConnectors;

    public int maxSize;
    public int maxHeight;
    public int variance;
    public int buildingGap;
    public float buildingVariation;

    public GameObject controller;
    private CityNodeGenerator cng;
    private HeatmapGen hmg;
    private BuildingCreator bc;

    // Start is called before the first frame update
    void Start()
    {
        bc = controller.GetComponent<BuildingCreator>();
        cng = controller.GetComponent<CityNodeGenerator>();
        hmg = controller.GetComponent<HeatmapGen>();
        
        citySize = citySize - (citySize % cityBlockSize); //makes sure the block size is a divisor of the actual size. 
        citySize++;
        
        //ensures that the variance isn't greater than the max width or height.
        //Variance should never get this high anyways.
        variance = variance > maxSize ? maxSize - 1 : variance;

        string output = hmg.generateHeatmap(citySize, heatmapRandomness, heatmapRandAmount) ? "Heatmap successfully generated" : "Heatmap generation failed";
        Debug.Log(output);
        output = cng.generateNode(citySize, cityBlockSize, cityNodeRemovalRand, cityNodeDiagRand, cityNodeNumConnectors) ? "City Node Successfully Generated" : "City node generation failed";
        Debug.Log(output);
        output = bc.createBuildings(maxSize, maxHeight, buildingGap, variance, buildingVariation) ? "Buildings successfully generated. " : "Building generation failed at some point";
        Debug.Log(output);

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
