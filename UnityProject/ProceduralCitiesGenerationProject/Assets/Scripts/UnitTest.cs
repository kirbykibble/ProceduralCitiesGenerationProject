using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class UnitTest : MonoBehaviour
{
    public GameObject controller;

    private GameObject[] buildings;
    private GameObject[] houses;
    private GameObject[] cRoad;
    private GameObject[] sRoad;

    private RoadUseMap rum;

    private int numBuildings;
    private int numHouses;
    private int numCRoad;
    private int numSRoad;

    public void test()
    {
        string output;
        Debug.Log("===================================");
        Debug.Log("===================================");
        Debug.Log("Running Unit Tests...");
        setup();
        Debug.Log("Found: " + numBuildings + " Buildings, " + numHouses + " Houses, " + numCRoad + " cRoads, " + numSRoad + " sRoads.");
        output = culdesac_test() ? "All cds connected" : "ERROR: Misconnected cds";
        Debug.Log(output);
        output = node_connection_test() ? "All nodes connected" : "ERROR: Unconnected nodes";
        Debug.Log(output);
        output = building_hover_test() ? "No Buildings hovering" : "ERROR: Building hovering";
        Debug.Log(output);
        output = building_clipping_test() ? "No buildings clipping" : "ERROR: Building clipping";
        Debug.Log(output);
        output = house_hover_test() ? "No houses hovering" : "ERROR: House hovering";
        Debug.Log(output);
        output = house_clipping_test() ? "No houses clipping" : "ERROR: House clipping";
        Debug.Log(output);
    }

    private void setup()
    {
        buildings = GameObject.FindGameObjectsWithTag("building");
        houses = GameObject.FindGameObjectsWithTag("house");
        cRoad = GameObject.FindGameObjectsWithTag("cRoad");
        sRoad = GameObject.FindGameObjectsWithTag("sRoad");
        rum = controller.GetComponent<RoadUseMap>();

        numBuildings = buildings.Length;
        numHouses = houses.Length;
        numCRoad = cRoad.Length;
        numSRoad = sRoad.Length;
    }

    private bool node_connection_test()
    {
        Texture2D testMap;
        byte[] fileData;

        Color node = Color.blue;
        Color connector = Color.magenta;
        Color bg = Color.white;
        Color roadDefault = Color.black;

        if (File.Exists("Assets/Debug/usemapUT.png"))
        {
            fileData = File.ReadAllBytes("Assets/Debug/usemapUT.png");
            testMap = new Texture2D(2, 2, TextureFormat.BGRA32, false);
            testMap.LoadImage(fileData);
        }
        else
        {
            Debug.Log("ERROR: File not Found");
            return false;
        }

        int size = testMap.width;
        int destx = 0;
        int desty = 0;

        for (int x = 0; x < size; x++)
        {
            for(int y = 0; y < size; y++)
            {
                if(testMap.GetPixel(x, y) == connector)
                {
                    destx = x;
                    desty = y;
                    break;
                }
            }
        }

        for(int x = 0; x < size; x++)
        {
            for(int y = 0; y < size; y++)
            {
                if(testMap.GetPixel(x, y) == node)
                {
                    bool output = rum.search(testMap, x, y, destx, desty, true, size);
                    if (!output) return false;
                }
            }
        }
        return true;
    }

    private bool road_distance_test()
    {
        return false;
    }

    private bool culdesac_test()
    {
        Color bg = Color.black;
        Color node = Color.blue;
        Color road = Color.yellow;
        Color cds = Color.green;

        Texture2D houseMap;
        byte[] fileData;

        if(File.Exists("Assets/Debug/houseMap.png"))
        {
            fileData = File.ReadAllBytes("Assets/Debug/houseMap.png");
            houseMap = new Texture2D(2, 2, TextureFormat.BGRA32, false);
            houseMap.LoadImage(fileData);
        }
        else
        {
            Debug.Log("ERROR: File not Found");
            return false;
        }

        int size = houseMap.width;
        for(int x = 0; x < size; x++)
        {
            for(int y = 0; y < size; y++)
            {
                Color curPixel = houseMap.GetPixel(x, y);
                if (curPixel == cds)
                {
                    //area around the curPixel
                    int txMov = 0;
                    int tyMov = 0;
                    if (houseMap.GetPixel(x + 1, y) == road && x + 1 < size) txMov = 1;
                    if (houseMap.GetPixel(x - 1, y) == road && x - 1 >= 0) txMov = -1;
                    if (houseMap.GetPixel(x, y + 1) == road && y + 1 < size) tyMov = 1;
                    if (houseMap.GetPixel(x, y - 1) == road && y - 1 >= 0) tyMov = -1;

                    if (txMov + tyMov != 1 && txMov + tyMov != -1)
                    {
                        return false;
                    }
                    int tx = x;
                    int ty = y;
                    while (houseMap.GetPixel(tx, ty) != node)
                    {
                        tx += txMov;
                        ty += tyMov;

                        if (houseMap.GetPixel(tx, ty) == cds || tx >= size || tx < 0 || ty >= size || ty < 0)
                        {
                            return false;
                        }
                    }
                }
            }
        }

        return true;
    }

    private bool building_hover_test()
    {
        bool result = true;
        for(int i = 0; i < buildings.Length; i++)
        {
            if (buildings[i].transform.position.y >= 1)
            {
                Debug.Log("ERROR: Building " + i + " is hovering.");
                result = false;
            }
        }
        return result;
    }

    private bool building_clipping_test()
    {
        bool result = true;
        for(int i = 0; i < buildings.Length; i++)
        {
            if(buildings[i].transform.position.y <= -1)
            {
                Debug.Log("ERROR: Building " + i + " is clipping into the ground");
                result = false;
            }
        }
        return result;
    }

    private bool house_hover_test()
    {
        bool result = true;
        for(int i = 0; i < houses.Length; i++)
        {
            if(houses[i].transform.position.y >= 1)
            {
                Debug.Log("ERROR: House: " + i + " is hovering");
                result = false;
            }
        }
        return result;
    }
    private bool house_clipping_test()
    {
        bool result = true;
        for(int i = 0; i < houses.Length; i++)
        {
            if(houses[i].transform.position.y <= -1)
            {
                Debug.Log("ERROR: House: " + i + " is clipping into the ground");
                result = false;
            }
        }
        return result;
    }

    //didn't have time / deemed unnecessary 
    //private bool road_length_test() 
    //population_assignment_test 
    //building_population_test 
    //road_limit_test 
    //node skew test
    //building_intersect_test
    //road_generation_test


}
