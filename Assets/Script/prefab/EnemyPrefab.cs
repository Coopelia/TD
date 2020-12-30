using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPrefab : MonoBehaviour
{
    public List<GameObject> enemyPrefab;

    public GameObject GetEnemy(int id)
    {
        GameObject obj = null;
        if (id < enemyPrefab.Count && id >= 0)
            obj = enemyPrefab[id];
        return obj;
    }
}
