using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Julingqu : MonoBehaviour
{
    [HideInInspector]
    public List<GameObject> toggle_obj_list;

    public GameObject box;
    public bool isEnter;
    public int power;//回复量
    public float power_ef;//回复效率

    private int max_num, mx, dx, dy;
    private Vector2 s_pos, scale;

    // Start is called before the first frame update
    void Start()
    {
        toggle_obj_list = new List<GameObject>();
        power = 0;
        power_ef = 1.0f;
        isEnter = false;
        max_num = 6;
        mx = 3;
        dx = 62;
        dy = -62;
        s_pos = transform.position;
        s_pos.x += -14;
        s_pos.y += -23;
        scale.x = 0.6f;
        scale.y = 0.6f;
    }

    // Update is called once per frame
    void Update()
    {
        //更新位置
        Vector2 pos;
        for (int i = 0; i < toggle_obj_list.Count; i++)
        {
            pos.x = s_pos.x + (i % mx) * dx;
            pos.y = s_pos.y + (i / mx) * dy;
            toggle_obj_list[i].transform.position = pos;
            toggle_obj_list[i].transform.localScale = scale;
        }

        isEnter = box.GetComponent<box_julingqu>().isOn;

        UpdateReRate();
    }

    //更新恢复灵力量
    private void UpdateReRate()
    {
        power = 0;
        power_ef = 1.0f;
        Turret t;
        for (int i = 0; i < toggle_obj_list.Count; i++)
        {
            t = toggle_obj_list[i].GetComponent<ToggleTurret>().turretPrefab.GetComponent<Turret>();
            switch (t.type)
            {
                case TurretType.None:
                    break;
                case TurretType.LingYuan_lv1:
                    power += 1;
                    power_ef += 0.2f;
                    break;
                case TurretType.LingYuan_lv2_1:
                    power += 2;
                    power_ef += 0.25f;
                    break;
                case TurretType.LingYuan_lv2_2:
                    power += 2;
                    power_ef += 0.25f;
                    break;
                case TurretType.Hanser_lv1:
                    power += 1;
                    power_ef += 0.2f;
                    break;
                case TurretType.Hanser_lv2:
                    power += 2;
                    power_ef += 0.25f;
                    break;
                case TurretType.Amiya_lv1:
                    power += 1;
                    power_ef += 0.2f;
                    break;
                case TurretType.Amiya_lv2:
                    power += 2;
                    power_ef += 0.25f;
                    break;
                case TurretType.Enterprise_lv1:
                    power += 1;
                    power_ef += 0.2f;
                    break;
                case TurretType.Enterprise_lv2_1:
                    power += 2;
                    power_ef += 0.25f;
                    break;
                case TurretType.Enterprise_lv2_2:
                    power += 2;
                    power_ef += 0.25f;
                    break;
                case TurretType.Enterprise_lv3_1:
                    power += 3;
                    power_ef += 0.3f;
                    break;
                case TurretType.Enterprise_lv3_2:
                    power += 3;
                    power_ef += 0.3f;
                    break;
                default:
                    break;
            }
        }
    }

    public bool AddTurret(GameObject turretToggle)
    {
        if (toggle_obj_list.Count >= max_num)
            return false;
        GameObject gameObject = GameObject.Instantiate(turretToggle);
        gameObject.transform.SetParent(transform);
        gameObject.GetComponent<UnityEngine.UI.Toggle>().group = this.GetComponent<UnityEngine.UI.ToggleGroup>();
        toggle_obj_list.Add(gameObject);
        return true;
    }

    public void RemoveTurret(int index)
    {
        if (index >= 0 && index < toggle_obj_list.Count)
        {
            Destroy(toggle_obj_list[index].gameObject);
            toggle_obj_list.RemoveAt(index);
        }
    }

    public void RemoveSeleted()
    {
        for (int i = 0; i < toggle_obj_list.Count; i++)
        {
            if (toggle_obj_list[i].GetComponent<UnityEngine.UI.Toggle>().isOn)
            {
                this.RemoveTurret(i);
                return;
            }
        }
    }
}
