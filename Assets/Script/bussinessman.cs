using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class bussinessman : MonoBehaviour
{
    //路线关键点
    private GameObject wayPoints;
    private Vector3[] positionList;
    private int index; //关键点索引
    private Canvas infoPanel;
    [HideInInspector]
    public List<int> eqs;

    public GameObject body;
    public float speed; //移动速度

    // Start is called before the first frame update
    void Awake()
    {
        index = 0;
        wayPoints = GameObject.Find("WayPoints");
        Vector3[] ps = wayPoints.GetComponent<way_points>().positionList;
        positionList = new Vector3[ps.Length];
        int j = 0;
        for (int i = ps.Length - 1; i >= 0; i--)
            positionList[j++] = ps[i];
        eqs = new List<int>();
        infoPanel = GameObject.Find("GameManager").GetComponent<InfoManager>().bussinessmanInfo;
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        OpenTheStore();
    }

    void OpenTheStore()
    {
        if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if(Physics.Raycast(ray,out hit,LayerMask.GetMask("Bussiness"))&&hit.collider.gameObject.tag=="Bussiness")
            {
                //打开商店界面
                infoPanel.gameObject.SetActive(true);
                infoPanel.GetComponent<bussiness_info>().bussiMan = this;
                for (int i = 0; i < eqs.Count; i++)
                {
                    infoPanel.GetComponent<bussiness_info>().AddEquipEntry(eqs[i]);
                }
            }
        }
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
        Destroy(this.gameObject);
        if (infoPanel.gameObject.activeSelf)
        {
            infoPanel.GetComponent<bussiness_info>().CloseStore();
            GameObject.Find("GameManager").GetComponent<InfoManager>().tipsInfo.GetComponent<TipInfo>().ShowContent("商人消失了!");
        }
    }

    //商店预设
    public void InitialEquips(int m)
    {
        int[][] es = new int[3][];
        for (int i = 0; i < 3; i++)
            es[i] = new int[8];
        //第一波商店
        es[0][0] = 8;
        es[0][1] = 8;
        es[0][2] = 6;
        es[0][3] = 6;
        es[0][4] = 7;
        es[0][5] = -1;
        es[0][6] = -1;
        es[0][7] = -1;
        //第二波商店
        es[1][0] = 0;
        es[1][1] = 0;
        es[1][2] = 4;
        es[1][3] = 3;
        es[1][4] = 3;
        es[1][5] = 8;
        es[1][6] = 6;
        es[1][7] = 7;
        //第三波商店
        es[2][0] = 5;
        es[2][1] = 5;
        es[2][2] = 1;
        es[2][3] = 1;
        es[2][4] = 2;
        es[2][5] = 2;
        es[2][6] = 0;
        es[2][7] = -1;

        for (int i = 0; i < 8; i++)
        {
            if (es[m][i] != -1)
                eqs.Add(es[m][i]);
        }
    }
}
