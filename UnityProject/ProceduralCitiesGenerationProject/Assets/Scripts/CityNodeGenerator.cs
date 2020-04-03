using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.IO;

public class CityNodeGenerator : MonoBehaviour
{
    private int size;
    private int blockSize; // blocksize must be a divisor of size! Script will automatically resize size if need be.
    private float removalRand; //lower is better!
    private float diagRand;
    private float numConnectors; // connectors will always appear on the west border.

    private Color bg = Color.black;
    private Color node = Color.blue;
    private Color road = Color.yellow;
    private Color noConnection = Color.red;
    private Color diagonalRoad = Color.green;
    private Color outConnector = Color.magenta;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool generateNode(int s, int bs, float rr, float dr, float nc)
    {
        size = s;
        blockSize = bs;
        removalRand = (float)rr;
        diagRand = (float)dr;
        numConnectors = (float)nc;

        Texture2D nodeMap = new Texture2D(size, size);

        //sets background
        for (int x = 0; x < size; x++)
        {
            for (int y = 0; y < size; y++)
            {
                nodeMap.SetPixel(x, y, bg);
            }
        }

        //creates nodes.
        for (int x = 0; x < size; x += blockSize)
        {
            //for left to right
            for (int y = 0; y < size; y += blockSize)
            {
                nodeMap.SetPixel(x, y, node);
            }
        }

        //creates lateral road connections. 
        for (int x = 0; x < size; x += blockSize)
        {
            for (int y = 0; y < size; y += blockSize)
            {
                //if a road has been decidedly removed. Only removes one road per node. Ensures that no node has more than 2 removed connections.
                if (y + 1 < size && nodeCheck(nodeMap, true, x, y) && Random.value < removalRand) //if the north pixel isn't out of bounds
                {
                    nodeMap.SetPixel(x, y + 1, noConnection);
                    nodeMap.SetPixel(x, y + blockSize - 1, noConnection);
                }
                if (x + 1 < size && nodeCheck(nodeMap, false, x, y) && Random.value < removalRand) //if the east pixel isn't out of bounds
                {
                    nodeMap.SetPixel(x + 1, y, noConnection);
                    nodeMap.SetPixel(x + blockSize - 1, y, noConnection);
                }

            }
        }

        //creates diagonal connections.
        for (int x = blockSize; x < size; x += blockSize)
        {
            for (int y = 0; y < size; y += blockSize)
            {
                if (x + 1 < size && y + 1 < size && Random.value < diagRand && diagCheck(nodeMap, true, x, y)) //if the top right connection isn't out of bounds. 
                {
                    nodeMap.SetPixel(x + 1, y + 1, diagonalRoad);
                    nodeMap.SetPixel(x + blockSize - 1, y + blockSize - 1, diagonalRoad);
                }
                if (x - 1 > 0 && y + 1 < size && Random.value < diagRand && diagCheck(nodeMap, false, x, y) && x - blockSize != 0) //if the top left connection isn't out of bounds
                {
                    nodeMap.SetPixel(x - 1, y + 1, diagonalRoad);
                    nodeMap.SetPixel(x - blockSize + 1, y + blockSize - 1, diagonalRoad);
                }
            }
        }

        //joins roads visually.
        for (int x = 0; x < size; x += blockSize)
        {
            for (int y = 0; y < size; y += blockSize)
            {
                if (nodeMap.GetPixel(x, y + 1) == bg)
                {
                    int yPos = y + 1;
                    while (nodeMap.GetPixel(x, yPos) != node)
                    {
                        nodeMap.SetPixel(x, yPos, road);
                        yPos++;
                    }
                }
                if (nodeMap.GetPixel(x + 1, y) == bg)
                {
                    int xPos = x + 1;
                    while (nodeMap.GetPixel(xPos, y) != node)
                    {
                        nodeMap.SetPixel(xPos, y, road);
                        xPos++;
                    }
                }
                if (nodeMap.GetPixel(x + 1, y + 1) == diagonalRoad)
                {
                    int xPos = x + 1;
                    int yPos = y + 1;
                    while (nodeMap.GetPixel(xPos, yPos) != node)
                    {
                        nodeMap.SetPixel(xPos, yPos, diagonalRoad);
                        yPos++;
                        xPos++;
                    }
                }
                if (nodeMap.GetPixel(x - 1, y + 1) == diagonalRoad)
                {
                    int xPos = x - 1;
                    int yPos = y + 1;
                    while (nodeMap.GetPixel(xPos, yPos) != node)
                    {
                        nodeMap.SetPixel(xPos, yPos, diagonalRoad);
                        yPos++;
                        xPos--;
                    }
                }
            }
        }

        int count = 0;
        float chance = (float)blockSize / size;
        while (count < numConnectors)
        {
            for (int y = 0; y < size; y += blockSize)
            {
                if (Random.value < chance && nodeMap.GetPixel(0, y) == node)
                {
                    nodeMap.SetPixel(0, y, outConnector);
                    count++;
                }
                if (count >= numConnectors)
                {
                    break;
                }
            }
        }

        byte[] bytes = nodeMap.EncodeToPNG();
        Object.Destroy(nodeMap);
        File.WriteAllBytes(Application.dataPath + "/../Assets/Debug/nodeMapOut.png", bytes);

        return true;
    }
    
    bool diagCheck(Texture2D map, bool tr, int x, int y) //pass the pixel of the current node.
    {
        bool result = true;
        //if making a diagonal connection from the top right, then you must check to see if the neighboring node isn't making a connection
        if (tr)
        {
            if (map.GetPixel(x + blockSize - 1, y + 1) == diagonalRoad)
            {
                result = false;
            }
        }
        if(!tr)
        {
            if (map.GetPixel(x - blockSize + 1, y + 1) == diagonalRoad)
            {
                result = false;
            }
        }

        return result;
    }

    bool nodeCheck(Texture2D map, bool yVal, int x, int y) //pass the pixel of the node itself.
    {
        bool result = true; //if the node is good to use.
        int count = 0;
        int nCount = 0;

        if(y+1 >= size || map.GetPixel(x, y + 1) == noConnection)
        {
            count++;
        }
        if (x + 1 >= size || map.GetPixel(x+1,y) == noConnection)
        {
            count++;
        }
        if (y - 1 < 0 || map.GetPixel(x, y - 1) == noConnection)
        {
            count++;
        }
        if (x - 1 < 0 || map.GetPixel(x-1,y) == noConnection)
        {
            count++;
        }
        if(yVal)
        {
            if (map.GetPixel(x, y + 1 - blockSize) == noConnection)
            {
                nCount++;
            }
            if (map.GetPixel(x, y - 1 - blockSize) == noConnection)
            {
                nCount++;
            }
            if (map.GetPixel(x + 1, y - blockSize) == noConnection)
            {
                nCount++;
            }
            if (map.GetPixel(x - 1, y - blockSize) == noConnection)
            {
                nCount++;
            }
        }
        if(count >= 3 || nCount >= 2)
        {
            result = false;
        }
        //Debug.Log("COUNT: " + count + " ||| " + "NCOUNT: " + nCount );
        return result;
    }
}
