using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : MonoBehaviour
{
    public List<Texture> windows;
    public List<Texture> doors;
    public List<Texture> bottom;
    public List<Texture> helipads;

    public float multipleDoorRand;
    public float heliRand;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //length, width, height, x pos, z pos, y pos (default 0)
    public void createCubic(float l, float w, float h, float x, float z, float y = 0, float rotation = 0, bool landmark = false)
    {
        createBuilding(l, w, h, x, y, z, true, rotation: rotation, landmark: landmark);
    }

    //opposite, adjacent, height, x pos, z pos, y pos (default 0)
    public void createTri(float o, float a, float h, float x, float z, float y = 0, float rotation = 0)
    {
        createBuilding(o, a, h, x, y, z, isTri: true, rotation: rotation);
    }

    //radius, height, x pos, z pos, y pos (default 0)
    public void createCirc(float r, float h, float x, float z, float y = 0)
    {
        createBuilding(r, c: h, d: x, e: y, f: z, isCirc: true);
    }

    private void createBuilding(float a = 0, float b = 0, float c = 0, float d = 0, float e = 0, float f = 0, bool isCubic = false, bool isTri = false, bool isCirc = false, bool landmark = false, float rotation = 0)
    {
        //parameters a and b are variable. 
        float x = (float)d;
        float y = (float)e;
        float z = (float)f;
        float height = (float)c; // total height

        float lowerHeight = Random.Range(7, 10);
        float upperHeight = height - lowerHeight;

        int idx = Random.Range(0, windows.Count);
        Texture chosenTexture = windows[idx];

        idx = Random.Range(0, bottom.Count);
        Texture bottomTex = bottom[idx];

        idx = Random.Range(0, doors.Count);
        Texture doorTex = doors[idx];

        idx = Random.Range(0, helipads.Count);
        Texture heliTex = helipads[idx];

        //building placement
        Vector3 bPos = new Vector3(x, y, z);
        GameObject building = new GameObject();
        building.name = "Building";


        if (isCubic)
        {
            float length = (float)a;
            float width = (float)b;

            List<GameObject> parts = new List<GameObject>();
            string[] names = { "north", "east", "south", "west", "northU", "eastU", "southU", "westU", "roof" };

            for(int i = 0; i < names.Length; i++)
            {
                GameObject temp = GameObject.CreatePrimitive(PrimitiveType.Quad);
                temp.name = names[i];
                parts.Add(temp);
                temp.transform.SetParent(building.transform);

                if(i < 4 && i != 8)
                {
                    //set floor texture.
                    Material mat = temp.GetComponent<Renderer>().material;
                    mat.SetTexture("_MainTex", bottomTex);

                    int tw = chosenTexture.width;
                    int th = chosenTexture.height;

                    //float scale = (float)width / tw / 10;
                    float scale = 0.25f;

                    mat.SetTextureScale("_MainTex", new Vector2(scale, 1));
                }
                else if(i >= 4 && i != 8)
                {
                    Material mat = temp.GetComponent<Renderer>().material;
                    mat.SetTexture("_MainTex", chosenTexture);

                    int tw = chosenTexture.width; //leave at 1. don't change unless absolutely necessary. 
                    int th = chosenTexture.height; //scale per height. round down? 

                    float heightScale = (float)th / 10;
                    float scale = (float)upperHeight / heightScale;

                    mat.SetTextureScale("_MainTex", new Vector2(1, scale));
                }
                else if(i == 8) // for roof
                {
                    Material roof = temp.GetComponent<Renderer>().material;
                    Color rc = Color.black;
                    float mod = Random.value;
                    rc.r += mod;
                    rc.g += mod;
                    rc.b += mod;

                    roof.SetColor("_Color", rc);

                    if(Random.value < heliRand)
                    {
                        roof.SetTexture("_MainTex", heliTex);
                    }
                }

            }
            //size 
            Vector3 EW = new Vector3(length, lowerHeight, 1);  //length applies to east and west facing.
            Vector3 NS = new Vector3(width, lowerHeight, 1);   //width applies to north and south facing.
            Vector3 EWU = new Vector3(length, upperHeight, 1);  //upper vector
            Vector3 NSU = new Vector3(width, upperHeight, 1);   //upper vector

            //rotation
            Vector3 north = new Vector3(0,180, 0);
            Vector3 east = new Vector3(0, 270, 0);
            Vector3 south = new Vector3(0, 0, 0);
            Vector3 west = new Vector3(0, 90, 0);

            //lower positions
            Vector3 northPos = new Vector3(0, (float) lowerHeight / 2, (float) length / 2);
            Vector3 eastPos = new Vector3((float) width / 2, (float) lowerHeight / 2, 0);
            Vector3 southPos = new Vector3(0, (float)lowerHeight / 2, -(float)length / 2);
            Vector3 westPos = new Vector3(-(float)width / 2, (float)lowerHeight / 2, 0);

            //upper positions
            Vector3 northPosU = new Vector3(0, (float) lowerHeight + (upperHeight / 2), (float) length / 2);
            Vector3 eastPosU = new Vector3((float) width / 2, (float) lowerHeight + (upperHeight / 2), 0);
            Vector3 southPosU = new Vector3(0, (float)lowerHeight + (upperHeight / 2), -(float)length / 2);
            Vector3 westPosU = new Vector3(-(float)width / 2, (float)lowerHeight + (upperHeight / 2), 0);

            //roof
            Vector3 roofSize = new Vector3(length, width, 1);
            Vector3 roofRotation = new Vector3(90, 90, 0);
            Vector3 roofPosition = new Vector3(0, height, 0);

            //lower
            parts[0].transform.localScale = NS;
            parts[0].transform.localEulerAngles = north;
            parts[0].transform.position = northPos;

            parts[1].transform.localScale = EW;
            parts[1].transform.localEulerAngles = east;
            parts[1].transform.position = eastPos;

            parts[2].transform.localScale = NS;
            parts[2].transform.localEulerAngles = south;
            parts[2].transform.position = southPos;

            parts[3].transform.localScale = EW;
            parts[3].transform.localEulerAngles = west;
            parts[3].transform.position = westPos;

            //upper
            parts[4].transform.localScale = NSU;
            parts[4].transform.localEulerAngles = north;
            parts[4].transform.position = northPosU;

            parts[5].transform.localScale = EWU;
            parts[5].transform.localEulerAngles = east;
            parts[5].transform.position = eastPosU;

            parts[6].transform.localScale = NSU;
            parts[6].transform.localEulerAngles = south;
            parts[6].transform.position = southPosU;

            parts[7].transform.localScale = EWU;
            parts[7].transform.localEulerAngles = west;
            parts[7].transform.position = westPosU;

            //roof
            parts[8].transform.localScale = roofSize;
            parts[8].transform.localEulerAngles = roofRotation;
            parts[8].transform.position = roofPosition;

            //main building construction completed. 
            //Door Placement algorithm. 
            int doorsToPlace = 1;

            if(Random.value < multipleDoorRand)
            {
                doorsToPlace++;
            }
            //north, east, south, west
            bool[] placedDoors = { false, false, false, false };
            int loc = 0;

            while (doorsToPlace > 0)
            {
                if(Random.value < 0.25f && !placedDoors[loc])
                {
                    //place a door.
                    doorsToPlace--;
                    GameObject temp = GameObject.CreatePrimitive(PrimitiveType.Quad);
                    temp.name = "door" + placedDoors;
                    parts.Add(temp);
                    temp.transform.SetParent(building.transform);

                    Vector3 doorLocation = new Vector3();
                    Vector3 doorRotation = new Vector3();
                    switch (loc)
                    {
                        case 0: //north
                            doorLocation = northPos;
                            doorLocation.z += 0.001f;
                            doorRotation = north;
                            break;
                        case 1: //east
                            doorLocation = eastPos;
                            doorLocation.x += 0.001f;
                            doorRotation = east;
                            break;       
                        case 2: //south
                            doorLocation = southPos;
                            doorLocation.z -= 0.001f;
                            doorRotation = south;
                            break;
                        case 3: //west
                            doorLocation = westPos;
                            doorLocation.z -= 0.001f;
                            doorRotation = west;
                            break;
                    }

                    temp.transform.localScale = new Vector3(2, 2, 2);
                    doorLocation.y = temp.transform.localScale.y / 2;
                    temp.transform.position = doorLocation;
                    temp.transform.eulerAngles = doorRotation;

                    Material mat = temp.GetComponent<Renderer>().material;
                    mat.SetTexture("_MainTex", doorTex);
                    mat.SetTextureScale("_MainTex", new Vector2(1, 1));
                }
                loc++;
                if(loc > 3)
                {
                    loc = 0;
                }
            }
        }
        if(isTri)
        {
            float opp = (float)a;
            float adj = (float)b;

            float hyp = (float)Mathf.Sqrt((opp * opp) + (adj * adj));

            //currently unused.
            /*List<GameObject> parts = new List<GameObject>();
            string[] names = { "Opp", "Adj", "Hyp", "OppU", "AdjU", "HypU" };
            for (int i = 0; i < names.Length; i++)
            {
                GameObject temp = GameObject.CreatePrimitive(PrimitiveType.Quad);
                temp.name = names[i];
                parts.Add(temp);
                temp.transform.SetParent(building.transform);

                float width = 1f;
                switch(i)
                {
                    case 0:
                        width = opp;
                        break;
                    case 1:
                        width = adj;
                        break;
                    case 2:
                        width = hyp;
                        break;
                }

                if (i < 3)
                {
                    //set floor texture.
                    Material mat = temp.GetComponent<Renderer>().material;
                    mat.SetTexture("_MainTex", bottomTex);

                    int tw = chosenTexture.width;
                    int th = chosenTexture.height;

                    float scale = (float)width / tw / 10;

                    mat.SetTextureScale("_MainTex", new Vector2(scale, 1));
                }
                else if (i >= 3)
                {
                    Material mat = temp.GetComponent<Renderer>().material;
                    mat.SetTexture("_MainTex", chosenTexture);

                    int tw = chosenTexture.width; //leave at 1. don't change unless absolutely necessary. 
                    int th = chosenTexture.height; //scale per height. round down? 

                    float heightScale = (float)th / 10;
                    float scale = (float)upperHeight / heightScale;

                    mat.SetTextureScale("_MainTex", new Vector2(1, scale));
                }
            }

            //size
            Vector3 oppSize = new Vector3(opp, lowerHeight, 1);
            Vector3 adjSize = new Vector3(adj, lowerHeight, 1);
            Vector3 hypSize = new Vector3(hyp, lowerHeight, 1);

            Vector3 oppSizeU = new Vector3(opp, upperHeight, 1);
            Vector3 adjSizeU = new Vector3(adj, upperHeight, 1);
            Vector3 hypSizeU = new Vector3(hyp, upperHeight, 1);

            //rotation
            float r = (float)Mathf.Atan(opp / adj) * Mathf.Rad2Deg;
            Vector3 oppRot = new Vector3(0, 0, 0);
            Vector3 adjRot = new Vector3(0, 90, 0);
            Vector3 hypRot = new Vector3(0, r, 0);

            //positions
            //Vector3 oppPos = new Vector3();

            //apply changes.
            parts[0].transform.localScale = oppSize;
            parts[0].transform.eulerAngles = oppRot;

            parts[1].transform.localScale = adjSize;
            parts[1].transform.eulerAngles = adjRot;

            parts[2].transform.localScale = hypSize;
            parts[2].transform.eulerAngles = hypRot;

            parts[3].transform.localScale = oppSizeU;
            parts[3].transform.eulerAngles = oppRot;

            parts[4].transform.localScale = adjSizeU;
            parts[4].transform.eulerAngles = adjRot;

            parts[5].transform.localScale = hypSizeU;
            parts[5].transform.eulerAngles = hypRot;
            */


        }
        if (isCirc)
        {
            float radius = (float)a;
            float circumference = (float)(radius * 2) * Mathf.PI;
            lowerHeight = Random.Range(4, 7); //overwrites the lowerheight. Looks too tall if using the rect scales.
            upperHeight = height - lowerHeight;

            GameObject upper = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
            GameObject lower = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
            GameObject roof = GameObject.CreatePrimitive(PrimitiveType.Cylinder); // 0.000000000001
            upper.name = "upper";
            lower.name = "lower";
            roof.name = "roof";

            Material mat = upper.GetComponent<Renderer>().material;
            mat.SetTexture("_MainTex", chosenTexture);
            int tw = chosenTexture.width;
            int th = chosenTexture.height;
            float scale = (float)circumference / tw * 2;
            float heightScale = (float)height / th * 10;
            mat.SetTextureScale("_MainTex", new Vector2(scale, heightScale));

            mat = lower.GetComponent<Renderer>().material;
            mat.SetTexture("_MainTex", bottomTex);
            tw = bottomTex.width;
            th = bottomTex.height;
            scale = (float)circumference / tw * 2;
            mat.SetTextureScale("_MainTex", new Vector2(scale, 1));

            Material r = roof.GetComponent<Renderer>().material;
            Color rc = Color.black;
            float mod = Random.value;
            rc.r += mod;
            rc.g += mod;
            rc.b += mod;
            if (Random.value < heliRand)
            {
                r.SetTexture("_MainTex", heliTex);
            }
            r.SetColor("_Color", rc);

            upper.transform.SetParent(building.transform);
            lower.transform.SetParent(building.transform);
            roof.transform.SetParent(building.transform);

            upper.transform.localScale = new Vector3(radius, upperHeight, radius);
            lower.transform.localScale = new Vector3(radius, lowerHeight, radius);
            roof.transform.localScale = new Vector3(radius, 0.000000000001f, radius);

            upper.transform.position = new Vector3(0, lowerHeight * 2 + upperHeight, 0);
            lower.transform.position = new Vector3(0, lowerHeight, 0);
            roof.transform.position = new Vector3(0, lowerHeight * 2 + upperHeight * 2 + 0.01f, 0);

        }

        building.transform.position = bPos;
        building.transform.eulerAngles = new Vector3(0, rotation, 0);
    }
}
