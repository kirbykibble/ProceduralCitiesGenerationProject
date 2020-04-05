using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorResizer : MonoBehaviour
{
    public GameObject floor;
    public bool resizeFloor(int citySize, int surbanSize, int cityMaxSize, int surbanMaxSize)
    {
        int size = citySize + surbanSize;
        int units = cityMaxSize + surbanMaxSize;

        float scale = size * units;
        floor.transform.localScale = new Vector3(scale / 2, 1, scale / 2);
        floor.transform.position = new Vector3(scale / 2, -0.01f, scale / 2);

        return true;
    }
}
