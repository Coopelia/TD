using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class enemy : MonoBehaviour
{
    public GameObject body;//身体
    //路线关键点
    private GameObject wayPoints;
    private Vector3[] positionList;

    public int lv;//怪物等级
    public float speed; //移动速度
    public int hp; //总血量
    public int wufang; //物防
    public int fafang; //法防

    private int index; //关键点索引
    private int n_hp; //当前hp;

    public GameObject dieEffect;
    private Slider slider_hp;
    // Start is called before the first frame update
    void Start()
    {
        wayPoints = GameObject.Find("WayPoints");
        positionList = wayPoints.GetComponent<way_points>().positionList;
        slider_hp = GetComponentInChildren<Canvas>().GetComponentInChildren<Slider>();
        index = 0;
        n_hp = hp;
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        slider_hp.value = (float)n_hp / hp;
    }

    void Move()
    {
        if (index < positionList.Length)
        {
            transform.Translate((positionList[index] - transform.position).normalized * Time.deltaTime * speed);
            if (Vector3.Distance(transform.position, positionList[index]) <= 0.2f)
            {
                index++;
                if (index < positionList.Length)
                    body.transform.LookAt(positionList[index]);
            }
        }
        else
            ReachDestination();
    }

    void ReachDestination()
    {
        EnemyDestroy();
        GameObject.Find("GameManager").GetComponent<PhaseManager>().n_hp -= n_hp;
    }

    void EnemyDestroy()
    {
        enemy_spawner.count_enemy_alive--;
        Destroy(this.gameObject);
    }

    public int TakeDamage(int[] damage)
    {
        if (n_hp <= 0)
            return 0;
        int m = damage[0] / (damage[0] + wufang - damage[1]);
        m = m < 5 ? 5 : m;
        int w_sum = damage[0] *  m; ;//物理伤害
        m = damage[2] / (damage[2] + fafang - damage[3]);
        m = m < 5 ? 5 : m;
        int f_sum = damage[2] * m; //法术伤害
        int sum = w_sum + f_sum; //总伤害
        //判断是否暴击
        System.Random rd = new System.Random();
        if (rd.Next(0, 100) <= damage[4]) //发生暴击
            sum = (int)(sum * ((float)damage[5] / 100));
        n_hp -= sum;
        if (n_hp <= 0)
            Die();
        return sum;
    }

    public void Die()
    {
        //获得灵力
        GameObject.Find("GameManager").GetComponent<build_manager>().money += lv * 10;
        //销毁
        GameObject eft = GameObject.Instantiate(dieEffect, transform.position, Quaternion.identity);
        EnemyDestroy();
        GameObject.Destroy(eft, 0.6f);
    }
}
