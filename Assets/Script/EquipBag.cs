using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipBag : MonoBehaviour
{
    [HideInInspector]
    public List<GameObject> equip_objs;

    private int max_num, mx, dx, dy;
    private Vector2 s_pos;
    // Start is called before the first frame update
    void Start()
    {
        equip_objs = new List<GameObject>();
        max_num = 28;
        mx = 4;
        dx = 46;
        dy = -46;
        s_pos = transform.position;
        s_pos.x -= 69;
        s_pos.y += 82;
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 pos;
        for (int i = 0; i < equip_objs.Count; i++)
        {
            pos.x = s_pos.x + (i % mx) * dx;
            pos.y = s_pos.y + (i / mx) * dy;
            equip_objs[i].transform.position = pos;
        }
    }

    public bool AddEquip(int id)
    {
        if (equip_objs.Count >= max_num)
            return false;
        GameObject obj = GameObject.Instantiate(GameObject.Find("GameManager").GetComponent<EquipmentPrefab>().GetEquipment(id));
        obj.GetComponent<Equipment>().indexInGroup = equip_objs.Count;
        obj.GetComponent<Equipment>().enable_delete = true;
        obj.transform.SetParent(transform);
        equip_objs.Add(obj);
        return true;
    }

    public void RemoveEquip(int i)
    {
        if (i < 0 || i >= equip_objs.Count)
            return;
        Destroy(equip_objs[i]);
        equip_objs.RemoveAt(i);
    }
}
