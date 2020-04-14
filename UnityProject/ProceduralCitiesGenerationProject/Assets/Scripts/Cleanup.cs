using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cleanup : MonoBehaviour
{
    public void clean()
    {
        GameObject[] objs = GameObject.FindGameObjectsWithTag("Untagged");
        //Debug.Log("should run with: " + objs.Length);
        for(int i = 0; i < objs.Length; i++)
        {
            //Debug.Log(objs[i].name);
            if (objs[i].name == "New Game Object")
            {
                Debug.Log("DESTROYING");
                Object.Destroy(objs[i]);
            }
        }
    }
}
