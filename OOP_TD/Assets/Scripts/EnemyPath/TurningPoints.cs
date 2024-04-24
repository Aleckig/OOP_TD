using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurningPoints : MonoBehaviour
{

    public GameObject[] paths;
    public GameObject decalProjector;

    public void BlockPath()
    {
        transform.GetChild(0).gameObject.SetActive(false);
        decalProjector.gameObject.SetActive(false);
    }
}
