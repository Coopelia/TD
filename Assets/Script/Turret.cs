using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Turret : MonoBehaviour
{
    //属性
    public Sprite head_icon; //图标
    public string _name; //名字
    public int cost;//价格
    public TurretType type; //类型
    public List<TurretType> upgradeTypeList; //进化预设
    //角色描述在Description.cs

    //基础数值属性
    public int b_wuli; //物理
    public int b_wuchuan; //物穿
    public int b_faqiang; //法强
    public int b_fachuan; //法穿
    public int b_baoji; //暴击
    public int b_gonsu; //攻速
    public int b_baoshang; //爆伤
    public int b_gonjifanwei; //攻击范围

    //总数值属性（装备加成等）
    [HideInInspector]
    public int t_wuli; //物理
    [HideInInspector]
    public int t_wuchuan; //物穿
    [HideInInspector]
    public int t_faqiang; //法强
    [HideInInspector]
    public int t_fachuan; //法穿
    [HideInInspector]
    public int t_baoji; //暴击
    [HideInInspector]
    public int t_gonsu; //攻速
    [HideInInspector]
    public int t_baoshang; //爆伤
    [HideInInspector]
    public int t_gonjifanwei; //攻击范围

    //技能
    public string skill_1;
    public string skill_2;
    public string skill_3;
    public string skill_4;

    //装备
    [HideInInspector]
    public int[] equips;

    public Image def_icon;
    public Image eq_0;
    public Image eq_1;
    public Image eq_2;

    public GameObject bulletPrefab;  //子弹
    public GameObject bullet_position; //子弹生成位置
    public GameObject head; //炮头

    private AudioSource audio_player;

    [HideInInspector]
    public List<GameObject> enemys = new List<GameObject>(); //进入攻击范围的敌人
    private float timer; //计时器

    [HideInInspector]
    public float dps;
    private float dps_timer;
    private int t_sum_damage;
    private float dt_time; //更新dps的周期

    private int old_fw;//之前的攻击范围
    private bool isActing;//是否可以动

    private EquipmentPrefab equipmentPrefab;
    public Animator anim;

    private void Awake()
    {
        equips = new int[3];
        equips[0] = -1;
        equips[1] = -1;
        equips[2] = -1;
    }

    void Start()
    {
        timer = 50 / (float)t_gonsu + 1;
        dps_timer = 0;
        dps = 0;
        t_sum_damage = 0;
        dt_time = 1;
        old_fw = t_gonjifanwei;
        isActing = true;
        equipmentPrefab = GameObject.Find("GameManager").GetComponent<EquipmentPrefab>();
        anim.SetBool("idle", true);
        anim.SetBool("attack", false);
        audio_player = GetComponent<AudioSource>();
    }

    void Update()
    {
        //更新攻击范围
        UpdateAttackRadius();
        //更新dps
        UpdateDPS();
        //更新属性
        UpdateProperty();
        //更新enemys，当enemy被销毁时移除
        for (int i = 0; i < enemys.Count;)
        {
            if (enemys[i] == null)
                enemys.RemoveAt(i);
            else
                i++;
        }

        if (isActing)
        {
            timer += Time.deltaTime;
            if (enemys.Count > 0)
            {
                //望向敌人
                Vector3 enemyPos = enemys[0].transform.position;
                enemyPos.y = head.transform.position.y;
                head.transform.LookAt(enemyPos);
                anim.SetBool("idle", false);
                anim.SetBool("attack", true);
                anim.speed = (float)t_gonsu / 100;

                //攻击
                if (timer * t_gonsu >= 100)
                {
                    timer = 0;
                    Attack();
                }
            }
            else
            {
                timer = 0;
                anim.SetBool("idle", true);
                anim.SetBool("attack", false);
            }
        }

        //更新头顶装备显示
        if (equips[0] != -1)
            eq_0.sprite = equipmentPrefab.GetEquipment(equips[0]).GetComponent<Equipment>().head_icon;
        else
            eq_0.sprite = def_icon.sprite;
        if (equips[1] != -1)
            eq_1.sprite = equipmentPrefab.GetEquipment(equips[1]).GetComponent<Equipment>().head_icon;
        else
            eq_1.sprite = def_icon.sprite;
        if (equips[2] != -1)
            eq_2.sprite = equipmentPrefab.GetEquipment(equips[2]).GetComponent<Equipment>().head_icon;
        else
            eq_2.sprite = def_icon.sprite;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Enemy")
        {
            enemys.Add(other.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Enemy")
        {
            enemys.Remove(other.gameObject);
        }
    }

    public void SetAct(bool f)
    {
        isActing = f;
    }

    private void Attack()
    {
        GameObject bullet = GameObject.Instantiate(bulletPrefab, bullet_position.transform.position, Quaternion.identity);
        bullet bt = bullet.GetComponent<bullet>();
        bt.turret = this.gameObject;
        bt.damage[0] = t_wuli;
        bt.damage[1] = t_wuchuan;
        bt.damage[2] = t_faqiang;
        bt.damage[3] = t_fachuan;
        bt.damage[4] = t_baoji;
        bt.damage[5] = t_baoshang;
        bt.GetComponent<bullet>().SetTarget(enemys[0].transform);
        audio_player.Play();
    }

    //更新属性
    private void UpdateProperty()
    {
        //角色基础属性
        t_wuli = b_wuli;
        t_wuchuan = b_wuchuan;
        t_faqiang = b_faqiang;
        t_fachuan = b_fachuan;
        t_baoji = b_baoji;
        t_baoshang = b_baoshang;
        t_gonsu = b_gonsu;
        t_gonjifanwei = b_gonjifanwei;
        //装备加成
        Equipment e = null;
        for (int i = 0; i < 3; i++)
        {
            if (equips[i] == -1)
                continue;
            e = equipmentPrefab.GetEquipment(equips[i]).GetComponent<Equipment>();
            //属性加成
            t_wuli += e.wuli;
            t_wuchuan += e.wuchuan;
            t_faqiang += e.faqiang;
            t_fachuan += e.fachuan;
            t_baoji += e.baoji;
            t_baoshang += e.baoshang;
            t_gonsu += e.gonsu;
            t_gonjifanwei += e.gonjifanwei;
            //技能加成
            switch (equips[i])
            {
                case 0:
                    break;
                case 1:
                    break;
                case 2:
                    break;
                case 3:
                    break;
                case 4:
                    break;
                case 5:
                    break;
                case 6:
                    break;
                case 7:
                    break;
                case 8:
                    break;
                default:
                    break;
            }
        }
    }

    //更新dps
    private void UpdateDPS()
    {
        dps_timer += Time.deltaTime;
        if (dps_timer > dt_time)
        {
            dps_timer = 0;
            dps = (float)t_sum_damage / dt_time;
            t_sum_damage = 0;
        }
    }

    //更新攻击范围
    private void UpdateAttackRadius()
    {
        if (t_gonjifanwei != old_fw)
        {
            old_fw = t_gonjifanwei;
            GetComponent<SphereCollider>().radius = t_gonjifanwei;
        }
    }

    public void GetFinalDamage(int fd)
    {
        //Debug.Log("Damage: " + fd);
        t_sum_damage += fd;
    }

    public bool AddEquip(int id)
    {
        for (int i = 0; i < 3; i++)
        {
            if (equips[i] == -1)
            {
                equips[i] = id;
                return true;
            }
        }
        return false;
    }

    public void RemoveEquip(int index)
    {
        equips[index] = -1;
    }
}
