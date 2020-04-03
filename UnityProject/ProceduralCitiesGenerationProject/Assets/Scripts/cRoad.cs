using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cRoad : MonoBehaviour
{
    public GameObject road;
    public GameObject medRoad;
    public GameObject bigroad;
    public GameObject intersection;
    public GameObject hexInter;
    public GameObject city;

    public bool createRoad(float x, float z, float rotation, float xs, float ys)
    {
        return create("ROAD", x, z, rotation, t:xs, u:ys);
    }
    public bool createMedRoad(float x, float z, float rotation, float xs, float ys)
    {
        return create("MED", x, z, rotation, t: xs, u: ys);
    }
    public bool createBigRoad(float x, float z, float rotation, float xs, float ys)
    {
        return create("BIG", x, z, rotation, t: xs, u: ys);
    }
    public bool createInter(float x, float z, float rotation, bool n, bool e, bool s, bool w, string nt, string et, string st, string wt, float xs, string nameOv)
    {
        return create("INT", x, z, rotation, n, e, s, w, l:nt, m:et, n:st, o:wt, t:xs, u:xs, v:nameOv);
    }
    public bool createHexInt(float x, float z, float rotation, bool n, bool e, bool s, bool w, bool ne, bool se, bool sw, bool nw, string nt, string et, string st, string wt, string net, string set, string swt, string nwt, float xs, string nameOv)
    {
        return create("HEX", x, z, rotation, n, e, s, w, ne, se, sw, nw, nt, et, st, wt, net, set, swt, nwt, t: xs, u: xs, v:nameOv);
    }


    private bool create(string type = "", float a = 0, float b = 0, float c = 0, bool d = false, bool e = false, bool f = false, bool g = false, bool h = false, bool i = false, bool j = false, bool k = false, string l = "", string m = "", string n = "", string o = "", string p = "", string q = "", string r = "", string s = "", float t = 1, float u = 1, string v = "")
    {

        float x = (float)a;
        float z = (float)b;
        float rotation = (float)c;
        float xScale = (float)t;
        float yScale = (float)u;

        bool north = d;
        bool east = e;
        bool south = f;
        bool west = g;
        bool northeast = h;
        bool southeast = i;
        bool southwest = j;
        bool northwest = k;

        string northType = l;
        string eastType = m;
        string southType = n;
        string westType = o;

        string northeastType = p;
        string southeastType = q;
        string southwestType = r;
        string northwestType = s;

        string name = " null ";
        string nameOverride = v;

        GameObject roadObj = new GameObject();
        GameObject outer = new GameObject();

        if(type == "ROAD")
        {
            name = "SmallRoad";
            roadObj = Instantiate(road, new Vector3(x, 0, z), Quaternion.identity);
        }
        else if(type == "MED")
        {
            name = "MediumRoad";
            roadObj = Instantiate(medRoad, new Vector3(x, 0, z), Quaternion.identity);
        }
        else if(type == "BIG")
        {
            name = "BigRoad";
            roadObj = Instantiate(bigroad, new Vector3(x, 0, z), Quaternion.identity);
        }
        else if(type == "INT")
        {
            name = "Intersection";
            if (nameOverride != "")
            {
                name = nameOverride;
                east = true;
                eastType = "SMALL";
            }
            roadObj = Instantiate(intersection, new Vector3(x, 0, z), Quaternion.identity);
            cityFourIntersectionController cfic = roadObj.GetComponent<cityFourIntersectionController>();
            cfic.north = north;
            cfic.east = east;
            cfic.south = south;
            cfic.west = west;

            cfic.Ntype = northType;
            cfic.Stype = southType;
            cfic.Etype = eastType;
            cfic.Wtype = westType;

            cfic.updateRoad();
        }
        else if(type == "HEX")
        {
            name = "HexIntersection";
            if(nameOverride != "")
            {
                name = nameOverride;
                east = true;
                eastType = "SMALL";
            }
            roadObj = Instantiate(hexInter, new Vector3(x, 0, z), Quaternion.identity);
            HexInterCont hic = roadObj.GetComponent<HexInterCont>();
            hic.north = north;
            hic.east = east;
            hic.south = south;
            hic.west = west;
            hic.northEast = northeast;
            hic.southEast = southeast;
            hic.southWest = southwest;
            hic.northWest = northwest;

            hic.nType = northType;
            hic.neType = northeastType;
            hic.eType = eastType;
            hic.seType = southeastType;
            hic.sType = southType;
            hic.swType = southwestType;
            hic.wType = westType;
            hic.nwType = northwestType;
            //not yet implemented road size types.

            hic.updateRoad();
        }

        else
        {
            Object.Destroy(roadObj);
            Object.Destroy(outer);
            return false;
        }

        outer.name = name;

        roadObj.transform.SetParent(outer.transform);
        roadObj.transform.localScale = new Vector3(xScale, 1, yScale);
        roadObj.transform.localEulerAngles = new Vector3(0, rotation, 0);

        outer.transform.SetParent(city.transform);

        //Object.Destroy(GameObject.Find("New Game Object"));

        return false;
    }

}
