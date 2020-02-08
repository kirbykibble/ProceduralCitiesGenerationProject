using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class BuildingCreator : MonoBehaviour
{
    //test function for building.cs
    public GameObject controller;
    public GameObject floor;
    private Building b;

    private Color bg = Color.black;
    private Color node = Color.blue;
    private Color road = Color.yellow;
    private Color noConnection = Color.red;
    private Color diagonalRoad = Color.green;
    private Color outConnector = Color.magenta;

    // Start is called before the first frame update
    void Start()
    {
        /*
        //debug code.
        b.createCirc(50, 200, 100, 100); //radius, height, x pos, z pos, y pos = 0
        b.createCirc(40, 100, 100, 150);
        b.createCirc(45, 150, 150, 100);
        b.createCirc(30, 80, 150, 150);
        
        b.createCubic(30, 20, 200, 50, 50); //length, width, height, x pos, z pos, y pos = 0
        b.createCubic(20, 20, 200, 0, 50);
        b.createCubic(30, 40, 200, 50, 0);
        b.createCubic(30, 30, 200, 0, 0);
        b.createCubic(10, 30, 200, 100, 50);
        */
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool createBuildings(int maxSize, int maxHeight, int gap, int variance, float buildingVariation)
    {
        b = controller.GetComponent<Building>();
        Texture2D heatmap = null;
        Texture2D cNodeMap = null;
        byte[] fileData;
        float totalDens = 0.0f; // unused as of now. 
        float maxDens = 0.0f;
        float xOffset = 0.0f;
        float yOffset = 0.0f;
        float roadSize = 10.0f; //temporary variable. Will update later.



        if (File.Exists("Assets/Debug/HeatmapOut.png"))
        {
            fileData = File.ReadAllBytes("Assets/Debug/HeatmapOut.png");
            heatmap = new Texture2D(2, 2, TextureFormat.BGRA32, false);
            heatmap.LoadImage(fileData);

            Debug.Log("Successfully loaded heatmap");
        }
        else { Debug.Log(" ERROR: File not found"); }

        if (File.Exists("Assets/Debug/nodeMapOut.png"))
        {
            fileData = File.ReadAllBytes("Assets/Debug/nodeMapOut.png");
            cNodeMap = new Texture2D(2, 2, TextureFormat.BGRA32, false);
            cNodeMap.LoadImage(fileData);
            Debug.Log("Node successfully read");
        }
        else { Debug.Log(" ERROR: File not found"); }

        Vector3 floorPos = new Vector3((maxSize * heatmap.width) / 2, 1, (maxSize * heatmap.height) / 2);
        Vector3 floorSize = new Vector3((maxSize * heatmap.width) / 8, 1, (maxSize * heatmap.height) / 8);
        floor.transform.localScale = floorSize;
        floor.transform.position = floorPos;

        //determines the total density of the heatmap
        for (int x = 0; x < cNodeMap.width; x++)
        {
            for(int y = 0; y < cNodeMap.height; y++)
            {
                if(cNodeMap.GetPixel(x, y) == bg || cNodeMap.GetPixel(x,y) == noConnection) 
                {
                    float red = (float)heatmap.GetPixel(x, y).r;
                    totalDens += red;
                    maxDens = red > maxDens ? red : maxDens;
                }
            }
        }
        Debug.Log("Total Density: " + (float)totalDens);

        //Actual building generation
        for (int x = 0; x < cNodeMap.width; x++)
        {
            for (int y = 0; y < cNodeMap.height; y++)
            {
                Color curPixel = cNodeMap.GetPixel(x, y);
                if (curPixel == bg || curPixel == noConnection) //accounts for everything
                {
                    //compare the current pixel to the total density.
                    float buildingScale = heatmap.GetPixel(x, y).r;
                    buildingScale = (float) buildingScale / maxDens;

                    int width = Random.Range(maxSize - variance, maxSize);
                    int length = Random.Range(maxSize - variance, maxSize);
                    float height = (float) maxHeight * buildingScale * 2;

                    float xMax = (float)maxSize - (width / 2);
                    float yMax = (float)maxSize - (length / 2);
                    float xMin = (float)width / 2;
                    float yMin = (float)length / 2;

                    float xPos = xOffset + Random.Range(xMin, xMax);
                    float yPos = yOffset + Random.Range(yMin, yMax);

                    if(height != 0)
                    {
                        if (Random.value < buildingVariation) // if variance is high, generate more cylindrical buildings.
                        {
                            //if circular, just use the width as size, and xPos as placement?.
                            yPos = yOffset + Random.Range(xMin, xMax);
                            height /= 2;
                            b.createCirc(width, height, xPos, yPos);
                        }
                        else //just generate a square building.
                        {
                            b.createCubic(length, width, height, xPos, yPos);
                        }
                    }
                }
                yOffset += maxSize; // offset is equals to the max size. 

            }
            yOffset = 0;
            xOffset += maxSize;
        }


        return true;
    }

}
