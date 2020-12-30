using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//炮台类型枚举
public enum TurretType
{
    None,

    LingYuan_lv1,
    LingYuan_lv2_1,
    LingYuan_lv2_2,

    Hanser_lv1,
    Hanser_lv2,

    Amiya_lv1,
    Amiya_lv2,

    Enterprise_lv1,
    Enterprise_lv2_1,
    Enterprise_lv2_2,
    Enterprise_lv3_1,
    Enterprise_lv3_2,
}

public class TurretPrefabs: MonoBehaviour
{
    public GameObject lingYuan_lv1;
    public GameObject lingYuan_lv2_1;
    public GameObject lingYuan_lv2_2;

    public GameObject hanser_lv1;
    public GameObject hanser_lv2;

    public GameObject amiya_lv1;
    public GameObject amiya_lv2;

    public GameObject enterprise_lv1;
    public GameObject enterprise_lv2_1;
    public GameObject enterprise_lv2_2;
    public GameObject enterprise_lv3_1;
    public GameObject enterprise_lv3_2;

    public GameObject GetGameObject(TurretType type, int i)//i=0:toggle, i=1:turret
    {
        GameObject obj = null;
        switch (type)
        {
            case TurretType.LingYuan_lv1:
                obj = lingYuan_lv1;
                break;
            case TurretType.LingYuan_lv2_1:
                obj = lingYuan_lv2_1;
                break;
            case TurretType.LingYuan_lv2_2:
                obj = lingYuan_lv2_2;
                break;
            case TurretType.Hanser_lv1:
                obj = hanser_lv1;
                break;
            case TurretType.Hanser_lv2:
                obj = hanser_lv2;
                break;
            case TurretType.Amiya_lv1:
                obj = amiya_lv1;
                break;
            case TurretType.Amiya_lv2:
                obj = amiya_lv2;
                break;
            case TurretType.Enterprise_lv1:
                obj = enterprise_lv1;
                break;
            case TurretType.Enterprise_lv2_1:
                obj = enterprise_lv2_1;
                break;
            case TurretType.Enterprise_lv2_2:
                obj = enterprise_lv2_2;
                break;
            case TurretType.Enterprise_lv3_1:
                obj = enterprise_lv3_1;
                break;
            case TurretType.Enterprise_lv3_2:
                obj = enterprise_lv3_2;
                break;
            default:
                break;
        }
        if (i == 1)
            obj = obj.GetComponent<ToggleTurret>().turretPrefab;
        return obj ;
    }
}
