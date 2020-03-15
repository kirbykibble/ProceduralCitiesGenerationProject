using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Linq;

// A majority of the A* script below has been sourced from:
// https://www.geeksforgeeks.org/a-search-algorithm/
// Adapted for use in Unity and to be compatible with current maps.

public class RoadUseMap : MonoBehaviour
{
    private int smallHousePop = 1;
    private int richHousePop = 2;
    private int highDensPop = 6;

    private Color node = Color.blue;
    private Color road = Color.yellow;
    private Color cds = Color.green;

    private Color diagRoad = Color.green;
    private Color bg = Color.white;
    private Color roadDefault = Color.black;

    private Color smallHouse = Color.red;
    private Color richHouse = Color.cyan;
    private Color connector = Color.magenta;
    private Color highDens = new Vector4(0, .4f, 0, 1);

    private List<Color> checker = new List<Color>();
    private int totalPop = 0;
    private int cityConnectors = 0;
    private int nodeSize;
    private int mapSize;

    private List<List<int>> connectors = new List<List<int>>();

    public bool createUseMap()
    {
        checker.Add(node);
        checker.Add(road);
        checker.Add(cds);

        Texture2D houseMap = null;
        Texture2D cityMap = null;
        Texture2D heatmap = null;
        Texture2D useMap = null;
        byte[] fileData;

        if (File.Exists("Assets/Debug/houseMap.png"))
        {
            fileData = File.ReadAllBytes("Assets/Debug/houseMap.png");
            houseMap = new Texture2D(2, 2, TextureFormat.BGRA32, false);
            houseMap.LoadImage(fileData);

            Debug.Log("Successfully Loaded Housemap");
        }
        else
        {
            Debug.Log("ERROR: File not found");
            return false;
        }
        if (File.Exists("Assets/Debug/nodeMapOut.png"))
        {
            fileData = File.ReadAllBytes("Assets/Debug/nodeMapOut.png");
            cityMap = new Texture2D(2, 2, TextureFormat.BGRA32, false);
            cityMap.LoadImage(fileData);

            Debug.Log("Successfully Loaded NodeMap");
        }
        else
        {
            Debug.Log("ERROR: File not found");
            return false;
        }
        if(File.Exists("Assets/Debug/HeatmapOut.png"))
        {
            fileData = File.ReadAllBytes("Assets/Debug/HeatmapOut.png");
            heatmap = new Texture2D(2, 2, TextureFormat.BGRA32, false);
            heatmap.LoadImage(fileData);

            Debug.Log("Successfully Loaded Heatmap");
        }
        else
        {
            Debug.Log("ERROR: File not found");
            return false;
        }

        //First, we calculate total population.
        int size = houseMap.width;
        mapSize = cityMap.width;
        for (int x = 0; x < size; x++)
        {
            for (int y = 0; y < size; y++)
            {
                if (closeToRoad(houseMap, x, y))
                {
                    if (houseMap.GetPixel(x, y) == smallHouse)
                    {
                        totalPop += smallHousePop;
                    }
                    else if (houseMap.GetPixel(x, y) == richHouse)
                    {
                        totalPop += richHousePop;
                    }
                    else if (houseMap.GetPixel(x, y) == highDens)
                    {
                        totalPop += highDensPop;
                    }
                    else if (houseMap.GetPixel(x, y) == connector)
                    {
                        cityConnectors++;
                    }
                }
            }
        }

        int connectorUse = totalPop / cityConnectors / 2; //Rounds to nearest value. Probably will eliminate or create a population, but this is negligible. (negible? Neglible?)

        //next, we recreate the city roadmap except with a usage heatmap.
        size = cityMap.width;
        useMap = new Texture2D(size, size);
        for (int x = 0; x < size; x++)
        {
            for (int y = 0; y < size; y++)
            {
                useMap.SetPixel(x, y, bg);
                if (cityMap.GetPixel(x, y) == road || cityMap.GetPixel(x, y) == diagRoad)
                {
                    useMap.SetPixel(x, y, roadDefault);
                }
                else if (cityMap.GetPixel(x, y) == node)
                {
                    useMap.SetPixel(x, y, node);
                }
                else if (cityMap.GetPixel(x, y) == connector)
                {
                    useMap.SetPixel(x, y, connector);
                }
            }
        }

        Debug.Log("Connector Use" + connectorUse);
        float redAmt = 0;


        //we need to calculate the building density, so we should determine the total heatmap and figure out the building population per resident. 
        for(int x = 0; x < mapSize; x++)
        {
            for (int y = 0; y < mapSize; y++)
            {
                if(useMap.GetPixel(x, y) == bg)
                {
                    redAmt += (float) heatmap.GetPixel(x, y).r;
                }
                if(useMap.GetPixel(x, y) == connector)
                {
                    List<int> temp = new List<int>();
                    temp.Add(x);
                    temp.Add(y);
                    connectors.Add(temp);
                }
            }
        }

        List<int> connectorToUse = connectors[0];
        bool changed = true;

        for (int x = 0; x < mapSize; x++)
        {
            for(int y = 0; y < mapSize; y++)
            {
                if(useMap.GetPixel(x, y) == bg)
                {
                    int buildingPop = Mathf.RoundToInt((heatmap.GetPixel(x, y).r / redAmt) * totalPop);
                    buildingPop /= 2;
                    while(buildingPop > 0)
                    {
                        
                        List<int> cnode = closestNode(useMap, x, y);
                        //Debug.Log("END NODE: " + connectorToUse[0] + ", " + connectorToUse[1]);
                        search(useMap, cnode[0], cnode[1], connectorToUse[0], connectorToUse[1]);
                        buildingPop--;
                        connectorUse--;
                    
                        if(connectorUse <= 0 && changed)
                        {
                            connectorToUse = connectors[1];
                            changed = false;
                        }
                    }
                }
            } 
        }

        /*for(int x = 0; x < mapSize; x++)
        {
            for(int y = 0; y < mapSize; y++)
            {
                if(useMap.GetPixel(x, y) == connector)
                {
                    connect = connectorUse;
                    while(connectorUse > 0)
                    {
                        dx = Random.Range(0, mapSize);
                        dy = Random.Range(0, mapSize);

                        while (useMap.GetPixel(dx, dy) != node)
                        {
                            dx = Random.Range(0, mapSize);
                            dy = Random.Range(0, mapSize);
                        }

                        //Debug.Log("Calculating from: [" + x + ", " + y + "] To Destination: [" + dx + ", " + dy + "]");
                        search(useMap, x, y, dx, dy);
                        connectorUse--;
                    }
                }
            }
        }*/



        byte[] output = useMap.EncodeToPNG();
        Object.Destroy(useMap);
        Object.Destroy(heatmap);
        Object.Destroy(cityMap);
        File.WriteAllBytes(Application.dataPath + "/../Assets/Debug/usemap.png", output);   
        return true;
    }

