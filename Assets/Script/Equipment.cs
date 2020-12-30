using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Equipment : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    //属性
    public Sprite head_icon; //图标
    public string _name; //名字
    public int id; //id
    public int cost; //价格
    //装备描述在Description.cs

    //基本数值属性
    public int wuli; //物理
    public int wuchuan; //物穿
    public int faqiang; //法强
    public int fachuan; //法穿
    public int baoji; //暴击
    public int gonsu; //攻速
    public int baoshang; //爆伤
    public int gonjifanwei; //攻击范围

    //技能
    //
    [HideInInspector]
    public Toggle toggle;
    private equipment_info infoPanel;
    [HideInInspector]
    public int indexInGroup;

    public bool enable_delete;
    public bool enable_uninstall;
    public bool isOn;
    // Start is called before the first frame update
    void Awake()
    {
        indexInGroup = -1;
        enable_delete = false;
        enable_uninstall = false;
        isOn = false;
        toggle = GetComponent<Toggle>();
        infoPanel = GameObject.Find("GameManager").GetComponent<InfoManager>().equipmentInfo.GetComponent<equipment_info>();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateSeletedState();
    }

    void UpdateSeletedState()
    {
        if (toggle.isOn && !infoPanel.isShowing)
        {
            infoPanel.isShowing = true;
            infoPanel.GetComponent<equipment_info>().ShowInfo(this);
        }
        //else if (!toggle.isOn && infoPanel.isShowing)
        //{
        //    infoPanel.GetComponent<equipment_info>().CloseInfo();
        //}
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        isOn = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isOn = false;
    }
}
