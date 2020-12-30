using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Lottery : MonoBehaviour
{
    public int cost; //召唤消耗
    private TurretGroup turretGroup;
    private TurretPrefabs turretPrefabs;
    private build_manager buildManager;

    private List<TurretType> turret_pool=new List<TurretType>();//卡池
    // Start is called before the first frame update
    void Start()
    {
        turretGroup = GetComponent<build_manager>().turretGroup.GetComponent<TurretGroup>();
        turretPrefabs = GetComponent<TurretPrefabs>();
        buildManager = GetComponent<build_manager>();
        //设置卡池内容
        for (int i = 0; i < 10; i++)
        {
            AddToPool(TurretType.LingYuan_lv1);
            AddToPool(TurretType.Hanser_lv1);
            AddToPool(TurretType.Amiya_lv1);
            AddToPool(TurretType.Enterprise_lv1);
            AddToPool(TurretType.Enterprise_lv1);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
            OnClickedCall();
    }

    public void AddToPool(TurretType turret)
    {
        turret_pool.Add(turret);
    }

    public void RemoveFromPool(TurretType turret)
    {
        for (int i = 0; i < turret_pool.Count; i++)
        {
            if(turret_pool[i]==turret)
            {
                turret_pool.RemoveAt(i);
                return;
            }
        }
    }

    //点击召唤
    public void OnClickedCall()
    {
        Canvas tipInfo = GetComponent<InfoManager>().tipsInfo;
        if (buildManager.money<cost)
            tipInfo.GetComponent<TipInfo>().ShowContent("灵力不足！");
        else if (turretGroup.turretsList.Count >= turretGroup.max_num)
            tipInfo.GetComponent<TipInfo>().ShowContent("放不下了！");
        else if(turret_pool.Count==0)
            tipInfo.GetComponent<TipInfo>().ShowContent("卡池空了！");
        else
            StartCoroutine(GetNewTurret());
    }

    //抽卡
    IEnumerator GetNewTurret()
    {
        buildManager.money -= cost;
        System.Random random = new System.Random();
        int i = random.Next(0, turret_pool.Count - 1);
        turretGroup.AddTurret(turretPrefabs.GetGameObject(turret_pool[i], 0));
        turret_pool.RemoveAt(i);
        yield return 0;
    }
}