    bool closeToRoad(Texture2D map, int x, int y)
    {
        for (int i = -1; i <= 1; i++)
        {
            for (int j = -1; j <= 1; j++)
            {
                if (checker.Contains(map.GetPixel(x + i, y + j)))
                {
                    return true;
                }
            }
        }
        return false;
    }

    List<int> closestNode(Texture2D map, int x, int y) //gets the closest node a driver can get to.
    {
        bool found = false;
        List<int> coords = new List<int>();
        int probeSize = 1;
        while (!found)
        {
            for (int i = -probeSize; i <= probeSize; i++)
            {
                for (int j = -probeSize; j <= probeSize; j++)
                {
                    if (map.GetPixel(x + i, y + j) == node)
                    {
                        found = true;
                        coords.Add(x + i);
                        coords.Add(y + j);
                    }
                }
            }
            probeSize++;
        }

        return coords;
    }

    bool isValid(int x, int y)
    {
        return (x >= 0 && x < mapSize && y >= 0 && y < mapSize);
    }

    bool isUnBlocked(Texture2D map, int x, int y)
    {
        //Debug.Log("RED: " + (float) map.GetPixel(x, y).r);
        if(map.GetPixel(x,y) == bg) //if the pixel is blocked. 
        {
            return false;
        }
        return true;
    }

    bool isDestination(int x, int y, int dx, int dy)
    {
        if(x == dx && y == dy)
        {
            return true;
        }
        return false;
    }

    double calcH(int x, int y, int dx, int dy)
    {
        return (double)Mathf.Sqrt(Mathf.Pow(x - dx, 2) + Mathf.Pow(y - dx, 2));
    }

