using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cityFourIntersectionController : MonoBehaviour
{
    private GameObject road;

    //for stop signs
    private GameObject sNorth;
    private GameObject sSouth;
    private GameObject sEast;
    private GameObject sWest;

    public bool north;
    public bool south;
    public bool east;
    public bool west;

    //whether or not these directions are open. 
    //if true, it means that there is a road leading to this location.
    private GameObject nLeft;
    private GameObject sLeft;
    private GameObject eLeft;
    private GameObject wLeft;

    private GameObject nRight;
    private GameObject sRight;
    private GameObject eRight;
    private GameObject wRight;

    public string Ntype; //Will either be "SMALL", "MEDIUM" or "LARGE"
    public string Etype; //Will either be "SMALL", "MEDIUM" or "LARGE"
    public string Stype; //Will either be "SMALL", "MEDIUM" or "LARGE"
    public string Wtype; //Will either be "SMALL", "MEDIUM" or "LARGE"

    // Start is called before the first frame update
    void Start()
    {
        road = this.gameObject.transform.Find("Road").gameObject;
        nLeft = this.gameObject.transform.Find("NLeft").gameObject;
        eLeft = this.gameObject.transform.Find("ELeft").gameObject;
        sLeft = this.gameObject.transform.Find("SLeft").gameObject;
        wLeft = this.gameObject.transform.Find("WLeft").gameObject;

        nRight = this.gameObject.transform.Find("NRight").gameObject;
        eRight = this.gameObject.transform.Find("ERight").gameObject;
        sRight = this.gameObject.transform.Find("SRight").gameObject;
        wRight = this.gameObject.transform.Find("WRight").gameObject;

        sNorth = this.gameObject.transform.Find("signNorth").gameObject;
        sSouth = this.gameObject.transform.Find("signSouth").gameObject;
        sEast = this.gameObject.transform.Find("signEast").gameObject;
        sWest = this.gameObject.transform.Find("signWest").gameObject;

        sNorth.SetActive(false);
        sSouth.SetActive(false);
        sEast.SetActive(false);
        sWest.SetActive(false);

        int count = 0;

        if (!north) count++;
        if (!south) count++;
        if (!east) count++;
        if (!west) count++;

        if (count == 3 || count >= 4) //if there are two closed road lanes. Meaning that this is a corner.
        {
            Debug.Log("ERROR: There was an error in Road Generation.");
        }
        else if (count == 1) // for a three way intersection
        {
            if (!north) sSouth.SetActive(true);
            if (!south) sNorth.SetActive(true);
            if (!east) sWest.SetActive(true);
            if (!west) sEast.SetActive(true);
        }
        else //we don't do anything for two.
        {
            sNorth.SetActive(true);
            sSouth.SetActive(true);
            sEast.SetActive(true);
            sWest.SetActive(true);
        }

        if (sSouth.activeSelf) sSouth.transform.position = new Vector3(-8.2f, 0.2f, -8.2f);
        if (sWest.activeSelf) sWest.transform.position = new Vector3(-8.2f, 0.2f, 8.2f);
        if (sNorth.activeSelf) sNorth.transform.position = new Vector3(8.2f, 0.2f, 8.2f);
        if (sEast.activeSelf) sEast.transform.position = new Vector3(8.2f, 0.2f, -8.2f);

        if (north)
        {
            if (Ntype == "LARGE")
            {
                nRight.SetActive(false);
                nLeft.SetActive(false);

            }
            else if (Ntype == "MEDIUM") // is medium or small
            {
                nRight.transform.position = new Vector3(6.75f, 0.1f, -9);
                nRight.transform.localScale = new Vector3(2.5f, .2f, 2);

                nLeft.transform.position = new Vector3(-6.75f, 0.1f, -9);
                nLeft.transform.localScale = new Vector3(2.5f, .2f, 2);
                if (sSouth.activeSelf)
                {
                    sSouth.transform.position = new Vector3(-5.8f, 0.2f, -8.2f);
                }
            }
            else if (Ntype == "SMALL")
            {
                nRight.transform.position = new Vector3(5.5f, 0.1f, -9);
                nRight.transform.localScale = new Vector3(5, .2f, 2);

                nLeft.transform.position = new Vector3(-5.5f, 0.1f, -9);
                nLeft.transform.localScale = new Vector3(5, .2f, 2);
                if (sSouth.activeSelf)
                {
                    sSouth.transform.position = new Vector3(-3.2f, 0.2f, -8.2f);
                }
            }
            else
            {
                Debug.Log("ERROR: Incorrect type given");
            }
        }
        else
        {
            nRight.transform.position = new Vector3(4, 0.1f, -9);
            nRight.transform.localScale = new Vector3(8, .2f, 2);

            nLeft.transform.position = new Vector3(-4, 0.1f, -9);
            nLeft.transform.localScale = new Vector3(8, .2f, 2);
        }
        if(east)
        {
            if (Etype == "LARGE")
            {
                eRight.SetActive(false);
                eLeft.SetActive(false);
            }
            else if (Etype == "MEDIUM")
            {
                eRight.transform.position = new Vector3(-9f, 0.1f, -6.75f);
                eRight.transform.localScale = new Vector3(2.5f, .2f, 2);

                eLeft.transform.position = new Vector3(-9, 0.1f, 6.75f);
                eLeft.transform.localScale = new Vector3(2.5f, .2f, 2);
                if (sWest.activeSelf)
                {
                    sWest.transform.position = new Vector3(-8.2f, 0.2f, 5.8f);
                }
            }
            else if (Etype == "SMALL")
            {
                eRight.transform.position = new Vector3(-9f, 0.1f, -5.5f);
                eRight.transform.localScale = new Vector3(5, .2f, 2);

                eLeft.transform.position = new Vector3(-9, 0.1f, 5.5f);
                eLeft.transform.localScale = new Vector3(5, .2f, 2);
                if (sWest.activeSelf)
                {
                    sWest.transform.position = new Vector3(-8.2f, 0.2f, 3.2f);
                }
            }
            else
            {
                Debug.Log("ERROR: Incorrect type given");
            }
        }
        else
        {
            eRight.transform.position = new Vector3(-9f, 0.1f, -4f);
            eRight.transform.localScale = new Vector3(8, .2f, 2);

            eLeft.transform.position = new Vector3(-9, 0.1f, 4);
            eLeft.transform.localScale = new Vector3(8, .2f, 2);
        }
        if(south)
        {
            if (Stype == "LARGE")
            {
                sRight.SetActive(false);
                sLeft.SetActive(false);

            }
            else if (Stype == "MEDIUM")
            {
                sRight.transform.position = new Vector3(6.75f, 0.1f, 9);
                sRight.transform.localScale = new Vector3(2.5f, .2f, 2);

                sLeft.transform.position = new Vector3(-6.75f, 0.1f, 9);
                sLeft.transform.localScale = new Vector3(2.5f, .2f, 2);

                if (sNorth.activeSelf)
                {
                    sNorth.transform.position = new Vector3(5.8f, 0.2f, 8.2f);
                }
            }
            else if (Stype == "SMALL")
            {
                sRight.transform.position = new Vector3(5.5f, 0.1f, 9);
                sRight.transform.localScale = new Vector3(5, .2f, 2);

                sLeft.transform.position = new Vector3(-5.5f, 0.1f, 9);
                sLeft.transform.localScale = new Vector3(5, .2f, 2);

                if (sNorth.activeSelf)
                {
                    sNorth.transform.position = new Vector3(3.2f, 0.2f, 8.2f);
                }
            }
            else
            {
                Debug.Log("ERROR: Incorrect Type given");
            }
        }
        else
        {
            sRight.transform.position = new Vector3(4, 0.1f, 9);
            sRight.transform.localScale = new Vector3(8, .2f, 2);

            sLeft.transform.position = new Vector3(-4, 0.1f, 9);
            sLeft.transform.localScale = new Vector3(8, .2f, 2);
        }
        if(west)
        {
            if (Wtype == "LARGE")
            {
                wRight.SetActive(false);
                wLeft.SetActive(false);
            }
            else if (Wtype == "MEDIUM")
            {
                wRight.transform.position = new Vector3(9f, 0.1f, -6.75f);
                wRight.transform.localScale = new Vector3(2.5f, .2f, 2);

                wLeft.transform.position = new Vector3(9, 0.1f, 6.75f);
                wLeft.transform.localScale = new Vector3(2.5f, .2f, 2);

                if (sEast.activeSelf)
                {
                    sEast.transform.position = new Vector3(8.2f, 0.2f, -5.8f);
                }
            }
            else if (Wtype == "SMALL")
            {
                wRight.transform.position = new Vector3(9f, 0.1f, -5.5f);
                wRight.transform.localScale = new Vector3(5, .2f, 2);

                wLeft.transform.position = new Vector3(9, 0.1f, 5.5f);
                wLeft.transform.localScale = new Vector3(5, .2f, 2);
                if (sEast.activeSelf)
                {
                    sEast.transform.position = new Vector3(8.2f, 0.2f, -3.2f);
                }
            }
            else
            {
                Debug.Log("ERROR: Incorrect Type given");
            }
        }
        else
        {
            wRight.transform.position = new Vector3(9f, 0.1f, -4);
            wRight.transform.localScale = new Vector3(8, .2f, 2);

            wLeft.transform.position = new Vector3(9, 0.1f, 4);
            wLeft.transform.localScale = new Vector3(8, .2f, 2);
        }
    }
}
