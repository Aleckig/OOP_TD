using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurningPoints : MonoBehaviour
{

    //public Transform[] points;
    //public int pathNumber = 0;
    public GameObject[] paths;
    public GameObject decalProjector;

    //void Awake()
    //{
    //    points = new Transform[transform.GetChild(pathNumber).childCount];
    //    for (int i=0; i < points.Length; i++)
    //    {
    //        points[i] = transform.GetChild(pathNumber).GetChild(i);
    //    }
    //}

    public void BlockPath()
    {
        transform.GetChild(0).gameObject.SetActive(false);
        decalProjector.gameObject.SetActive(false);
    }
}
