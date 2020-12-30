using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum PHASE
{
    Preparation,
    Fighting
}

public class PhaseManager : MonoBehaviour
{
    public int hp;
    public int n_hp;
    public Text t_hp;
    public Slider slider_hp;
    public Text phaseInfo;
    public PHASE _phase;
    public float dt_time;

    private float timer;
    private bool isTiming;
    private bool isLose;
    // Start is called before the first frame update
    void Start()
    {
        n_hp = hp;
        _phase = PHASE.Preparation;
        timer = 0;
        isTiming = false;
        isLose = false;
    }

    // Update is called once per frame
    void Update()
    {
        t_hp.text = n_hp.ToString() + "/" + hp.ToString();
        if (n_hp <= 0 && !isLose)
        {
            n_hp = 0;
            isLose = true;
            Lose();
        }
        slider_hp.value = (float)n_hp / hp;
        if (_phase==PHASE.Preparation&&!isTiming)
            isTiming = true;
        if(isTiming == true)
        {
            timer += Time.deltaTime;
            if(timer>=dt_time)
            {
                timer = 0;
                isTiming = false;
                dt_time += 15; //下次准备阶段多15秒
                _phase = PHASE.Fighting;
                GameObject.Find("BGMManager").GetComponent<bgm_manager>().PlayNextFightBGM();
                phaseInfo.text = "战斗阶段";
                phaseInfo.color = Color.red;
            }
        }

        if(_phase==PHASE.Preparation)
        {
            phaseInfo.text = "准备阶段(" + ((int)(dt_time - timer)).ToString() + ")";
            phaseInfo.color = Color.green;
        }
    }

    public void Win()
    {
        GameObject.Find("BGMManager").GetComponent<bgm_manager>().PlayeBgm(0, false);
        GameObject.Find("GameManager").GetComponent<InfoManager>().gameOverInfo.GetComponent<GameOverInfo>().SetTextAndShow("胜利！");
    }

    public void Lose()
    {
        GameObject.Find("BGMManager").GetComponent<bgm_manager>().PlayeBgm(1, true);
        GameObject.Find("GameManager").GetComponent<InfoManager>().gameOverInfo.GetComponent<GameOverInfo>().SetTextAndShow("失败了~");
    }
}
