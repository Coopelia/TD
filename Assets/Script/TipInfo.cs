using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TipInfo : MonoBehaviour
{
    public Text content;
    // Start is called before the first frame update

    public void ShowContent(string s)
    {
        content.text = s;
        this.gameObject.SetActive(true);
    }

    public void OnClickedOk()
    {
        content.text = "";
        this.gameObject.SetActive(false);
    }
 }
