using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text;
using System.IO;

public class enemy_spawner : MonoBehaviour
{
    public static int count_enemy_alive; //当前存活的敌人数量
    public Wave[] waves; //敌人波
    public GameObject bussinessmanPrefab;
    public Text wavesInfo;
    public float waveRate;  //每两波之间的时间间隔
    public GameObject start, end;//起始点
    private EnemyPrefab enemyPrefab;

    private void Awake()
    {
        InitialWaves();
    }

    // Start is called before the first frame update
    private void Start()
    {
        enemyPrefab = GetComponent<EnemyPrefab>();
        StartCoroutine(SpawnEnemy());
        count_enemy_alive = 0;
        wavesInfo.text = "waves:" + "0/" + waves.Length.ToString();
    }

    private void Update()
    {
        
    }

    //生成敌人
    IEnumerator SpawnEnemy()
    {
        for (int i = 0; i < waves.Length; i++)
        {
            //第5,15,25为中立商人波次
            if(i==4||i==14||i==24)
            {
                GameObject.Instantiate(bussinessmanPrefab, waves[i].end.transform.position, Quaternion.identity).GetComponent<bussinessman>().InitialEquips(i / 10);
            }
            //每10波一个阶段
            if (i % 10 == 0)
            {
                while (count_enemy_alive > 0)  //当场上还有敌人时循环等待
                    yield return 0;
                GetComponent<PhaseManager>()._phase = PHASE.Preparation;
                GameObject.Find("BGMManager").GetComponent<bgm_manager>().PlayNextPreBGM();
            }
            //当在准备阶段时循环等待
            while (GetComponent<PhaseManager>()._phase == PHASE.Preparation)
                yield return 0;
            wavesInfo.text = "waves:" + (i + 1).ToString() + "/" + waves.Length.ToString();
            for (int j = 0; j < waves[i].enemyList.Count; j++)
            {
                GameObject.Instantiate(enemyPrefab.GetEnemy(waves[i].enemyList[j]), waves[i].start.transform.position, Quaternion.identity);
                count_enemy_alive++;
                if (i < waves[i].enemyList.Count - 1)
                    yield return new WaitForSeconds((float)waves[i].rate / 1000);
            }
            //一波结束，等待一段时间
            yield return new WaitForSeconds(waveRate);
        }
        while (count_enemy_alive > 0)  //当场上还有敌人时循环等待
            yield return 0;
        if(GetComponent<PhaseManager>().n_hp>0)
        {
            GetComponent<PhaseManager>().Win();
        }
    }

    //设置敌人
    private void InitialWaves()
    {
        //string path = Application.dataPath + "/Resources/datas/enemy_waves.txt";
        //string[] data = File.ReadAllLines(path);
        TextAsset t;
        t = Resources.Load("enemy_waves") as TextAsset;
        string s = t.text;

        List<string> data = new List<string>(); 
        int num = 0;

        for (int i = 0; i < s.Length; i++)
        {
            string c = s.Substring(i, 1);
            if (c == "\n") 
            {
                num += 1;
                continue;
            }
            if (data.Count <= num)  
            {
                data.Add(c);
            }
            else
            {
                data[data.Count - 1] += c;
            }
        }

        waves = new Wave[data.Count];
        int m, n = 0;
        for (int i = 0; i <data.Count ; i++)
        {
            waves[n] = new Wave();
            waves[n].start = start;
            waves[n].end = end;
            m = 0;
            int k;
            for (k = 0; k < data[i].Length; k++)
            {
                if (data[i][k].ToString() == ":")
                    break;
            }
            for (k++; k < data[i].Length; k++)
            {
                if (data[i][k].ToString() == ";")
                {
                    waves[n].rate = m;
                    n++;
                    break;
                }
                else if (data[i][k].ToString() == ",")
                {
                    waves[n].enemyList.Add(m);
                    m = 0;
                }
                else
                {
                    m *= 10;
                    m += int.Parse(data[i][k].ToString());
                }
            }
        }
    }
}