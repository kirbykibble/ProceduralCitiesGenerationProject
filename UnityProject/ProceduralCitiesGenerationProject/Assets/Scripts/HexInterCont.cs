using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexInterCont : MonoBehaviour
{
    private GameObject n;
    private GameObject ne;
    private GameObject e;
    private GameObject se;
    private GameObject s;
    private GameObject sw;
    private GameObject w;
    private GameObject nw;

    //for stop Lights
    private GameObject Ln;
    private GameObject Lne;
    private GameObject Le;
    private GameObject Lse;
    private GameObject Ls;
    private GameObject Lsw;
    private GameObject Lw;
    private GameObject Lnw;


    public bool north;
    public bool northEast;
    public bool east;
    public bool southEast;
    public bool south;
    public bool southWest;
    public bool west;
    public bool northWest;

    void Start()
    {
        n = this.gameObject.transform.Find("north").gameObject;
        ne = this.gameObject.transform.Find("northeast").gameObject;
        e = this.gameObject.transform.Find("east").gameObject;
        se = this.gameObject.transform.Find("southeast").gameObject;
        s = this.gameObject.transform.Find("south").gameObject;
        sw = this.gameObject.transform.Find("southwest").gameObject;
        w = this.gameObject.transform.Find("west").gameObject;
        nw = this.gameObject.transform.Find("northwest").gameObject;

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
            n.SetActive(false);
            Ls.SetActive(true);
        }
        if(northEast)
        {
            ne.SetActive(false);
            Lsw.SetActive(true);
        }
        if(east)
        {
            e.SetActive(false);
            Lw.SetActive(true);
        }
        if(southEast)
        {
            se.SetActive(false);
            Lnw.SetActive(true);
        }
        if(south)
        {
            s.SetActive(false);
            Ln.SetActive(true);
        }
        if (southWest)
        {
            sw.SetActive(false);
            Lne.SetActive(true);
        }
        if(west)
        {
            w.SetActive(false);
            Le.SetActive(true);
        }
        if (northWest)
        {
            nw.SetActive(false);
            Lse.SetActive(true);
        }
    }
}
