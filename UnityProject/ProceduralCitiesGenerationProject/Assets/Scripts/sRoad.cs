using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sRoad : MonoBehaviour
{
    public GameObject road;
    public GameObject intersection;
    public GameObject cds;

    public GameObject subUrban;

    public bool createRoad(float x, float z, float rotation, float xScale, float zScale, float yScale = 0, float y = 0)
    {
        return create(x, y, z, rotation, k:xScale, l:yScale, m:zScale);
    }

    public bool createIntersection(float x, float z, bool north, bool east, bool south, bool west, float xScale, float zScale, float yScale = 0, float y = 0, string nameOv = "")
    {
        return create(x, y, z, e: true, f: north, g: east, h: south, i: west, k: xScale, l: yScale, m: zScale, n:nameOv);
    }

    public bool createCDS(float x, float z, float xScale, float zScale, float yScale = 0, float y = 0, float rotation = 0)
    {
        return create(x, y, z, j: true, k: xScale, l: yScale, m: zScale, d:rotation);
    }

    private bool create(float a = 0, float b = 0, float c = 0, float d = 0, bool e = false, bool f = false, bool g = false, bool h = false, bool i = false, bool j = false, float k = 0, float l = 0, float m = 0, string n = "")
    {
        float x = (float)a;
        float y = (float)b;
        float z = (float)c;
        float xScale = (float)k;
        float yScale = (float)l;
        float zScale = (float)m;
        float rotation = (float)d;
        bool isIntersection = e;
        bool isCDS = j;
        bool north = f;
        bool east = g;
        bool south = h;
        bool west = i;
        string name = " null ";
        string nameOverride = n;

        GameObject roadObj = new GameObject();
        GameObject outer = new GameObject();

        if(isIntersection && isCDS)
        {
            Debug.Log("ERROR: Road marked as cds and intersection");
            return false;
        }

        if(isIntersection)
        {
            name = "Intersection";

            if(nameOverride != "")
            {
                name = nameOverride;
                west = true;
            }
            roadObj = Instantiate(intersection, new Vector3(x, y, z), Quaternion.identity);
            IntersectionController ic = roadObj.GetComponent<IntersectionController>();
            ic.north = north;
            ic.east = east;
            ic.south = south;
            ic.west = west;
            ic.updateRoad();
            outer.name = name;
        }
        else if(isCDS)
        {
            name = "CulDeSac";
            roadObj = Instantiate(cds, new Vector3(x, y, z), Quaternion.identity);
            outer.name = name;
        }
        else
        {
            name = "Road";
            roadObj = Instantiate(road, new Vector3(x, y, z), Quaternion.identity);
            outer.name = name;
        }

        roadObj.transform.SetParent(outer.transform);
        roadObj.transform.localScale = new Vector3(xScale, yScale, zScale);
        roadObj.transform.localEulerAngles = new Vector3(0, rotation, 0);

        outer.transform.SetParent(subUrban.transform);

        return true;
    }


}
