using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class houseMap : MonoBehaviour
{
    private float smallPerc;
    private float richPerc;
    private float highDensPerc;
    private float utilPerc;
    private float schoolRatio;

    private Color bg = Color.black;
    private Color node = Color.blue;
    private Color road = Color.yellow;
    private Color cds = Color.green;

    private Color smallHouse = Color.red;
    private Color richHouse = Color.cyan;
    private Color highDens = new Vector4(0, .4f, 0, 1);
    private Color util = new Vector4(0.2f, 0.2f, 0.2f, 1);
    private Color school = Color.white;
    private Color uni = new Vector4(1, 0.6f, 0, 1);

    private List<Color> checker = new List<Color>();
    private int s;

    public bool createHouseMap(float sp, float rp, float hdp, float up, float sr)
    {

        smallPerc = sp;
        richPerc = rp;
        highDensPerc = hdp;
        utilPerc = up;
        schoolRatio = sr;

        checker.Add(bg);
        checker.Add(smallHouse);
        checker.Add(richHouse);
        checker.Add(highDens);
        checker.Add(util);
        checker.Add(school);
        checker.Add(uni);

        Texture2D surbanMap = null;
        byte[] fileData;

        if (File.Exists("Assets/Debug/SurbanNodeMapOut.png"))
        {
            fileData = File.ReadAllBytes("Assets/Debug/SurbanNodeMapOut.png");
            surbanMap = new Texture2D(2, 2, TextureFormat.BGRA32, false);
            surbanMap.LoadImage(fileData);

            Debug.Log("Successfully loaded surbanNodemap");
        }
        else Debug.Log("ERROR: File not found");

        Texture2D houseMap = new Texture2D(surbanMap.width, surbanMap.height);

        int size = surbanMap.width;
        s = size;
        for (int x = 0; x < size; x++)
        {
            for (int y = 0; y < size; y++)
            {
                houseMap.SetPixel(x, y, bg);
                if (surbanMap.GetPixel(x, y) != Color.red)
                {
                    houseMap.SetPixel(x, y, surbanMap.GetPixel(x, y));
                }
            }
        }

        //first, we place the school and university.
        //there is only one university,
        //there is one school for every x pixels in the map
        int numSchools = Mathf.RoundToInt(size / schoolRatio);
        float schoolPerc = 0.2f; //internal chance. 
        float uniPerc = 0.2f; // internal chance

        while (numSchools > 0)
        {
            int x = Random.Range(0, size);
            int y = Random.Range(0, size);
            //if you are a bg pixel AND close to a road AND the school percentage goes off.
            if (houseMap.GetPixel(x, y) == bg && closeToRoad(houseMap, x, y) && Random.value < (float)schoolPerc)
            {
                if (closeToRoad(houseMap, x, y + 1) && houseMap.GetPixel(x, y + 1) == bg && closeToRoad(houseMap, x, y - 1) && houseMap.GetPixel(x, y - 1) == bg)
                {
                    //north south directioned road
                    houseMap.SetPixel(x, y, school);
                    houseMap.SetPixel(x, y + 1, school);
                    houseMap.SetPixel(x, y - 1, school);
                    numSchools--;
                }
                else if (closeToRoad(houseMap, x + 1, y) && houseMap.GetPixel(x + 1, y) == bg && closeToRoad(houseMap, x - 1, y) && houseMap.GetPixel(x - 1, y) == bg)
                {
                    //east west directioned road
                    houseMap.SetPixel(x, y, school);
                    houseMap.SetPixel(x + 1, y, school);
                    houseMap.SetPixel(x - 1, y, school);
                    numSchools--;
                }
            }
        }

        bool placingUni = true;
        while (placingUni)
        {
            int x = Random.Range(0, size);
            int y = Random.Range(0, size);

            if (houseMap.GetPixel(x, y) == bg && closeToRoad(houseMap, x, y) && Random.value < (float)uniPerc)
            {
                int locationToPlace = uniCheck(houseMap, x, y);
                if(locationToPlace != -1) //if there is a valid location, then place it.
                {
                    if(locationToPlace == 1) uniColor(houseMap, x - 1, y + 1);
                    else if(locationToPlace == 2) uniColor(houseMap, x, y + 1);
                    else if(locationToPlace == 3) uniColor(houseMap, x + 1, y + 1);
                    else if(locationToPlace == 4) uniColor(houseMap, x - 1, y);
                    else if(locationToPlace == 5) uniColor(houseMap, x + 1, y);
                    else if(locationToPlace == 6) uniColor(houseMap, x - 1, y - 1);
                    else if(locationToPlace == 7) uniColor(houseMap, x, y - 1);
                    else if(locationToPlace == 8) uniColor(houseMap, x + 1, y - 1);

                    placingUni = false;
                }
            }
        }

        //now, place the utility buildings
        for(int x = 0; x < size; x++)
        {
            for(int y = 0; y < size; y++)
            {
                if(closeToRoad(houseMap, x, y) && houseMap.GetPixel(x,y) == bg)
                {
                    float chance = (float) utilPerc;
                    if(isCorner(houseMap, x,y))
                    {
                        chance += (float)0.1f;
                    }
                    if(Random.value < chance)
                    {
                        houseMap.SetPixel(x, y, util);
                    }
                }
            }
        }

        for(int x = 0; x < size; x++)
        {
            for(int y = 0; y < size; y++)
            {
                if (houseMap.GetPixel(x,y) == bg)
                {
                    float smallChance = (float)smallPerc;
                    float richChance = (float)richPerc;
                    float densChance = (float)highDensPerc;

                    smallChance += (float)closeToSim(houseMap, smallHouse, x, y);
                    richChance += (float)closeToSim(houseMap, richHouse, x, y);
                    densChance += (float)closeToSim(houseMap, highDens, x, y);

                    while(true)
                    {
                        if (Random.value < smallChance)
                        {
                            houseMap.SetPixel(x, y, smallHouse);
                            break;
                        }
                        if(Random.value < richChance)
                        {
                            houseMap.SetPixel(x, y, richHouse);
                            break;
                        }
                        if(Random.value < densChance)
                        {
                            houseMap.SetPixel(x, y, highDens);
                            break;
                        }
                    }
                }
            }
        }

        byte[] output = houseMap.EncodeToPNG();
        File.WriteAllBytes(Application.dataPath + "/../Assets/Debug/houseMap.png", output);

        return true;
    }

    bool houseClose(Texture2D map, int x, int y)
    {
        bool result = false;
        for (int i = -1; i <= 1; i++)
        {
            for (int j = -1; j <= 1; j++)
            {
                if (map.GetPixel(x + i, y + j) == road || map.GetPixel(x + i, y + j) == cds)
                {
                    return true;
                }
            }
        }
        return result;
    }

    float closeToSim(Texture2D map, Color type, int x, int y)
    {
        float result = 0.0f;
        for (int i = -1; i <= 1; i++)
        {
            for (int j = -1; j <= 1; j++)
            {
                if (map.GetPixel(x + i, y + j) == type)
                {
                    result += (float)0.2f;
                }
            }
        }
        return result;
    }

    bool isCorner (Texture2D map, int x, int y)
    {
        bool result = false;
        result = map.GetPixel(x + 1, y + 1) == node ||
            map.GetPixel(x - 1, y + 1) == node ||
            map.GetPixel(x + 1, y - 1) == node ||
            map.GetPixel(x - 1, y - 1) == node;

        return result;
    }

    bool closeToRoad(Texture2D map, int x, int y)
    {
        bool result = false;

        if (!checker.Contains(map.GetPixel(x + 1, y)) || !checker.Contains(map.GetPixel(x - 1, y)) || !checker.Contains(map.GetPixel(x, y + 1)) || !checker.Contains(map.GetPixel(x, y - 1))) //east west
        {
            result = true;
        }
        if (x - 1 <= 0 || x + 1 >= s || y - 1 <= 0 || y + 1 >= s)
        {
            result = false;
        }
        return result;
    }

    int uniCheck(Texture2D map, int x, int y)
    {
        //result 
        //1 2 3
        //4 x 5
        //6 7 8
        int result = -1;

        //growth patterns
        //a b c
        //d x e
        //f g h
        bool a = surroundCheck(map, x - 1, y + 1);
        bool b = surroundCheck(map, x, y + 1);
        bool c = surroundCheck(map, x + 1, y + 1);
        bool d = surroundCheck(map, x - 1, y);
        bool e = surroundCheck(map, x + 1, y);
        bool f = surroundCheck(map, x - 1, y - 1);
        bool g = surroundCheck(map, x, y - 1);
        bool h = surroundCheck(map, x + 1, y - 1);

        //a VERY inelegant solution, but not inneficient!
        if (a) return 1;
        if (b) return 2;
        if (c) return 3;
        if (d) return 4;
        if (e) return 5;
        if (f) return 6;
        if (g) return 7;
        if (h) return 8;

        return result;
    }

    bool surroundCheck(Texture2D map, int x, int y)
    {
        bool result = true;
        for(int i = -1; i <= 1; i++)
        {
            for (int j = -1; j <= 1; j++)
            {
                if(map.GetPixel(x + i, y + j) != bg) //if at any point a surrounding pixel is not a bg
                {
                    return false;
                }
            }
        }

        return result;
    }

    void uniColor(Texture2D map, int x, int y)
    {
        for (int i = -1; i <= 1; i++)
        {
            for (int j = -1; j <= 1; j++)
            {
                map.SetPixel(x + i, y + j, uni);
            }
        }
    }
}
