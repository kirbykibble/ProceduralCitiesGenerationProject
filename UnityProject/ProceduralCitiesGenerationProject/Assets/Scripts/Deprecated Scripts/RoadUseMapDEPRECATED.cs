using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class RoadUseMapDEPRECATED : MonoBehaviour
{
    //since suburban roads will remain static, this will only be generated for the city.
    //To calculate population size:
    // - Each small house will contain one population unit
    // - Each rich house will contain two population units
    // - Each high density house will contain six population units

    private int smallHousePop = 1;
    private int richHousePop = 2;
    private int highDensPop = 6;

    private Color node = Color.blue;
    private Color road = Color.yellow;
    private Color cds = Color.green;

    private Color diagRoad = Color.green;
    private Color bg = Color.white;
    private Color roadDefault = Color.black;

    private Color smallHouse = Color.red;
    private Color richHouse = Color.cyan;
    private Color connector = Color.magenta;
    private Color highDens = new Vector4(0, .4f, 0, 1);

    private List<Color> checker = new List<Color>();
    private int totalPop = 0;
    private int cityConnectors = 0;
    private int nodeSize;

    public bool createUseMap(int ns)
    {
        nodeSize = ns;

        checker.Add(node);
        checker.Add(road);
        checker.Add(cds);

        Texture2D houseMap = null;
        Texture2D cityMap = null;
        Texture2D useMap = null;
        byte[] fileData;

        if (File.Exists("Assets/Debug/houseMap.png"))
        {
            fileData = File.ReadAllBytes("Assets/Debug/houseMap.png");
            houseMap = new Texture2D(2, 2, TextureFormat.BGRA32, false);
            houseMap.LoadImage(fileData);

            Debug.Log("Successfully Loaded Housemap");
        }
        else
        {
            Debug.Log("ERROR: File not found");
            return false;
        }
        if (File.Exists("Assets/Debug/nodeMapOut.png"))
        {
            fileData = File.ReadAllBytes("Assets/Debug/nodeMapOut.png");
            cityMap = new Texture2D(2, 2, TextureFormat.BGRA32, false);
            cityMap.LoadImage(fileData);

            Debug.Log("Successfully Loaded NodeMap");
        }
        else
        {
            Debug.Log("ERROR: File not found");
            return false;
        }

        //First, we calculate total population.
        int size = houseMap.width;
        for(int x = 0; x < size; x++)
        {
            for(int y = 0; y < size; y++)
            {
                if(closeToRoad(houseMap, x, y))
                {
                    if (houseMap.GetPixel(x, y) == smallHouse)
                    {
                        totalPop += smallHousePop;
                    }
                    else if (houseMap.GetPixel(x, y) == richHouse)
                    {
                        totalPop += richHousePop;
                    }
                    else if (houseMap.GetPixel(x, y) == highDens)
                    {
                        totalPop += highDensPop;
                    }
                    else if (houseMap.GetPixel(x, y) == connector)
                    {
                        cityConnectors++;
                    }
                }
            }
        }

        int connectorUse = totalPop / cityConnectors; //Rounds to nearest value. Probably will eliminate or create a population, but this is negligible. (negible? Neglible?)

        //next, we recreate the city roadmap except with a usage heatmap.
        size = cityMap.width;
        useMap = new Texture2D(size, size);
        for(int x = 0; x < size; x++)
        {
            for(int y = 0; y < size; y++)
            {
                useMap.SetPixel(x, y, bg);
                if (cityMap.GetPixel(x, y) == road || cityMap.GetPixel(x,y) == diagRoad)
                {
                    useMap.SetPixel(x, y, roadDefault);
                }
                else if (cityMap.GetPixel(x,y) == node)
                {
                    useMap.SetPixel(x, y, node);
                }
            }
        }

        Color roadColor = new Color();
        roadColor.a = 1;
        roadColor.g = 0;
        roadColor.b = 0;

        for(int x = 0; x < size; x++)
        {
            for(int y = 0; y < size; y++)
            {
                if (cityMap.GetPixel(x, y) == connector)
                {
                    useMap.SetPixel(x, y, connector); // set the pixel as the connector
                    cityConnectors--;
                    connectorUse = 1; //test code. Comment out later.
                    while(connectorUse > 0)
                    {
                        bool destReached = false;
                        int destX = x;
                        int destY = y;
                        int curX = x;
                        int curY = y;
                        while(useMap.GetPixel(destX, destY) != bg)
                        {
                            destX = Random.Range(0, size);
                            destY = Random.Range(0, size);
                        }

                        List<int> destinationNode = closestNode(cityMap, destX, destY);
                        destX = destinationNode[0];
                        destY = destinationNode[1];

                        Debug.Log("DESTINATION NODE: " + destX + ", " + destY);

                        int g = 0;
                        int h = 0;
                        int f = 0;

                        int bestF = 9999;

                        //direction gets updated with the bestF. 
                        // 1 2 3 
                        // 4 5 6
                        // 7 8 9
                        // 5 isn't used by the way

                        int direction = -1;
                        //List<List<int>> visitedNodes = new List<List<int>>();


                        while (!destReached)
                        {
                            g++;
                            bestF = 9999;

                            for (int i = -nodeSize; i <= nodeSize; i += nodeSize)
                            {
                                for (int j = -nodeSize; j <= nodeSize; j += nodeSize)
                                {
                                    int testX = i / nodeSize;
                                    int testY = j / nodeSize;
                                    if(useMap.GetPixel(curX + testX, curY + testY) == road)
                                    {
                                        h = (int)distFromDest(useMap, destX, destY, curX + i, curY + j);
                                        h /= nodeSize;

                                        f = g + h;
                                        if (f < bestF)
                                        {
                                            bestF = f;
                                            if (i < 0 && j > 0) direction = 1;
                                            if (i < 0 && j == 0) direction = 4;
                                            if (i < 0 && j < 0) direction = 7;
                                            if (i == 0 && j > 0) direction = 2;
                                            if (i == 0 && j < 0) direction = 8;
                                            if (i > 0 && j > 0) direction = 3;
                                            if (i > 0 && j == 0) direction = 6;
                                            if (i > 0 && j < 0) direction = 9;
                                        }
                                    }
                                }
                            }

                            if(direction == 1)
                            {
                                while(useMap.GetPixel(curX - 1, curY + 1) != node)
                                {
                                    curX--;
                                    curY++;
                                    roadColor.r = useMap.GetPixel(curX, curY).r + 0.001f;
                                    useMap.SetPixel(curX, curY, roadColor);
                                }
                                curX--;
                                curY++;
                            }
                            if (direction == 2)
                            {
                                while (useMap.GetPixel(curX, curY + 1) != node)
                                {
                                    curY++;
                                    roadColor.r = useMap.GetPixel(curX, curY).r + 0.001f;
                                    useMap.SetPixel(curX, curY, roadColor);
                                }
                                curY++;
                            }
                            if (direction == 3)
                            {
                                while (useMap.GetPixel(curX + 1, curY + 1) != node)
                                {
                                    curX++;
                                    curY++;
                                    roadColor.r = useMap.GetPixel(curX, curY).r + 0.001f;
                                    useMap.SetPixel(curX, curY, roadColor);
                                }
                                curX++;
                                curY++;
                            }
                            if (direction == 4)
                            {
                                while (useMap.GetPixel(curX - 1, curY) != node)
                                {
                                    curX--;
                                    roadColor.r = useMap.GetPixel(curX, curY).r + 0.001f;
                                    useMap.SetPixel(curX, curY, roadColor);
                                }
                                curX--;
                            }
                            if (direction == 6)
                            {
                                while (useMap.GetPixel(curX, curY) != node)
                                {
                                    curX++;
                                    roadColor.r = useMap.GetPixel(curX, curY).r + 0.001f;
                                    useMap.SetPixel(curX, curY, roadColor);
                                }
                                curX++;
                            }
                            if (direction == 7)
                            {
                                while (useMap.GetPixel(curX - 1, curY - 1) != node)
                                {
                                    curX--;
                                    curY--;
                                    roadColor.r = useMap.GetPixel(curX, curY).r + 0.001f;
                                    useMap.SetPixel(curX, curY, roadColor);
                                }
                                curX--;
                                curY--;
                            }
                            if (direction == 8)
                            {
                                while (useMap.GetPixel(curX, curY - 1) != node)
                                {
                                    curY--;
                                    roadColor.r = useMap.GetPixel(curX, curY).r + 0.001f;
                                    useMap.SetPixel(curX, curY, roadColor);
                                }
                                curY--;
                            }
                            if (direction == 9)
                            {
                                while (useMap.GetPixel(curX + 1, curY - 1) != node)
                                {
                                    curX++;
                                    curY--;
                                    roadColor.r = useMap.GetPixel(curX, curY).r + 0.001f;
                                    useMap.SetPixel(curX, curY, roadColor);
                                }
                                curX++;
                                curY--;
                            }

                            if (curX == destX && curY == destY) destReached = true;
                        }
                        connectorUse--;
                    }
                }
            }
        }

        byte[] output = useMap.EncodeToPNG();
        File.WriteAllBytes(Application.dataPath + "/../Assets/Debug/usemap.png", output);
        return true;
    }

    bool closeToRoad(Texture2D map, int x, int y)
    {
        for (int i = -1; i <= 1; i++)
        {
            for (int j = -1; j <= 1; j++)
            {
                if (checker.Contains(map.GetPixel(x + i, y + j)))
                {
                    return true;
                }
            }
        }
        return false;
    } 

    List<int> closestNode(Texture2D map, int x, int y)
    {
        bool found = false;
        List<int> coords = new List<int>();
        int probeSize = 1;
        while(!found)
        {
            for (int i = -probeSize; i <= probeSize; i++)
            {
                for(int j = -probeSize; j <= probeSize; j++)
                {
                    if(map.GetPixel(x + i, y + j) == node)
                    {
                        found = true;
                        coords.Add(x + i);
                        coords.Add(y + j);
                    }
                }
            }
            probeSize++;
        }
        
        return coords;
    }

    float distFromDest(Texture2D map, int x, int y, int destX, int destY)
    {
        float dist = Mathf.Sqrt(Mathf.Pow((destY - y), 2) + Mathf.Pow((destX - x), 2));
        return dist;
    }
}
