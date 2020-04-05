using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class csConnector : MonoBehaviour
{
    public GameObject road;

    public GameObject city;
    public GameObject subUrban;

    public Vector3 debug;

    public GameObject cConnector;
    public GameObject sConnector;

    public bool connect(int citySize, int surbanSize, int cityMaxSize, int surbanMaxSize)
    {
        cConnector = GameObject.Find("cConnector1");
        sConnector = GameObject.Find("sConnector1");

        subUrban.transform.localScale = new Vector3(2 * 0.882352941f, 2 * 0.882352941f, 2 * 0.882352941f);

        Vector3 distance = cConnector.transform.GetChild(0).position - sConnector.transform.GetChild(0).position;
        debug = distance;

        int cityDimensions = citySize * cityMaxSize;
        int surbanDimensions = surbanSize * surbanMaxSize;

        float amtToMove = (cityDimensions / 2) + (surbanDimensions / 2);
        distance -= new Vector3(surbanMaxSize * 10, 0, 0);
        subUrban.transform.position += distance;

        GameObject roadObj = new GameObject();


        Vector3 position = cConnector.transform.GetChild(0).position - sConnector.transform.GetChild(0).position;
        position = sConnector.transform.GetChild(0).position + (position / 2);
        position.x -= 7.529f;

        roadObj = Instantiate(road, position, Quaternion.identity);
        roadObj.name = "CONNECTOR";

        float scale = (float)surbanMaxSize / 10 * 2 * 0.882352941f;
        
        roadObj.transform.localScale = new Vector3(scale * 4.16666667f, 1, scale);
        return true;


        //return false;
    }
}
