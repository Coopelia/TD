using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

[System.Serializable]
public class Wave
{
    public GameObject start;  //出生点
    public GameObject end;  //终点
    public int rate;  //每两个之间的生成时间间隔(毫秒)
    public List<int> enemyList =new List<int>(); //敌人
}
