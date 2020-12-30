using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class build_manager : MonoBehaviour
{
    public GameObject turretGroup; //待选区
    public GameObject julingqu; //聚灵区
    public GameObject equipBag; //装备背包

    public int max_num_turret;//场上最大炮台数
    [HideInInspector]
    public List<GameObject> onStateTurretCubes = new List<GameObject>();//场上有炮台的Cube
    public GameObject area;//显示攻击范围

    public Texture2D cursor_build;
    public Texture2D cursor_default;
    public int money; //灵力
    public Text t_monty;

    private GameObject seletedTurret; //当前选中的炮台
    private int seletedEquipIndex; //当前选中的装备在背包的下标
    private MapCube seletedMapCube;

    private TurretPrefabs turretPrefabs;

    public bool isUpdatedInfo;
    private bool isOnUpGradeInfo;
    private bool isOnSeletedTurretInfo;
    private TurretType upgradeType;
    private Vector3 oldCurserPos;
    private bool isHoldMouseLeft;
    private bool isDraging;
    private GameObject t_turret;

    private float money_timer;//灵力恢复计时
    void Start()
    {
        Cursor.SetCursor(cursor_default, Vector2.zero, CursorMode.Auto);
        seletedTurret = null;
        seletedEquipIndex = -1;
        turretPrefabs = GetComponent<TurretPrefabs>();
        isUpdatedInfo = false;
        isOnUpGradeInfo = false;
        isOnSeletedTurretInfo = false;
        upgradeType = TurretType.None;
        oldCurserPos = Vector3.zero;
        isHoldMouseLeft = false;
        isDraging = false;
        t_turret = null;
    }

    void Update()
    {
        UpdateMouse();
        UpdateInfoPanel();
        RecoverMoney();
        t_monty.text = money.ToString();

        //按E出售
        if (Input.GetKeyDown(KeyCode.E) && !isOnSeletedTurretInfo && !isOnUpGradeInfo)
            PressedE();
    }

    //灵力自动恢复
    private void RecoverMoney()
    {
        //上限999999
        if (money > 999999)
            money = 999999;
        //自动恢复基础量，每秒恢复1点，聚灵区有加成
        money_timer += Time.deltaTime;
        if (money_timer >= 1.0f)
        {
            Julingqu jq = julingqu.GetComponent<Julingqu>();
            money += (int)((1 + jq.power) * jq.power_ef + 0.5);
            money_timer = 0;
        }
    }

    private void PressedE()
    {
        //待选区的牌
        TurretGroup turrets = turretGroup.GetComponent<TurretGroup>();
        for (int i = 0; i < turrets.turretsList.Count; i++)
        {
            if (turrets.turretsList[i].GetComponent<ToggleTurret>().isOn)
            {
                turrets.turretsList[i].GetComponent<Toggle>().isOn = true;
                seletedTurret = turrets.turretsList[i];
                OnClickedDeleted();
                break;
            }
        }
        //场上的
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 1000, LayerMask.GetMask("MapCube")))
        {
            seletedMapCube = hit.collider.GetComponent<MapCube>(); //获取当前选中的MapCube
            if (seletedMapCube.current_turret != null)
            {
                seletedMapCube.Seleted();
                seletedTurret = seletedMapCube.current_turret;
                OnClickedDeleted();
            }
            else
            {
                seletedMapCube = null;
            }
        }
    }

    private void UpdateMouse()
    {
        //鼠标左键按下
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            if (isHoldMouseLeft == false)
                oldCurserPos = Input.mousePosition;
            isHoldMouseLeft = true;
            //Debug.Log("鼠标左键按下");
            MouseLeftDownProcess();
        }
        //进入拖拽状态
        if (!isDraging && isHoldMouseLeft && (Input.mousePosition - oldCurserPos).sqrMagnitude > 5.0f)
        {
            isDraging = true;
            //Debug.Log("进入拖拽状态");
            EnterDragingProcess();
        }
        //拖拽中
        if (isDraging)
        {
            //Debug.Log("拖拽中");
            DragingProcess();
        }
        //鼠标左键抬起
        if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            //点击
            if (!isDraging)
            {
                //Debug.Log("点击");
                ClickedProcess();
            }
            else //拖拽松开
            {
                //Debug.Log("拖拽松开");
                DragedOverProcess();
            }
            isDraging = false;
            isHoldMouseLeft = false;
            oldCurserPos = Vector3.zero;
        }
    }

    //左键按下后要处理的事
    private void MouseLeftDownProcess()
    {
        //no
    }

    //进入拖拽状态要处理的事
    private void EnterDragingProcess()
    {
        //在拖待选区的牌
        if (!isOnSeletedTurretInfo && !isOnUpGradeInfo)
        {
            TurretGroup turrets = turretGroup.GetComponent<TurretGroup>();
            for (int i = 0; i < turrets.turretsList.Count; i++)
            {
                if (turrets.turretsList[i].GetComponent<ToggleTurret>().isOn)
                {
                    turrets.turretsList[i].GetComponent<Toggle>().isOn = true;
                    t_turret = GameObject.Instantiate(turretPrefabs.GetGameObject(turrets.turretsList[i].GetComponent<ToggleTurret>().turretPrefab.GetComponent<Turret>().type, 1));
                    Cursor.SetCursor(cursor_build, Vector2.zero, CursorMode.Auto);
                    t_turret.SetActive(false);
                    //Debug.Log("在拖待选区的牌");
                    return;
                }
            }
        }

        //准备阶段，在拖场上的炮台
        if (GetComponent<PhaseManager>()._phase == PHASE.Preparation)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 1000, LayerMask.GetMask("MapCube")))
            {
                seletedMapCube = hit.collider.gameObject.GetComponent<MapCube>();
                t_turret = seletedMapCube.SeparateTurret();
                if (t_turret != null)
                {
                    //Debug.Log("在拖场上的炮台");
                    Cursor.SetCursor(cursor_build, Vector2.zero, CursorMode.Auto);
                    for (int i = 0; i < onStateTurretCubes.Count; i++)
                    {
                        if (onStateTurretCubes[i] == seletedMapCube.gameObject)
                        {
                            onStateTurretCubes.RemoveAt(i);
                            break;
                        }
                    }
                }
            }
        }

        //在拖背包的装备
        if (!isOnSeletedTurretInfo && !isOnUpGradeInfo)
        {
            EquipBag ebag = equipBag.GetComponent<EquipBag>();
            for (int i = 0; i < ebag.equip_objs.Count; i++)
            {
                if (ebag.equip_objs[i].GetComponent<Equipment>().isOn)
                {
                    ebag.equip_objs[i].SetActive(false);
                    seletedEquipIndex = i;
                    Cursor.SetCursor(ebag.equip_objs[i].GetComponent<Equipment>().head_icon.texture, Vector2.zero, CursorMode.Auto);
                    //Debug.Log("在拖背包的装备");
                    return;
                }
            }
        }
    }

    //拖拽中处理的事
    private void DragingProcess()
    {
        if (t_turret != null)
        {
            t_turret.GetComponent<Turret>().SetAct(false);
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 1000, LayerMask.GetMask("MapCube")))
            {
                area.gameObject.SetActive(true);
                Vector3 pos = hit.collider.transform.position;
                pos.y = 0.8f;
                area.transform.position = pos;
                float r = t_turret.GetComponent<Turret>().t_gonjifanwei * 0.11f;
                Vector2 s = new Vector2(r, r);
                area.transform.localScale = s;
                t_turret.transform.position = pos;
                t_turret.SetActive(true);
            }
        }
    }

    //点击处理
    private void ClickedProcess()
    {
        UpdateSeletedTurret();
    }

    //拖拽完成处理
    private void DragedOverProcess()
    {
        //拖了炮台
        if (t_turret != null)
        {
            //放到聚灵区
            if (julingqu.GetComponent<Julingqu>().isEnter)
            {
                if (julingqu.GetComponent<Julingqu>().AddTurret(turretPrefabs.GetGameObject(t_turret.GetComponent<Turret>().type, 0)))
                {
                    turretGroup.GetComponent<TurretGroup>().RemoveSeleted();
                    seletedMapCube = null;
                }
            }
            //放到场上
            else if (onStateTurretCubes.Count >= max_num_turret)
                GetComponent<InfoManager>().tipsInfo.GetComponent<TipInfo>().ShowContent("最多只能配置" + max_num_turret + "个灵兽");
            else
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit, 1000, LayerMask.GetMask("MapCube")))
                {
                    seletedMapCube = hit.collider.GetComponent<MapCube>(); //获取当前选中的MapCube
                    if (!isOnSeletedTurretInfo && !isOnUpGradeInfo && seletedMapCube.current_turret == null)
                        BuildTurret();
                }
            }
        }
        //拖了装备
        if (seletedEquipIndex != -1)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 1000, LayerMask.GetMask("MapCube")))
            {
                seletedMapCube = hit.collider.GetComponent<MapCube>(); //获取当前选中的MapCube
                if (!isOnSeletedTurretInfo && !isOnUpGradeInfo && seletedEquipIndex != -1 && seletedMapCube.current_turret != null)
                {
                    //是否可以安装
                    if (seletedMapCube.current_turret.GetComponent<Turret>().AddEquip(equipBag.GetComponent<EquipBag>().equip_objs[seletedEquipIndex].GetComponent<Equipment>().id))
                    {
                        equipBag.GetComponent<EquipBag>().RemoveEquip(seletedEquipIndex);
                        seletedEquipIndex = -1;
                    }
                }
            }
        }
        UnSeletedAll();
        Cursor.SetCursor(cursor_default, Vector2.zero, CursorMode.Auto);
        area.gameObject.SetActive(false);
    }

    //创建Turret
    private void BuildTurret()
    {
        seletedMapCube.BuildTurret(t_turret);
        //Destroy(t_turret);
        t_turret = null;
        onStateTurretCubes.Add(seletedMapCube.gameObject);
        turretGroup.GetComponent<TurretGroup>().RemoveSeleted();
    }

    public void OnClickedUpgrade()
    {
        isUpdatedInfo = false;
        isOnSeletedTurretInfo = false;
        isOnUpGradeInfo = true;
        if (seletedTurret.tag == "TurretToggle")
        {
            seletedTurret.GetComponent<Toggle>().isOn = false;
            GetComponent<InfoManager>().upgradeTurretInfo.GetComponent<upgradeInfo>().upgradeTargets.GetComponent<upgradeTargets>().SetTargetsList(seletedTurret.GetComponent<ToggleTurret>().turretPrefab.GetComponent<Turret>().upgradeTypeList);
        }
        else
            GetComponent<InfoManager>().upgradeTurretInfo.GetComponent<upgradeInfo>().upgradeTargets.GetComponent<upgradeTargets>().SetTargetsList(seletedTurret.GetComponent<Turret>().upgradeTypeList);
    }

    //升级Turret
    public void OnClickedConfirmUpgrade()
    {
        int m = 0;
        TurretType _type;
        PHASE ph = GetComponent<PhaseManager>()._phase;
        if (seletedTurret.tag == "TurretToggle")
            _type = seletedTurret.GetComponent<ToggleTurret>().turretPrefab.GetComponent<Turret>().type;
        else
        {
            if (ph == PHASE.Preparation)
                _type = seletedTurret.GetComponent<Turret>().type;
            else
            {
                GetComponent<InfoManager>().tipsInfo.GetComponent<TipInfo>().ShowContent("该灵兽目前正在战斗中，请稍后再试");
                return;
            }
        }
        if (ph == PHASE.Preparation) //准备阶段
        {
            for (int i = 0; i < onStateTurretCubes.Count;)  //优先场上
            {
                if (onStateTurretCubes[i].GetComponent<MapCube>().current_turret.GetComponent<Turret>().type == _type)
                {
                    m++;
                    onStateTurretCubes[i].GetComponent<MapCube>().DestroyTurret();
                    onStateTurretCubes.RemoveAt(i);
                }
                else
                    i++;
                if (m >= 3)
                    break;
            }
        }
        if (m < 3) //其次待选区
        {
            for (int i = 0; i < turretGroup.GetComponent<TurretGroup>().turretsList.Count;)
            {
                if (turretGroup.GetComponent<TurretGroup>().turretsList[i].GetComponent<ToggleTurret>().turretPrefab.GetComponent<Turret>().type == _type)
                {
                    m++;
                    turretGroup.GetComponent<TurretGroup>().RemoveTurret(i);
                }
                else
                    i++;
                if (m >= 3)
                    break;
            }
        }
        GameObject newTurret = null;
        //待选区放不下的话就把合成到的新角色放到原来位置
        if (turretGroup.GetComponent<TurretGroup>().turretsList.Count >= turretGroup.GetComponent<TurretGroup>().max_num)
        {
            newTurret = turretPrefabs.GetGameObject(upgradeType, 1);
            seletedMapCube.BuildTurret(newTurret);
            onStateTurretCubes.Add(seletedMapCube.gameObject);
            seletedTurret = null;
        }
        else //放到待选区
        {
            newTurret = turretPrefabs.GetGameObject(upgradeType, 0);
            turretGroup.GetComponent<TurretGroup>().AddTurret(newTurret);
        }
        seletedTurret = null;
        isUpdatedInfo = false;
        isOnUpGradeInfo = false;
    }

    //更新信息面板
    private void UpdateInfoPanel()
    {
        if (!isUpdatedInfo)
        {
            if (isOnSeletedTurretInfo)
                ShowSeletedTurretInfo();
            else
                GetComponent<InfoManager>().seletedTurretInfo.gameObject.SetActive(false);
            isUpdatedInfo = true;
        }
        if (isOnSeletedTurretInfo)
            UpdateButton();
        if (isOnUpGradeInfo)
            ShowUpgradedPanel();
        else
            GetComponent<InfoManager>().upgradeTurretInfo.gameObject.SetActive(false);
    }

    //显示选中炮台信息面板
    private void ShowSeletedTurretInfo()
    {
        GetComponent<InfoManager>().seletedTurretInfo.gameObject.SetActive(true);
        SeletedTurretInfo info = GetComponent<InfoManager>().seletedTurretInfo.GetComponent<SeletedTurretInfo>();
        //内容
        info.SetInfo(seletedTurret);
    }

    //更新按钮的状态
    private void UpdateButton()
    {
        SeletedTurretInfo info = GetComponent<InfoManager>().seletedTurretInfo.GetComponent<SeletedTurretInfo>();
        //选中聚灵区
        Julingqu js = julingqu.GetComponent<Julingqu>();
        for (int i = 0; i < js.toggle_obj_list.Count; i++)
        {
            if (js.toggle_obj_list[i].GetComponent<Toggle>().isOn)
            {
                info.bt_up.gameObject.SetActive(false);
                info.bt_take.gameObject.SetActive(true);
                return;
            }
        }

        info.bt_up.gameObject.SetActive(true);
        info.bt_take.gameObject.SetActive(false);
        int m = 0;
        TurretType _type;
        PHASE ph = GetComponent<PhaseManager>()._phase;
        if (seletedTurret.tag == "TurretToggle")
            _type = seletedTurret.GetComponent<ToggleTurret>().turretPrefab.GetComponent<Turret>().type;
        else
        {
            if (ph == PHASE.Preparation)
                _type = seletedTurret.GetComponent<Turret>().type;
            else
            {
                info.bt_up.interactable = true;
                return;
            }
        }
        if (GetComponent<PhaseManager>()._phase == PHASE.Preparation)
        {
            for (int i = 0; i < onStateTurretCubes.Count; i++)  //优先场上
            {
                if (onStateTurretCubes[i].GetComponent<MapCube>().current_turret.GetComponent<Turret>().type == _type)
                    m++;
                if (m >= 3)
                    break;
            }
        }
        if (m < 3) //其次待选区
        {
            List<GameObject> turretsList = turretGroup.GetComponent<TurretGroup>().turretsList;
            for (int i = 0; i < turretsList.Count; i++)
            {
                if (turretsList[i].GetComponent<ToggleTurret>().turretPrefab.GetComponent<Turret>().type == _type)
                    m++;
                if (m >= 3)
                    break;
            }
        }

        //m>=3时可以升级，按钮亮起
        if (m >= 3)
            info.bt_up.interactable = true;
        else //灰色
            info.bt_up.interactable = false;
    }

    //显示升级面板
    private void ShowUpgradedPanel()
    {
        Canvas infoPanel = GetComponent<InfoManager>().upgradeTurretInfo;
        infoPanel.gameObject.SetActive(true);
        infoPanel.GetComponent<upgradeInfo>().bt_ok.interactable = false;
        //选中升级目标
        GameObject targetsGroup = GetComponent<InfoManager>().upgradeTurretInfo.GetComponent<upgradeInfo>().upgradeTargets;
        List<GameObject> tagetsList = targetsGroup.GetComponent<upgradeTargets>().turretsList;
        for (int i = 0; i < tagetsList.Count; i++)
        {
            if (tagetsList[i].GetComponent<Toggle>().isOn)
            {
                upgradeType = tagetsList[i].GetComponent<ToggleTurret>().turretPrefab.GetComponent<Turret>().type;
                infoPanel.GetComponent<upgradeInfo>().bt_ok.interactable = true;
                return;
            }
        }
    }

    //选中UI炮塔
    public void UpdateSeletedTurret()
    {
        //选中待选区
        TurretGroup turrets = turretGroup.GetComponent<TurretGroup>();
        for (int i = 0; i < turrets.turretsList.Count; i++)
        {
            if (turrets.turretsList[i].GetComponent<Toggle>().isOn)
            {
                seletedTurret = turrets.turretsList[i];
                isUpdatedInfo = false;
                isOnUpGradeInfo = false;
                isOnSeletedTurretInfo = true;
                return;
            }
        }
        //选中聚灵区
        Julingqu js = julingqu.GetComponent<Julingqu>();
        for (int i = 0; i < js.toggle_obj_list.Count; i++)
        {
            if (js.toggle_obj_list[i].GetComponent<Toggle>().isOn)
            {
                seletedTurret = js.toggle_obj_list[i];
                isUpdatedInfo = false;
                isOnUpGradeInfo = false;
                isOnSeletedTurretInfo = true;
                return;
            }
        }
        //选中场上
        if (!isOnSeletedTurretInfo && !isOnUpGradeInfo && !EventSystem.current.IsPointerOverGameObject())
        {
            if (seletedMapCube != null)
            {
                seletedMapCube.UnSeleted();
                seletedMapCube = null;
            }
            //点击MapCube
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 1000, LayerMask.GetMask("MapCube")))
            {
                seletedMapCube = hit.collider.GetComponent<MapCube>(); //获取当前选中的MapCube
                if (seletedMapCube.current_turret != null)
                {
                    seletedMapCube.Seleted();
                    seletedTurret = seletedMapCube.current_turret;
                    isUpdatedInfo = false;
                    isOnSeletedTurretInfo = true;
                }
                else
                {
                    seletedMapCube = null;
                }
                return;
            }
        }

        if (seletedTurret == null)
        {
            isUpdatedInfo = false;
            isOnSeletedTurretInfo = false;
            isOnUpGradeInfo = false;
        }
    }

    //取消选中
    public void UnSeletedAll()
    {
        if (seletedMapCube != null)
        {
            seletedMapCube.UnSeleted();
            seletedMapCube = null;
        }
        if (t_turret != null)
        {
            if (seletedMapCube != null)
            {
                seletedMapCube.BuildTurret(t_turret);
                //Debug.Log("归位");
            }
            else
            {
                Destroy(t_turret);
                //Debug.Log("删除了");
            }
            t_turret = null;
        }
        if (seletedEquipIndex != -1)
        {
            equipBag.GetComponent<EquipBag>().equip_objs[seletedEquipIndex].SetActive(true);
        }
        TurretGroup turrets = turretGroup.GetComponent<TurretGroup>();
        for (int i = 0; i < turrets.turretsList.Count; i++)
            turrets.turretsList[i].GetComponent<Toggle>().isOn = false;
        Julingqu js = julingqu.GetComponent<Julingqu>();
        for (int i = 0; i < js.toggle_obj_list.Count; i++)
            js.toggle_obj_list[i].GetComponent<Toggle>().isOn = false;
        seletedTurret = null;
        seletedEquipIndex = -1;
        isUpdatedInfo = false;
        isOnSeletedTurretInfo = false;
        isOnUpGradeInfo = false;
        //Debug.Log("取消选中");
    }

    private void GetMoneyOfTurretDeleted(Turret t)
    {
        int res = 0;
        if (t != null)
            res = t.cost;
        money += res;
    }

    //取出
    public void OnClickedTakeOut()
    {
        Turret t = null;
        List<GameObject> js = julingqu.GetComponent<Julingqu>().toggle_obj_list;
        for (int i = 0; i < js.Count; i++)
        {
            if (js[i].GetComponent<Toggle>().isOn)
            {
                t = js[i].GetComponent<ToggleTurret>().turretPrefab.GetComponent<Turret>();
                break;
            }
        }
        if (t == null)
            return;
        if (money < t.cost * 2) //取出花双倍的灵力
            GetComponent<InfoManager>().tipsInfo.GetComponent<TipInfo>().ShowContent("灵力不足！");
        else if (turretGroup.GetComponent<TurretGroup>().turretsList.Count >= turretGroup.GetComponent<TurretGroup>().max_num)
            GetComponent<InfoManager>().tipsInfo.GetComponent<TipInfo>().ShowContent("待选区放不下了！");
        else
        {
            turretGroup.GetComponent<TurretGroup>().AddTurret(turretPrefabs.GetGameObject(t.type, 0));
            money -= t.cost * 2;
            OnClickedDeleted();
        }
    }

    //回收
    public void OnClickedDeleted()
    {
        if (seletedTurret.tag == "Turret")
        {
            if (GetComponent<PhaseManager>()._phase == PHASE.Fighting)
            {
                GetComponent<InfoManager>().tipsInfo.GetComponent<TipInfo>().ShowContent("该灵兽目前正在战斗中，请稍后再试");
                return;
            }
            GetMoneyOfTurretDeleted(seletedTurret.GetComponent<Turret>());
            for (int i = 0; i < onStateTurretCubes.Count; i++)
            {
                if (onStateTurretCubes[i].GetComponent<MapCube>().current_turret == seletedTurret)
                {
                    onStateTurretCubes[i].GetComponent<MapCube>().DestroyTurret();
                    onStateTurretCubes.RemoveAt(i);
                    AddToLotteryAfterDelete(seletedTurret.GetComponent<Turret>().type);
                    break;
                }
            }
        }
        else if (seletedTurret.tag == "TurretToggle")
        {
            GetMoneyOfTurretDeleted(seletedTurret.GetComponent<ToggleTurret>().turretPrefab.GetComponent<Turret>());
            AddToLotteryAfterDelete(seletedTurret.GetComponent<ToggleTurret>().turretPrefab.GetComponent<Turret>().type);
            List<GameObject> turrets = turretGroup.GetComponent<TurretGroup>().turretsList;
            for (int i = 0; i < turrets.Count; i++)
            {
                if (turrets[i].GetComponent<Toggle>().isOn)
                {
                    turretGroup.GetComponent<TurretGroup>().RemoveTurret(i);
                    break;
                }
            }
            List<GameObject> js = julingqu.GetComponent<Julingqu>().toggle_obj_list;
            for (int i = 0; i < js.Count; i++)
            {
                if (js[i].GetComponent<Toggle>().isOn)
                {
                    julingqu.GetComponent<Julingqu>().RemoveTurret(i);
                    break;
                }
            }
        }
        seletedTurret = null;
    }

    //回收后卡池随机添加对应卡牌
    private void AddToLotteryAfterDelete(TurretType deletedTurretType)
    {
        int num_t = 0;
        switch (deletedTurretType)
        {
            case TurretType.None:
                num_t = 0;
                break;
            case TurretType.LingYuan_lv1:
                num_t = 1;
                break;
            case TurretType.LingYuan_lv2_1:
                num_t = 3;
                break;
            case TurretType.LingYuan_lv2_2:
                num_t = 3;
                break;
            case TurretType.Hanser_lv1:
                num_t = 1;
                break;
            case TurretType.Hanser_lv2:
                num_t = 3;
                break;
            case TurretType.Amiya_lv1:
                num_t = 1;
                break;
            case TurretType.Amiya_lv2:
                num_t = 3;
                break;
            case TurretType.Enterprise_lv1:
                num_t = 1;
                break;
            case TurretType.Enterprise_lv2_1:
                num_t = 3;
                break;
            case TurretType.Enterprise_lv2_2:
                num_t = 3;
                break;
            case TurretType.Enterprise_lv3_1:
                num_t = 9;
                break;
            case TurretType.Enterprise_lv3_2:
                num_t = 9;
                break;
            default:
                break;
        }
        TurretType type = TurretType.Amiya_lv1;
        System.Random random = new System.Random();
        switch (random.Next(0, 3))
        {
            case 0:
                type = TurretType.LingYuan_lv1;
                break;
            case 1:
                type = TurretType.Hanser_lv1;
                break;
            case 2:
                type = TurretType.Amiya_lv1;
                break;
            case 3:
                type = TurretType.Enterprise_lv1;
                break;
            default:
                break;
        }
        for (int ii = 0; ii < num_t; ii++)
            GetComponent<Lottery>().AddToPool(type);
    }

    //取消升级
    public void CancelUpgrade()
    {
        if (seletedMapCube != null)
        {
            seletedMapCube.UnSeleted();
            seletedMapCube = null;
        }
        seletedTurret = null;
        isUpdatedInfo = false;
        isOnUpGradeInfo = false;
    }
}