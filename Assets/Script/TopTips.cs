using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TopTips : MonoBehaviour
{
    public GameObject content;
    public GameObject bt_open;
    public GameObject bt_close;
    // Start is called before the first frame update
    void Start()
    {
        content.SetActive(false);
        bt_open.SetActive(true);
        bt_close.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnClickedOpen()
    {
        content.SetActive(true);
        bt_open.SetActive(false);
        bt_close.SetActive(true);
    }

    public void OnClickedClose()
    {
        content.SetActive(false);
        bt_open.SetActive(true);
        bt_close.SetActive(false);
    }
}
