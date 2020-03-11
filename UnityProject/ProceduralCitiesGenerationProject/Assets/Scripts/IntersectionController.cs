using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntersectionController : MonoBehaviour
{
    private GameObject road;
    private GameObject rNorth;
    private GameObject rSouth;
    private GameObject rEast;
    private GameObject rWest;

    //for stop signs
    private GameObject sNorth;
    private GameObject sSouth;
    private GameObject sEast;
    private GameObject sWest;

    //whether or not these directions are open. 
    //if true, it means that there is a road leading to this location.
    public bool north;
    public bool south;
    public bool east;
    public bool west;

    // Start is called before the first frame update
    public void updateRoad()
    {
        road = this.gameObject.transform.Find("Road").gameObject;
        rNorth = this.gameObject.transform.Find("North").gameObject;
        rSouth = this.gameObject.transform.Find("South").gameObject;
        rEast = this.gameObject.transform.Find("East").gameObject;
        rWest = this.gameObject.transform.Find("West").gameObject;

        sNorth = this.gameObject.transform.Find("signNorth").gameObject;
        sSouth = this.gameObject.transform.Find("signSouth").gameObject;
        sEast = this.gameObject.transform.Find("signEast").gameObject;
        sWest = this.gameObject.transform.Find("signWest").gameObject;

        sNorth.SetActive(false);
        sSouth.SetActive(false);
        sEast.SetActive(false);
        sWest.SetActive(false);

        if (north) rNorth.SetActive(false);
        if (south) rSouth.SetActive(false);
        if (east) rEast.SetActive(false);
        if (west) rWest.SetActive(false);

        int count = 0;

        if (!north) count++;
        if (!south) count++;
        if (!east) count++;
        if (!west) count++;

        if(count == 3 || count >= 4) //if there are two closed road lanes. Meaning that this is a corner.
        {
            Debug.Log("ERROR: There was an error in Road Generation.");
        }
        else if(count == 1) // for a three way intersection
        {
            if (!north) sSouth.SetActive(true);
            if (!south) sNorth.SetActive(true);
            if (!east) sWest.SetActive(true);
            if (!west) sEast.SetActive(true);
        }
        else //we don't do anything for two.
        {
            if(Random.value < 0.4)
            {
                sNorth.SetActive(true);
                sSouth.SetActive(true);
            }
            if(Random.value < 0.4f)
            {
                sEast.SetActive(true);
                sWest.SetActive(true);
            }
        }
    }
}
