using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentPrefab : MonoBehaviour
{

    public List<GameObject> equipmentPrefab;

    public GameObject GetEquipment(int id)
    {
        GameObject obj = null;
        if (id < equipmentPrefab.Count && id >= 0)
            obj = equipmentPrefab[id];
        return obj;
    }
}
