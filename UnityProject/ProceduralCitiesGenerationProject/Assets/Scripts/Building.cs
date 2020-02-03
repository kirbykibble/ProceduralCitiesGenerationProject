using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : MonoBehaviour
{
    public List<Texture> windows;
    public List<Texture> doors;
    public List<Texture> bottom;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //length, width, height, x pos, z pos, y pos (default 0)
    public void createCubic(float l, float w, float h, float x, float z, float y = 0)
    {
        createBuilding(l, w, h, x, y, z, true);
    }

    //opposite, adjacent, height, x pos, z pos, y pos (default 0)
    public void createTri(float o, float a, float h, float x, float z, float y = 0)
    {
        createBuilding(o, a, h, x, y, z, isTri: true);
    }

    //radius, height, x pos, z pos, y pos (default 0)
    public void createCirc(float r, float h, float x, float z, float y = 0)
    {
        createBuilding(r, c: h, d: x, e: y, f: z, isCirc: true);
    }

    private void createBuilding(float a = 0, float b = 0, float c = 0, float d = 0, float e = 0, float f = 0, bool isCubic = false, bool isTri = false, bool isCirc = false)
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

        if (isCubic)
        {
            float length = (float)a;
            float width = (float)b;

            GameObject building = new GameObject();
            building.name = "Building";
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



        }
        if(isTri)
        {
            float opp = (float)a;
            float adj = (float)b;
        }
        if(isCirc)
        {
            float radius = (float)a;
        }
    }
}
