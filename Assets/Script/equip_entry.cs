using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class equip_entry : MonoBehaviour
{
    public GameObject equip;
    public Text _name;
    public Text cost;
    public Button bt_buy;
    [HideInInspector]
    public int id;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetInfo(int id)
    {
        this.id = id;
        GameObject obj = GameObject.Instantiate(GameObject.Find("GameManager").GetComponent<EquipmentPrefab>().GetEquipment(id));
        obj.transform.SetParent(transform);
        obj.transform.position = equip.transform.position;
        _name.text = obj.GetComponent<Equipment>()._name;
        cost.text = obj.GetComponent<Equipment>().cost.ToString();
    }

    public void Buy()
    {
        Equipment e = GameObject.Find("GameManager").GetComponent<EquipmentPrefab>().GetEquipment(id).GetComponent<Equipment>();
        if (GameObject.Find("GameManager").GetComponent<build_manager>().money <e.cost)
        {
            GameObject.Find("GameManager").GetComponent<InfoManager>().tipsInfo.GetComponent<TipInfo>().ShowContent("灵力不足");
            return;
        }
        if (GameObject.Find("MainCanvas").GetComponentInChildren<EquipBag>().AddEquip(id))
        {
            GameObject.Find("GameManager").GetComponent<build_manager>().money -= e.cost;
            GetComponentInParent<bussiness_info>().RemoveEquipEntry(e.indexInGroup);
        }
    }
}
