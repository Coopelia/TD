using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SeletedTurretInfo : MonoBehaviour
{
    public Button bt_up;
    public Button bt_take;
    public Sprite default_icon;

    public Image head_icon;
    public Text t_name;
    public Text wuli; //物理
    public Text wuchuan; //物穿
    public Text faqiang; //法强
    public Text fachuan; //法穿
    public Text baoji; //暴击
    public Text gonsu; //攻速
    public Text baoshang; //爆伤
    public Text gonjifanwei; //攻击范围
    public Text t_description; //角色描述
    //三件装备位置
    public GameObject equip1;
    public GameObject equip2;
    public GameObject equip3;
    //技能描述
    public Text sk1;
    public Text sk2;
    public Text sk3;
    public Text sk4;
    //装备
    [HideInInspector]
    public List<GameObject> equip_objs = new List<GameObject>();
    [HideInInspector]
    public GameObject current_turret;

    public void ClearAll()
    {
        head_icon.sprite = default_icon;
        t_name.text = "";
        wuli.text = "";
        wuchuan.text = "";
        faqiang.text = "";
        fachuan.text = "";
        baoji.text = "";
        gonsu.text = "";
        baoshang.text = "";
        gonjifanwei.text = "";
        t_description.text = "";
        for (int i = 0; i < equip_objs.Count; i++)
            Destroy(equip_objs[i]);
        equip_objs.Clear();
        current_turret = null;
    }

    public void SetInfo(GameObject tur_obj)
    {
        ClearAll();
        current_turret = tur_obj;
        Turret tur = null;
        if (tur_obj.tag == "Turret")
            tur = tur_obj.GetComponent<Turret>();
        else if (tur_obj.tag == "TurretToggle")
            tur = tur_obj.GetComponent<ToggleTurret>().turretPrefab.GetComponent<Turret>();
        //显示属性
        head_icon.sprite = tur.head_icon;
        t_name.text = tur._name;
        wuli.text = "物理：" + tur.t_wuli;
        wuchuan.text = "物穿：" + tur.t_wuchuan;
        faqiang.text = "法强：" + tur.t_faqiang;
        fachuan.text = "法穿：" + tur.t_fachuan;
        baoji.text = "暴击：" + tur.t_baoji + "%";
        gonsu.text = "攻速：" + tur.t_gonsu;
        baoshang.text = "爆伤：" + tur.t_baoshang + "%";
        gonjifanwei.text = "攻击范围：" + tur.t_gonjifanwei;
        t_description.text = Description.GetTurretDescription(tur.type);
        //显示技能描述
        sk1.text = "技能1：" + tur.skill_1;
        sk2.text = "技能2：" + tur.skill_2;
        sk3.text = "技能3：" + tur.skill_3;
        sk4.text = "技能4：" + tur.skill_4;
        //显示装备
        if (tur_obj.tag == "Turret")
        {
            EquipmentPrefab equipmentPrefab = GameObject.Find("GameManager").GetComponent<EquipmentPrefab>();
            GameObject equ = equipmentPrefab.GetEquipment(tur.equips[0]);
            if (equ != null)
            {
                GameObject obj = GameObject.Instantiate(equ);
                obj.GetComponent<Equipment>().indexInGroup = 0;
                obj.GetComponent<Equipment>().enable_uninstall = true;
                obj.transform.SetParent(equip1.transform);
                obj.transform.position = equip1.transform.position;
                equip_objs.Add(obj);
            }
            equ = equipmentPrefab.GetEquipment(tur.equips[1]);
            if (equ != null)
            {
                GameObject obj = GameObject.Instantiate(equ);
                obj.GetComponent<Equipment>().indexInGroup = 1;
                obj.GetComponent<Equipment>().enable_uninstall = true;
                obj.transform.SetParent(equip2.transform);
                obj.transform.position = equip2.transform.position;
                equip_objs.Add(obj);
            }
            equ = equipmentPrefab.GetEquipment(tur.equips[2]);
            if (equ != null)
            {
                GameObject obj = GameObject.Instantiate(equ);
                obj.GetComponent<Equipment>().indexInGroup = 2;
                obj.GetComponent<Equipment>().enable_uninstall = true;
                obj.transform.SetParent(equip3.transform);
                obj.transform.position = equip3.transform.position;
                equip_objs.Add(obj);
            }
        }
    }
}
