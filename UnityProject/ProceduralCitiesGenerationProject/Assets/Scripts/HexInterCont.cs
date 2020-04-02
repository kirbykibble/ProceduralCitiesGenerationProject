using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexInterCont : MonoBehaviour
{
    private GameObject nL;
    private GameObject neL;
    private GameObject eL;
    private GameObject seL;
    private GameObject sL;
    private GameObject swL;
    private GameObject wL;
    private GameObject nwL;

    private GameObject nR;
    private GameObject neR;
    private GameObject eR;
    private GameObject seR;
    private GameObject sR;
    private GameObject swR;
    private GameObject wR;
    private GameObject nwR;

    //for stop Lights
    private GameObject Ln;
    private GameObject Lne;
    private GameObject Le;
    private GameObject Lse;
    private GameObject Ls;
    private GameObject Lsw;
    private GameObject Lw;
    private GameObject Lnw;

    public string nType;
    public string neType;
    public string eType;
    public string seType;
    public string sType;
    public string swType;
    public string wType;
    public string nwType;

    public bool north;
    public bool northEast;
    public bool east;
    public bool southEast;
    public bool south;
    public bool southWest;
    public bool west;
    public bool northWest;

    public bool debug;

    private void Start()
    {
        if (debug) updateRoad();
    }

    public void updateRoad()
    {
        nL = this.gameObject.transform.Find("northLeft").gameObject;
        neL = this.gameObject.transform.Find("northeastLeft").gameObject;
        eL = this.gameObject.transform.Find("eastLeft").gameObject;
        seL = this.gameObject.transform.Find("southeastLeft").gameObject;
        sL = this.gameObject.transform.Find("southLeft").gameObject;
        swL = this.gameObject.transform.Find("southwestLeft").gameObject;
        wL = this.gameObject.transform.Find("westLeft").gameObject;
        nwL = this.gameObject.transform.Find("northwestLeft").gameObject;

        nR = this.gameObject.transform.Find("northRight").gameObject;
        neR = this.gameObject.transform.Find("northeastRight").gameObject;
        eR = this.gameObject.transform.Find("eastRight").gameObject;
        seR = this.gameObject.transform.Find("southeastRight").gameObject;
        sR = this.gameObject.transform.Find("southRight").gameObject;
        swR = this.gameObject.transform.Find("southwestRight").gameObject;
        wR = this.gameObject.transform.Find("westRight").gameObject;
        nwR = this.gameObject.transform.Find("northwestRight").gameObject;

        Ln = this.gameObject.transform.Find("signNorth").gameObject;
        Lne = this.gameObject.transform.Find("signNorthEast").gameObject;
        Le = this.gameObject.transform.Find("signEast").gameObject;
        Lse = this.gameObject.transform.Find("signSouthEast").gameObject;
        Ls = this.gameObject.transform.Find("signSouth").gameObject;
        Lsw = this.gameObject.transform.Find("signSouthWest").gameObject;
        Lw = this.gameObject.transform.Find("signWest").gameObject;
        Lnw = this.gameObject.transform.Find("signNorthWest").gameObject;

        Ln.SetActive(false);
        Lne.SetActive(false);
        Le.SetActive(false);
        Lse.SetActive(false);
        Ls.SetActive(false);
        Lsw.SetActive(false);
        Lw.SetActive(false);
        Lnw.SetActive(false);

        if(north) //if the north road is open.
        {
            Ls.SetActive(true);

            Vector3 transformL = nL.transform.localScale;
            Vector3 transformR = nR.transform.localScale;

            Vector3 posL = nL.transform.position;
            Vector3 posR = nR.transform.position;

            if(nType == "LARGE")
            {
                transformL.z = 0.7f;
                transformR.z = 0.7f;

                nR.transform.localScale = transformR;
                nL.transform.localScale = transformL;

                nL.transform.position += new Vector3(4.336f, 0, 0);
                nR.transform.position += new Vector3(-4.336f, 0, 0);
            }
            else if (nType == "MEDIUM")
            {
                transformL.z = 1.5f;
                transformR.z = 1.5f;

                nR.transform.localScale = transformR;
                nL.transform.localScale = transformL;

                nL.transform.position += new Vector3(3.965f, 0, 0);
                nR.transform.position += new Vector3(-3.965f, 0, 0);
            }
            else if (nType == "SMALL")
            {
                transformL.z = 2.5f;
                transformR.z = 2.5f;

                nR.transform.localScale = transformR;
                nL.transform.localScale = transformL;

                nL.transform.position += new Vector3(3.695f, 0, 0);
                nR.transform.position += new Vector3(-3.695f, 0, 0);

            }
        }
        if(northEast)
        {
            Lsw.SetActive(true);

            Vector3 transformL = neL.transform.localScale;
            Vector3 transformR = neR.transform.localScale;

            Vector3 posL = neL.transform.position;
            Vector3 posR = neR.transform.position;

            if (neType == "LARGE")
            {
                transformL.z = 0.7f;
                transformR.z = 0.7f;

                neL.transform.localScale = transformR;
                neR.transform.localScale = transformL;

                neL.transform.position += new Vector3(2.998f, 0, -2.998f);
                neR.transform.position += new Vector3(-2.998f, 0, 2.998f);
            }
            else if (neType == "MEDIUM")
            {
                transformL.z = 1.5f;
                transformR.z = 1.5f;

                neL.transform.localScale = transformR;
                neR.transform.localScale = transformL;

                neL.transform.position += new Vector3(2.798f, 0, -2.798f);
                neR.transform.position += new Vector3(-2.798f, 0, 2.798f);
            }
            else if (neType == "SMALL")
            {
                transformL.z = 2.5f;
                transformR.z = 2.5f;

                neR.transform.localScale = transformR;
                neL.transform.localScale = transformL;

                neL.transform.position += new Vector3(2.607f, 0, -2.607f);
                neR.transform.position += new Vector3(-2.607f, 0, 2.607f);

            }
        }
        if(east)
        {
            Lw.SetActive(true);

            Vector3 transformL = eL.transform.localScale;
            Vector3 transformR = eR.transform.localScale;

            Vector3 posL = eL.transform.position;
            Vector3 posR = eR.transform.position;

            if (eType == "LARGE")
            {
                transformL.z = 0.7f;
                transformR.z = 0.7f;

                eR.transform.localScale = transformR;
                eL.transform.localScale = transformL;

                eL.transform.position += new Vector3(0, 0, -4.336f);
                eR.transform.position += new Vector3(0, 0, 4.336f);
            }
            else if (eType == "MEDIUM")
            {
                transformL.z = 1.5f;
                transformR.z = 1.5f;

                eR.transform.localScale = transformR;
                eL.transform.localScale = transformL;

                eL.transform.position += new Vector3(0, 0, -3.965f);
                eR.transform.position += new Vector3(0, 0, 3.965f);
            }
            else if (eType == "SMALL")
            {
                transformL.z = 2.5f;
                transformR.z = 2.5f;

                eR.transform.localScale = transformR;
                eL.transform.localScale = transformL;

                eL.transform.position += new Vector3(0, 0, -3.695f);
                eR.transform.position += new Vector3(0, 0, 3.695f);

            }
        }
        if(southEast)
        {
            Lnw.SetActive(true);

            Vector3 transformL = seL.transform.localScale;
            Vector3 transformR = seR.transform.localScale;

            Vector3 posL = seL.transform.position;
            Vector3 posR = seR.transform.position;

            if (seType == "LARGE")
            {
                transformL.z = 0.7f;
                transformR.z = 0.7f;

                seL.transform.localScale = transformR;
                seR.transform.localScale = transformL;

                seL.transform.position += new Vector3(-2.998f, 0, -2.998f);
                seR.transform.position += new Vector3(2.998f, 0, 2.998f);
            }
            else if (seType == "MEDIUM")
            {
                transformL.z = 1.5f;
                transformR.z = 1.5f;

                seL.transform.localScale = transformR;
                seR.transform.localScale = transformL;

                seL.transform.position += new Vector3(-2.798f, 0, -2.798f);
                seR.transform.position += new Vector3(2.798f, 0, 2.798f);
            }
            else if (seType == "SMALL")
            {
                transformL.z = 2.5f;
                transformR.z = 2.5f;

                seR.transform.localScale = transformR;
                seL.transform.localScale = transformL;

                seL.transform.position += new Vector3(-2.607f, 0, -2.607f);
                seR.transform.position += new Vector3(2.607f, 0, 2.607f);

            }
        }
        if(south)
        {
            Ln.SetActive(true);

            Vector3 transformL = sL.transform.localScale;
            Vector3 transformR = sR.transform.localScale;

            Vector3 posL = sL.transform.position;
            Vector3 posR = sR.transform.position;

            if (sType == "LARGE")
            {
                transformL.z = 0.7f;
                transformR.z = 0.7f;

                sR.transform.localScale = transformR;
                sL.transform.localScale = transformL;

                sL.transform.position += new Vector3(-4.336f, 0, 0);
                sR.transform.position += new Vector3(4.336f, 0, 0);
            }
            else if (sType == "MEDIUM")
            {
                transformL.z = 1.5f;
                transformR.z = 1.5f;

                sR.transform.localScale = transformR;
                sL.transform.localScale = transformL;

                sL.transform.position += new Vector3(-3.965f, 0, 0);
                sR.transform.position += new Vector3(3.965f, 0, 0);
            }
            else if (sType == "SMALL")
            {
                transformL.z = 2.5f;
                transformR.z = 2.5f;

                sR.transform.localScale = transformR;
                sL.transform.localScale = transformL;

                sL.transform.position += new Vector3(-3.695f, 0, 0);
                sR.transform.position += new Vector3(3.695f, 0, 0);

            }
        }
        if (southWest)
        {
            Lne.SetActive(true);

            Vector3 transformL = swL.transform.localScale;
            Vector3 transformR = swR.transform.localScale;

            Vector3 posL = swL.transform.position;
            Vector3 posR = swR.transform.position;

            if (swType == "LARGE")
            {
                transformL.z = 0.7f;
                transformR.z = 0.7f;

                swL.transform.localScale = transformR;
                swR.transform.localScale = transformL;

                swL.transform.position += new Vector3(-2.998f, 0, 2.998f);
                swR.transform.position += new Vector3(2.998f, 0, -2.998f);
            }
            else if (swType == "MEDIUM")
            {
                transformL.z = 1.5f;
                transformR.z = 1.5f;

                swL.transform.localScale = transformR;
                swR.transform.localScale = transformL;

                swL.transform.position += new Vector3(-2.798f, 0, 2.798f);
                swR.transform.position += new Vector3(2.798f, 0, -2.798f);
            }
            else if (swType == "SMALL")
            {
                transformL.z = 2.5f;
                transformR.z = 2.5f;

                swR.transform.localScale = transformR;
                swL.transform.localScale = transformL;

                swL.transform.position += new Vector3(-2.607f, 0, 2.607f);
                swR.transform.position += new Vector3(2.607f, 0, -2.607f);

            }
        }
        if(west)
        {
            Le.SetActive(true);

            Vector3 transformL = wL.transform.localScale;
            Vector3 transformR = wR.transform.localScale;

            Vector3 posL = wL.transform.position;
            Vector3 posR = wR.transform.position;

            if (wType == "LARGE")
            {
                transformL.z = 0.7f;
                transformR.z = 0.7f;

                wR.transform.localScale = transformR;
                wL.transform.localScale = transformL;

                wL.transform.position += new Vector3(0, 0, 4.336f);
                wR.transform.position += new Vector3(0, 0, -4.336f);
            }
            else if (wType == "MEDIUM")
            {
                transformL.z = 1.5f;
                transformR.z = 1.5f;

                wR.transform.localScale = transformR;
                wL.transform.localScale = transformL;

                wL.transform.position += new Vector3(0, 0, 3.965f);
                wR.transform.position += new Vector3(0, 0, -3.965f);
            }
            else if (wType == "SMALL")
            {
                transformL.z = 2.5f;
                transformR.z = 2.5f;

                wR.transform.localScale = transformR;
                wL.transform.localScale = transformL;

                wL.transform.position += new Vector3(0, 0, 3.695f);
                wR.transform.position += new Vector3(0, 0, -3.695f);

            }
        }
        if (northWest)
        {
            Lse.SetActive(true);

            Vector3 transformL = nwL.transform.localScale;
            Vector3 transformR = nwR.transform.localScale;

            Vector3 posL = nwL.transform.position;
            Vector3 posR = nwR.transform.position;

            if (nwType == "LARGE")
            {
                transformL.z = 0.7f;
                transformR.z = 0.7f;

                nwL.transform.localScale = transformR;
                nwR.transform.localScale = transformL;

                nwL.transform.position += new Vector3(2.998f, 0, 2.998f);
                nwR.transform.position += new Vector3(-2.998f, 0, -2.998f);
            }
            else if (nwType == "MEDIUM")
            {
                transformL.z = 1.5f;
                transformR.z = 1.5f;

                nwL.transform.localScale = transformR;
                nwR.transform.localScale = transformL;

                nwL.transform.position += new Vector3(2.798f, 0, 2.798f);
                nwR.transform.position += new Vector3(-2.798f, 0, -2.798f);
            }
            else if (nwType == "SMALL")
            {
                transformL.z = 2.5f;
                transformR.z = 2.5f;

                nwR.transform.localScale = transformR;
                nwL.transform.localScale = transformL;

                nwL.transform.position += new Vector3(2.607f, 0, 2.607f);
                nwR.transform.position += new Vector3(-2.607f, 0, -2.607f);

            }
        }
    }
}
