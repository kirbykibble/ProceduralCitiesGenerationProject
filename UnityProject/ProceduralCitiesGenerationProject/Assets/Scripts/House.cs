using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class House : MonoBehaviour
{
    public List<GameObject> houseBasic;
    public List<GameObject> houseRich;
    public List<GameObject> houseHighDens;
    public List<GameObject> buildingUtil;

    public GameObject school;
    public GameObject uni;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void createHouse(string type, float x, float z, float rotation = 0, float y = 0)
    {
        //type = HOUSE_SMALL, HOUSE_RICH, HOUSE_DENS, UTILITY, SCHOOL, UNI
        create(x, y, z, rotation, type);
    }

    private bool create(float a = 0, float b = 0.01f, float c = 0, float d = 0, string e = "null")
    {
        string type = e;
        float x = (float) a;
        float y = (float) b;
        float z = (float) c;
        float rot = (float) d; //rotation
        List<GameObject> buildingList = new List<GameObject>();
        string name = " null ";

        switch(type)
        {
            case "HOUSE_SMALL":
                buildingList = houseBasic;
                name = "Small house";
                break;
            case "HOUSE_RICH":
                buildingList = houseRich;
                name = "Rich house";
                break;
            case "HOUSE_DENS":
                buildingList = houseHighDens;
                name = "High Density House";
                break;
            case "UTILITY":
                buildingList = buildingUtil;
                name = "Utility Building";
                break;
            case "SCHOOL":
                break;
            case "UNI":
                break;
            case "null":
                Debug.Log("ERROR: No house type provided.");
                return false;
            default:
                Debug.Log("ERROR: Incorrect house type");
                return false;
        }
        GameObject house = new GameObject();
        GameObject outer = new GameObject();
        if (type != "SCHOOL" && type != "UNI")
        {
            int idx = Random.Range(0, buildingList.Count);
            outer.name = name;
            house = Instantiate(buildingList[idx], new Vector3(x, y, z), Quaternion.identity);
        }
        else
        {
            if(type == "SCHOOL")
            {
                house = Instantiate(school, new Vector3(x, y, z), Quaternion.identity);
                name = "School";
            }
            else if( type == "UNI")
            {
                house = Instantiate(uni, new Vector3(x, y, z), Quaternion.identity);
                name = "Uni";
            }
        } 

        house.transform.SetParent(outer.transform);
        house.transform.localScale = new Vector3(10, 10, 10);
        house.transform.localEulerAngles = new Vector3(0, rot, 0);

        return true;
    }
}
