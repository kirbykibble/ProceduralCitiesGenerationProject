using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.IO;

public class HeatmapGen : MonoBehaviour
{
    public int size;
    public float randomness; //set between 0 and 1
    public float randAmount; //set between 0 and 1 is maximum randomness applied.
    public int center;

    private Texture2D heatmap;
    private Color color = Color.black;

    // Start is called before the first frame update
    void Start()
    {

        Texture2D heatmap = new Texture2D(size, size);
        center = (size - (size % 2)) / 2;
        //heatmap.SetPixel(0, 0, Color.white);
        for (int i = 0; i < size; i++)
        {
            for (int j = 0; j < size; j++)
            {
                color.g = 0;
                float iClose = (Mathf.Abs(i - center));
                iClose = iClose / center;
                iClose = 1 - iClose;

                float jClose = (Mathf.Abs(j - center));
                jClose = jClose / center;
                jClose = 1 - jClose;

                float output = iClose + jClose;
                output = output / 2;

                if (Random.value < randomness)
                {
                    int upOrDown = Random.Range(0, 2);
                    float amtToChange = Random.Range(0, randAmount * 100);
                    amtToChange = amtToChange / 100;
                    if(upOrDown == 0)
                    {
                        //shifts the heatmap up
                        output += amtToChange;
                    }
                    if(upOrDown == 1)
                    {
                        //shifts the heatmap down
                        output -= amtToChange;
                    }
                    //color.g = 1; //shows which pixels were affected. Use for debug purposes only.
                }

                if (output > 1)
                {
                    output = 1;
                }
                if (output < 0)
                {
                    output = 0;
                }
                color.r = output;
                heatmap.SetPixel(i, j, color);
            }
        }



        //outputs the created heatmap to a png. For debugging purposes only. 
        byte[] bytes = heatmap.EncodeToPNG();
        Object.Destroy(heatmap);
        File.WriteAllBytes(Application.dataPath + "/../Assets/Debug/HeatmapOut.png", bytes);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
