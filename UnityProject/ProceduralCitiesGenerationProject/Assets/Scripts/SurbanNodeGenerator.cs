using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.IO;

public class SurbanNodeGenerator : MonoBehaviour
{
    private int size;
    private int blockSize;
    private float CDSRand; // Cul-De-Sac likeliness to spawn
    private float numConnectors; // connectors will always appear on the east border.
    private int offsetSize; // should be smaller than the size. ##DEPRECATED##

    private Color bg = Color.black;
    private Color node = Color.blue;
    private Color road = Color.yellow;
    private Color cds = Color.green;
    private Color noConnection = Color.red;
    private Color outConnector = Color.magenta;
    private Color offsetNodes = Color.cyan;
    private Color offsetCDS = Color.white;

    public bool createSurbanNodemap(int s, int bs, float cdsr, float nc)
    {
        size = s;
        blockSize = bs;
        CDSRand = cdsr;
        numConnectors = nc;

        Texture2D nodeMap = new Texture2D(size, size);

        //sets background
        for (int x = 0; x < size; x++)
        {
            for (int y = 0; y < size; y++)
            {
                nodeMap.SetPixel(x, y, bg);
            }
        }

        //creates nodes. Randomly assigns culdesacs
        for (int x = 0; x < size; x += blockSize)
        {
            //for left to right
            for (int y = 0; y < size; y += blockSize)
            {
                if (Random.value < CDSRand && cdsCheck(nodeMap, x, y))
                {
                    nodeMap.SetPixel(x, y, cds);
                }
                else
                {
                    nodeMap.SetPixel(x, y, node);
                }
            }
        }

        byte[] bytes2 = nodeMap.EncodeToPNG();
        File.WriteAllBytes(Application.dataPath + "/../Assets/Debug/TESTOUT.png", bytes2);

        //assigns non-connections for culdesacs.
        for (int x = 0; x < size; x += blockSize)
        {
            for (int y = 0; y < size; y += blockSize)
            {
                //if current node is assigned as a cds.
                if (nodeMap.GetPixel(x, y) == cds) // this isn't the issue. Reports correct nodes.
                {
                    // if not out of bounds AND passes rng. If passes, this connection is deemed as the only connector for the cds
                    bool isNorth = false;
                    bool isEast = false;
                    bool isSouth = false;
                    bool isWest = false;

                    bool assigned = false;

                    while (!assigned) //ensures that something gets assigned.
                    {
                        isNorth = false;
                        isEast = false;
                        isSouth = false;
                        isWest = false;
                        assigned = false;

                        float rand = Random.value;
                        //if the east pixel isn't out of bounds.  AND the rng passes AND the next node isn't a cds. 
                        //somewhere along the path a random pixel is being read as a cds.
                        if (x + 1 < size && rand <= 0.25f && !(nodeMap.GetPixel(x + blockSize, y) == cds)) //for east
                        {
                            isEast = true;
                            assigned = true;
                        }
                        else if (x - 1 > 0 && rand > 0.25f && rand <= 0.5f && !(nodeMap.GetPixel(x - blockSize, y) == cds)) // west
                        {
                            isWest = true;
                            assigned = true;
                        }
                        else if (y + 1 < size && rand > 0.5f && rand <= 0.75f && !(nodeMap.GetPixel(x, y + blockSize) == cds)) // north
                        {
                            isNorth = true;
                            assigned = true;
                        }
                        else if (y - 1 > 0 && rand > 0.75f && !(nodeMap.GetPixel(x, y - blockSize) == cds)) // south
                        {
                            isSouth = true;
                            assigned = true;
                        }
                    }

                    /*
                    Debug.Log("NORTH: " + isNorth + " AT: " + x + ", " + y);
                    Debug.Log("EAST: " + isEast + " AT: " + x + ", " + y);
                    Debug.Log("SOUTH: " + isSouth + " AT: " + x + ", " + y);
                    Debug.Log("WEST: " + isWest + " AT: " + x + ", " + y);
                    */

                    if (!isNorth) // if the only connection is NOT north, then remove the north connection
                    {
                        nodeMap.SetPixel(x, y + 1, noConnection); // remove north connection
                        nodeMap.SetPixel(x, y - 1 + blockSize, noConnection);

                        //Debug.Log("I HAVE DRAWN AT: " + x + ", " + y + " (+1) || MY PERMISSION IS NORTH: " + !isNorth);
                        //Debug.Log("I HAVE DRAWN AT: " + x + ", " + (y-1+blockSize) + " (y - 1 + blocksize) || MY PERMISSION IS NORTH: " + !isNorth);

                    }
                    if (!isEast)
                    {
                        nodeMap.SetPixel(x + 1, y, noConnection); // remove east connection
                        nodeMap.SetPixel(x - 1 + blockSize, y, noConnection);

                        // Debug.Log("I HAVE DRAWN AT: " + (x+1) + " (+1) , " + y + " || MY PERMISSION IS EAST: " + !isEast);
                        // Debug.Log("I HAVE DRAWN AT: " + (x-1+blockSize) + " (-1+blockSize), " + y + " || MY PERMISSION IS NORTH: " + !isEast);
                    }
                    if (!isSouth)
                    {
                        nodeMap.SetPixel(x, y - 1, noConnection); // remove south connection
                        nodeMap.SetPixel(x, y + 1 - blockSize, noConnection);

                        // Debug.Log("I HAVE DRAWN AT: " + x + ", " + (y-1) +  " (-1) || MY PERMISSION IS SOUTH: " + !isSouth);
                        // Debug.Log("I HAVE DRAWN AT: " + x + ", " + (y + 1 - blockSize) + " (y + 1 - blocksize) || MY PERMISSION IS SOUTH: " + !isSouth);
                    }
                    if (!isWest)
                    {
                        nodeMap.SetPixel(x - 1, y, noConnection); // remove west connection
                        nodeMap.SetPixel(x + 1 - blockSize, y, noConnection);

                        // Debug.Log("I HAVE DRAWN AT: " + (x - 1) + " (-1) , " + y + " || MY PERMISSION IS WEST: " + !isWest);
                        // Debug.Log("I HAVE DRAWN AT: " + (x + 1 + blockSize) + " (+1-blockSize), " + y + " || MY PERMISSION IS NORTH: " + !isWest);
                    }
                }
                else
                {
                    //Debug.Log("THERE SHOULDNT BE ANY DRAWING AT: " + x + " , " + y);
                }
            }
        }

        //creates nodes. Randomly assigns culdesacs
        for (int x = 0; x < size; x += blockSize)
        {
            //for left to right
            for (int y = 0; y < size; y += blockSize)
            {
                if (nodeMap.GetPixel(x, y) == noConnection)
                {
                    nodeMap.SetPixel(x, y, node);
                    nodeMap.SetPixel(x - 2 + blockSize, y, bg);
                    nodeMap.SetPixel(x + 2 - blockSize, y, bg);
                    nodeMap.SetPixel(x, y - 2 + blockSize, bg);
                    nodeMap.SetPixel(x, y + 2 - blockSize, bg);
                }
            }
        }

        //visually draws roads.
        for (int x = 0; x < size; x += blockSize)
        {
            for (int y = 0; y < size; y += blockSize)
            {
                if (nodeMap.GetPixel(x, y + 1) == bg)
                {
                    int yPos = y + 1;
                    while (nodeMap.GetPixel(x, yPos) != node && nodeMap.GetPixel(x, yPos) != cds)
                    {
                        nodeMap.SetPixel(x, yPos, road);
                        yPos++;
                    }
                }
                if (nodeMap.GetPixel(x + 1, y) == bg)
                {
                    int xPos = x + 1;
                    while (nodeMap.GetPixel(xPos, y) != node && nodeMap.GetPixel(xPos, y) != cds)
                    {
                        nodeMap.SetPixel(xPos, y, road);
                        xPos++;
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
                if (Random.value < chance && nodeMap.GetPixel(size - 1, y) == node)
                {
                    nodeMap.SetPixel(size - 1, y, outConnector);
                    count++;
                }
                if (count >= numConnectors)
                {
                    break;
                }
            }
        }

        /* ###DEPRECATED###
        if (offsetSize > size) // if someone accidentally makes the offset bigger than the size, than the offset size automatically becomes half the size.
        {
            offsetSize = size / 2;
        }
        offsetSize += offsetSize % blockSize - 1; // rounds up to the nearest block. I hope.
        for (int x = offsetSize; x < size; x += blockSize)
        {
            for (int y = offsetSize; y < size; y += blockSize)
            {
                if (nodeMap.GetPixel(x, y) == cds)
                {
                    nodeMap.SetPixel(x, y, offsetCDS);
                }
                else
                {
                    nodeMap.SetPixel(x, y, offsetNodes);
                }
            }
        }*/

        byte[] bytes = nodeMap.EncodeToPNG();
        Object.Destroy(nodeMap);
        File.WriteAllBytes(Application.dataPath + "/../Assets/Debug/SurbanNodeMapOut.png", bytes);
        return true;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //checks to see if the culdesac isn't neighboring more than 1 other culdesac. Also checks the nighbors neighbors in order to prevent isolated cds.
    bool cdsCheck(Texture2D map, int x, int y)
    {
        bool result = true;
        int count = 0;

        //checks lateral neighbors
        if(map.GetPixel(x + blockSize, y) == cds || x + 1 > size)
        {
            count++;
        }
        if (map.GetPixel(x - blockSize, y) == cds || x - 1 < 0)
        {
            count++;
        }
        if (map.GetPixel(x, y + blockSize) == cds || y + 1 > size)
        {
            count++;
        }
        if (map.GetPixel(x, y - blockSize) == cds || y - 1 < 0)
        {
            count++;
        }

        //checks diagonal neighbors
        if (map.GetPixel(x - blockSize, y - blockSize) == cds)
        {
            count++;
        }
        if (map.GetPixel(x + blockSize, y - blockSize) == cds)
        {
            count++;
        }
        if (map.GetPixel(x - blockSize, y + blockSize) == cds)
        {
            count++;
        }
        if (map.GetPixel(x + blockSize, y + blockSize) == cds)
        {
            count++;
        }
        
        //checks double lateral neighbor
        if (map.GetPixel(x, y + (blockSize * 2)) == cds)
        {
            count++;
        }
        if (map.GetPixel(x, y - (blockSize * 2)) == cds)
        {
            count++;
        }
        if (map.GetPixel(x + (blockSize * 2), y) == cds)
        {
            count++;
        }
        if (map.GetPixel(x - (blockSize * 2), y) == cds)
        {
            count++;
        }


        if (count >= 2)
        {
            result = false;
        }
        return result;
    }
}
