using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class audioController : MonoBehaviour
{
    public GameObject surbanAudioSource;
    public GameObject cityAudioSource;
    private AudioSource subAS;
    private AudioSource cityAS;

    public bool placeSurbanAudio(int surbanSize, int surbanMaxSize)
    {
        subAS = surbanAudioSource.GetComponent<AudioSource>();

        float sasPlacement = surbanSize * surbanMaxSize;
        sasPlacement /= 2;

        Vector3 sasPos = new Vector3(sasPlacement, 0, sasPlacement);
        subAS.maxDistance = sasPlacement * 2;
        surbanAudioSource.transform.position = sasPos;

        return true;
    }

    public bool placeCityAudio(int citySize, int cityMaxSize)
    {
        cityAS = cityAudioSource.GetComponent<AudioSource>();

        float casPlacement = citySize * cityMaxSize;
        casPlacement /= 2;

        Vector3 casPos = new Vector3(casPlacement, 0, casPlacement);
        cityAS.maxDistance = casPlacement * 1.5f;
        cityAudioSource.transform.position = casPos;

        return true;
    }
}
