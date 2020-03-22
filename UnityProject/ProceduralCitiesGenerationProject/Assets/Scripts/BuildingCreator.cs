using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class BuildingCreator : MonoBehaviour
{
    //test function for building.cs
    public GameObject controller;
    //public GameObject floor;
    private cRoad c;
    private Building b;

    private Color bg = Color.black;
    private Color node = Color.blue;
    private Color road = Color.yellow;
    private Color noConnection = Color.red;
    private Color diagonalRoad = Color.green;
    private Color outConnector = Color.magenta;

    private bool landmark = false;
    private int size;
    private List<Color> checker = new List<Color>();
    private List<Color> checkerDiag = new List<Color>();

    public bool createBuildings(int maxSize, int maxHeight, int gap, int variance, float buildingVariation)
    {
        checker.Add(road);
        checker.Add(node);
        checker.Add(outConnector);

        checkerDiag.Add(diagonalRoad);
        checkerDiag.Add(node);
        checkerDiag.Add(outConnector);

        b = controller.GetComponent<Building>();
        c = controller.GetComponent<cRoad>();
        Texture2D heatmap = null;
        Texture2D cNodeMap = null;
        Texture2D usemap = null;
        byte[] fileData;
        float totalDens = 0.0f; // unused as of now. 
        float maxDens = 0.0f;
        float xOffset = 0.0f;
        float yOffset = 0.0f;
        float roadOffset = 10.0f;

        if (File.Exists("Assets/Debug/HeatmapOut.png"))
        {
            fileData = File.ReadAllBytes("Assets/Debug/HeatmapOut.png");
            heatmap = new Texture2D(2, 2, TextureFormat.BGRA32, false);
            heatmap.LoadImage(fileData);

            Debug.Log("Successfully loaded heatmap");
        }
        else {
            Debug.Log(" ERROR: File not found");
            return false;
        }

        if (File.Exists("Assets/Debug/nodeMapOut.png"))
        {
            fileData = File.ReadAllBytes("Assets/Debug/nodeMapOut.png");
            cNodeMap = new Texture2D(2, 2, TextureFormat.BGRA32, false);
            cNodeMap.LoadImage(fileData);
            Debug.Log("Node successfully read");
        }
        else {
            Debug.Log(" ERROR: File not found");
            return false;
        }

        if (File.Exists("Assets/Debug/usemap.png"))
        {
            fileData = File.ReadAllBytes("Assets/Debug/usemap.png");
            usemap = new Texture2D(2, 2, TextureFormat.BGRA32, false);
            usemap.LoadImage(fileData);
            Debug.Log("Usemap successfully read");
        }
        else
        {
            Debug.Log("ERROR: File not found");
            return false;
        }
        size = usemap.width;

        Vector3 floorPos = new Vector3((maxSize * heatmap.width) / 2, 1, (maxSize * heatmap.height) / 2);
        Vector3 floorSize = new Vector3((maxSize * heatmap.width) / 8, 1, (maxSize * heatmap.height) / 8);
        //floor.transform.localScale = floorSize;
        //floor.transform.position = floorPos;

        int center = (heatmap.width - (heatmap.width % 2)) / 2;
        Debug.Log("CENTER: " + center);
        float redCount;
        float centerClose;

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
                bool dn = cNodeMap.GetPixel(x + 1, y) == diagonalRoad || cNodeMap.GetPixel(x - 1, y) == diagonalRoad || cNodeMap.GetPixel(x, y + 1) == diagonalRoad || cNodeMap.GetPixel(x, y - 1) == diagonalRoad ? true : false;

                if ((curPixel == bg || curPixel == noConnection) && !dn) //accounts for everything
                {
                    bool isLandmark = false;
                    //compare the current pixel to the total density.
                    float buildingScale = heatmap.GetPixel(x, y).r;
                    buildingScale = (float) buildingScale / maxDens;

                    int width = Random.Range(maxSize - variance, maxSize);
                    int length = Random.Range(maxSize - variance, maxSize);
                    float height = (float) maxHeight * buildingScale * 2 + Random.Range(-variance, variance);

                    redCount = (float)heatmap.GetPixel(x, y).r;
                    float iClose = (Mathf.Abs(x - center));
                    iClose = iClose / center;
                    iClose = 1 - iClose;

                    float jClose = (Mathf.Abs(y - center));
                    jClose = jClose / center;
                    jClose = 1 - jClose;

                    float output = iClose + jClose;
                    output = output / 2;

                    if (redCount == 1 && !landmark && Random.value < output)
                    {
                        width = maxSize;
                        length = maxSize;
                        landmark = true;
                        isLandmark = true;
                    }

                    float xMax = (float)maxSize - (width / 2);
                    float yMax = (float)maxSize - (length / 2);
                    float xMin = (float)width / 2;
                    float yMin = (float)length / 2;

                    float xPos = xOffset + Random.Range(xMin, xMax);
                    float yPos = yOffset + Random.Range(yMin, yMax);


                    if(height != 0)
                    {
                        if (Random.value < buildingVariation && !isLandmark) // if variance is high, generate more cylindrical buildings.
                        {
                            //if circular, just use the width as size, and xPos as placement
                            //y pos needs to be modified due to cylinder scaling diferences
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
                else if (curPixel == road)
                {
                    float xScale = (float)maxSize / 10;
                    roadOffset = (float)maxSize / 2;
                    float rotation = 0.0f;
                    if (isNorthSouth(cNodeMap, x, y)) rotation = 90.0f;

                    if (usemap.GetPixel(x, y).r >= 0.8f)
                    {
                        c.createBigRoad(xOffset + roadOffset, yOffset + roadOffset, rotation, xScale);
                    }
                    else if(usemap.GetPixel(x, y).r >= 0.4f)
                    {
                        c.createMedRoad(xOffset + roadOffset, yOffset + roadOffset, rotation, xScale);
                    }
                    else
                    {
                        c.createRoad(xOffset + roadOffset, yOffset + roadOffset, rotation, xScale);
                    }                   
                }
                else if (curPixel == diagonalRoad)
                {
                    float xScale = (float)maxSize / 10;
                    xScale = Mathf.Sqrt(Mathf.Pow(xScale, 2) * 2);
                    roadOffset = (float)maxSize / 2;
                    float rotation = 45.0f;
                    if (isDiagNW(cNodeMap, x, y)) rotation = -45.0f;

                    if ((float) usemap.GetPixel(x, y).r >= 0.8f)
                    {
                        c.createBigRoad(xOffset + roadOffset, yOffset + roadOffset, rotation, xScale);
                    }
                    else if ((float) usemap.GetPixel(x, y).r >= 0.4f)
                    {
                        c.createMedRoad(xOffset + roadOffset, yOffset + roadOffset, rotation, xScale);
                    }
                    else
                    {
                        c.createRoad(xOffset + roadOffset, yOffset + roadOffset, rotation, xScale);
                    }
                }
                else if (curPixel == node)
                {
                    float scale = (float)maxSize / 20;
                    roadOffset = (float)maxSize / 2;
                    List<bool> op = getOpenRoads(cNodeMap, x, y);
                    List<string> ty = getRoadTypes(usemap, x, y);

                    if(isHex(cNodeMap, x, y))
                    {
                        c.createHexInt(xOffset + roadOffset, yOffset + roadOffset, 0, op[0], op[1], op[2], op[3], op[4], op[5], op[6], op[7], "null", "null", "null", "null", "null", "null", "null", "null", scale);
                    }
                    else // is regular intersection
                    {
                        // 2 5 8
                        // 1 4 7 
                        // 0 3 6
                    
                        c.createInter(xOffset + roadOffset, yOffset + roadOffset, 0, op[0], op[1], op[2], op[3], ty[3], ty[1], ty[5], ty[7], 1);
                    }
                }

                yOffset += maxSize; // offset is equals to the max size. 

            }
            yOffset = 0;
            xOffset += maxSize;
        }


        return true;
    }

    bool isNorthSouth(Texture2D map, int x, int y)
    {
        if (y + 1 >= size || y - 1 < 0) return false;
        if (checker.Contains(map.GetPixel(x, y + 1)) || checker.Contains(map.GetPixel(x, y - 1)))
        {
            return true;
        }
        return false;
    }

    bool isDiagNW(Texture2D map, int x, int y)
    {
        if (y + 1 >= size || y - 1 < 0 || x + 1 >= size || x - 1 < 0) return false;
        if(checkerDiag.Contains(map.GetPixel(x + 1, y + 1)) || checkerDiag.Contains(map.GetPixel(x - 1, y - 1)))
        {
            return true; //means that the road is northeast / southwest bound
        }
        return false; // means that the road is northwest / southeast bound
    }
    bool isHex(Texture2D map, int x, int y)
    {
        for(int i = -1; i <= 1; i++)
        {
            for(int j = -1; j <= 1; j++)
            {
                if (map.GetPixel(x + i, y + j) == diagonalRoad) return true;
            }
        }
        return false;
    }

    List<bool> getOpenRoads(Texture2D map, int x, int y)
    {
        List<bool> openRoads = new List<bool>();
        if (checker.Contains(map.GetPixel(x, y - 1)) && y - 1 >= 0) openRoads.Add(true); // south
        else openRoads.Add(false);

        if (checker.Contains(map.GetPixel(x - 1, y)) && x - 1 >= 0) openRoads.Add(true); // west
        else openRoads.Add(false);

        if (checker.Contains(map.GetPixel(x, y + 1)) && y + 1 < size) openRoads.Add(true); // north
        else openRoads.Add(false);

        if (checker.Contains(map.GetPixel(x + 1, y)) && x + 1 < size) openRoads.Add(true); // east
        else openRoads.Add(false);

        //diags
        if (checkerDiag.Contains(map.GetPixel(x - 1, y - 1)) && y - 1 >= 0 && x - 1 >= 0) openRoads.Add(true); // southwest
        else openRoads.Add(false);

        if (checkerDiag.Contains(map.GetPixel(x - 1, y + 1)) && x - 1 >= 0 && y + 1 < size) openRoads.Add(true); //northwest
        else openRoads.Add(false);

        if (checkerDiag.Contains(map.GetPixel(x + 1, y + 1)) && y + 1 < size && x + 1 < size) openRoads.Add(true); //northeast
        else openRoads.Add(false);

        if (checkerDiag.Contains(map.GetPixel(x + 1, y - 1)) && x + 1 < size && y - 1 >= 0) openRoads.Add(true); //southeast
        else openRoads.Add(false);

        return openRoads;
    }

    List<string> getRoadTypes(Texture2D map, int x, int y)
    {
        // x x x 
        // x c x
        // x x x

        // 2 5 8
        // 1 4 7 
        // 0 3 6 
        List<string> roadType = new List<string>();
        for (int i = -1; i <= 1; i++)
        {
            for (int j = -1; j <= 1; j++)
            {
                Debug.Log((float)map.GetPixel(x + i, j + y).r);
                if ((float) map.GetPixel(x + i, y + j).r >= 0.8f) roadType.Add("LARGE");
                else if ((float) map.GetPixel(x + i, y + j).r >= 0.4f) roadType.Add("MEDIUM");
                else roadType.Add("SMALL");
            }
        }

        return roadType;
    }

}
