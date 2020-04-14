using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class testCreator : MonoBehaviour
{
    private Color bg = Color.black;
    private Color node = Color.blue;
    private Color road = Color.yellow;
    private Color outConnector = Color.magenta;
    private Color diagonalRoad = Color.green;

    private Color cds = Color.green;

    private List<Color> cChecker = new List<Color>();
    private List<Color> sChecker = new List<Color>();

    public void createTests()
    {
        cChecker.Add(node);
        cChecker.Add(road);
        cChecker.Add(diagonalRoad);
        cChecker.Add(outConnector);

        sChecker.Add(node);
        sChecker.Add(road);
        sChecker.Add(cds);
        sChecker.Add(outConnector);


        Texture2D cinMap;
        Texture2D sinMap;

        Texture2D coutMap;
        Texture2D soutMap;

        byte[] filedata;

        filedata = File.ReadAllBytes("Assets/Debug/nodeMapOut.png");
        cinMap = new Texture2D(2, 2, TextureFormat.BGRA32, false);
        cinMap.LoadImage(filedata);

        filedata = File.ReadAllBytes("Assets/Debug/SurbanNodeMapOut.png");
        sinMap = new Texture2D(2, 2, TextureFormat.BGRA32, false);
        sinMap.LoadImage(filedata);

        coutMap = new Texture2D(cinMap.width, cinMap.height);
        soutMap = new Texture2D(sinMap.width, sinMap.height);

        for(int x = 0; x < cinMap.width; x++)
        {
            for(int y = 0; y < cinMap.width; y++)
            {
                if (cChecker.Contains(cinMap.GetPixel(x, y)))
                {
                    coutMap.SetPixel(x, y, road);
                }
                else coutMap.SetPixel(x, y, bg);
            }
        }
        byte[] bytes = coutMap.EncodeToPNG();
        Object.Destroy(coutMap);
        File.WriteAllBytes(Application.dataPath + "/../Assets/TestMaterials/city/layout" + Random.Range(0, 1000) + ".png", bytes);

        for(int x = 0; x < sinMap.width; x++)
        {
            for(int y = 0; y < sinMap.width; y++)
            {
                if (sChecker.Contains(sinMap.GetPixel(x, y)))
                {
                    soutMap.SetPixel(x, y, road);
                }
                else soutMap.SetPixel(x, y, bg);
            }
        }

        bytes = soutMap.EncodeToPNG();
        Object.Destroy(soutMap);
        File.WriteAllBytes(Application.dataPath + "/../Assets/TestMaterials/surban/layout" + Random.Range(0, 1000) + ".png", bytes);
    }


}
