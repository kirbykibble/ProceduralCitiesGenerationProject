using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingDeprecated : MonoBehaviour
{
    private float length;
    private float width;
    private float height;
    public List<Texture> windows;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void createBuilding(float l, float w, float h, float x, float z, float y = 0)
    {
        int idx = Random.Range(0, windows.Count);
        Texture chosenTexture = windows[idx]; 
        GameObject building;

        length = (float) l;
        width = (float) w;
        height = (float) h;

        building = GameObject.CreatePrimitive(PrimitiveType.Cube);
        building.name = "Building";

        //sets building scale
        Vector3 bScale = new Vector3(l, h, w);
        building.transform.localScale = bScale;

        //sets building position. Translates building upwards an amount so that the building doesn't clip into the ground.
        y += (float)h / 2;
        Vector3 bPosition = new Vector3(x, y, z);
        building.transform.position = bPosition;

        Material bMat = building.GetComponent<Renderer>().material;
        bMat.SetTexture("_MainTex", chosenTexture);
        // unity units equate to 100 pixels per unit.
        int tw = chosenTexture.width; //leave at 1. don't change unless absolutely necessary. 
        int th = chosenTexture.height; //scale per height. round down? 

        float heightScale = (float) th / 10;
        float scale = (float)h / heightScale;

        bMat.SetTextureScale("_MainTex", new Vector2(1, scale));
    }
}