    void drawPath(Texture2D map, cell[,] cellDetails, int dx, int dy)
    {
        //printCellDetails(cellDetails); //Debug Script
        //draws the path
        int x = dx;
        int y = dy;

        Stack<KeyValuePair<int, int>> path = new Stack<KeyValuePair<int, int>>();
        while(!((cellDetails[x, y].parent_i == x) && cellDetails[x, y].parent_j == y))
        {
            path.Push(new KeyValuePair<int, int>(x, y));
            int tempX = cellDetails[x, y].parent_i;
            int tempY = cellDetails[x, y].parent_j;
            x = tempX;
            y = tempY;
        }
        path.Push(new KeyValuePair<int, int>(x, y));
        while(path.Count > 0)
        {
            KeyValuePair<int, int> p = path.Pop();
            if (map.GetPixel(p.Key, p.Value) != node)
            {
                //float temp = (float)map.GetPixel(p.Key, p.Value).r;
                Color tc = map.GetPixel(p.Key, p.Value);
                tc.r += 0.01f;
                map.SetPixel(p.Key, p.Value, tc);
            }
        }

        for (int q = 0; q < mapSize; q++)
        {
            for (int e = 0; e < mapSize; e++)
            {
                Object.Destroy(cellDetails[q, e]);
            }
        }
    }

