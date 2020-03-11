using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class HouseCreator : MonoBehaviour
{
    //currently a test script. will update later.
    public GameObject controller;
    private House h;
    private sRoad s;

    private Color bg = Color.black;
    private Color node = Color.blue;
    private Color road = Color.yellow;
    private Color cds = Color.green;
    private Color connector = Color.magenta;

    private Color smallHouse = Color.red;
    private Color richHouse = Color.cyan;
    private Color highDens = new Vector4(0, .4f, 0, 1);
    private Color util = new Vector4(0.2f, 0.2f, 0.2f, 1);
    private Color school = Color.white;
    private Color uni = new Vector4(1, 0.6f, 0, 1);

    private List<Color> checker = new List<Color>();
    private int size;

    public bool createHouses(int maxSize)
    {
        checker.Add(road);
        checker.Add(node);
        checker.Add(cds);
        checker.Add(connector);
        h = controller.GetComponent<House>();
        s = controller.GetComponent<sRoad>();
        Texture2D houseMap = null;
        byte[] fileData;
        float xOffset = 0.0f;
        float yOffset = 0.0f;
        
        if(File.Exists("Assets/Debug/houseMap.png"))
        {
            fileData = File.ReadAllBytes("Assets/Debug/houseMap.png");
            houseMap = new Texture2D(2, 2, TextureFormat.BGRA32, false);
            houseMap.LoadImage(fileData);

            Debug.Log("Successfully loaded housemap");
        }
        else
        {
            Debug.Log("ERROR: File not found");
            return false;
        }

        size = houseMap.width;
        for(int x = 0; x < houseMap.width; x++)
        {
            for(int y = 0; y < houseMap.width; y++)
            {
                Color curPixel = houseMap.GetPixel(x, y);
                int Rotation = getFacing(houseMap, x, y);
                if (closeToRoad(houseMap, x, y))
                {
                    if (curPixel == smallHouse)
                    {
                        h.createHouse("HOUSE_SMALL", xOffset, yOffset, rotation: Rotation);
                    }
                    if (curPixel == richHouse)
                    {
                        h.createHouse("HOUSE_RICH", xOffset, yOffset, rotation: Rotation);
                    }
                    if (curPixel == highDens)
                    {
                        h.createHouse("HOUSE_DENS", xOffset, yOffset, rotation: Rotation);
                    }
                    if (curPixel == util)
                    {
                        h.createHouse("UTILITY", xOffset, yOffset, rotation: Rotation);
                    }
                }
                if (curPixel == uni && isUniCenter(houseMap, x, y))
                {
                    h.createHouse("UNI", xOffset, yOffset, rotation: Rotation);
                }
                if (curPixel == school && isSchoolCenter(houseMap, x, y))
                {
                    h.createHouse("SCHOOL", xOffset, yOffset, rotation: Rotation);
                }
                if (curPixel == road || curPixel == connector)
                {
                    float xScale = (float)maxSize / 10;
                    float rotation = 0.0f;
                    if (isNorthSouth(houseMap, x, y)) rotation = 90.0f;
                    s.createRoad(xOffset, yOffset, rotation, xScale, xScale, 1);
                }
                if(curPixel == node)
                {
                    float xScale = (float)maxSize / 10;
                    List<bool> op = getOpenRoads(houseMap, x, y);
                    s.createIntersection(xOffset, yOffset, op[0], op[1], op[2], op[3], xScale, xScale, 1);
                }
                if(curPixel == cds)
                {
                    float xScale = (float)maxSize / 10;
                    float facing = (float) getCdsFace(houseMap, x, y);
                    s.createCDS(xOffset, yOffset, xScale, xScale, 1, rotation:facing);
                }
                yOffset += maxSize;
            }
            xOffset += maxSize;
            yOffset = 0;
        }

        return true;
    }

    bool closeToRoad(Texture2D map, int x, int y)
    {
        bool result = false;

        if (checker.Contains(map.GetPixel(x + 1, y)) || 
            checker.Contains(map.GetPixel(x - 1, y)) || 
            checker.Contains(map.GetPixel(x, y + 1)) || 
            checker.Contains(map.GetPixel(x, y - 1))) //east west
        {
            result = true;
        }


        return result;
    }

    bool isUniCenter(Texture2D map, int x, int y)
    {
        for (int i = -1; i <= 1; i++)
        {
            for (int j = -1; j <= 1; j++)
            {
                if (map.GetPixel(x + i, y + j) != uni)
                {
                    return false;
                }
            }
        }
        return true;
    }

    bool isSchoolCenter(Texture2D map, int x, int y)
    {
        if(map.GetPixel(x - 1, y) == school && map.GetPixel(x - 2, y) == school)
        {
            return true;
        }
        else if (map.GetPixel(x, y + 1) == school && map.GetPixel(x, y + 2) == school)
        {
            return true;
        }
        return false;
    }
    
    List<bool> getOpenRoads(Texture2D map, int x, int y)
    {
        List<bool> openRoads = new List<bool>();
        if (checker.Contains(map.GetPixel(x, y - 1)) && y - 1 >= 0) openRoads.Add(true);
        else openRoads.Add(false);

        if (checker.Contains(map.GetPixel(x - 1, y)) && x - 1 >= 0) openRoads.Add(true);
        else openRoads.Add(false);

        if (checker.Contains(map.GetPixel(x, y + 1)) && y + 1 < size) openRoads.Add(true);
        else openRoads.Add(false);

        if (checker.Contains(map.GetPixel(x + 1, y)) && x + 1 < size) openRoads.Add(true);
        else openRoads.Add(false);

        return openRoads;
    }

    bool isNorthSouth(Texture2D map, int x, int y)
    {
        if (y + 1 >= size || y - 1 < 0) return false;
        if(checker.Contains(map.GetPixel(x, y + 1)) || checker.Contains(map.GetPixel(x, y - 1)))
        {
            return true;
        }
        return false;
    }

    int getCdsFace(Texture2D map, int x, int y)
    {
        if (map.GetPixel(x + 1, y) == road && x + 1 < size) return 0;
        if (map.GetPixel(x - 1, y) == road && x - 1 >= 0) return 180;
        if (map.GetPixel(x, y + 1) == road && y + 1 < size) return 270;
        return 90;
    }

    int getFacing(Texture2D map, int x, int y)
    {
        if (checker.Contains(map.GetPixel(x + 1, y)) ||
            checker.Contains(map.GetPixel(x - 1, y)) ||
            checker.Contains(map.GetPixel(x, y + 1)) ||
            checker.Contains(map.GetPixel(x, y - 1))) //east west
        {

        }

        if (checker.Contains(map.GetPixel(x + 1, y))) 
        {
            return 90;
        }
        else if (checker.Contains(map.GetPixel(x - 1, y)))
        {
            return -90;
        }
        else if (checker.Contains(map.GetPixel(x, y + 1)))
        {
            return 180;
        }
        
        return 0;
    }
}
