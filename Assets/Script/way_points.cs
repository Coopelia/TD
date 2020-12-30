using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class way_points : MonoBehaviour
{
    [HideInInspector]
    public Transform[] wayPoint; //路线关键点
    [HideInInspector]
    public Vector3[] positionList;
    void Awake()
    {
        wayPoint = new Transform[transform.childCount];
        positionList = new Vector3[wayPoint.Length];
        for (int i = 0; i < wayPoint.Length; i++)
        {
            wayPoint[i] = transform.GetChild(i);
            positionList[i] = wayPoint[i].position;
        }
    }
}
