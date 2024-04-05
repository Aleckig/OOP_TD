using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackPoints : MonoBehaviour
{
    public static Transform[] attackPoints;

    void Awake()
    {
        attackPoints = new Transform[transform.childCount];
        for (int i = 0; i < attackPoints.Length; i++)
        {
            attackPoints[i] = transform.GetChild(i);
        }
    }
}
