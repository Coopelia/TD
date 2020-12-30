using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class equipment_info : MonoBehaviour
{
    public Image head_icon;
    public Text t_name;
    public Text t_description; //描述

    public Text wuli; //物理
    public Text wuchuan; //物穿
    public Text faqiang; //法强
    public Text fachuan; //法穿
    public Text baoji; //暴击
    public Text gonsu; //攻速
    public Text baoshang; //爆伤
    public Text gonjifanwei; //攻击范围

    public GameObject equipBag;
    public GameObject seletedTurretInfo;
    public Button bt_delete;
    public Button bt_uninstall;
    private Equipment equipment;
    public bool isShowing;
    // Start is called before the first frame update
    void Start()
    {
        isShowing = false;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ShowInfo(Equipment e)
    {
        t_name.text = e._name;
        head_icon.sprite = e.head_icon;
        wuli.text = "物理" + e.wuli.ToString();
        wuchuan.text = "物穿" + e.wuchuan.ToString();
        faqiang.text = "法强" + e.faqiang.ToString();
        fachuan.text = "法穿" + e.fachuan.ToString();
        baoji.text = "暴击" + e.baoji.ToString() + "%";
        gonsu.text = "攻速" + e.gonsu.ToString();
        baoshang.text = "爆伤" + e.baoshang.ToString() + "%";
        gonjifanwei.text = "攻击范围" + e.gonjifanwei.ToString();
        t_description.text = Description.GetEquipmentDescription(e.id);
        if (e.enable_delete)
            bt_delete.gameObject.SetActive(true);
        else
            bt_delete.gameObject.SetActive(false);
        if (e.enable_uninstall)
            bt_uninstall.gameObject.SetActive(true);
        else
            bt_uninstall.gameObject.SetActive(false);
        equipment = e;
        this.gameObject.SetActive(true);
        isShowing = true;
    }

    public void CloseInfo()
    {
        this.gameObject.SetActive(false);
        equipment.toggle.isOn = false;
        isShowing = false;
        equipment = null;
    }

    public void OnClickedDelete()
    {
        GameObject.Find("GameManager").GetComponent<build_manager>().money += equipment.cost;
        equipBag.GetComponent<EquipBag>().RemoveEquip(equipment.indexInGroup);
        CloseInfo();
    }

    public void OnClickedUninstall()
    {
        GameObject tur = seletedTurretInfo.GetComponent<SeletedTurretInfo>().current_turret;
        if (tur.tag == "Turret")
            tur.GetComponent<Turret>().RemoveEquip(equipment.indexInGroup);
        equipBag.GetComponent<EquipBag>().AddEquip(equipment.id);
        GameObject.Find("GameManager").GetComponent<build_manager>().isUpdatedInfo = false;
        CloseInfo();
    }
}
