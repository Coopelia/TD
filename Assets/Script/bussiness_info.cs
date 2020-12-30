using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class bussiness_info : MonoBehaviour
{
    public Button bt_close;
    public GameObject entryPrefab;
    [HideInInspector]
    public bussinessman bussiMan;

    private int lx;
    private int ly;
    private int dx;
    private int dy;
    private int max_num;

    private List<GameObject> equipEntries = new List<GameObject>();
    private EquipmentPrefab equipPrefabs;
    // Start is called before the first frame update
    void Awake()
    {
        lx = 678;
        ly = 565;
        dx = 237;
        dy = -58;
        max_num = 8;
        equipPrefabs = GameObject.Find("GameManager").GetComponent<EquipmentPrefab>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 pos = new Vector2();
        for (int i = 0; i < equipEntries.Count; i++)
        {
            pos.x = lx + (i % 2) * dx;
            pos.y = ly + (i / 2) * dy;
            equipEntries[i].transform.position = pos;
        }
    }

    public void AddEquipEntry(int id)
    {
        if (equipEntries.Count >= max_num)
            return;
        GameObject e = equipPrefabs.GetEquipment(id);
        if (e == null)
            return;
        GameObject obj = GameObject.Instantiate(entryPrefab);
        obj.GetComponent<equip_entry>().SetInfo(id);
        obj.transform.SetParent(this.transform);
        equipEntries.Add(obj);
    }

    public void RemoveEquipEntry(int index)
    {
        if (index < 0 || index >= equipEntries.Count)
            return;
        Destroy(equipEntries[index]);
        equipEntries.RemoveAt(index);
        bussiMan.eqs.RemoveAt(index);
    }

    public void CloseStore()
    {
        this.gameObject.SetActive(false);
        bussiMan = null;
        for (int i = 0; i < equipEntries.Count;)
        {
            Destroy(equipEntries[i].gameObject);
            equipEntries.RemoveAt(i);
        }
    }
}