    bool search(Texture2D map, int x, int y, int dx, int dy)
    {
        //Debug.Log("Calculating from: [" + x + ", " + y + "] To Destination: [" + dx + ", " + dy + "]");
        //for ease of reading
        int startX = x;
        int startY = y;
        int destX = dx;
        int destY = dy;

        if (isValid(startX, startY) == false)
        {
            Debug.Log("Start isn't valid!");
            return false;
        }
        if (isValid(destX, destY) == false)
        {
            //Debug.Log("End isn't valid!");
            return false;
        }
        if (isUnBlocked(map, startX, startY) == false || isUnBlocked(map, destX, destY) == false)
        {
            Debug.Log("ERROR: Start or End is Blocked!!");
            return false;
        }
        if(isDestination(startX, startY, destX, destY) == true)
        {
            Debug.Log("Already at destination!");
            return false;
        }

        List<int> temp = new List<int>();
        List <KeyValuePair<double, KeyValuePair<int, int>>> openList = new List<KeyValuePair<double, KeyValuePair<int, int>>>();
        //Dictionary<double, KeyValuePair<int, int>> openList = new Dictionary<double, KeyValuePair<int, int>>(); // <f, <i , j>>
        bool[,] closedList = new bool[mapSize, mapSize]; // list of all closed nodes;
        cell[,] cellDetails = new cell[mapSize, mapSize]; //list of the cell details, f, g, & h respectively

        int i;
        int j;

        // Populate the cell with items. 
        for (i = 0; i < mapSize; i++)
        {
            for (j = 0; j < mapSize; j++)
            {
                closedList[i, j] = false;
                GameObject t = new GameObject();
                cell tc = t.AddComponent<cell>();
                cellDetails[i, j] = tc;
                cellDetails[i, j].f = float.MaxValue;
                cellDetails[i, j].g = float.MaxValue;
                cellDetails[i, j].h = float.MaxValue;
                cellDetails[i, j].parent_i = -1;
                cellDetails[i, j].parent_j = -1;
                Object.Destroy(t);
            }
        }

        //creates the params of the start node.
        i = startX;
        j = startY;
   
        cellDetails[i, j].f = 0.0f;
        cellDetails[i, j].g = 0.0f;
        cellDetails[i, j].h = 0.0f;
        cellDetails[i, j].parent_i = i;
        cellDetails[i, j].parent_j = j;
        //Debug.Log("CELL [" + i + ", " + j + "] DETAILS [F: " + cellDetails[i, j].f + "] [G:" + cellDetails[i, j].g + "] [H:" + cellDetails[i, j].h + "]");


        openList.Add(new KeyValuePair<double, KeyValuePair<int, int>>(0.0, new KeyValuePair<int, int>(i, j)));

        bool foundDest = false;
        while(openList.Count > 0) //while the list has elements in it
        {
            KeyValuePair<int, int> p = openList.ElementAt(0).Value;
            openList.Remove(openList.ElementAt(0));
            //Debug.Log("LIST COUNT: " + openList.Count);
            i = p.Key;
            j = p.Value;

            closedList[i, j] = true;

            double gNew;
            double hNew;
            double fNew;

            /* (Copied from source material)
                Generating all the 8 successor of this cell 
  
                    N.W   N   N.E 
                      \   |   / 
                       \  |  / 
                    W----Cell----E 
                         / | \ 
                       /   |  \ 
                    S.W    S   S.E 
 
           */

            // For West
            if (isValid(i - 1, j) == true)
            {
                //if the destination cell is the same as the current successor
                if(isDestination(i - 1, j, destX, destY) == true)
                {
                    cellDetails[i - 1, j].parent_i = i;
                    cellDetails[i - 1, j].parent_j = j;
                    drawPath(map, cellDetails, destX, destY);
                    foundDest = true;
                    for (int q = 0; q < mapSize; q++)
                    {
                        for (int e = 0; e < mapSize; e++)
                        {
                            Object.Destroy(cellDetails[q, e]);
                        }
                    }
                    return true;
                }
                //otherwise, if the succesor is on the closed list or blocked, then we gotta ignore it. 
                //Then we do the following to those remaining.
                //Also I kept the `== false` and `==true` just in case c# interprets non null values as true.
                else if (closedList[i-1, j] == false && isUnBlocked(map, i - 1, j) == true)
                {
                    gNew = cellDetails[i, j].g + 1.0; //increments the g value of the north cell.
                    hNew = calcH(i - 1, j, destX, destY); //calculates the h value.
                    fNew = gNew + hNew; // calculates the f value.

                    //if the cell isn't on the openlist, add it to the open list. 
                    //make the current cell the parent of this square
                    //copy down the new f, g, and h values to the cell details
                    //UNLESS 👀👀👀
                    //if it is already on the open list, determine if the pathing is better by comparing the f cost.
                    //if it is, then overwrite the f, g and h values to the cell details
                    if (cellDetails[i - 1, j].f == float.MaxValue || cellDetails[i-1, j].f > fNew)
                    {
                        openList.Add(new KeyValuePair<double, KeyValuePair<int, int>>(fNew, new KeyValuePair<int, int>(i - 1, j)));

                        cellDetails[i - 1, j].f = fNew;
                        cellDetails[i - 1, j].g = gNew;
                        cellDetails[i - 1, j].h = hNew;
                        cellDetails[i - 1, j].parent_i = i;
                        cellDetails[i - 1, j].parent_j = j;
                    }
                }
            }

            //For East
            if(isValid(i + 1, j) == true)
            {
                if (isDestination(i + 1, j, destX, destY) == true) {
                    cellDetails[i + 1, j].parent_i = i;
                    cellDetails[i + 1, j].parent_j = j;
                    drawPath(map, cellDetails, destX, destY);
                    foundDest = true;
                    for (int q = 0; q < mapSize; q++)
                    {
                        for (int e = 0; e < mapSize; e++)
                        {
                            Object.Destroy(cellDetails[q, e]);
                        }
                    }
                    return true;
                }
                else if (closedList[i + 1, j] == false && isUnBlocked(map, i + 1, j) == true)
                {
                    gNew = cellDetails[i, j].g + 1.0;
                    hNew = calcH(i + 1, j, destX, destY);
                    fNew = gNew + hNew;

                    if (cellDetails[i + 1, j].f == float.MaxValue || cellDetails[i + 1, j].f > fNew)
                    {
                        openList.Add(new KeyValuePair<double, KeyValuePair<int, int>>(fNew, new KeyValuePair<int, int>(i + 1, j)));

                        cellDetails[i + 1, j].f = fNew;
                        cellDetails[i + 1, j].g = gNew;
                        cellDetails[i + 1, j].h = hNew;
                        cellDetails[i + 1, j].parent_i = i;
                        cellDetails[i + 1, j].parent_j = j;
                    }
                }
            }

            //for North
            if (isValid(i, j + 1) == true)
            {
                if (isDestination(i, j + 1, destX, destY) == true)
                {
                    cellDetails[i, j + 1].parent_i = i;
                    cellDetails[i, j + 1].parent_j = j;
                    drawPath(map, cellDetails, destX, destY);
                    foundDest = true;
                    for (int q = 0; q < mapSize; q++)
                    {
                        for (int e = 0; e < mapSize; e++)
                        {
                            Object.Destroy(cellDetails[q, e]);
                        }
                    }
                    return true;
                }
                else if (closedList[i, j + 1] == false && isUnBlocked(map, i, j + 1) == true)
                {
                    gNew = cellDetails[i, j].g + 1.0;
                    hNew = calcH(i, j + 1, destX, destY);
                    fNew = gNew + hNew;

                    if (cellDetails[i, j + 1].f == float.MaxValue || cellDetails[i, j + 1].f > fNew)
                    {
                        openList.Add(new KeyValuePair<double, KeyValuePair<int, int>>(fNew, new KeyValuePair<int, int>(i, j + 1)));

                        cellDetails[i, j + 1].f = fNew;
                        cellDetails[i, j + 1].g = gNew;
                        cellDetails[i, j + 1].h = hNew;
                        cellDetails[i, j + 1].parent_i = i;
                        cellDetails[i, j + 1].parent_j = j;
                    }
                }
            }

            //For South
            if (isValid(i, j - 1) == true)
            {
                if (isDestination(i, j - 1, destX, destY) == true)
                {
                    cellDetails[i, j - 1].parent_i = i;
                    cellDetails[i, j - 1].parent_j = j;
                    drawPath(map, cellDetails, destX, destY);
                    foundDest = true;
                    for (int q = 0; q < mapSize; q++)
                    {
                        for (int e = 0; e < mapSize; e++)
                        {
                            Object.Destroy(cellDetails[q, e]);
                        }
                    }
                    return true;
                }
                else if (closedList[i, j - 1] == false && isUnBlocked(map, i, j - 1) == true)
                {
                    gNew = cellDetails[i, j].g + 1.0;
                    hNew = calcH(i, j - 1, destX, destY);
                    fNew = gNew + hNew;

                    if (cellDetails[i, j - 1].f == float.MaxValue || cellDetails[i, j - 1].f > fNew)
                    {
                        openList.Add(new KeyValuePair<double, KeyValuePair<int, int>>(fNew, new KeyValuePair<int, int>(i, j - 1)));

                        cellDetails[i, j - 1].f = fNew;
                        cellDetails[i, j - 1].g = gNew;
                        cellDetails[i, j - 1].h = hNew;
                        cellDetails[i, j - 1].parent_i = i;
                        cellDetails[i, j - 1].parent_j = j;
                    }
                }
            }

            //North West
            if (isValid(i - 1, j + 1) == true)
            {
                if (isDestination(i - 1, j + 1, destX, destY) == true)
                {
                    cellDetails[i - 1, j + 1].parent_i = i;
                    cellDetails[i - 1, j + 1].parent_j = j;
                    drawPath(map, cellDetails, destX, destY);
                    foundDest = true;
                    for (int q = 0; q < mapSize; q++)
                    {
                        for (int e = 0; e < mapSize; e++)
                        {
                            Object.Destroy(cellDetails[q, e]);
                        }
                    }
                    return true;
                }
                else if (closedList[i - 1, j + 1] == false && isUnBlocked(map, i - 1, j + 1) == true)
                {
                    gNew = cellDetails[i, j].g + 1.414;
                    hNew = calcH(i - 1, j + 1, destX, destY);
                    fNew = gNew + hNew;

                    if (cellDetails[i - 1, j + 1].f == float.MaxValue || cellDetails[i - 1, j + 1].f > fNew)
                    {
                        openList.Add(new KeyValuePair<double, KeyValuePair<int, int>>(fNew, new KeyValuePair<int, int>(i - 1, j + 1)));

                        cellDetails[i - 1, j + 1].f = fNew;
                        cellDetails[i - 1, j + 1].g = gNew;
                        cellDetails[i - 1, j + 1].h = hNew;
                        cellDetails[i - 1, j + 1].parent_i = i;
                        cellDetails[i - 1, j + 1].parent_j = j;
                    }
                }
            }

            //South West
            if (isValid(i - 1, j - 1) == true)
            {
                if (isDestination(i - 1, j - 1, destX, destY) == true)
                {
                    cellDetails[i - 1, j - 1].parent_i = i;
                    cellDetails[i - 1, j - 1].parent_j = j;
                    drawPath(map, cellDetails, destX, destY);
                    foundDest = true;
                    for (int q = 0; q < mapSize; q++)
                    {
                        for (int e = 0; e < mapSize; e++)
                        {
                            Object.Destroy(cellDetails[q, e]);
                        }
                    }
                    return true;
                }
                else if (closedList[i - 1, j - 1] == false && isUnBlocked(map, i - 1, j - 1) == true)
                {
                    gNew = cellDetails[i, j].g + 1.414;
                    hNew = calcH(i - 1, j - 1, destX, destY);
                    fNew = gNew + hNew;

                    if (cellDetails[i - 1, j - 1].f == float.MaxValue || cellDetails[i - 1, j - 1].f > fNew)
                    {
                        openList.Add(new KeyValuePair<double, KeyValuePair<int, int>>(fNew, new KeyValuePair<int, int>(i - 1, j - 1)));

                        cellDetails[i - 1, j - 1].f = fNew;
                        cellDetails[i - 1, j - 1].g = gNew;
                        cellDetails[i - 1, j - 1].h = hNew;
                        cellDetails[i - 1, j - 1].parent_i = i;
                        cellDetails[i - 1, j - 1].parent_j = j;
                    }
                }
            }

            //North East
            if (isValid(i + 1, j + 1) == true)
            {
                if (isDestination(i + 1, j + 1, destX, destY) == true)
                {
                    cellDetails[i + 1, j + 1].parent_i = i;
                    cellDetails[i + 1, j + 1].parent_j = j;
                    drawPath(map, cellDetails, destX, destY);
                    foundDest = true;
                    for (int q = 0; q < mapSize; q++)
                    {
                        for (int e = 0; e < mapSize; e++)
                        {
                            Object.Destroy(cellDetails[q, e]);
                        }
                    }
                    return true;
                }
                else if (closedList[i + 1, j + 1] == false && isUnBlocked(map, i + 1, j + 1) == true)
                {
                    gNew = cellDetails[i, j].g + 1.414;
                    hNew = calcH(i + 1, j + 1, destX, destY);
                    fNew = gNew + hNew;

                    if (cellDetails[i + 1, j + 1].f == float.MaxValue || cellDetails[i + 1, j + 1].f > fNew)
                    {
                        openList.Add(new KeyValuePair<double, KeyValuePair<int, int>>(fNew, new KeyValuePair<int, int>(i + 1, j + 1)));

                        cellDetails[i + 1, j + 1].f = fNew;
                        cellDetails[i + 1, j + 1].g = gNew;
                        cellDetails[i + 1, j + 1].h = hNew;
                        cellDetails[i + 1, j + 1].parent_i = i;
                        cellDetails[i + 1, j + 1].parent_j = j;
                    }
                }
            }

            //South East
            if (isValid(i + 1, j - 1) == true)
            {
                if (isDestination(i + 1, j - 1, destX, destY) == true)
                {
                    cellDetails[i + 1, j - 1].parent_i = i;
                    cellDetails[i + 1, j - 1].parent_j = j;
                    drawPath(map, cellDetails, destX, destY);
                    foundDest = true;
                    for (int q = 0; q < mapSize; q++)
                    {
                        for (int e = 0; e < mapSize; e++)
                        {
                            Object.Destroy(cellDetails[q, e]);
                        }
                    }
                    return true;
                }
                else if (closedList[i + 1, j - 1] == false && isUnBlocked(map, i + 1, j - 1) == true)
                {
                    gNew = cellDetails[i, j].g + 1.414;
                    hNew = calcH(i + 1, j - 1, destX, destY);
                    fNew = gNew + hNew;

                    if (cellDetails[i + 1, j - 1].f == float.MaxValue || cellDetails[i + 1, j - 1].f > fNew)
                    {
                        openList.Add(new KeyValuePair<double, KeyValuePair<int, int>>(fNew, new KeyValuePair<int, int>(i + 1, j - 1)));

                        cellDetails[i + 1, j - 1].f = fNew;
                        cellDetails[i + 1, j - 1].g = gNew;
                        cellDetails[i + 1, j - 1].h = hNew;
                        cellDetails[i + 1, j - 1].parent_i = i;
                        cellDetails[i + 1, j - 1].parent_j = j;
                    }
                }
            }

            //Debug.Log("LIST COUNT: " + openList.Count);
        }

        if (foundDest == false)
        {
            Debug.Log("ERROR: Unable to Reach Destination");
            //printCellDetails(cellDetails); //Debug Script
        }

        return false;
    }

    void printCellDetails(cell[,] cellDetails)
    {
        for (int a = 0; a < cellDetails.GetLength(0); a++)
        {
            for (int b = 0; b < cellDetails.GetLength(1); b++)
            {
                Debug.Log("CELL [" + a + ", " + b + "] DETAILS [F: " + cellDetails[a, b].f + "] [G:" + cellDetails[a, b].g + "] [H:" + cellDetails[a, b].h + "]");
            }
        }
    }
}
